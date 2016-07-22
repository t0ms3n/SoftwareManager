System.register(["./index"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __extends = (this && this.__extends) || function (d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
    var index_1;
    var ApplicationEntity;
    return {
        setters:[
            function (index_1_1) {
                index_1 = index_1_1;
            }],
        execute: function() {
            ApplicationEntity = (function (_super) {
                __extends(ApplicationEntity, _super);
                function ApplicationEntity() {
                    _super.call(this, "Application");
                }
                ApplicationEntity.createFromObject = function (obj, parent) {
                    var entity = new ApplicationEntity();
                    Object.assign(entity, obj);
                    //Map custom instances
                    for (var itemIndex in entity.applicationApplicationManagers) {
                        if (entity.applicationApplicationManagers.hasOwnProperty(itemIndex)) {
                            var item = entity.applicationApplicationManagers[itemIndex];
                            entity.applicationApplicationManagers[itemIndex] = index_1.ApplicationApplicationManagerEntity.createFromObject(item, entity);
                        }
                    }
                    //for (let itemIndex in entity.applicationVersions) {
                    //    if (entity.applicationVersions.hasOwnProperty(itemIndex)) {
                    //        let item = entity.applicationVersions[itemIndex];
                    //        entity.applicationVersions[itemIndex] = ApplicationVersionEntity.createFromObject(item, null);
                    //    }
                    //}
                    return entity;
                };
                return ApplicationEntity;
            }(index_1.DateTrackedEntity));
            exports_1("ApplicationEntity", ApplicationEntity);
        }
    }
});
//# sourceMappingURL=applicationEntity.js.map