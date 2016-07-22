import {EntityViewModel} from '../shared/entity-view-model';
import {ApplicationService, LookupService} from "../services/index";
import {autoinject, computedFrom} from 'aurelia-framework';
import {ApplicationEntity} from "../resources/models/index"
import {Router} from 'aurelia-router';
import {DeepObserver} from "../resources/index";

@autoinject
export class Application extends EntityViewModel {
    availableManagers: LookupEntity[];
    entity: ApplicationEntity;

    constructor(private appliactionService: ApplicationService, private lookupService: LookupService, router: Router, observer: DeepObserver) {
        super(appliactionService, router, observer);

        this.lookupService.getApplicationManager().then(data =>
            this.availableManagers = data
        );
    }

    @computedFrom('entity.id', 'entity.name')
    get title() {
        if (this.entity.id <= 0) {
            return "New Application";
        }

        return `Application #${this.entity.id}`;
    }
}