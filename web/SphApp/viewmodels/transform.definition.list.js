/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/underscore.js" />
/// <reference path="/Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app, "services/new-item"],
    function (context, logger, router, app, addItemService) {

        const isBusy = ko.observable(false),
            mappings = ko.observableArray(),
            activate = function () {
                return true;
            },
            attached = function () {

            },
            remove = function (p) {
                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want to remove " + p.Name() + ", this action cannot be undone", "Rx Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            context.send(null, "/adapter/" + p.Id(), "DELETE")
                                .done(function () {
                                    tcs.resolve();
                                    mappings.remove(p);
                                    logger.info(p.Name() + " has been successfully removed");
                                });
                        }
                    });

                return tcs.promise();
            },
            edMap = new Map(),
            adapterMap = new Map(),
            getTextAndIcon = function(type, text, icon){
                const edMatches = /Bespoke\.PosEntt\..*?\.Domain\.(.*?), PosEntt.*?/g.exec(ko.unwrap(type));
                if(edMatches && edMatches.length == 2){
                    text(edMatches[1]);
                    // get the icon
                    if(edMap.has(edMatches[1])){
                        icon(edMap.get(edMatches[1]));
                        return;
                    }
                    context.loadOneAsync("EntityDefinition", `Name eq '${edMatches[1]}'`)
                        .then(function(ed){
                               icon(ko.unwrap(ed.IconClass));
                               edMap.set(edMatches[1], ko.unwrap(ed.IconClass));
                        });
                }

                const adapterMatches = /Bespoke\.PosEntt\.Adapters\.(.*?)\.(.*?), PosEntt\.(.*?)/g.exec(ko.unwrap(type));
                if(adapterMatches && adapterMatches.length){
                    // console.log("Adapter matches", adapterMatches);
                    text(`${adapterMatches[1]}.${adapterMatches[2]}`);
                    const setIcon = function(at){
                            switch(at){
                                case "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter":
                                    icon("bowtie-icon bowtie-brand-windows");
                                    break;
                                case  "Bespoke.Sph.Integrations.Adapters.RestApiAdapter, restapi.adapter":
                                    icon("bowtie-icon bowtie-azure-api-management");
                                    break;
                                default : icon("fa fa-database");
                            }
                    };

                    if(adapterMap.has(adapterMatches[1])){
                          const adapterType = adapterMap.get(adapterMatches[1]);
                          setIcon(adapterType);
                          return;
                    }
                    
                    context.loadOneAsync("Adapter", `Name eq '${adapterMatches[1]}'`)
                        .then(function(adapter){
                            const adapterType = ko.unwrap(adapter.$type);
                            setIcon(adapterType);
                            adapterMap.set(adapterMatches[1], adapterType);
                        });
                }
            },
            map = function(d){
                d.inputIcon = ko.observable();
                d.input = ko.observable();
                getTextAndIcon(d.InputTypeName, d.input, d.inputIcon);


                // output
                d.outputIcon = ko.observable();    
                d.output  = ko.observable();                     
                getTextAndIcon(d.OutputTypeName, d.output, d.outputIcon);          
                
                return d;
            };

        const vm = {
            remove: remove,
            mappings: mappings,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            map : map,
            toolbar: {
                addNewCommand: function () {
                    return addItemService.addTransformDefinitionAsync();
                },
                commands: ko.observableArray()
            }
        };

        return vm;

    });
