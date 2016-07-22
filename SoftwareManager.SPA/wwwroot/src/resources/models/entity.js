System.register([], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Entity;
    return {
        setters:[],
        execute: function() {
            Entity = (function () {
                function Entity(type) {
                    this.id = 0;
                    this["@odata.type"] = "SoftwareManager.Entities." + type;
                }
                return Entity;
            }());
            exports_1("Entity", Entity);
        }
    }
});
//# sourceMappingURL=entity.js.map