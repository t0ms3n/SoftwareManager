import {autoinject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';
import {ODataBaseService} from './index';
import {Settings} from '../resources/index';
import {ApplicationManagerEntity} from "../resources/models/index"

@autoinject
export class ApplicationManagerService extends ODataBaseService {
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
                    result.push(ApplicationManagerEntity.createFromObject(value, null));
                }
            }
        } else {
            result = ApplicationManagerEntity.createFromObject(obj, null);
        }
        console.log("Converted from", obj, "to" , result);
        return result;
    }

    getApplicationManagers(): Promise<any[]> {
        var url = `${this.settings.serviceBaseUrl}ApplicationManagers`;

        return this.createGetRequest(url).then(
            data => {
                if (data.statusCode === 200) {
                    var obj: any = JSON.parse(data.response);
                    console.log(obj.value);
                    return this.convertToResult(obj.value);
                }

                return data;
            });;
    }

    getApplicationManager(id: number): Promise<any> {
        let url = `${this.settings.serviceBaseUrl}ApplicationManagers(${id})?$expand=applications($expand=application($select=id,name))`;

        return this.createGetRequest(url).then(
            data => {
                if (data.statusCode === 200) {
                    let obj = JSON.parse(data.response);
                    console.log(obj);
                    return this.convertToResult(obj);
                }

                return data;
            });
    }

    deleteEntity(entity: ApplicationManagerEntity): Promise<boolean> {
        if (entity.id > 0) {
            let url = `${this.settings.serviceBaseUrl}ApplicationManagers(${entity.id})`;

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

    insertEntity(entity: ApplicationManagerEntity): Promise<any> {
        let url = `${this.settings.serviceBaseUrl}ApplicationManagers`;
        let content = JSON.stringify(entity);

        return this.createPostRequest(url, content).then(response => {
            if (response.statusCode === 201) {
                let obj = JSON.parse(response.response);
                console.log(obj);
                return this.convertToResult(obj);
            }

            return response;
        });
    }

    updateEntity(entity: ApplicationManagerEntity, changedProperties: string[]) {
        if (changedProperties.length === 0)
            return <Promise<any>>Promise.resolve(entity);

        let url = `${this.settings.serviceBaseUrl}ApplicationManagers(${entity.id})`;
        let patchItem = {};
        changedProperties.forEach((value) => {
            patchItem[value] = entity[value];
        });

        let content = JSON.stringify(patchItem);

        return this.createPatchRequest(url, content, entity["@odata.etag"]);
    }

    // Factory?
    createNew() {
        let item = new ApplicationManagerEntity();
        return Promise.resolve(item);
    }

    loadExisting(id: number) {
        return this.getApplicationManager(id);
    }
}
