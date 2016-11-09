using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace rts.functoids
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "CombineDateTime", BootstrapIcon = "calendar", Category = FunctoidCategory.DATE)]
    public class CombineDateTime : Functoid
    {
        public override string GenerateAssignmentCode()
        {
            return @"new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.Time.Hour, item.Time.Minute, item.Time.Second)";
        }
    }
}