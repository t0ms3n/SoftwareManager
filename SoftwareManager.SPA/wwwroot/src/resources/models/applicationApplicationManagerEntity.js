System.register(["./index"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __extends = (this && this.__extends) || function (d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
    var index_1;
    var ApplicationApplicationManagerEntity;
    return {
        setters:[
            function (index_1_1) {
                index_1 = index_1_1;
            }],
        execute: function() {
            ApplicationApplicationManagerEntity = (function (_super) {
                __extends(ApplicationApplicationManagerEntity, _super);
                function ApplicationApplicationManagerEntity() {
                    _super.call(this, "ApplicationApplicationManager");
                }
                ApplicationApplicationManagerEntity.createFromObject = function (obj, parent) {
                    var entity = new ApplicationApplicationManagerEntity();
                    Object.assign(entity, obj);
                    //Map custom instances
                    if (parent !== undefined && parent !== null && parent.isPrototypeOf(index_1.ApplicationEntity)) {
                        entity.application = parent;
                    }
                    else {
                        entity.application = index_1.ApplicationEntity.createFromObject(obj.application, entity);
                    }
                    if (parent !== undefined && parent !== null && parent.isPrototypeOf(index_1.ApplicationManagerEntity)) {
                        entity.applicationManager = parent;
                    }
                    else {
                        entity.applicationManager = index_1.ApplicationManagerEntity.createFromObject(obj.applicationManager, entity);
                    }
                    return entity;
                };
                return ApplicationApplicationManagerEntity;
            }(index_1.DateTrackedEntity));
            exports_1("ApplicationApplicationManagerEntity", ApplicationApplicationManagerEntity);
        }
    }
});
//# sourceMappingURL=applicationApplicationManagerEntity.js.map