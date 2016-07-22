import {ListViewModel} from "../shared/list-view-model";
import {ApplicationManagerService} from "../services/index"
import {AppRouter} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';
import {ApplicationManagerEntity} from "../resources/models/index"

@autoinject()
export class ApplicationList extends ListViewModel {
    applicationManagers : ApplicationManagerEntity[] = [];

    constructor(private applicationManagerService: ApplicationManagerService, router: AppRouter) {
        super("application-managers", router, applicationManagerService);
    }

    activate(params) {
        this.applicationManagerService.getApplicationManagers().then(data => {
            this.applicationManagers = data;
        });
    }

}