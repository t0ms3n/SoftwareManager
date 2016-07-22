import {DateTrackedEntity, ApplicationManagerEntity, ApplicationEntity} from "./index"

export class ApplicationApplicationManagerEntity extends DateTrackedEntity implements odata.entities.IApplicationApplicationManagerEntity {
    applicationId: number;
    application: odata.entities.IApplicationEntity;
    applicationManagerId: number;
    applicationManager: odata.entities.IApplicationManagerEntity;
    constructor() {
        super("ApplicationApplicationManager");
    }

    static createFromObject(obj: any, parent: any): ApplicationApplicationManagerEntity {
        let entity = new ApplicationApplicationManagerEntity();
        Object.assign(entity, obj);

        //Map custom instances
        if (parent !== undefined && parent !== null && parent.isPrototypeOf(ApplicationEntity)) {
            entity.application = parent;
        } else {
            entity.application = ApplicationEntity.createFromObject(obj.application, entity);
        }

        if (parent !== undefined && parent !== null && parent.isPrototypeOf(ApplicationManagerEntity)) {
            entity.applicationManager = parent;
        } else {
            entity.applicationManager = ApplicationManagerEntity.createFromObject(obj.applicationManager, entity);
        }
        
        return entity;
    }

}
