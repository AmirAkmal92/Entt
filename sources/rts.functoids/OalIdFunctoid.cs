using System;
using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.PosEntt.Functoids
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Oal Id", FontAwesomeIcon = "bullseye", Category = FunctoidCategory.STRING)]
    public class OalIdFunctoid : Functoid
    {
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public int? Length { get; set; }

        public OalIdFunctoid()
        {
            this.OutputTypeName = typeof(string).GetShortAssemblyQualifiedName();
        }

        public override bool Initialize()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "Prefix", Type = typeof(string), IsOptional = true });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "Suffix", Type = typeof(DateTime), IsOptional = true });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "Length", Type = typeof(int), IsOptional = true });

            return base.Initialize();

        }


        public override string GenerateStatementCode()
        {
            if (this.Length.HasValue)
            {
                return $@"
                    var __ticks{Index} = System.DateTime.Now.Ticks;
                    var __id{Index} = (""{Prefix}"" + __ticks{Index}.ToString().Substring(6) + System.Guid.NewGuid().ToString(""N"") + ""{Suffix}"").Substring(0, {Length});";
            }
            var length = this["Length"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();
            var code = $@"
                var __ticks{Index} = System.DateTime.Now.Ticks;
                int __length{Index} = {length};
                var __id{Index} = (""{Prefix}"" +  __ticks{Index}.ToString().Substring(6) + System.Guid.NewGuid().ToString(""N"") + ""{Suffix}"").Substring(0, length);
";
            return code;
        }

        public override string GenerateAssignmentCode()
        {
            return $@"__id{Index}";
        }

        public override string GetEditorView()
        {
            return Properties.Resources.OalIdFunctoidDialogView;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.OalIdFunctoidDialogViewModel;
        }
    }
}