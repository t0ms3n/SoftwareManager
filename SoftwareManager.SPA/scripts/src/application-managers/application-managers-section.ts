import {inject} from "aurelia-framework";
import {Router} from "aurelia-router";

@inject(Router)
export class ApplicationManagersSection {
    private router: Router;

    constructor(private routerConfig: Router) {
    }

    configureRouter(config, router: Router) {
        this.router = router;

        config.map([
            { route: '', moduleId: "./application-manager-list", nav: false, title: "" },
            { route: ':id', moduleId: "./application-manager", nav: false, title: "" }
        ]);
    }
}