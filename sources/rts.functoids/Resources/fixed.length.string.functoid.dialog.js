define(["services/datacontext", "services/logger", "plugins/dialog"],
    function (context, logger, dialog) {
        const functoid = ko.observable(),
            okClick = function (data, ev) {
                dialog.close(this, "OK");

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };
        const vm = {
            functoid: functoid,
            okClick: okClick,
            cancelClick: cancelClick
        };
        return vm;
    });