define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app], function (context, logger, router, system, app) {
    const isBusy = ko.observable(false),
        list = ko.observableArray(),
        queueOptions = ko.observableArray(),
        selectedItems = ko.observableArray(),
        queue = ko.observable(),
        rtsType = ko.observable(),
        total = ko.observable(),
        from = ko.observable(0),
        size = ko.observable(20),
        query = {
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
                }).then(function(lo) {
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
        },
        requeue = function() {
            return context.post(ko.toJSON(query), `api/rts-dashboard/${rtsType()}`)
                    .then(function (result) {
                        total(result.hits.total);
                        list(result.hits.hits);
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
        queue: queue,
        isBusy: isBusy
    };

});