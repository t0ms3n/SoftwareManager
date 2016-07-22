import {inject} from "aurelia-framework";
import {Router} from "aurelia-router";

@inject(Router)
export class ApplicationsSection {
    private router: Router;

    constructor(private routerConfig: Router) {
    }

    configureRouter(config, router: Router) {
        this.router = router;

        config.map([
            { route: '', moduleId: "./application-list", nav: false, title: "" },
            { route: ':id', moduleId: "./application", nav: false, title: "" }
        ]);
    }
}