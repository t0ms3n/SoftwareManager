System.register(['aurelia-http-client'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var aurelia_http_client_1;
    var ODataBatchService, BatchResult, BatchResponseGroup, BatchResponse, SingleBatchRequestType;
    return {
        setters:[
            function (aurelia_http_client_1_1) {
                aurelia_http_client_1 = aurelia_http_client_1_1;
            }],
        execute: function() {
            ODataBatchService = (function () {
                function ODataBatchService(baseUrl) {
                    this.http = new aurelia_http_client_1.HttpClient();
                    this.http.configure(function (cfg) {
                        cfg.asPost();
                        cfg.withBaseUrl("" + baseUrl);
                    });
                }
                // Start a new batch request by reseting the values and generating a new batch id
                ODataBatchService.prototype.beginBatchRequest = function () {
                    this.runningContentId = 1;
                    this.changeset = "";
                    this.content = "";
                    this.batch = "batch_" + this.createGuid();
                    return this;
                };
                // Append the end of the batch request to the body
                ODataBatchService.prototype.endBatchRequest = function () {
                    this.content += "\n--" + this.batch + "--";
                    return this;
                };
                // Generate a new changeset id and append it to the body
                ODataBatchService.prototype.beginChangeSet = function () {
                    this.changeset = "changeset_" + this.createGuid();
                    this.content += "--" + this.batch + "\n";
                    this.content += "Content-Type: multipart/mixed; boundary=" + this.changeset + "\n\n";
                    return this;
                };
                // Close the current changeset and append it to the body
                ODataBatchService.prototype.endChangeSet = function () {
                    this.content += "--" + this.changeset + "--\n";
                    this.changeset = "";
                    return this;
                };
                // Generate required body for the specific request and append it to the body
                ODataBatchService.prototype.addRequest = function (request) {
                    if (this.changeset !== "") {
                        this.content += "--" + this.changeset + "\n";
                    }
                    else {
                        this.content += "--" + this.batch + "\n";
                    }
                    this.content += "Content-Type: application/http\n";
                    this.content += "Content-Transfer-Encoding: binary\n";
                    this.content += this.getContentIdString(request) + "\n";
                    this.content += this.getUrlString(request);
                    if (request.type !== SingleBatchRequestType.Get) {
                        this.content += "Content-Type: application/json\n";
                        this.content += "Accept: application/json\n";
                        if (typeof request.data !== "undefined" && request.data != null) {
                            if (typeof request.data["@odata.etag"] !== "undefined") {
                                this.content += "If-Match: " + request.data["@odata.etag"] + "\n\n";
                            }
                            else {
                                this.content += "\n";
                            }
                            this.content += JSON.stringify(request.data);
                        }
                        else {
                            this.content += "\n";
                        }
                    }
                    this.content += "\n";
                    return this;
                };
                // Execute the created batch request
                ODataBatchService.prototype.sendBatchRequest = function () {
                    var _this = this;
                    return this.http.createRequest("$batch")
                        .withHeader("Content-Type", "multipart/mixed; boundary=" + this.batch)
                        .withContent(this.content)
                        .send()
                        .then(function (data) { return _this.extractResponse(data); });
                };
                ;
                // Extract the result from the response
                ODataBatchService.prototype.extractResponse = function (response) {
                    var result = new BatchResult();
                    result.statusCode = response.statusCode;
                    result.statusText = response.statusText;
                    result.isSuccess = response.isSuccess;
                    // Copy headers
                    var respHeader = response.headers.headers;
                    Object.assign(result.headers, respHeader);
                    var splittedContent = response.content.split("\r\n");
                    var responseGroup = new BatchResponseGroup();
                    var currentResponse = new BatchResponse();
                    var breakCount = 0;
                    for (var index in splittedContent) {
                        if (splittedContent.hasOwnProperty(index)) {
                            var line = splittedContent[index];
                            if (breakCount === 0) {
                                // Skip Content-Type: application/http
                                //      Content-Transfer-Encoding: binary
                                if (line === "") {
                                    breakCount++;
                                }
                            }
                            else if (breakCount === 1) {
                                // Do we have a changeset
                                if (line.startsWith("--changesetresponse")) {
                                    // New group if we already had an request added
                                    if (responseGroup.responses.length > 0) {
                                        result.responseGroups.push(responseGroup);
                                        responseGroup = new BatchResponseGroup();
                                    }
                                    responseGroup.isChangeSetResponse = true;
                                    // restart response building
                                    breakCount = 0;
                                }
                                else if (line.startsWith("HTTP")) {
                                    // Extract statusCode and text
                                    var splittedLine = line.split(" ");
                                    currentResponse.statusCode = parseInt(splittedLine[1]);
                                    currentResponse.statusText = splittedLine.slice(2).join(" ").trim();
                                }
                                else if (line.startsWith("Content-ID")) {
                                    var splittedLine = line.split(":");
                                    currentResponse.contentId = parseInt(splittedLine[1]);
                                }
                                else if (line === "") {
                                    breakCount++;
                                }
                                else {
                                    // Headers of single response
                                    var splittedLine = line.split(":");
                                    currentResponse.headers[splittedLine[0]] = splittedLine.slice(1).join(":").trim();
                                }
                            }
                            else if (breakCount === 2) {
                                // Single response completed
                                if (line.startsWith("--batchresponse") || line.startsWith("--changesetresponse")) {
                                    breakCount = 0;
                                    responseGroup.responses.push(currentResponse);
                                    currentResponse = new BatchResponse();
                                    if (line.endsWith("--")) {
                                        // Group is finished
                                        result.responseGroups.push(responseGroup);
                                        responseGroup = new BatchResponseGroup();
                                    }
                                }
                                else {
                                    currentResponse.content += line;
                                }
                            }
                        }
                    }
                    return result;
                };
                // Get the Content-ID string, use defined content id if possible
                ODataBatchService.prototype.getContentIdString = function (request) {
                    var retValue = "";
                    if (request.type !== SingleBatchRequestType.Get) {
                        if (typeof request.contentId !== "undefined" && request.contentId > 0) {
                            retValue = "Content-ID: " + request.contentId + "\n";
                        }
                        else {
                            retValue = "Content-ID: " + this.runningContentId + "\n";
                            this.runningContentId++;
                        }
                    }
                    return retValue;
                };
                // Build single request url content
                ODataBatchService.prototype.getUrlString = function (request) {
                    return SingleBatchRequestType[request.type].toUpperCase() + " " + request.url + " HTTP/1.1\n";
                };
                //http://stackoverflow.com/a/2117523
                //Create a Guid required for batch and changeset
                ODataBatchService.prototype.createGuid = function () {
                    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                        return v.toString(16);
                    });
                };
                return ODataBatchService;
            }());
            exports_1("ODataBatchService", ODataBatchService);
            BatchResult = (function () {
                function BatchResult() {
                    this.headers = {};
                    this.statusText = "";
                    this.responseGroups = [];
                    this.isSuccess = false;
                }
                return BatchResult;
            }());
            BatchResponseGroup = (function () {
                function BatchResponseGroup() {
                    this.isChangeSetResponse = false;
                    this.responses = [];
                }
                return BatchResponseGroup;
            }());
            BatchResponse = (function () {
                function BatchResponse() {
                    this.statusText = "";
                    this.headers = {};
                    this.content = "";
                }
                return BatchResponse;
            }());
            (function (SingleBatchRequestType) {
                SingleBatchRequestType[SingleBatchRequestType["Get"] = 0] = "Get";
                SingleBatchRequestType[SingleBatchRequestType["Post"] = 1] = "Post";
                SingleBatchRequestType[SingleBatchRequestType["Delete"] = 2] = "Delete";
                SingleBatchRequestType[SingleBatchRequestType["Put"] = 3] = "Put";
                SingleBatchRequestType[SingleBatchRequestType["Patch"] = 4] = "Patch";
            })(SingleBatchRequestType || (SingleBatchRequestType = {}));
            exports_1("SingleBatchRequestType", SingleBatchRequestType);
        }
    }
});
//# sourceMappingURL=odata-batch-service.js.map