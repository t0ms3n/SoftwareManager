import {DateTrackedEntity, ApplicationApplicationManagerEntity, ApplicationVersionEntity} from "./index"

export class ApplicationEntity extends DateTrackedEntity implements odata.entities.IApplicationEntity {
    name: string;
    applicationIdentifier: any;
    applicationApplicationManagers: odata.entities.IApplicationApplicationManagerEntity[];
    applicationVersions: odata.entities.IApplicationVersionEntity[];

    constructor() {
        super("Application");
    }

    static createFromObject(obj: any, parent: any): ApplicationEntity {
        let entity = new ApplicationEntity();
        Object.assign(entity, obj);

        //Map custom instances
        for (let itemIndex in entity.applicationApplicationManagers) {
            if (entity.applicationApplicationManagers.hasOwnProperty(itemIndex)) {
                let item = entity.applicationApplicationManagers[itemIndex];
                entity.applicationApplicationManagers[itemIndex] = ApplicationApplicationManagerEntity.createFromObject(item, entity);
            }
        }

        //for (let itemIndex in entity.applicationVersions) {
        //    if (entity.applicationVersions.hasOwnProperty(itemIndex)) {
        //        let item = entity.applicationVersions[itemIndex];
        //        entity.applicationVersions[itemIndex] = ApplicationVersionEntity.createFromObject(item, null);
        //    }
        //}

        return entity;
    }

    
}