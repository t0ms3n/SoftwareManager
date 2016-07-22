System.register([], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var ApplicationManagerViewModel;
    return {
        setters:[],
        execute: function() {
            ApplicationManagerViewModel = (function () {
                function ApplicationManagerViewModel(entity) {
                    this.shouldBeDeleted = false;
                    this.shouldBeAdded = false;
                    this.entity = entity;
                }
                Object.defineProperty(ApplicationManagerViewModel.prototype, "id", {
                    get: function () {
                        return this.entity.id;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(ApplicationManagerViewModel.prototype, "managerId", {
                    get: function () {
                        return this.entity.applicationManagerId;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(ApplicationManagerViewModel.prototype, "managerName", {
                    get: function () {
                        return this.entity.applicationManager.name;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(ApplicationManagerViewModel.prototype, "managerLoginName", {
                    get: function () {
                        return this.entity.applicationManager.loginName;
                    },
                    enumerable: true,
                    configurable: true
                });
                ApplicationManagerViewModel.prototype.setManager = function (lookupEntity) {
                    this.entity.applicationManagerId = lookupEntity.id;
                    this.entity.applicationManager = lookupEntity;
                };
                ApplicationManagerViewModel.prototype.activate = function () {
                };
                return ApplicationManagerViewModel;
            }());
            exports_1("ApplicationManagerViewModel", ApplicationManagerViewModel);
        }
    }
});
//# sourceMappingURL=application-manager-vm.js.map