System.register(["aurelia-framework", "aurelia-router"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var aurelia_framework_1, aurelia_router_1;
    var ApplicationsSection;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_router_1_1) {
                aurelia_router_1 = aurelia_router_1_1;
            }],
        execute: function() {
            ApplicationsSection = (function () {
                function ApplicationsSection(routerConfig) {
                    this.routerConfig = routerConfig;
                }
                ApplicationsSection.prototype.configureRouter = function (config, router) {
                    this.router = router;
                    config.map([
                        { route: '', moduleId: "./application-list", nav: false, title: "" },
                        { route: ':id', moduleId: "./application", nav: false, title: "" }
                    ]);
                };
                ApplicationsSection = __decorate([
                    aurelia_framework_1.inject(aurelia_router_1.Router), 
                    __metadata('design:paramtypes', [aurelia_router_1.Router])
                ], ApplicationsSection);
                return ApplicationsSection;
            }());
            exports_1("ApplicationsSection", ApplicationsSection);
        }
    }
});
//# sourceMappingURL=applications-section.js.map