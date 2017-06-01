define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app, objectbuilders.validation],
    function (context, logger, router, app, validation) {
        var isBusy = ko.observable(false),
        ItemNo = ko.observable(""),
        ScannerId = ko.observable(""),
        AuthToken = ko.observableArray(),
        SearchItemNoResults = ko.observableArray(),
        BeatNo = ko.observable(""),
        CourierId = ko.observable(""),
        ReportDate = ko.observable(""),
        ActiveScannerEvent = ko.observable("Sip"),//todo: hard code for now
        ActiveScannerLocationId = ko.observable(""),
        ActiveScannerDate = ko.observable(""),
        SearchByBeatNoAndCourierIdDeliResults = ko.observableArray(),
        SearchByBeatNoAndCourierIdPickResults = ko.observableArray(),
        SearchByEventAndLocationIdResults = ko.observableArray(),
        partial = partial || {},
        list = ko.observableArray([]),
        map = function (v) {
            if (typeof partial.map === "function") {
                return partial.map(v);
            }
            return v;
        },
        activate = function (id) {
            if (typeof partial.activate === "function") {
                return partial.activate(list);
            }
            return true;
        },
        searchItemNo = function () {
            if (!$("#search-form").valid()) {
                return;
            }
            if (ItemNo() != "") {
                isBusy(true);
                context.get("/api/rts-dashboard/search/" + ItemNo())
                    .then(function (result) {
                        isBusy(false);
                        SearchItemNoResults(result.hits.hits);
                    }, function (e) {
                        if (e.status == 422) {
                            console.log("Unprocessable Entity");
                        }
                        isBusy(false);
                    }).done(function () {
                        $("#search-item-no").text(ItemNo());
                    });
            }
        },
        searchScannerId = function () {
            if (!$("#search-auth-form").valid()) {
                return;
            }
            if (ScannerId() != "") {
                isBusy(true);
                context.get(`/api/auth-tokens/_search?q=${ScannerId()}`)
                    .then(function (result) {
                        isBusy(false);
                        AuthToken(result.ItemCollection);

                    }, function (e) {
                        if (e.status == 422) {
                            console.log("Unprocessable Entity");
                        }
                        isBusy(false);
                    }).done(function () {
                        $("#search-auth-token").text(ScannerId())
                    });
            }
        },
        clearSearchItemNo = function () {
            ItemNo("");
            SearchItemNoResults.removeAll();
        },
        clearsearchScannerId = function () {
            ScannerId("");
            searchScannerId.removeAll();
        },
        searchByBeatNoAndCourierIdDeli = function () {
            searchByBeatNoAndCourierId("delivery", SearchByBeatNoAndCourierIdDeliResults);
        },
        clearBeatNoAndCourierIdDeli = function () {
            clearBeatNoAndCourierId(SearchByBeatNoAndCourierIdDeliResults);
        },
        searchByBeatNoAndCourierIdPick = function () {
            searchByBeatNoAndCourierId("pickup", SearchByBeatNoAndCourierIdPickResults);
        },
        clearBeatNoAndCourierIdPick = function () {
            clearBeatNoAndCourierId(SearchByBeatNoAndCourierIdPickResults);
        },
        searchByBeatNoAndCourierId = function (type, resultsArray) {
            if (type == "delivery") {
                if (!$("#delivery-report-form").valid()) {
                    return;
                }
            }
            if (type == "pickup") {
                if (!$("#pickup-report-form").valid()) {
                    return;
                }
            }
            if (CourierId() != "" && ReportDate() != "") {
                var query = {
                    "query": {
                        "bool": {
                            "must": [
                                //push conditions in
                            ]
                        }
                    },
                    "from": 0,
                    "size": 1000,
                    "sort": [
                       {
                           "Time": {
                               "order": "acs"
                           }
                       }
                    ]
                };
                query.query.bool.must.push({
                    "range": {
                        "CreatedDate": {
                            "from": ReportDate() + "T00:00:00+08:00",
                            "to": ReportDate() + "T23:59:59+08:00"
                        }
                    }
                });
                query.query.bool.must.push({
                    "term": {
                        "CourierId": CourierId()
                    }
                });
                if (BeatNo() != "") {
                    query.query.bool.must.push({
                        "term": {
                            "BeatNo": BeatNo()
                        }
                    });
                }
                isBusy(true);
                context.post(ko.toJSON(query), `/api/rts-dashboard/${type}`)
                    .then(function (result) {
                        isBusy(false);
                        resultsArray(result.hits.hits);
                    }, function (e) {
                        if (e.status == 422) {
                            console.log("Unprocessable Entity");
                        }
                        isBusy(false);
                    }).done(function () {
                        $(`#${type}-beat-no`).text(BeatNo());
                        $(`#${type}-courier-id`).text(CourierId());
                        $(`#${type}-date`).text(ReportDate());
                        $(`#${type}-total`).text(resultsArray().length);
                    });
            }
        },
        clearBeatNoAndCourierId = function (resultsArray) {
            BeatNo("");
            CourierId("");
            ReportDate("");
            resultsArray.removeAll();
        },
        searchByEventAndLocationId = function () {
            if (!$("#active-scanner-form").valid()) {
                return;
            }
            //mock the result
            result = {
                "took": 172,
                "timed_out": false,
                "_shards": {
                    "total": 905,
                    "successful": 905,
                    "failed": 0
                },
                "hits": {
                    "total": 4861,
                    "max_score": 0,
                    "hits": []
                },
                "aggregations": {
                    "scanneridaggs": {
                        "doc_count_error_upper_bound": 0,
                        "sum_other_doc_count": 0,
                        "buckets": [
                           {
                               "key": "PEN28",
                               "doc_count": 1190
                           },
                           {
                               "key": "PEN38",
                               "doc_count": 712
                           },
                           {
                               "key": "PEN41",
                               "doc_count": 630
                           },
                           {
                               "key": "PEN18",
                               "doc_count": 509
                           },
                           {
                               "key": "PEN30",
                               "doc_count": 443
                           },
                           {
                               "key": "PEN39",
                               "doc_count": 428
                           },
                           {
                               "key": "PEN54",
                               "doc_count": 363
                           },
                           {
                               "key": "PEN37",
                               "doc_count": 169
                           },
                           {
                               "key": "PEN03",
                               "doc_count": 149
                           },
                           {
                               "key": "PEN15",
                               "doc_count": 137
                           },
                           {
                               "key": "PEN60",
                               "doc_count": 75
                           },
                           {
                               "key": "PEN13",
                               "doc_count": 31
                           },
                           {
                               "key": "PEN50",
                               "doc_count": 11
                           },
                           {
                               "key": "PEN59",
                               "doc_count": 5
                           },
                           {
                               "key": "PEN61",
                               "doc_count": 4
                           },
                           {
                               "key": "PEN69",
                               "doc_count": 4
                           },
                           {
                               "key": "PEN53",
                               "doc_count": 1
                           }
                        ]
                    }
                }
            };

            SearchByEventAndLocationIdResults(result.aggregations.scanneridaggs.buckets);
            $("#active-scanner-total-events").text(result.hits.total);
            $("#active-scanner-total-active-scanners").text(result.aggregations.scanneridaggs.buckets.length);
        },
        clearEventAndLocationId = function () {
            ActiveScannerEvent("Sip");//todo: hard code for now
            ActiveScannerLocationId("");
            ActiveScannerDate("");
            SearchByEventAndLocationIdResults.removeAll();
        },

        attached = function (view) {
            $("#ReportDateDeli").kendoDatePicker({
                format: "yyyy-MM-dd"
            });
            $("#ReportDatePick").kendoDatePicker({
                format: "yyyy-MM-dd"
            });
            $("#ActiveScannerDate").kendoDatePicker({
                format: "yyyy-MM-dd"
            });
            $("#search-form").validate({
                rules: {
                    ItemNo: {
                        required: true
                    }
                },
                messages: {
                    ItemNo: "Please provide valid consignment number or console tag."
                }
            });
            $("#search-auth-form").validate({
                rules: {
                    ScannerId: {
                        required: true
                    }
                },
                messages: {
                    ScannerId: "Please provide valid scanner id."
                }
            })
            $("#delivery-report-form").validate({
                rules: {
                    BeatNo: {
                        required: false
                    },
                    CourierId: {
                        required: true
                    },
                    ReportDateDeli: {
                        required: true
                    }
                },
                messages: {
                    BeatNo: "Please provide valid Beat Number.",
                    CourierId: "Please provide valid Courier Id.",
                    ReportDateDeli: "Please provide valid Date."
                }
            });
            $("#pickup-report-form").validate({
                rules: {
                    BeatNo: {
                        required: false
                    },
                    CourierId: {
                        required: true
                    },
                    ReportDatePick: {
                        required: true
                    }
                },
                messages: {
                    BeatNo: "Please provide valid Beat Number.",
                    CourierId: "Please provide valid Courier Id.",
                    ReportDatePick: "Please provide valid Date."
                }
            });
            $("#active-scanner-form").validate({
                rules: {
                    ActiveScannerEvent: {
                        required: false//todo: no validation for now
                    },
                    ActiveScannerLocationId: {
                        required: true
                    },
                    ActiveScannerDate: {
                        required: true
                    }
                },
                messages: {
                    ActiveScannerEvent: "Please provide valid Event.",
                    ActiveScannerLocationId: "Please provide valid Location Id.",
                    ActiveScannerDate: "Please provide valid Date."
                }
            });
            if (typeof partial.attached === "function") {
                partial.attached(view);
            }
        },
        compositionComplete = function () {

        };
        return {
            isBusy: isBusy,
            //query: query,
            map: map,
            ItemNo: ItemNo,
            ScannerId: ScannerId,
            AuthToken: AuthToken,
            SearchItemNoResults: SearchItemNoResults,
            searchScannerId: searchScannerId,
            searchItemNo: searchItemNo,
            clearSearchItemNo: clearSearchItemNo,
            clearsearchScannerId: clearsearchScannerId,
            BeatNo: BeatNo,
            CourierId: CourierId,
            ReportDate: ReportDate,
            ActiveScannerEvent: ActiveScannerEvent,
            ActiveScannerLocationId: ActiveScannerLocationId,
            ActiveScannerDate: ActiveScannerDate,
            SearchByBeatNoAndCourierIdDeliResults: SearchByBeatNoAndCourierIdDeliResults,
            SearchByBeatNoAndCourierIdPickResults: SearchByBeatNoAndCourierIdPickResults,
            SearchByEventAndLocationIdResults: SearchByEventAndLocationIdResults,
            searchByBeatNoAndCourierIdDeli: searchByBeatNoAndCourierIdDeli,
            clearBeatNoAndCourierIdDeli: clearBeatNoAndCourierIdDeli,
            searchByBeatNoAndCourierIdPick: searchByBeatNoAndCourierIdPick,
            clearBeatNoAndCourierIdPick: clearBeatNoAndCourierIdPick,
            searchByEventAndLocationId: searchByEventAndLocationId,
            clearEventAndLocationId: clearEventAndLocationId,
            activate: activate,
            attached: attached,
            partial: partial,
            list: list,
            compositionComplete: compositionComplete
        };
    });