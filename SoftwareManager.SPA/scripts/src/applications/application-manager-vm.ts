import {ApplicationApplicationManagerEntity, ApplicationManagerEntity} from "../resources/models/index"

export class ApplicationManagerViewModel {
    private entity: ApplicationApplicationManagerEntity;
    shouldBeDeleted = false;
    shouldBeAdded = false;

    constructor(entity: ApplicationApplicationManagerEntity) {
        this.entity = entity;
    }

    get id() {
        return this.entity.id;
    }

    get managerId() {
        return this.entity.applicationManagerId;
    }

    get managerName() {
        return this.entity.applicationManager.name;
    }

    get managerLoginName() {
        return this.entity.applicationManager.loginName;
    }

    setManager(lookupEntity: ApplicationManagerEntity) {
        this.entity.applicationManagerId = lookupEntity.id;
        this.entity.applicationManager = lookupEntity;
    }

    activate() {

    }

}