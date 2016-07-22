System.register(["../shared/list-view-model", "../services/index", 'aurelia-router', 'aurelia-framework'], function(exports_1, context_1) {
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
    var list_view_model_1, index_1, aurelia_router_1, aurelia_framework_1;
    var ApplicationList;
    return {
        setters:[
            function (list_view_model_1_1) {
                list_view_model_1 = list_view_model_1_1;
            },
            function (index_1_1) {
                index_1 = index_1_1;
            },
            function (aurelia_router_1_1) {
                aurelia_router_1 = aurelia_router_1_1;
            },
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            }],
        execute: function() {
            ApplicationList = (function (_super) {
                __extends(ApplicationList, _super);
                function ApplicationList(applicationService, router) {
                    _super.call(this, "applications", router, applicationService);
                    this.applicationService = applicationService;
                    this.applications = [];
                }
                ApplicationList.prototype.activate = function (params) {
                    var _this = this;
                    this.applicationService.getApplications().then(function (data) {
                        _this.applications = data;
                    });
                };
                ApplicationList = __decorate([
                    aurelia_framework_1.autoinject(), 
                    __metadata('design:paramtypes', [index_1.ApplicationService, aurelia_router_1.AppRouter])
                ], ApplicationList);
                return ApplicationList;
            }(list_view_model_1.ListViewModel));
            exports_1("ApplicationList", ApplicationList);
        }
    }
});
//# sourceMappingURL=application-list.js.map