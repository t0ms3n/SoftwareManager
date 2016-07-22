import {autoinject} from "aurelia-framework";
import {DialogController} from "aurelia-dialog";

@autoinject
export class LookupSelectionDialog {
    items = [];
    title;

    result = { selecteItem: {} };

    constructor(public controller: DialogController) {

    }

    activate(data) {
        this.items = data.items;
        this.title = data.title;
    }
}