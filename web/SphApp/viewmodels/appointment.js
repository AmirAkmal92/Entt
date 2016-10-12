/// <reference path="/Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="/Scripts/knockout-3.2.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/underscore.js" />
/// <reference path="/Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],

function(context, logger, router) {

    var isBusy = ko.observable(false),
        id = ko.observable([]),
        tools = ko.observableArray([]),
        reports = ko.observableArray([]),
        recentItems = ko.observableArray([]),
        charts = ko.observableArray([]),
        views = ko.observableArray([]),
        entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
        activate = function() {
            var query = String.format("Name eq '{0}'", 'Appointment'),
                tcs = new $.Deferred(),
                chartsQuery = String.format("Entity eq 'Appointment' and IsDashboardItem eq 1"),
                formsQuery = String.format("EntityDefinitionId eq 'appointment' and IsPublished eq 1 and IsAllowedNewItem eq 1"),
                edTask = context.loadOneAsync("EntityDefinition", query),
                chartsTask = context.loadAsync("EntityChart", chartsQuery),
                formsTask = context.loadAsync("EntityForm", formsQuery),
                reportTask = context.loadAsync("ReportDefinition", "[DataSource.EntityName] eq 'Appointment'"),
                viewsTask = $.get("/Sph/EntityView/Dashboard/Appointment");


            $.when(edTask, formsTask, viewsTask, reportTask, chartsTask)
                .done(function(b, formsLo, viewsLo, reportsLo, chartsLo) {
                entity(b);
                var formsCommands = _(formsLo.itemCollection).map(function(v) {
                    return {
                        caption: v.Name(),
                        command: function() {
                            window.location = '#' + v.Route() + '/0';
                            return Task.fromResult(0);
                        },
                        icon: v.IconClass()
                    };
                });
                charts(chartsLo.itemCollection);
                reports(reportsLo.itemCollection);

                if(_.isArray(viewsLo) && _.isArray(viewsLo[0])){
                    views(viewsLo[0]);
                }

                // get counts
                _(views()).each(function(v) {
                    v.CountMessage("....");
                    var tm = setInterval(function() {
                        v.CountMessage(v.CountMessage() == "...." ? "..." : "....");
                    }, 250);
                    $.get("/api/product-all/_count/")
                        .done(function(c) {
                        clearInterval(tm);
                        v.CountMessage(c._count);
                    });
                });

                vm.toolbar.commands(formsCommands);
                tcs.resolve(true);
            });

            // TODO : get views

            // TODO: get recent items

            //TODO : reports

            // TODO : tools
            //http://i.imgur.com/OZ6mSFq.png

            return tcs.promise();
        },
        attached = function(view) {
            $(view).on('click', 'a.unpin-chart', function(e) {
                e.preventDefault();
                var chart = ko.dataFor(this),
                    link = $(this);
                if (!chart) {
                    return;
                }
                if (typeof chart.unpin === "function") {
                    link.prop('disabled', true);
                    chart.unpin().done(function() {
                        charts.remove(chart);
                    });
                }
            });
        },
        addForm = function() {

        },
        addView = function() {

        },
        recentItemsQuery = {
            "sort": [{
                "ChangedDate": {
                    "order": "desc"
                }
            }]
        },
        getMetronicColor = function(color) {
            switch (ko.unwrap(color)) {
                case "borange":
                    return "yellow-gold";
                case "bviolet":
                    return "purple";
                case "blightblue":
                    return "blue";
                case "bblue":
                    return "blue-madison";
                case "bred":
                    return "red";
                case "bgreen":
                    return "green-meadow";

            }
            return "blue-madison";
        };

    var vm = {
        isBusy: isBusy,
        getMetronicColor: getMetronicColor,
        views: views,
        charts: charts,
        entity: entity,
        activate: activate,
        attached: attached,
        reports: reports,
        tools: tools,
        recentItems: recentItems,
        addForm: addForm,
        addView: addView,
        recentItemsQuery: recentItemsQuery,
        toolbar: {
            commands: ko.observableArray([])
        }
    };

    return vm;

});