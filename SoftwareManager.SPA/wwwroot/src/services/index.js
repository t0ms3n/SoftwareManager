System.register(['./odata-base-service', './application-service', './application-manager-service', './lookup-service'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    return {
        setters:[
            function (odata_base_service_1_1) {
                exports_1({
                    "ODataBaseService": odata_base_service_1_1["ODataBaseService"]
                });
            },
            function (application_service_1_1) {
                exports_1({
                    "ApplicationService": application_service_1_1["ApplicationService"]
                });
            },
            function (application_manager_service_1_1) {
                exports_1({
                    "ApplicationManagerService": application_manager_service_1_1["ApplicationManagerService"]
                });
            },
            function (lookup_service_1_1) {
                exports_1({
                    "LookupService": lookup_service_1_1["LookupService"]
                });
            }],
        execute: function() {
        }
    }
});
//# sourceMappingURL=index.js.map