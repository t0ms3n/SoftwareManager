import {Entity, ApplicationApplicationManagerEntity} from "./index"


export class ApplicationManagerEntity extends Entity implements odata.entities.IApplicationManagerEntity {
    name: string;
    loginName: string;
    isActive: boolean;
    isAdmin: boolean;
    createdApplications: odata.entities.IApplicationEntity[];
    modifiedApplications: odata.entities.IApplicationEntity[];
    createdApplicationApplicationManagers: odata.entities.IApplicationApplicationManagerEntity[];
    modifiedApplicationApplicationManagers: odata.entities.IApplicationApplicationManagerEntity[];
    applications: odata.entities.IApplicationApplicationManagerEntity[];
    createdApplicationVersions: odata.entities.IApplicationVersionEntity[];
    modifiedApplicationVersions: odata.entities.IApplicationVersionEntity[];

    constructor() {
        super("ApplicationManager");
    }

    static createFromObject(obj: any, parent: any): ApplicationManagerEntity {
        let entity = new ApplicationManagerEntity();
        Object.assign(entity, obj);

        //Map custom instances
        for (let itemIndex in entity.applications) {
            if (entity.applications.hasOwnProperty(itemIndex)) {
                let item = entity.applications[itemIndex];
                entity.applications[itemIndex] = ApplicationApplicationManagerEntity.createFromObject(item, entity);
            }
        }

        return entity;
    }
    
}
