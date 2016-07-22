System.register(["./index"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __extends = (this && this.__extends) || function (d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
    var index_1;
    var ApplicationVersionEntity;
    return {
        setters:[
            function (index_1_1) {
                index_1 = index_1_1;
            }],
        execute: function() {
            ApplicationVersionEntity = (function (_super) {
                __extends(ApplicationVersionEntity, _super);
                function ApplicationVersionEntity() {
                    _super.call(this, "ApplicationVersion");
                }
                return ApplicationVersionEntity;
            }(index_1.DateTrackedEntity));
            exports_1("ApplicationVersionEntity", ApplicationVersionEntity);
        }
    }
});
//# sourceMappingURL=applicationVersionEntity.js.map