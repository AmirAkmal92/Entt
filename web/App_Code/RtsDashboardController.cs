using System;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;



[RoutePrefix("api/rts-dashboard")]
public class RtsDashboadController : BaseApiController
{

    private readonly HttpClient m_client = new HttpClient{BaseAddress = new Uri( ConfigurationManager.ElasticSearchHost)};
    public RtsDashboadController()
    {

    }

    [Route("")]
    [HttpPost]
    public async Task<IHttpActionResult> Aggregates([RawBody]string query)
    {
        var response = await m_client.PostAsync($"{ConfigurationManager.ElasticSearchIndex}_rts/_search", new StringContent(query));
        var content = response.Content as StreamContent;
        if (null == content) throw new Exception("Cannot execute query on es ");
        var result = await content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Cannot execute query for :  {query}, Result = {result} ");
        return Json(result);
    }
}