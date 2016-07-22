import {computedFrom} from "aurelia-framework"
import {Router} from "aurelia-router";
import {HttpResponseMessage} from "aurelia-http-client";
import {DeepObserver} from "../resources/index";

export abstract class EntityViewModel {
    service: any;
    entity: any;

    private observerDispose: any;
    changedEntityProperties: string[] = [];

    constructor(service, private router: Router, private observer: DeepObserver) {
        this.service = service;
        this.router = router;

        // ChangeTracking der Properties der Entity
        this.observerDispose = this.observer.observe(this, "entity", (n, o, p) => {
            let property = p.replace("entity.", "");
            if (property !== "entity" && this.changedEntityProperties.indexOf(property) < 0) {
                this.changedEntityProperties.push(property);
            }
        });
    }

    detached() {
        if (this.observerDispose)
            this.observerDispose();
    }

    activate(info) {
        return this.loadOrCreateNew(info.id).then(result => {
            this.entity = result;
        });
    }

    hasEntityChanges() {
        return this.changedEntityProperties.length > 0;
    }


    get isExisting() {
        return this.entity.id > 0;
    }

    delete() {
        let result: Promise<Boolean> = this.service.deleteEntity(this.entity);
        result.then(res => {
            if (res) {
                this.router.navigate("");
            }
        });
    }


    get canRevert() {
        return this.entity.id > 0 && this.hasEntityChanges();
    }

    revert() {
        return this.loadOrCreateNew(this.entity.id).then(result => {
            this.entity = result;
            this.resetChangeTracking();
        });
    }

    save() {
        let promise: Promise<any>;
        if (this.entity.id <= 0)
            promise = this.service.insertEntity(this.entity);
        else
            promise = this.service.updateEntity(this.entity, this.changedEntityProperties);

        promise.then(result => {
            if (result["statusCode"]) {
                let statusCode = (<HttpResponseMessage>result).statusCode;
                if (statusCode === 204) {
                    this.resetChangeTracking();
                } else if (statusCode === 412) {
                    this.revert();
                }
            }
            else if (result["id"]) {
                this.router.navigate(result["id"]);
                //this.entity = result;
                //this.resetChangeTracking();
            }
        });
    }

    private loadOrCreateNew(id: any): Promise<any> {
        let promise;
        if (id > 0) {
            promise = this.service.loadExisting(id);
        } else {
            promise = this.service.createNew();
        }
        return promise;
    }

    private resetChangeTracking() {
        // ChangeTracking zurücksetzen
        this.changedEntityProperties.splice(0);
    }
}