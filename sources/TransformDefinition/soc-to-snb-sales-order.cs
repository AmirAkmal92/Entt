using System.Linq;
using Bespoke.Sph.Domain;
using System.Collections.Generic;

namespace Bespoke.PosEntt.Integrations.Transforms
{
    public partial class SocToSnbSalesOrder
    {

        private static List<Bespoke.PosEntt.Products.Domain.Product> m_products = new List<Bespoke.PosEntt.Products.Domain.Product>();
        private static List<Bespoke.PosEntt.SurchargeAddOns.Domain.SurchargeAddOn> m_surcharges = new List<Bespoke.PosEntt.SurchargeAddOns.Domain.SurchargeAddOn>();
        private static List<Bespoke.PosEntt.ItemCategories.Domain.ItemCategory> m_categories = new List<Bespoke.PosEntt.ItemCategories.Domain.ItemCategory>();
        public static IEnumerable<string> Split(string value, int length)
        {
            if (string.IsNullOrWhiteSpace(value))
                yield break;
            if (value.Length < length)
                yield break;
            var c = 0;
            while (c < value.Length)
            {
                if (value.Length % length != 0)
                {
                    if (c + length > value.Length)
                        yield break;
                }
                var item = value.Substring(c, length);
                c += length;
                yield return item;
            }
        }
        partial void BeforeTransform(Bespoke.PosEntt.SalesOrders.Domain.SalesOrder item, Bespoke.PosEntt.Adapters.SnbWebApi.PostSalesOrdersRequest destination)
        {
            var context = new SphDataContext();
            if (m_products.Count == 0)
            {
                var query = context.CreateQueryable<Bespoke.PosEntt.Products.Domain.Product>()
                                .Where(p => p.Id != "0");
                var productRepos = ObjectBuilder.GetObject<IRepository<Bespoke.PosEntt.Products.Domain.Product>>();
                var lo = productRepos.LoadAsync(query, 1, 200, false).Result;
                m_products.AddRange(lo.ItemCollection);
            }
            if (m_surcharges.Count == 0)
            {
                var surchargeQuery = context.CreateQueryable<Bespoke.PosEntt.SurchargeAddOns.Domain.SurchargeAddOn>()
                                .Where(p => p.Id != "0");
                var surchargeRepos = ObjectBuilder.GetObject<IRepository<Bespoke.PosEntt.SurchargeAddOns.Domain.SurchargeAddOn>>();
                var scLo = surchargeRepos.LoadAsync(surchargeQuery, 1, 200, false).Result;
                m_surcharges.AddRange(scLo.ItemCollection);
            }
            if (m_categories.Count == 0)
            {
                var categoryQuery = context.CreateQueryable<Bespoke.PosEntt.ItemCategories.Domain.ItemCategory>()
                                .Where(p => p.Id != "0");
                var categoryRepos = ObjectBuilder.GetObject<IRepository<Bespoke.PosEntt.ItemCategories.Domain.ItemCategory>>();
                var categoriesLo = categoryRepos.LoadAsync(categoryQuery, 1, 200, false).Result;
                m_categories.AddRange(categoriesLo.ItemCollection);
            }
        }

        partial void AfterTransform(Bespoke.PosEntt.SalesOrders.Domain.SalesOrder item, Bespoke.PosEntt.Adapters.SnbWebApi.PostSalesOrdersRequest destination)
        {

            foreach (var con in destination.Body.Consignments)
            {
                Bespoke.PosEntt.SalesOrders.Domain.Consignment source = null;
                if (string.IsNullOrWhiteSpace(con.ChildConNoteNo) || con.ChildConNoteNo == "-")
                {
                    source = item.Consignments.First(x => x.ConNoteNumberParent == con.ConNoteNo && (string.IsNullOrWhiteSpace(x.ConNoteNumberChild) || x.ConNoteNumberChild == "-"));
                }
                else
                {
                    source = item.Consignments.Single(x => x.ConNoteNumberParent == con.ConNoteNo && x.ConNoteNumberChild == con.ChildConNoteNo);
                }

                // #5014
                if (source.ProductCodeMaterial == "81000012")
                    con.ActualWeight = 1;
                if (source.ProductCodeMaterial == "81000013")
                    con.ActualWeight = 2;
                if (source.ProductCodeMaterial == "81000014")
                    con.ActualWeight = 3;
                if (source.ProductCodeMaterial == "81000015")
                    con.ActualWeight = 1;

                //#5015
                if (NonSpecialityProducts.Contains(source.ProductCodeMaterial))
                {
                    con.SenderPostCode = "87000";
                    con.ReceiverPostCode = "87000";
                    if (string.IsNullOrWhiteSpace(destination.Body.BranchCode))
                        destination.Body.BranchCode = "8700";
                }


                // SNB need category name, not the bloody sequence, while soc use creepy code
                var cat = m_categories.Where(x => x.Code == source.ItemCategoryType).LastOrDefault();
                if (null != cat)
                    con.ItemCategory = cat.Name;

                // in soc, surchages in presented in a multiple of 4, e.g. 0101|1101 , "|" is not present of course 
                var chargesCode = Split((source.ValueAdded + source.SurchargeCode), 4);
                foreach (var code in chargesCode)
                {
                    var code1 = code;
                    var surcharges = m_surcharges.Where(x => x.Code == code1 && x.SnbCode.StartsWith("S")).Select(x => x.SnbCode).ToArray();
                    con.Surcharges.AddRange(surcharges);
                }

                foreach (var code in chargesCode)
                {
                    var code1 = code;
                    var services = m_surcharges.Where(x => x.Code == code1 && x.SnbCode.StartsWith("V")).Select(x => x.SnbCode).ToArray();
                    con.ValueAddedServices.AddRange(services);
                }

                con.ProductCode = m_products.Where(x => x.SocCode == source.ProductCodeMaterial).Select(x => x.Code).LastOrDefault() ?? "-";

                // split the dimensions
                if (!string.IsNullOrWhiteSpace(source.VolumetricDimension))
                {
                    var dimensions = source.VolumetricDimension.Split(new[] { "X", "x" }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (dimensions.Length == 3)
                    {
                        con.Width = decimal.Parse(dimensions[0]);
                        con.Height = decimal.Parse(dimensions[1]);
                        con.Length = decimal.Parse(dimensions[2]);
                    }
                }

            }


            destination.Body.WebId = item.Id;
        }

        public static string[] NonSpecialityProducts = new[]{
"81000012",
"81000013",
"81000014",
"81000015",
"81000016",
"81000030",
"81000031",
"81000032",
"81000033",
"81000034",
"81000023",
"81000022",
"81000024",
"81000029",
"81000028",
"81000025",
"81000027",
"81000026",
"81000001",
"81000002",
"81000003",
"81000004",
"81000005",
"81000006",
"81000007",
"81000008",
"81000009",
"20000051",
"20000052",
"20000053",
"20000054",
"20000055",
"20000056",
"81000035",
"80001219",
"80001220",
"80001221",
"80001222",
"80001223",
"80001224",
"80001225",
"80001226",
"80001227",
"80001228",
"81000017",
"81000018",
"81000019",
"81000020",
"20003843",
"20003844",
"20003845",
"20003846",
"20003847",
"20003848",
"20003849",
"20003850",
"20003851",
"20003852",
"20002730",
"20002731",
"20002732",
"20002733"
};

    }
}
