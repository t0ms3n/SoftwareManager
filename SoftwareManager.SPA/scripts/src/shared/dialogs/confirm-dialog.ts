import {autoinject} from "aurelia-framework";
import {DialogController} from "aurelia-dialog";

@autoinject
export class ConfirmDialog {

    constructor(public controller: DialogController) {

    }
    message = "";
    activate(data) {
        this.message = data;
    }
}