import {Entity} from "./index"

export abstract  class DateTrackedEntity extends Entity implements odata.entities.IDateTrackedEntity {
        createDate: Date;
        createById: number;
        createBy: odata.entities.IApplicationManagerEntity;
        modifyDate: Date;
        modifyById: number;
        modifyBy: odata.entities.IApplicationManagerEntity;
    }
