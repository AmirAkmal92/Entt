define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
objectbuilders.app],

function(context, logger, router, system, validation, eximp, dialog, watcher, config, app) {

    var entity = ko.observable(new bespoke.PosEntt_surchargeAddOn.domain.SurchargeAddOn(system.guid())),
        errors = ko.observableArray(),
        form = ko.observable(new bespoke.sph.domain.EntityForm()),
        watching = ko.observable(false),
        id = ko.observable(),
        partial = partial || {},
        i18n = null,
        headers = {},
        activate = function(entityId) {
            id(entityId);
            var tcs = new $.Deferred();
            context.loadOneAsync("EntityForm", "Route eq 'surcharge-add-lookup-details'")
                .then(function(f) {
                form(f);
                return watcher.getIsWatchingAsync("SurchargeAddOn", entityId);
            })
                .then(function(w) {
                watching(w);
                return $.getJSON("i18n/" + config.lang + "/surcharge-add-lookup-details");
            })
                .then(function(n) {
                i18n = n[0];
                if (!entityId || entityId === "0") {
                    return Task.fromResult({
                        WebId: system.guid()
                    });
                }
                return context.get("/api/surcharge-add-ons/" + entityId);
            }).then(function(b, textStatus, xhr) {

                if (xhr) {
                    var etag = xhr.getResponseHeader("ETag"),
                        lastModified = xhr.getResponseHeader("Last-Modified");
                    if (etag) {
                        headers["If-Match"] = etag;
                    }
                    if (lastModified) {
                        headers["If-Modified-Since"] = lastModified;
                    }
                }
                entity(new bespoke.PosEntt_surchargeAddOn.domain.SurchargeAddOn(b[0] || b));
            }, function(e) {
                if (e.status == 404) {
                    app.showMessage("Sorry, but we cannot find any SurchargeAddOn with location : " + "/api/surcharge-add-ons/" + entityId, "Engineering Team Development", ["OK"]);
                }
            }).always(function() {
                if (typeof partial.activate === "function") {
                    partial.activate(ko.unwrap(entity))
                        .done(tcs.resolve)
                        .fail(tcs.reject);
                } else {
                    tcs.resolve(true);
                }
            });
            return tcs.promise();

        },

        defaultSurchargeAndAddonCommand = function() {

            if (!validation.valid()) {
                return Task.fromResult(false);
            }

            var data = ko.mapping.toJSON(entity),
                tcs = new $.Deferred();

            context.put(data, "/api/surcharge-addons/" + ko.unwrap(entity().Id) + "", headers)
                .fail(function(response) {
                var result = response.responseJSON;
                errors.removeAll();
                if (response.status === 428) {
                    // out of date conflict
                    logger.error(result.message);
                }
                if (response.status === 422 && _(result.rules).isArray()) {
                    _(result.rules).each(function(v) {
                        errors(v.ValidationErrors);
                    });
                }
                logger.error("There are errors in your entity, !!!");
                tcs.resolve(false);
            })
                .then(function(result) {
                logger.info(result.message);
                entity().Id(result.id);
                errors.removeAll();
                tcs.resolve(result);
            });
            return tcs.promise();
        }, remove = function() {
            return context.sendDelete("/api/surcharge-addons/" + ko.unwrap(entity().Id))
                .then(function(result) {
                return app.showMessage("Deleted", "Engineering Team Development", ["OK"]);
            })
                .then(function(result) {
                router.navigate("soc-dashboard");
            });
        },

        attached = function(view) {
            // validation
            validation.init($('#surcharge-add-lookup-details-form'), form());

            if (typeof partial.attached === "function") {
                partial.attached(view);
            }

        },

        compositionComplete = function() {
            $("[data-i18n]").each(function(i, v) {
                var $label = $(v),
                    text = $label.data("i18n");
                if (i18n && typeof i18n[text] === "string") {
                    $label.text(i18n[text]);
                }
            });
        },
        saveCommand = function() {
            return defaultSurchargeAndAddonCommand()
                .then(function(result) {
                if (result.success) {
                    return app.showMessage("Done", ["OK"]);
                } else {
                    return Task.fromResult(false);
                }
            })
                .then(function(result) {
                if (result) {
                    router.navigate("soc-dashboard");
                }
            });
        };
    var vm = {
        partial: partial,
        activate: activate,
        config: config,
        attached: attached,
        compositionComplete: compositionComplete,
        entity: entity,
        errors: errors,
        toolbar: {
            removeCommand: remove,
            canExecuteRemoveCommand: ko.computed(function() {
                return entity().Id();
            }),
            saveCommand: saveCommand,
            canExecuteSaveCommand: ko.computed(function() {
                if (typeof partial.canExecuteSaveCommand === "function") {
                    return partial.canExecuteSaveCommand();
                }
                return true;
            }),

        }, // end toolbar

        commands: ko.observableArray([])
    };

    return vm;
});