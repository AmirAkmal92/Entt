define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app], function (context, logger, router, system, app) {
    const isBusy = ko.observable(false),
        list = ko.observableArray(),
        queueOptions = ko.observableArray(),
        selectedItems = ko.observableArray(),
        members = ko.observableArray(),
        queues = ko.observableArray(),
        rtsType = ko.observable(),
        searchText = ko.observable(""),
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
        loadListAsync = function (q) {
            q = q || query;
            return context.post(ko.toJSON(q), `api/rts-dashboard/${rtsType().toLowerCase()}`)
                .then(function (result) {
                    total(result.hits.total);
                    const eventsList = result.hits.hits.map(function (v) {
                        v.log = ko.observable({
                            total: ko.observable(0),
                            busy: ko.observable(false),
                            hits: ko.observableArray()
                        });
                        context.get(`api/rts-dashboard/logs/${v._type}/${v._id}`).then(function (log) {
                            v.log().total(log.hits.total);
                            const hits = log.hits.hits.map(function (v) {
                                v.canRequeue = ko.observable(v._source.otherInfo.requeued !== true);
                                return v;
                            });
                            v.log().hits(hits);
                        });
                        return v;
                    });
                    list(eventsList);
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
            return loadListAsync(q)
                .fail(function (e, arg) {
                    const result = JSON.parse(e.responseJSON.result);
                    logger.error("Error in your search syntax", result);
                })
                .always(function () {
                    isBusy(false);
                });
        },
        activate = function (type, id) {
            rtsType(type);
            selectedItems([]);
            queues([]);
            if(id){
                searchText(`Id:"${id.split('-')[0]}"`);
                dateFrom(moment().subtract(6, "month").format());
                return search();
            }
            return loadListAsync()
                .then(function (result) {
                    return context.loadAsync("Trigger", `Entity eq '${type}'`);
                }).then(function (lo) {
                    queueOptions(lo.itemCollection);
                    return context.loadOneAsync("EntityDefinition", `Id eq '${type.toLowerCase()}'`);
                })
                .then(function (ed) {
                    members(ed.MemberCollection());
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
                    return loadListAsync();
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
                       return loadListAsync();
                   }
               );

            $(view).on("click", "form#search-form ul.dropdown-menu a", function () {
                const term = $(this).data("term") || ($(this).text() + ":"),
                    member = ko.dataFor(this);

                let text = searchText();
                if (text) {
                    text = `${text} AND `;
                }
                if (ko.unwrap(member.TypeName) === "System.DateTime, mscorlib") {
                    searchText(`${text}  ${term} [ TO ]`);
                } else {
                    searchText(`${text} ${term}""`);
                }
                $("input#search-text").focus();
            });

        },
        requeue = function (log, element) {
           // console.log(log);
           // console.log(element);
            const rows = $(element.target).parents("tr"),
                item = ko.dataFor(rows[0]),
                id = item._id,
                type = ko.unwrap(rtsType);
                
            let data = {date : item._source.CreatedDate};
            if(ko.isObservable(log.canRequeue)){
                log.canRequeue(false);
                data = { date: log._source.time, logId: log._id, queueName: log._source.source };
            }
            return context.post(ko.toJSON(data), `api/rts-dashboard/${type}/${id}/requeue`)
                    .then(function (result) {
                        console.log(result);
                    });
        },
        viewLog = function (log) {
            console.log(log);
            require(["viewmodels/log.details.dialog", "durandal/app"], function (dialog, app2) {
                dialog.log(log._source);
                app2.showDialog(dialog);

            });
        },
        downloadMessages = function(){
            function download(data, filename, type) {
                var a = document.createElement("a"),
                    file = new Blob([data], {type: type});
                if (window.navigator.msSaveOrOpenBlob) // IE10+
                    window.navigator.msSaveOrOpenBlob(file, filename);
                else { // Others
                    var url = URL.createObjectURL(file);
                    a.href = url;
                    a.download = filename;
                    document.body.appendChild(a);
                    a.click();
                    setTimeout(function() {
                        document.body.removeChild(a);
                        window.URL.revokeObjectURL(url);  
                    }, 0); 
                }
            }
            var json = ko.toJSON(selectedItems);
            download(json, ko.unwrap(rtsType) + '.json', 'application/json');
        };

    return {
        activate: activate,
        attached: attached,
        type: rtsType,
        list: list,
        selectedItems: selectedItems,
        downloadMessages : downloadMessages,
        members: members,
        queueOptions: queueOptions,
        requeue: requeue,
        queues: queues,
        dateFrom: dateFrom,
        searchText: searchText,
        search: search,
        to: to,
        viewLog: viewLog,
        isBusy: isBusy,
        toolbar: {
            commands: ko.observableArray([
            {
                "caption": "Reload",
                "icon": "bowtie-icon bowtie-navigate-refresh",
                "id": "rts-messages-reload",
                command: function () {
                    return loadListAsync();
                }
            }])
        }
    };

});