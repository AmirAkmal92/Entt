
define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {
    const dlq = ko.observable({ messages : ko.observable()}),
        activate = function(){
           return context.get("/api/rts-dashboard/dlq").done(x => dlq(ko.mapping.fromJS(x)));

        },
        attached  = function(view){
        
        };

    return {
        activate : activate,
        attached : attached,
        dlq : dlq
    };

});