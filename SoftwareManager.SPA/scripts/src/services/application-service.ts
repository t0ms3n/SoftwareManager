import {Settings} from '../resources/index';
import {ODataBaseService} from './index';
import {HttpClient} from 'aurelia-http-client';
import {autoinject} from 'aurelia-framework';
import {ApplicationEntity, ApplicationApplicationManagerEntity, ApplicationManagerEntity} from "../resources/models/index"

@autoinject
export class ApplicationService extends ODataBaseService {
    constructor(protected http: HttpClient, private settings: Settings) {
        super(http);
    }

    private convertToResult(obj): any {
        let result: any;
        if (Array.isArray(obj)) {
            result = [];
            for (var property in obj) {
                if (obj.hasOwnProperty(property)) {
                    let value = obj[property];
                    if (value == null)
                        continue;
                    result.push(ApplicationEntity.createFromObject(value, null));
                }
            }
        } else {
            result = ApplicationEntity.createFromObject(obj, null);
        }
        console.log("Converted from", obj, "to", result);
        return result;
    }

    getApplications(): Promise<any[]> {
        let url = `${this.settings.serviceBaseUrl}Applications`;

        return this.createGetRequest(url).then(data => {
            if (data.statusCode === 200) {
                let obj = JSON.parse(data.response);
                return this.convertToResult(obj.value);
            }
            return data;
        });
    }

    getApplication(applicationId: number): Promise<any> {
        let url = `${this.settings.serviceBaseUrl}Applications(${applicationId})?$expand=applicationApplicationManagers($expand=applicationManager($select=name,loginName))`;
        return this.createGetRequest(url).then(data => {
            if (data.statusCode === 200) {
                let obj = JSON.parse(data.response);
                return this.convertToResult(obj);
            }
            return data;
        });
    }

    deleteEntity(entity: ApplicationEntity): Promise<boolean> {
        if (entity.id > 0) {
            let url = `${this.settings.serviceBaseUrl}Applications(${entity.id})`;

            return this.createDeleteRequest(url, entity["@odata.etag"]).then(response => {
                if (response.statusCode === 204) {
                    // No Content -> Success
                    return true;
                }
                return false;
            });
        }

        return Promise.resolve(false);
    }

    insertEntity(entity: ApplicationEntity) {
        let url = `${this.settings.serviceBaseUrl}Applications`;
        let content = JSON.stringify(entity);

        return this.createPostRequest(url, content).then(response => {
            if (response.statusCode === 201) {
                let obj = JSON.parse(response.response);
                console.log(obj);
                return obj;
            }

            return response;
        });
    }

    updateEntity(entity: ApplicationEntity, changedProperties: string[]) {
        if (changedProperties.length === 0)
            return <Promise<any>>Promise.resolve(entity);

        let url = `${this.settings.serviceBaseUrl}Applications(${entity.id})`;
        let patchItem = {};
        changedProperties.forEach((value) => {
            patchItem[value] = entity[value];
        });

        let content = JSON.stringify(patchItem);

        return this.createPatchRequest(url, content, entity["@odata.etag"]);
    }

    // Factory?
    createNew() {
        let item = new ApplicationEntity();
        return Promise.resolve(item);
    }

    loadExisting(id: number) {
        return this.getApplication(id);
    }
}
