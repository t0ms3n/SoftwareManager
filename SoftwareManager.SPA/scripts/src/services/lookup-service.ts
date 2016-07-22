import {autoinject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';
import {Settings} from '../resources/index';

@autoinject
export class LookupService {
    constructor(private http: HttpClient, private settings: Settings) {
    }

    private sendRequest(url: string): Promise<any[]> {
        return this.http.createRequest(url).asGet().send().then(
            data => {
                if (data.statusCode === 200) {
                    var obj: any = JSON.parse(data.response);
                    console.log(obj.value);
                    return obj.value;
                }

                return data;
            });
    }

    getApplications(): Promise<LookupEntity[]> {
        let url = `${this.settings.serviceBaseUrl}Applications?$select=id,name`;

        return this.sendRequest(url);
    }

    getApplicationManager(): Promise<LookupEntity[]> {
        let url = `${this.settings.serviceBaseUrl}ApplicationManagers?$select=id,name`;

        return this.sendRequest(url);
    }

    getLocations(): Promise<LookupEntity[]> {
        let url = `${this.settings.serviceBaseUrl}Locations?$select=id,name`;

        return this.sendRequest(url);
    }

    getPrincipalTypes(): Promise<LookupEntity[]> {
        let url = `${this.settings.serviceBaseUrl}PrincipalTypes?$select=id,name`;

        return this.sendRequest(url);
    }

    getSetupPathTypes(): Promise<LookupEntity[]> {
        let url = `${this.settings.serviceBaseUrl}PrincipalTypes?$select=id,name`;

        return this.sendRequest(url);
    }
}
