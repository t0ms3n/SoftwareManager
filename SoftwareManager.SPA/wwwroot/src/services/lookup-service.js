System.register(['aurelia-framework', 'aurelia-http-client', '../resources/index'], function(exports_1, context_1) {
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
    var aurelia_framework_1, aurelia_http_client_1, index_1;
    var LookupService;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_http_client_1_1) {
                aurelia_http_client_1 = aurelia_http_client_1_1;
            },
            function (index_1_1) {
                index_1 = index_1_1;
            }],
        execute: function() {
            LookupService = (function () {
                function LookupService(http, settings) {
                    this.http = http;
                    this.settings = settings;
                }
                LookupService.prototype.sendRequest = function (url) {
                    return this.http.createRequest(url).asGet().send().then(function (data) {
                        if (data.statusCode === 200) {
                            var obj = JSON.parse(data.response);
                            console.log(obj.value);
                            return obj.value;
                        }
                        return data;
                    });
                };
                LookupService.prototype.getApplications = function () {
                    var url = this.settings.serviceBaseUrl + "Applications?$select=id,name";
                    return this.sendRequest(url);
                };
                LookupService.prototype.getApplicationManager = function () {
                    var url = this.settings.serviceBaseUrl + "ApplicationManagers?$select=id,name";
                    return this.sendRequest(url);
                };
                LookupService.prototype.getLocations = function () {
                    var url = this.settings.serviceBaseUrl + "Locations?$select=id,name";
                    return this.sendRequest(url);
                };
                LookupService.prototype.getPrincipalTypes = function () {
                    var url = this.settings.serviceBaseUrl + "PrincipalTypes?$select=id,name";
                    return this.sendRequest(url);
                };
                LookupService.prototype.getSetupPathTypes = function () {
                    var url = this.settings.serviceBaseUrl + "PrincipalTypes?$select=id,name";
                    return this.sendRequest(url);
                };
                LookupService = __decorate([
                    aurelia_framework_1.autoinject, 
                    __metadata('design:paramtypes', [aurelia_http_client_1.HttpClient, index_1.Settings])
                ], LookupService);
                return LookupService;
            }());
            exports_1("LookupService", LookupService);
        }
    }
});
//# sourceMappingURL=lookup-service.js.map