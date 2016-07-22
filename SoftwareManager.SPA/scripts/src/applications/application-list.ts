import {ListViewModel} from "../shared/list-view-model";
import {ApplicationService} from "../services/index"
import {AppRouter} from 'aurelia-router';
import {autoinject} from 'aurelia-framework';

@autoinject()
export class ApplicationList extends ListViewModel {
    applications = [];

    constructor(private applicationService: ApplicationService, router: AppRouter) {
        super("applications", router, applicationService);
    }  

    activate(params) {
        this.applicationService.getApplications().then(data => {
            this.applications = data;
        });
    }
   
}