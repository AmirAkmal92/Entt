using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;

namespace Entt.Acceptance.CustomActions
{
    public class EventWithChildrenAction : CustomAction
    {
        public override bool UseAsync => true;

        protected Task<string> GetItemConsigmentsFromConsoleDetailsAsync(string consoleNo)
        {
            var consoleDetailsAdapter = new Entt_ConsoleAdapter();
            var query = $"SELECT TOP 1 [ItemConsignments] FROM [Entt].[ConsoleDetails] WHERE [ConsoleNo] = '{consoleNo}' ORDER BY [DateTime] DESC";
            return consoleDetailsAdapter.ExecuteScalarAsync<string>(query);
        }

        protected static string GenerateId(int length)
        {
            var id = $"en{Guid.NewGuid():N}";
            return id.Substring(0, length);
        }

        protected static bool IsConsole(string connoteNo)
        {
            const string PATTERN = "CG[0-9]{9}MY";
            var match = System.Text.RegularExpressions.Regex.Match(connoteNo, PATTERN);
            return match.Success;
        }

        protected static bool IsIpsImportItem(string connoteNo)
        {
            if (!string.IsNullOrEmpty(connoteNo))
            {
                if (!connoteNo.EndsWith("MY") && connoteNo.Length == 13)
                    return true;
            }
            return false;
        }
        
        public override string GetEditorView()
        {
            // Note : remove $ string interpolation to use resharper HTML syntax
            //language=html
            var html = $@"<section class=""view-model-modal"" id=""messaging-action-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"" data-bind=""click : cancelClick"">&times;</button>
                <h3>{this.GetType().Name}</h3>
            </div>
            <div class=""modal-body"" data-bind=""with: action"">

                <form class=""form-horizontal"" id=""messaging-dialog-form"">


                </form>
            </div>
            <div class=""modal-footer"">
                <input form=""messaging-dialog-form"" data-dismiss=""modal"" type=""submit"" class=""btn btn-default"" value=""OK"" data-bind=""click: okClick"" />
                <a href=""#"" class=""btn btn-default"" data-dismiss=""modal"" data-bind=""click : cancelClick"">Cancel</a>
            </div>
        </div>
    </div>
</section>";

            return html;
        }
    }
}
