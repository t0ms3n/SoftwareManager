System.register([], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Settings;
    return {
        setters:[],
        execute: function() {
            Settings = (function () {
                function Settings() {
                    this.serviceBaseUrl = "http://localhost:58278/odata/";
                }
                return Settings;
            }());
            exports_1("Settings", Settings);
        }
    }
});
//# sourceMappingURL=settings.js.map