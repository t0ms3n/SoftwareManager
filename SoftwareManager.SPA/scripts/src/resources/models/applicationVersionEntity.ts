import {DateTrackedEntity} from "./index"
export class ApplicationVersionEntity extends DateTrackedEntity implements odata.entities.IApplicationVersionEntity {
        applicationId: number;
        application: odata.entities.IApplicationEntity;
        isActive: boolean;
        isCurrent: boolean;
        isUpdateRequired: boolean;
        versionNumber: string;
        releaseDate: Date;

        constructor() {
            super("ApplicationVersion");
        }
    }
