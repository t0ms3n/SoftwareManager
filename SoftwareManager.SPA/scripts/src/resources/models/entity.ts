export abstract class Entity implements odata.entities.IEntity {
    "@odata.type": string;
    "@odata.etag": string;
    id: number;
    rowVersion: any[];

    constructor(type: string) {
        this.id = 0;
        this["@odata.type"] = "SoftwareManager.BLL.Contracts.Models." + type;
    }
}

