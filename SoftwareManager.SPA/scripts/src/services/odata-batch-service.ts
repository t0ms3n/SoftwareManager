import {HttpClient, HttpResponseMessage} from 'aurelia-http-client';

export class ODataBatchService implements IODataBatchService {
    private batch: string;
    private changeset: string;
    private content: string;
    private runningContentId: number;
    private http: HttpClient;

    constructor(baseUrl: string) {
        this.http = new HttpClient();
        this.http.configure(cfg => {
            cfg.asPost();
            cfg.withBaseUrl(`${baseUrl}`);
        });
    }

    // Start a new batch request by reseting the values and generating a new batch id
    beginBatchRequest(): ODataBatchService {
        this.runningContentId = 1;
        this.changeset = "";
        this.content = "";
        this.batch = `batch_${this.createGuid()}`;
        return this;
    }

    // Append the end of the batch request to the body
    endBatchRequest(): ODataBatchService {
        this.content += `\n--${this.batch}--`;
        return this;
    }

    // Generate a new changeset id and append it to the body
    beginChangeSet(): ODataBatchService {
        this.changeset = `changeset_${this.createGuid()}`;
        this.content += `--${this.batch}\n`;
        this.content += `Content-Type: multipart/mixed; boundary=${this.changeset}\n\n`;
        return this;
    }

    // Close the current changeset and append it to the body
    endChangeSet(): ODataBatchService {
        this.content += `--${this.changeset}--\n`;
        this.changeset = "";
        return this;
    }

    // Generate required body for the specific request and append it to the body
    addRequest(request: ISingleBatchRequest): ODataBatchService {
        if (this.changeset !== "") {
            this.content += `--${this.changeset}\n`;
        } else {
            this.content += `--${this.batch}\n`;
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
                    this.content += `If-Match: ${request.data["@odata.etag"]}\n\n`;
                } else {
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
    }

    // Execute the created batch request
    sendBatchRequest(): Promise<IBatchResult> {
        return this.http.createRequest("$batch")
            .withHeader("Content-Type", `multipart/mixed; boundary=${this.batch}`)
            .withContent(this.content)
            .send()
            .then(
            data => this.extractResponse(data)
            );
    };

    // Extract the result from the response
    private extractResponse(response: HttpResponseMessage): IBatchResult {
        let result = new BatchResult();

        result.statusCode = response.statusCode;
        result.statusText = response.statusText;
        result.isSuccess = response.isSuccess;

        // Copy headers
        let respHeader = (<any>response.headers).headers;
        Object.assign(result.headers, respHeader);

        var splittedContent = response.content.split("\r\n");
        let responseGroup = new BatchResponseGroup();
        let currentResponse = new BatchResponse()

        let breakCount = 0;
        for (let index in splittedContent) {
            if (splittedContent.hasOwnProperty(index)) {
                var line = splittedContent[index];
                if (breakCount === 0) {
                    // Skip Content-Type: application/http
                    //      Content-Transfer-Encoding: binary
                    if (line === "") {
                        breakCount++;
                    }
                } else if (breakCount === 1) {
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
                        let splittedLine = line.split(" ");
                        currentResponse.statusCode = parseInt(splittedLine[1]);
                        currentResponse.statusText = splittedLine.slice(2).join(" ").trim();
                    }
                    else if (line.startsWith("Content-ID")) {
                        let splittedLine = line.split(":");
                        currentResponse.contentId = parseInt(splittedLine[1]);
                    }
                    else if (line === "") {
                        breakCount++;
                    } else {
                        // Headers of single response
                        let splittedLine = line.split(":");
                        currentResponse.headers[splittedLine[0]] = splittedLine.slice(1).join(":").trim();
                    }
                } else if (breakCount === 2) {
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
                    // Still more content
                    else {
                        currentResponse.content += line;
                    }
                }
            }
        }

        return result;
    }

    // Get the Content-ID string, use defined content id if possible
    private getContentIdString(request: ISingleBatchRequest): string {
        let retValue = "";
        if (request.type !== SingleBatchRequestType.Get) {
            if (typeof request.contentId !== "undefined" && request.contentId > 0) {
                retValue = `Content-ID: ${request.contentId}\n`;
            } else {
                retValue = `Content-ID: ${this.runningContentId}\n`;
                this.runningContentId++;
            }
        }

        return retValue;
    }

    // Build single request url content
    private getUrlString(request: ISingleBatchRequest): string {
        return `${SingleBatchRequestType[request.type].toUpperCase()} ${request.url} HTTP/1.1\n`;
    }

    //http://stackoverflow.com/a/2117523
    //Create a Guid required for batch and changeset
    private createGuid(): string {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}


export interface IODataBatchService {

    beginBatchRequest(): IODataBatchService;
    endBatchRequest(): IODataBatchService;
    beginChangeSet(): IODataBatchService;
    endChangeSet(): IODataBatchService;
    addRequest(request: ISingleBatchRequest): IODataBatchService;
    sendBatchRequest(): Promise<IBatchResult>;
}


export interface IBatchResult {
    headers: {};
    statusCode: number;
    statusText: string;
    responseGroups: IBatchResponseGroup[];
    isSuccess: boolean;
}

class BatchResult implements IBatchResult {
    headers: any = {};
    statusCode: number;
    statusText: string = "";
    responseGroups: IBatchResponseGroup[] = [];
    isSuccess: boolean = false;
}

class BatchResponseGroup implements IBatchResponseGroup {
    isChangeSetResponse: boolean = false;
    responses: IBatchResponse[] = [];
}

export interface IBatchResponseGroup {
    isChangeSetResponse: boolean;
    responses: IBatchResponse[];
}

class BatchResponse implements IBatchResponse {
    statusCode: number;
    statusText: string = "";
    headers = {};
    contentId: number;
    content: string = "";
}

export interface IBatchResponse {
    statusCode: number;
    statusText: string;
    headers: {};
    content: string;
    contentId?: number;
}

export interface ISingleBatchRequest {
    type: SingleBatchRequestType;
    url: string;
    data?: any;
    contentId?: number;
}

export enum SingleBatchRequestType {
    Get,
    Post,
    Delete,
    Put,
    Patch
}
