/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />

/**
 * @param{{aggregations, buckets}}result
 * @param{{daterangepicker:function}}$
 */

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {

        var isBusy = ko.observable(false),
            entities = ko.observableArray(),
            from = ko.observable(moment().subtract(1, "day").format("YYYY-MM-DD")),
            to = ko.observable(moment().add(1, "day").format("YYYY-MM-DD")),
            interval = ko.observable("hour"),
            query = {
                "query": {
                    "range": {
                        "CreatedDate": {
                            "from": from,
                            "to": to
                        }
                    }
                },
                "aggs": {
                    "requests_over_time": {
                        "date_histogram": {
                            "field": "CreatedDate",
                            "interval": interval,
                            "offset": "+8h",
                            "format": "yyy-MM-dd:HH"
                        }
                    }
                },
                "fields": [],
                "from": 0,
                "size": 1
            },
            activate = function () {
                return context.loadAsync("EntityDefinition", "Transient eq true").done(function(lo){
                    entities(lo.itemCollection);
                });
            },
            getCategory = function(v){
                switch(ko.unwrap(interval)){
                    case "hour": return moment(v.key).format("HH");
                    case "day": return moment(v.key).format("D/MM");
                    case "month": return moment(v.key).format("MM/YYYY");
                    case "minute": return moment(v.key).format("HH:mm");
                    case "second": return moment(v.key).format("mm:ss");
                }
                return moment(v.key).format("D/M/YY");
                        
            },
            createCharts = function(buckets) {
                const categories = buckets.map(getCategory),
                    data = buckets.map(function(v) {
                        return {
                            category: getCategory(v),
                            value: v.doc_count
                        };
                    });
                var chart = $("#request-logs-charts").empty().kendoChart({
                    theme: "metro",
                    title: {
                        text: "RTS requests"
                    },
                    legend: {
                        position: "bottom"
                    },
                    seriesDefaults: {
                        labels: {
                            visible: true,
                            format: "{0}"
                        }
                    },
                    series: [
                        {
                            type: "line",
                            data: data
                        }
                    ],
                    categoryAxis: {
                        categories: categories,
                        majorGridLines: {
                            visible: false
                        }
                    },
                    seriesClick: function (e) {
                       
                    }, tooltip: {
                        visible: true,
                        format: "{0}",
                        template: "#= category #: #= value #"
                    }
                }).data("kendoChart");
                console.log(chart);
            },
            search = function () {
                var data = ko.mapping.toJSON(query);
                isBusy(true);

                return context.post(data, "/api/rts-dashboard/")
                    .then(function (result) {
                        isBusy(false);
                        createCharts(result.aggregations.requests_over_time.buckets);
                    });
            },
            attached = function (view) {
                
                $(view).find("div.date-range")
                    .daterangepicker(
                        {
                            ranges: {
                                'Today': [moment().startOf("day"), moment().endOf("day")],
                                'Yesterday': [moment().startOf("day").subtract(1, "days"), moment().endOf("day").subtract(1, "days")],
                                'Last 7 Days': [moment().startOf("day").subtract(6, "days"), moment()],
                                'Last 30 Days': [moment().startOf("day").subtract(29, "days"), moment()],
                                'This Month': [moment().startOf("day").startOf("month"), moment().endOf("month")],
                                'Last Month': [
                                    moment().subtract(1, "month").startOf("month"),
                                    moment().subtract(1, "month").endOf("month")
                                ]
                            },
                            startDate: moment().subtract(29, "days"),
                            endDate: moment()
                        },
                        function (start, end) {
                            from(start.format());
                            to(end.format());
                            return search();
                        }
                    );
                return search();
            };

            interval.subscribe(search);

        return {
            from: from,
            to: to,
            interval : interval,
            isBusy: isBusy,
            search: search,
            activate: activate,
            attached: attached,
            query: query,
            entities :entities
        };


    });
