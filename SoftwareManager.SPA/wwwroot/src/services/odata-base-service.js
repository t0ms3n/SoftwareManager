System.register([], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var ODataBaseService;
    return {
        setters:[],
        execute: function() {
            ODataBaseService = (function () {
                function ODataBaseService(http) {
                    this.http = http;
                    this.http.configure(function (cfg) {
                        return cfg.withHeader("Accept", "application/json")
                            .withHeader("Content-Type", "application/json");
                    });
                }
                ODataBaseService.prototype.createGetRequest = function (url) {
                    return this.createHttpRequest(url).asGet().send().then(function (data) {
                        console.log(data);
                        return data;
                    });
                };
                ODataBaseService.prototype.createPostRequest = function (url, content) {
                    return this.createHttpRequest(url).asPost().withContent(content).send().then(function (data) {
                        console.log(data);
                        return data;
                    });
                };
                ODataBaseService.prototype.createPutRequest = function (url, content, etag) {
                    var request = this.createHttpRequest(url).asPut().withContent(content);
                    this.addEtagHeaderIfRequired(request, etag);
                    return request.send().then(function (data) {
                        console.log(data);
                        return data;
                    });
                };
                ODataBaseService.prototype.createPatchRequest = function (url, content, etag) {
                    var request = this.createHttpRequest(url).asPatch().withContent(content);
                    this.addEtagHeaderIfRequired(request, etag);
                    return request.send().then(function (data) {
                        console.log(data);
                        return data;
                    });
                };
                ODataBaseService.prototype.createDeleteRequest = function (url, etag) {
                    var request = this.createHttpRequest(url).asDelete();
                    this.addEtagHeaderIfRequired(request, etag);
                    return request.send().then(function (data) {
                        console.log(data);
                        return data;
                    });
                };
                ODataBaseService.prototype.createHttpRequest = function (url) {
                    return this.http.createRequest(url); //.withCredentials(true);
                };
                ODataBaseService.prototype.addEtagHeaderIfRequired = function (request, etag) {
                    if (etag && etag.length > 0)
                        request.withHeader("If-Match", etag);
                    return request;
                };
                return ODataBaseService;
            }());
            exports_1("ODataBaseService", ODataBaseService);
        }
    }
});
//# sourceMappingURL=odata-base-service.js.map