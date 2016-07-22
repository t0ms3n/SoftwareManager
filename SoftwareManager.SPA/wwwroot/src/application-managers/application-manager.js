System.register(['../shared/entity-view-model', "../services/index", 'aurelia-framework', 'aurelia-router', "../resources/index"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __extends = (this && this.__extends) || function (d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var entity_view_model_1, index_1, aurelia_framework_1, aurelia_router_1, index_2;
    var ApplicationManager;
    return {
        setters:[
            function (entity_view_model_1_1) {
                entity_view_model_1 = entity_view_model_1_1;
            },
            function (index_1_1) {
                index_1 = index_1_1;
            },
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_router_1_1) {
                aurelia_router_1 = aurelia_router_1_1;
            },
            function (index_2_1) {
                index_2 = index_2_1;
            }],
        execute: function() {
            ApplicationManager = (function (_super) {
                __extends(ApplicationManager, _super);
                function ApplicationManager(applicationManagerService, observer, router) {
                    _super.call(this, applicationManagerService, router, observer);
                    this.applicationManagerService = applicationManagerService;
                }
                Object.defineProperty(ApplicationManager.prototype, "title", {
                    get: function () {
                        if (this.entity.id <= 0) {
                            return "New Application Manager";
                        }
                        return "Application Manager #" + this.entity.id;
                    },
                    enumerable: true,
                    configurable: true
                });
                ApplicationManager = __decorate([
                    aurelia_framework_1.autoinject, 
                    __metadata('design:paramtypes', [index_1.ApplicationManagerService, index_2.DeepObserver, aurelia_router_1.Router])
                ], ApplicationManager);
                return ApplicationManager;
            }(entity_view_model_1.EntityViewModel));
            exports_1("ApplicationManager", ApplicationManager);
        }
    }
});
//# sourceMappingURL=application-manager.js.map