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
        SearchByBeatNoAndCourierIdDeliResults = ko.observableArray(),
        SearchByBeatNoAndCourierIdPickResults = ko.observableArray(),
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

        attached = function (view) {
            $("#ReportDateDeli").kendoDatePicker({
                format: "yyyy-MM-dd"
            });
            $("#ReportDatePick").kendoDatePicker({
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
            SearchByBeatNoAndCourierIdDeliResults: SearchByBeatNoAndCourierIdDeliResults,
            SearchByBeatNoAndCourierIdPickResults: SearchByBeatNoAndCourierIdPickResults,
            searchByBeatNoAndCourierIdDeli: searchByBeatNoAndCourierIdDeli,
            clearBeatNoAndCourierIdDeli: clearBeatNoAndCourierIdDeli,
            searchByBeatNoAndCourierIdPick: searchByBeatNoAndCourierIdPick,
            clearBeatNoAndCourierIdPick: clearBeatNoAndCourierIdPick,
            activate: activate,
            attached: attached,
            partial: partial,
            list: list,
            compositionComplete: compositionComplete
        };
    });