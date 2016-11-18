define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app], function(context, logger, router, system, app){
    var isBusy = ko.observable(false),
        rtsType = ko.observable(),
        activate = function(type){
            rtsType(type);
            return true;
        },
        attached  = function(view){
        
        };

    return {
        activate : activate,
        attached : attached,
        isBusy : isBusy
    };

});