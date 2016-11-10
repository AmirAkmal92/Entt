using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.PosEntt.Functoids
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "CombineDateTime", BootstrapIcon = "calendar", Category = FunctoidCategory.DATE)]
    public class CombineDateTimeFunctoid : Functoid
    {

        public CombineDateTimeFunctoid()
        {
            this.OutputTypeName = typeof(DateTime).GetShortAssemblyQualifiedName();
        }

        public override bool Initialize()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "date", Type = typeof(DateTime) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "time", Type = typeof(DateTime) });
            
            return base.Initialize();

        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();
            var date = this["date"].GetFunctoid(this.TransformDefinition);
            if (null == date)
                errors.Add("date", "Date part for CombineDateTime is required", this.WebId);

            var time = this["date"].GetFunctoid(this.TransformDefinition);
            if (null == time)
                errors.Add("time", "Time part for CombineDateTime is required", this.WebId);
            
            return errors;
        }

        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            var date = this["date"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();
            code.AppendLine($"System.DateTime __datePart{Index} = {date};");

            var time = this["time"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();
            code.AppendLine($"System.DateTime __timePart{Index} = {time};");
            return code.ToString();
        }

        public override string GenerateAssignmentCode()
        {
            return $@"new DateTime(__datePart{Index}.Year, __datePart{Index}.Month, __datePart{Index}.Day, __timePart{Index}.Hour, __timePart{Index}.Minute, __timePart{Index}.Second)";
        }
        
    }
}