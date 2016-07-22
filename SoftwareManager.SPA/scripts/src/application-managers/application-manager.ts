import {EntityViewModel} from '../shared/entity-view-model';
import {ApplicationManagerService} from "../services/index";
import {autoinject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {DeepObserver} from "../resources/index";


@autoinject
export class ApplicationManager extends EntityViewModel {
    entity: odata.entities.IApplicationManagerEntity;

    constructor(private applicationManagerService: ApplicationManagerService, observer: DeepObserver, router : Router) {
        super(applicationManagerService,router, observer);
    }

    get title() {
        if (this.entity.id <= 0) {
            return "New Application Manager";
        }

        return `Application Manager #${this.entity.id}`;
    }
}