declare module odata.entities {

    export interface IApplicationEntity extends IDateTrackedEntity {
        name: string;
        identifier: any;
        applicationApplicationManagers: IApplicationApplicationManagerEntity[];
        applicationVersions: IApplicationVersionEntity[];
    }

    export interface IApplicationApplicationManagerEntity extends IDateTrackedEntity {
        applicationId: number;
        application: IApplicationEntity;
        applicationManagerId: number;
        applicationManager: IApplicationManagerEntity;
    }

    export interface IApplicationManagerEntity extends IEntity {
        name: string;
        loginName: string;
        isActive: boolean;
        isAdmin: boolean;
        createdApplications: IApplicationEntity[];
        modifiedApplications: IApplicationEntity[];
        createdApplicationApplicationManagers: IApplicationApplicationManagerEntity[];
        modifiedApplicationApplicationManagers: IApplicationApplicationManagerEntity[];
        applications: IApplicationApplicationManagerEntity[];
        createdApplicationVersions: IApplicationVersionEntity[];
        modifiedApplicationVersions: IApplicationVersionEntity[];
    }

    export interface IApplicationVersionEntity extends IDateTrackedEntity {
        applicationId: number;
        application: IApplicationEntity;
        isActive: boolean;
        isCurrent: boolean;
        isUpdateRequired: boolean;
        versionNumber: string;
        releaseDate: Date;
    }
    
    export interface IDateTrackedEntity extends IEntity {
        createDate: Date;
        createById: number;
        createBy: IApplicationManagerEntity;
        modifyDate: Date;
        modifyById: number;
        modifyBy: IApplicationManagerEntity;
    }

    export interface IEntity {
        id: number;
        rowVersion: any[];
    }
}