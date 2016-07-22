import {HttpClient, RequestBuilder, HttpResponseMessage} from 'aurelia-http-client';

export class ODataBaseService {

    constructor(protected http: HttpClient) {
        this.http.configure(cfg =>
            cfg.withHeader("Accept", "application/json")
                .withHeader("Content-Type", "application/json"));
    }

    createGetRequest(url: string): Promise<HttpResponseMessage> {
        return this.createHttpRequest(url).asGet().send().then(data => {
            console.log(data);
            return data;
        });
    }

    createPostRequest(url: string, content: string): Promise<HttpResponseMessage> {
        return this.createHttpRequest(url).asPost().withContent(content).send().then(data => {
            console.log(data);
            return data;
        });
    }

    createPutRequest(url: string, content: string, etag: string): Promise<HttpResponseMessage> {
        let request = this.createHttpRequest(url).asPut().withContent(content);
        this.addEtagHeaderIfRequired(request, etag);
        return request.send().then(data => {
            console.log(data);
            return data;
        });
    }
    
    createPatchRequest(url: string, content: string, etag: string): Promise<HttpResponseMessage> {
        let request = this.createHttpRequest(url).asPatch().withContent(content);
        this.addEtagHeaderIfRequired(request, etag);
        return request.send().then(data => {
            console.log(data);
            return data;
        });
    }

    createDeleteRequest(url: string, etag: string): Promise<HttpResponseMessage> {
        let request = this.createHttpRequest(url).asDelete();
        this.addEtagHeaderIfRequired(request, etag);

        return request.send().then(data => {
            console.log(data);
            return data;
        });
    }

    private createHttpRequest(url: string): RequestBuilder {
        return this.http.createRequest(url);//.withCredentials(true);
    }

    private addEtagHeaderIfRequired(request: RequestBuilder, etag: string): RequestBuilder {
        if (etag && etag.length > 0)
            request.withHeader("If-Match", etag);
        return request;
    }
}
