
import {autoinject, bindable} from "aurelia-framework";
import {DialogService} from "aurelia-dialog";
import {LookupSelectionDialog} from "../../shared/dialogs/lookup-selection-dialog";

@autoinject
export class LookupSelectionButton {
    @bindable
    title = "Select an item";

    @bindable
    action = () => { };

    @bindable
    items = [];

    constructor(public dlg: DialogService) {

    }

    do() {
        this.dlg.open({
            viewModel: LookupSelectionDialog,
            model: { items: this.items, title: this.title }
        }).then(result => {
            if (result.wasCancelled) return;
            this.action();
        });
    }
}