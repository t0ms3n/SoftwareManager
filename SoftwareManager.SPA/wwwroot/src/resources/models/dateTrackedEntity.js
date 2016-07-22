System.register(["./index"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __extends = (this && this.__extends) || function (d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
    var index_1;
    var DateTrackedEntity;
    return {
        setters:[
            function (index_1_1) {
                index_1 = index_1_1;
            }],
        execute: function() {
            DateTrackedEntity = (function (_super) {
                __extends(DateTrackedEntity, _super);
                function DateTrackedEntity() {
                    _super.apply(this, arguments);
                }
                return DateTrackedEntity;
            }(index_1.Entity));
            exports_1("DateTrackedEntity", DateTrackedEntity);
        }
    }
});
//# sourceMappingURL=dateTrackedEntity.js.map