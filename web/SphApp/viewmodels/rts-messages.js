define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app], function (context, logger, router, system, app) {
    const isBusy = ko.observable(false),
        list = ko.observableArray(),
        queueOptions = ko.observableArray(),
        selectedItems = ko.observableArray(),
        queues = ko.observableArray(),
        rtsType = ko.observable(),
        searchText = ko.observable(),
        total = ko.observable(),
        from = ko.observable(0),
        size = ko.observable(20),
        dateFrom = ko.observable(moment().subtract(1, "month").format()),
        to = ko.observable(moment().endOf("day").format()),
        query = {
            "query": {
                "bool": {
                    "must": [
                       {
                           "range": {
                               "CreatedDate": {
                                   "from": dateFrom,
                                   "to": to
                               }
                           }
                       }
                    ]
                }
            },
            "from": from,
            "size": size,
            "sort": [
               {
                   "CreatedDate": {
                       "order": "desc"
                   }
               }
            ]
        },
        activate = function (type) {
            rtsType(type);
            return context.post(ko.toJSON(query), `api/rts-dashboard/${type.toLowerCase()}`)
                .then(function (result) {
                    total(result.hits.total);
                    list(result.hits.hits);

                    return context.loadAsync("Trigger", `Entity eq '${type}'`);
                }).then(function (lo) {
                    queueOptions(lo.itemCollection);
                });
        },
        attached = function (view) {
            const element = $(view).find("div#messages-pager");
            const pager = new bespoke.utils.ServerPager({
                count: ko.unwrap(total),
                element: element,
                changed: function (page, ps) {
                    from(ps * (page - 1));
                    size(ps);
                    return context.post(ko.toJSON(query), `api/rts-dashboard/${rtsType().toLowerCase()}`)
                      .then(function (result) {
                          total(result.hits.total);
                          list(result.hits.hits);
                      });
                }
            });

            total.subscribe(pager.update);

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
                       dateFrom(start.format());
                       to(end.format());
                       return context.post(ko.toJSON(query), `api/rts-dashboard/${rtsType().toLowerCase()}`)
                          .then(function (result) {
                              total(result.hits.total);
                              list(result.hits.hits);
                          });
                   }
               );

        },
        requeue = function () {
            const qs = queues().map(x => ko.unwrap(x.Id)).join(","),
                items = selectedItems().map(x => x._source);
            return context.post(ko.toJSON(items), `api/rts-dashboard/${rtsType()}/requeue/${qs}`)
                    .then(function (result) {
                        console.log(result);
                    });
        },
        search = function () {
            const q = ko.toJS(query);
            q.query.bool.must.push({
                "query_string": {
                    "default_field": "_all",
                    "query": searchText()
                }
            });
            isBusy(true);
            return context.post(ko.toJSON(q), `api/rts-dashboard/${rtsType().toLowerCase()}`)
                   .then(function (result) {
                       total(result.hits.total);
                       list(result.hits.hits);
                    isBusy(false);
                });
        };

    return {
        activate: activate,
        attached: attached,
        type: rtsType,
        list: list,
        selectedItems: selectedItems,
        queueOptions: queueOptions,
        requeue: requeue,
        queues: queues,
        dateFrom: dateFrom,
        searchText: searchText,
        search: search,
        to: to,
        isBusy: isBusy
    };

});