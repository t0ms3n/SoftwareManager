System.register(["./index"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __extends = (this && this.__extends) || function (d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
    var index_1;
    var ApplicationManagerEntity;
    return {
        setters:[
            function (index_1_1) {
                index_1 = index_1_1;
            }],
        execute: function() {
            ApplicationManagerEntity = (function (_super) {
                __extends(ApplicationManagerEntity, _super);
                function ApplicationManagerEntity() {
                    _super.call(this, "ApplicationManager");
                }
                ApplicationManagerEntity.createFromObject = function (obj, parent) {
                    var entity = new ApplicationManagerEntity();
                    Object.assign(entity, obj);
                    //Map custom instances
                    for (var itemIndex in entity.applications) {
                        if (entity.applications.hasOwnProperty(itemIndex)) {
                            var item = entity.applications[itemIndex];
                            entity.applications[itemIndex] = index_1.ApplicationApplicationManagerEntity.createFromObject(item, entity);
                        }
                    }
                    return entity;
                };
                return ApplicationManagerEntity;
            }(index_1.Entity));
            exports_1("ApplicationManagerEntity", ApplicationManagerEntity);
        }
    }
});
//# sourceMappingURL=applicationManagerEntity.js.map