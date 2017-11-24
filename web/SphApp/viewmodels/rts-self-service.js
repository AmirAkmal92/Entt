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
            UnknownLocationId = ko.observable(""),
            UnknownDate = ko.observable(""),
            ActiveScannerEvent = ko.observable(""),
            ActiveScannerLocationId = ko.observable(""),
            ActiveScannerDate = ko.observable(""),
            SearchByBeatNoAndCourierIdDeliResults = ko.observableArray(),
            SearchByBeatNoAndCourierIdPickResults = ko.observableArray(),
            SearchByEventAndLocationIdResults = ko.observableArray(),
            SearchUnknownByLocationIdResults = ko.observableArray(),
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
                    SearchItemNoResults.removeAll();
                    context.get("/api/rts-dashboard/search/" + ItemNo())
                        .then(function (result) {
                            isBusy(false);
                            SearchItemNoResults(result.hits.hits);
                            console.log(result.hits.hits);
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
            searchScannerId = function () {
                if (!$("#search-auth-form").valid()) {
                    return;
                }
                if (ScannerId() != "") {
                    isBusy(true);
                    AuthToken.removeAll();
                    context.get("/api/auth-tokens/_search?q=" + ScannerId() + "&size=10000")
                        .then(function (result) {
                            isBusy(false);
                            AuthToken(result.ItemCollection);
                            AuthToken.sort(function (l, r) { return l.iat == r.iat ? 0 : (l.iat > r.iat ? -1 : 1) });

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
            clearsearchScannerId = function () {
                ScannerId("");
                AuthToken.removeAll();
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
                    resultsArray.removeAll();
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
                if (ActiveScannerEvent() != ""
                    && ActiveScannerLocationId() != ""
                    && ActiveScannerDate() != "") {
                    var query = {
                        "query": {
                            "bool": {
                                "must": [
                                   {
                                       "range": {
                                           "CreatedDate": {
                                               "from": ActiveScannerDate() + "T00:00:00+08:00",
                                               "to": ActiveScannerDate() + "T23:59:59+08:00"
                                           }
                                       }
                                   },
                                   {
                                       "term": {
                                           "LocationId": {
                                               "value": ActiveScannerLocationId()
                                           }
                                       }
                                   }
                                ]
                            }
                        },
                        "size": 0,
                        "aggs": {
                            "scanneridaggs": {
                                "terms": {
                                    "field": "ScannerId",
                                    "size": 100
                                }
                            }
                        }
                    };
                    isBusy(true);
                    SearchByEventAndLocationIdResults.removeAll();
                    context.post(ko.toJSON(query), `/api/rts-dashboard/${ActiveScannerEvent()}`)
                        .then(function (result) {
                            isBusy(false);
                            SearchByEventAndLocationIdResults(result.aggregations.scanneridaggs.buckets);
                            $("#active-scanner-event").text(ActiveScannerEvent());
                            $("#active-scanner-location-id").text(ActiveScannerLocationId());
                            $("#active-scanner-date").text(ActiveScannerDate());
                            $("#active-scanner-total-events").text(result.hits.total);
                            $("#active-scanner-total-active-scanners").text(result.aggregations.scanneridaggs.buckets.length);
                        }, function (e) {
                            if (e.status == 422) {
                                console.log("Unprocessable Entity");
                            }
                            isBusy(false);
                        });
                }
            },
            clearEventAndLocationId = function () {
                ActiveScannerEvent("");
                ActiveScannerLocationId("");
                ActiveScannerDate("");
                SearchByEventAndLocationIdResults.removeAll();
            },
            searchUnknownByLocationId = function ()
            {
                if (!$("#unknown-form").valid()) {
                    return;
                }
                if (UnknownLocationId() != "" && UnknownDate != "") {
                    var query = {
                        "query": {
                            "bool": {
                                "must": [
                                    {
                                        "query_string": {
                                            "default_field": "_all",
                                            "query": "_type:unknown"
                                        }
                                    }
                                    ,
                                    {
                                        "term": {
                                            "LocationId": {
                                                "value": UnknownLocationId()
                                            }

                                        }
                                    },
                                    {
                                        "range": {
                                            "CreatedDate": {
                                                "from": UnknownDate() + "T00:00:00+08:00",
                                                "to": UnknownDate() + "T23:59:59+08:00"
                                            }
                                        }
                                    }
                                ]
                            }
                        },
                        "size": 250,
                        "sort": [
                            {
                                "CreatedDate": {
                                    "order": "asc"
                                }
                            }
                        ]
                    }

                    isBusy(true);
                    SearchUnknownByLocationIdResults.removeAll();
                    context.post(ko.toJSON(query), `/api/rts-dashboard/unknown`)
                        .then(function (result) {
                            isBusy(false);
                            console.log(result.hits.hits);
                            SearchUnknownByLocationIdResults(result.hits.hits);
                            $("#unknown-location-id").text(UnknownLocationId());
                            $("#unknown-date").text(UnknownDate());
                            $("#unknown-total").text(result.hits.total);
                        }, function (e) {
                            if (e.status == 422) {
                                console.log("Unprocessable Entity");
                            }
                            isBusy(false);
                        });
                    
                }
            },

            clearLocationIdAndDate = function () {
                UnknownLocationId("");
                UnknownDate("");
                UnknownStatus("");
                SearchUnknownByLocationIdResults.removeAll();
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
                $("#UnknownDate").kendoDatePicker({
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
                            required: true
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
                $("#unknown-form").validate({
                    rules: {
                        UnknownLocationId: {
                            required: true
                        },
                        UnknownDate: {
                            required: true
                        },
                        UnknownStatus: {
                            required: false
                        }
                    },
                    messages: {
                        UnknownLocationId: "Please provide valid Location Id.",
                        UnknownDate: "Please provide date.",
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
            UnknownLocationId: UnknownLocationId,
            UnknownDate: UnknownDate,
            SearchByBeatNoAndCourierIdDeliResults: SearchByBeatNoAndCourierIdDeliResults,
            SearchByBeatNoAndCourierIdPickResults: SearchByBeatNoAndCourierIdPickResults,
            SearchByEventAndLocationIdResults: SearchByEventAndLocationIdResults,
            SearchUnknownByLocationIdResults: SearchUnknownByLocationIdResults,
            searchUnknownByLocationId: searchUnknownByLocationId,
            clearLocationIdAndDate: clearLocationIdAndDate,
            searchByBeatNoAndCourierIdDeli: searchByBeatNoAndCourierIdDeli,
            clearBeatNoAndCourierIdDeli: clearBeatNoAndCourierIdDeli,
            searchByBeatNoAndCourierIdPick: searchByBeatNoAndCourierIdPick,
            clearBeatNoAndCourierIdPick: clearBeatNoAndCourierIdPick,
            searchByEventAndLocationId: searchByEventAndLocationId,
            clearEventAndLocationId: clearEventAndLocationId,
            availableEvents: ko.observableArray(['Delivery', 'Pickup', 'Sip']),
            activate: activate,
            attached: attached,
            partial: partial,
            list: list,
            compositionComplete: compositionComplete
        };
    });