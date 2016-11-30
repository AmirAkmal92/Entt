using System;
using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.PosEntt.Functoids
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Fixed length string", FontAwesomeIcon = "crop", Category = FunctoidCategory.STRING)]
    public class FixedLengthStringFunctoid : Functoid
    {
        public string Input { get; set; }
        public int Length { get; set; }
        public bool Pad { get; set; }
        public string PadLeftOrRight { get; set; }
        public string PadChar { get; set; }

        public FixedLengthStringFunctoid()
        {
            this.OutputTypeName = typeof(string).GetShortAssemblyQualifiedName();
        }

        public override bool Initialize()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "Input", Type = typeof(string), IsOptional = false });

            return base.Initialize();

        }


        public override string GenerateStatementCode()
        {
            var input = this["Input"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();
            if (!this.Pad)
            {
                return $@"
                    var __fixedLengthInput{Index} = $@""{{{input}}}"";
                    var __fixedLengthOutput{Index} = __fixedLengthInput{Index};
                    if(!string.IsNullOrWhiteSpace(__fixedLengthInput{Index}) && __fixedLengthInput{Index}.Length > {Length})
                        __fixedLengthOutput{Index} = __fixedLengthInput{Index}.Substring(0, {Length});
                    ";
            }
            var code = $@"

                    var __fixedLengthInput{Index} = $@""{{{input}}}"";
                    var __fixedLengthOutput{Index} = __fixedLengthInput{Index};
                    if(!string.IsNullOrWhiteSpace(__fixedLengthInput{Index}) && __fixedLengthInput{Index}.Length > {Length})
                        __fixedLengthOutput{Index} = __fixedLengthInput{Index}.Substring(0, {Length});
                    if(!string.IsNullOrWhiteSpace(__fixedLengthInput{Index}) && __fixedLengthInput{Index}.Length < {Length})
                        ";
            if (PadLeftOrRight.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                code += $@"__fixedLengthOutput{Index} = (new string('{PadChar}', {Length} - __fixedLengthInput{Index}.Length)) + __fixedLengthInput{Index};";
            else
                code += $@"__fixedLengthOutput{Index} = __fixedLengthInput{Index} + (new string('{PadChar}', {Length} - __fixedLengthInput{Index}.Length));";
            return code;
        }

        public override string GenerateAssignmentCode()
        {
            return $"__fixedLengthOutput{Index}";
        }

        public override string GetEditorView()
        {
            return Properties.Resources.FixedLengthStringFunctoidView;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.FixedLengthStringFunctoidViewModel;
        }
    }
}