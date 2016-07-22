/// <reference path="../../typings/index.d.ts" />

import {inject} from "aurelia-framework";
import {Router} from "aurelia-router";

@inject(Router)
export class App {
    router: Router;
    navItems: INavItem[];

    constructor() {
        this.navItems = [
            { isActive: true, link: "#/applications", title: "Applications" },
            { isActive: false, link: "#/application-managers", title: "Application Managers" }
        ];
    }

    configureRouter(config, router: Router) {
        this.router = router;

        config.title = "Software Update Manager";
        config.map([
            { route: ["", "applications"], moduleId: "./applications/applications-section", nav: true, title: "Applications" },
            { route: "application-managers", moduleId: "./application-managers/application-managers-section", nav: true, title: "Application Managers" },
        ]);
    }

    activeNavItemChanged(activeNavLink) {
        for (let index = 0; index < this.navItems.length; index++) {
            let navItem = this.navItems[index];
            navItem.isActive = navItem.link === activeNavLink;
        }
        this.router.navigate(activeNavLink);
    }

}

