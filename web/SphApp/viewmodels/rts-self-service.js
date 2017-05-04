define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app, objectbuilders.validation],
    function (context, logger, router, app, validation) {
        var isBusy = ko.observable(false),
        ItemNo = ko.observable(""),
        SearchItemNoResults = ko.observableArray(),
        BeatNo = ko.observable(""),
        CourierId = ko.observable(""),
        ReportDate = ko.observable(""),
        SearchByBeatNoAndCourierIdDeliResults = ko.observableArray(),
        SearchByBeatNoAndCourierIdPickResults = ko.observableArray(),
        activate = function (id) {
            return true;
        },
        searchItemNo = function () {
            //if (!$("#search-form").valid()) {
            //    return;
            //}
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
        clearSearchItemNo = function () {
            ItemNo("");
            SearchItemNoResults.removeAll();
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
            //if (type == "delivery") {
            //    if (!$("#delivery-report-form").valid()) {
            //        return;
            //    }
            //}
            //if (type == "pickup") {
            //    if (!$("#pickup-report-form").valid()) {
            //        return;
            //    }
            //}
            if (BeatNo() != "" && CourierId() != "" && ReportDate() != "") {
                var query = {
                    "query": {
                        "bool": {
                            "must": [
                               {
                                   "range": {
                                       "CreatedDate": {
                                           "from": ReportDate() + "T00:00:00+08:00",
                                           "to": ReportDate() + "T23:59:59+08:00"
                                       }
                                   }
                               },
                               {
                                   "term": {
                                       "BeatNo": BeatNo()
                                   }
                               },
                               {
                                   "term": {
                                       "CourierId": CourierId()
                                   }
                               }
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
        attached = function (view) {
            $("#ReportDateDeli").kendoDatePicker({
                format: "yyyy-MM-dd"
            });
            $("#ReportDatePick").kendoDatePicker({
                format: "yyyy-MM-dd"
            });
            /*
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
            $("#delivery-report-form").validate({
                rules: {
                    BeatNo: {
                        required: true
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
                        required: true
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
            */
        },
        compositionComplete = function () {

        };
        return {
            isBusy: isBusy,
            ItemNo: ItemNo,
            SearchItemNoResults: SearchItemNoResults,
            searchItemNo: searchItemNo,
            clearSearchItemNo: clearSearchItemNo,
            BeatNo: BeatNo,
            CourierId: CourierId,
            ReportDate: ReportDate,
            SearchByBeatNoAndCourierIdDeliResults: SearchByBeatNoAndCourierIdDeliResults,
            SearchByBeatNoAndCourierIdPickResults: SearchByBeatNoAndCourierIdPickResults,
            searchByBeatNoAndCourierIdDeli: searchByBeatNoAndCourierIdDeli,
            clearBeatNoAndCourierIdDeli: clearBeatNoAndCourierIdDeli,
            searchByBeatNoAndCourierIdPick: searchByBeatNoAndCourierIdPick,
            clearBeatNoAndCourierIdPick: clearBeatNoAndCourierIdPick,
            activate: activate,
            attached: attached,
            compositionComplete: compositionComplete
        };
    });