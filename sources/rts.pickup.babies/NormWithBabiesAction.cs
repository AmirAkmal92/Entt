using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Polly;
using System.Linq;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "NormWithBabies", TypeName = "Bespoke.PosEntt.CustomActions.NormWithBabiesAction, rts.pickup.babies", Description = "RTS Norm with babies", FontAwesomeIcon = "calendar-check-o")]
    public class NormWithBabiesAction : CustomAction
    {
        public override bool UseAsync => true;

        private List<Adapters.Oal.dbo_normal_console_event_new> m_normEventRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_normEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var norm = context.Item as Norms.Domain.Norm;
            if (null == norm) return;
            if (norm.TotalConsignment <= 0) return;

            m_normEventRows = new List<Adapters.Oal.dbo_normal_console_event_new>();
            m_normEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>();

            await RunAsync(norm);
        }

        public async Task RunAsync(Norms.Domain.Norm norm)
        {
            var consoleList = new List<string>();
            if (IsConsole(norm.ConsoleTag)) consoleList.Add(norm.ConsoleTag);

            //norm console_details
            var normEventMap = new Integrations.Transforms.RtsNormToNormalConsoleEventNew();
            var normEventAdapter = new Adapters.Oal.dbo_normal_console_event_newAdapter();
            var parentRow = await normEventMap.TransformAsync(norm);
            m_normEventRows.Add(parentRow);

            var itemList = norm.AllConsignmentNotes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in itemList)
            {
                var childItem = parentRow.Clone();
                ProcessChild(parentRow, item);

                //2 level
                var console = IsConsole(item);
                if (console)
                {
                    consoleList.Add(item);
                    var childConsole = await SearchConsoleDetails(item);
                    if (null != childConsole)
                    {
                        var childConsoleItems = childConsole.item_consignments.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var cc in childConsoleItems)
                        {
                            if (consoleList.Contains(cc)) continue;
                            ProcessChild(parentRow, cc);

                            //3 level
                            var anotherConsole = IsConsole(cc);
                            if (anotherConsole)
                            {
                                consoleList.Add(cc);
                                var anotherChildConsole = await SearchConsoleDetails(cc);
                                if (null != anotherChildConsole)
                                {
                                    var anotherChildConsoleItems = anotherChildConsole.item_consignments.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var ccc in anotherChildConsoleItems)
                                    {
                                        if (consoleList.Contains(ccc)) continue;
                                        ProcessChild(parentRow, ccc);
                                    }
                                }
                                else
                                {
                                    AddPendingItems(parentRow.id, cc);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddPendingItems(parentRow.id, item);
                    }
                }

            }

            foreach (var item in m_normEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => normEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_normEventPendingConsoleRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => pendingAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            //
            //execute pending items, if any
            var pendingItems = await SearchEventPending(norm.ConsoleTag);
            foreach (var pending in pendingItems)
            {
                await ProcessEventPendingItem(norm, pending);
            }


            var consolePendingList = new List<string>();
            consolePendingList.Add(norm.ConsoleTag);
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                if (console)
                {
                    if (consolePendingList.Contains(item)) continue;
                    
                    consolePendingList.Add(item);
                }
            }
        }

        async private Task ProcessEventPendingItem(Norms.Domain.Norm norm, Adapters.Oal.dbo_event_pending_console pending)
        {
            var itemList = norm.AllConsignmentNotes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var ok = false;
            switch(pending.event_class)
            {
                case "pos.oal.SopEventNew":
                    await ProcessSopPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.SipEventNew":
                    await ProcessSipPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.HipEventNew":
                    await ProcessHipPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.HopEventNew":
                    await ProcessHopPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.StatusEventNew":
                    await ProcessStatPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.VasnEventNew":
                    await ProcessVasnPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.NormalConsoleEventNew":
                    //ProcessNormPendingItem(item.console_no, item.event_id);
                    break;
                case "pos.oal.MissortEventNew":
                    break;
                case "pos.oal.WwpEventNewLog":
                    await ProcessWwpPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
            }

            if (ok)
            {
                var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
                await pendingAdapter.DeleteAsync(pending.id);
            }
        }

        async private Task ProcessStatPendingItem(string statEventId, string[] itemList)
        {
            var statAdapter = new Adapters.Oal.dbo_status_code_event_newAdapter();
            var stat = await statAdapter.LoadOneAsync(statEventId);
            var statItems = new List<Adapters.Oal.dbo_status_code_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = stat.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                statItems.Add(child);
            }
            foreach (var item in statItems)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => statAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        async private Task ProcessVasnPendingItem(string vasnEventId, string[] itemList)
        {
            var vasnAdapter = new Adapters.Oal.dbo_vasn_event_newAdapter();
            var vasn = await vasnAdapter.LoadOneAsync(vasnEventId);
            var vasnItems = new List<Adapters.Oal.dbo_vasn_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = vasn.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                vasnItems.Add(child);
            }
            foreach (var item in vasnItems)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => vasnAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        async private Task ProcessSopPendingItem(string sopEventId, string[] itemList)
        {
            var sopAdapter = new Adapters.Oal.dbo_sop_event_newAdapter();
            var sop = await sopAdapter.LoadOneAsync(sopEventId);
            var sops = new List<Adapters.Oal.dbo_sop_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = sop.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                sops.Add(child);
            }
            foreach (var item in sops)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sopAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        async private Task ProcessSipPendingItem(string sipEventId, string[] itemList)
        {
            var sipAdapter = new Adapters.Oal.dbo_sip_event_newAdapter();
            var sip = await sipAdapter.LoadOneAsync(sipEventId);
            var sips = new List<Adapters.Oal.dbo_sip_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = sip.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                sips.Add(child);
            }
            foreach (var item in sips)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sipAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        async private Task ProcessHopPendingItem(string hopEventId, string[] itemList)
        {
            var hopAdapter = new Adapters.Oal.dbo_hop_event_newAdapter();
            var hop = await hopAdapter.LoadOneAsync(hopEventId);
            var hops = new List<Adapters.Oal.dbo_hop_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = hop.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                hops.Add(child);
            }
            foreach (var item in hops)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hopAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        async private Task ProcessHipPendingItem(string hipEventId, string[] itemList)
        {
            var hipAdapter = new Adapters.Oal.dbo_hip_event_newAdapter();
            var hip = await hipAdapter.LoadOneAsync(hipEventId);
            var hips = new List<Adapters.Oal.dbo_hip_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = hip.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                hips.Add(child);
            }
            foreach (var item in hips)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hipAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        async private Task ProcessWwpPendingItem(string wwpEventId, string[] itemList)
        {
            var wwpAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var wwp = await wwpAdapter.LoadOneAsync(wwpEventId);
            var wwpItems = new List<Adapters.Oal.dbo_wwp_event_new_log>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = wwp.Clone();
                child.id = GenerateId(34);
                child.consignment_note_number = item;
                child.date_sent_date_field = null;
                wwpItems.Add(child);
            }
            foreach (var item in wwpItems)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => wwpAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private void ProcessNormPendingItem(string consoleTag, string id)
        {
        }

        async private Task<ObjectCollection<Adapters.Oal.dbo_event_pending_console>> SearchEventPending(string consoleNo)
        {
            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            var query = string.Format("SELECT * FROM [dbo].[event_pending_console] WHERE console_no = '{0}'", consoleNo);
            var lo = await pendingAdapter.LoadAsync(query);
            return lo.ItemCollection;

        }

        async private Task<Adapters.Oal.dbo_console_details> SearchConsoleDetails(string consignmentNo)
        {
            var consoleDetailsAdapter = new Adapters.Oal.dbo_console_detailsAdapter();
            var query = string.Format("SELECT * FROM [dbo].[console_details] WHERE console_no = '{0}'", consignmentNo);
            var lo = await consoleDetailsAdapter.LoadAsync(query);
            return lo.ItemCollection.FirstOrDefault();
        }

        private void ProcessChild(Adapters.Oal.dbo_normal_console_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_normEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console();
            pendingConsole.id = GenerateId(20);
            pendingConsole.event_id = parentId;
            pendingConsole.event_class = "pos.oal.NormalConsoleEventNew";
            pendingConsole.console_no = consignmentNo;
            pendingConsole.version = 0;
            pendingConsole.date_field = DateTime.Now;
            m_normEventPendingConsoleRows.Add(pendingConsole);
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.NormWithBabiesAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.NormWithBabiesAction, rts.pickup.babies"";
                    if (optionOrWebid && typeof optionOrWebid === ""object"")
                    {
                        for (var n in optionOrWebid)
                        {
                            if (typeof v[n] === ""function"")
                            {
                                v[n](optionOrWebid[n]);
                            }
                        }
                    }
                    if (optionOrWebid && typeof optionOrWebid === ""string"")
                    {
                        v.WebId(optionOrWebid);
                    }

                    if (bespoke.sph.domain.NormWithBabiesActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.NormWithBabiesActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.NormWithBabiesAction(system.guid())),
            trigger = ko.observable(),                   
            activate = function() {
                   return true;

                },
            attached = function(view) {
                },
            okClick = function(data, ev) {
                    if (bespoke.utils.form.checkValidity(ev.target))
                    {
                        dialog.close(this, ""OK"");
                    }
                },
            cancelClick = function() {
                    dialog.close(this, ""Cancel"");
                };

                const vm = {
                    trigger: trigger,
                    action: action,
                    activate: activate,
                    attached: attached,
                    okClick: okClick,
                    cancelClick: cancelClick
                };


            return vm;

        });
";
        }

        public override string GetEditorView()
        {
            //language=html
            var html = @"<section class=""view-model-modal"" id=""messaging-action-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"" data-bind=""click : cancelClick"">&times;</button>
                <h3>RTS norm with child items</h3>
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

        private string GenerateId(int length)
        {
            var id = string.Format("en{0}", System.Guid.NewGuid().ToString("N"));
            return id.Substring(0, length);
        }

        private bool IsConsole(string connoteNo)
        {
            var pattern = "CG[0-9]{9}MY";
            var match = System.Text.RegularExpressions.Regex.Match(connoteNo, pattern);
            return match.Success;
        }
    }
}