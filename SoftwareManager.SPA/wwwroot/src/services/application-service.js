System.register(['../resources/index', './index', 'aurelia-http-client', 'aurelia-framework', "../resources/models/index"], function(exports_1, context_1) {
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
    var index_1, index_2, aurelia_http_client_1, aurelia_framework_1, index_3;
    var ApplicationService;
    return {
        setters:[
            function (index_1_1) {
                index_1 = index_1_1;
            },
            function (index_2_1) {
                index_2 = index_2_1;
            },
            function (aurelia_http_client_1_1) {
                aurelia_http_client_1 = aurelia_http_client_1_1;
            },
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (index_3_1) {
                index_3 = index_3_1;
            }],
        execute: function() {
            ApplicationService = (function (_super) {
                __extends(ApplicationService, _super);
                function ApplicationService(http, settings) {
                    _super.call(this, http);
                    this.http = http;
                    this.settings = settings;
                }
                ApplicationService.prototype.convertToResult = function (obj) {
                    var result;
                    if (Array.isArray(obj)) {
                        result = [];
                        for (var property in obj) {
                            if (obj.hasOwnProperty(property)) {
                                var value = obj[property];
                                if (value == null)
                                    continue;
                                result.push(index_3.ApplicationEntity.createFromObject(value, null));
                            }
                        }
                    }
                    else {
                        result = index_3.ApplicationEntity.createFromObject(obj, null);
                    }
                    console.log("Converted from", obj, "to", result);
                    return result;
                };
                ApplicationService.prototype.getApplications = function () {
                    var _this = this;
                    var url = this.settings.serviceBaseUrl + "Applications";
                    return this.createGetRequest(url).then(function (data) {
                        if (data.statusCode === 200) {
                            var obj = JSON.parse(data.response);
                            return _this.convertToResult(obj.value);
                        }
                        return data;
                    });
                };
                ApplicationService.prototype.getApplication = function (applicationId) {
                    var _this = this;
                    var url = this.settings.serviceBaseUrl + "Applications(" + applicationId + ")?$expand=applicationApplicationManagers($expand=applicationManager($select=name,loginName))";
                    return this.createGetRequest(url).then(function (data) {
                        if (data.statusCode === 200) {
                            var obj = JSON.parse(data.response);
                            return _this.convertToResult(obj);
                        }
                        return data;
                    });
                };
                ApplicationService.prototype.deleteEntity = function (entity) {
                    if (entity.id > 0) {
                        var url = this.settings.serviceBaseUrl + "Applications(" + entity.id + ")";
                        return this.createDeleteRequest(url, entity["@odata.etag"]).then(function (response) {
                            if (response.statusCode === 204) {
                                // No Content -> Success
                                return true;
                            }
                            return false;
                        });
                    }
                    return Promise.resolve(false);
                };
                ApplicationService.prototype.insertEntity = function (entity) {
                    var url = this.settings.serviceBaseUrl + "Applications";
                    var content = JSON.stringify(entity);
                    return this.createPostRequest(url, content).then(function (response) {
                        if (response.statusCode === 201) {
                            var obj = JSON.parse(response.response);
                            console.log(obj);
                            return obj;
                        }
                        return response;
                    });
                };
                ApplicationService.prototype.updateEntity = function (entity, changedProperties) {
                    if (changedProperties.length === 0)
                        return Promise.resolve(entity);
                    var url = this.settings.serviceBaseUrl + "Applications(" + entity.id + ")";
                    var patchItem = {};
                    changedProperties.forEach(function (value) {
                        patchItem[value] = entity[value];
                    });
                    var content = JSON.stringify(patchItem);
                    return this.createPatchRequest(url, content, entity["@odata.etag"]);
                };
                // Factory?
                ApplicationService.prototype.createNew = function () {
                    var item = new index_3.ApplicationEntity();
                    return Promise.resolve(item);
                };
                ApplicationService.prototype.loadExisting = function (id) {
                    return this.getApplication(id);
                };
                ApplicationService = __decorate([
                    aurelia_framework_1.autoinject, 
                    __metadata('design:paramtypes', [aurelia_http_client_1.HttpClient, index_1.Settings])
                ], ApplicationService);
                return ApplicationService;
            }(index_2.ODataBaseService));
            exports_1("ApplicationService", ApplicationService);
        }
    }
});
//# sourceMappingURL=application-service.js.map