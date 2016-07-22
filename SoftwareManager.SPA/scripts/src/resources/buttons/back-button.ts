import {inject, customElement, singleton} from "aurelia-framework";
import {Router} from "aurelia-router";

@inject(Router)
@singleton()
@customElement('back-button')
export class BackButton {

    constructor(private router: Router) {
    
    }

    back() {
        this.router.navigateBack();
    }
}