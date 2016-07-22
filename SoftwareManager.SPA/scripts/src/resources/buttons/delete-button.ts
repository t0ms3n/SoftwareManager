
import {autoinject, bindable} from "aurelia-framework";
import {DialogService} from "aurelia-dialog";
import {ConfirmDialog} from "../../shared/dialogs/confirm-dialog";

@autoinject
export class DeleteButton {

    @bindable
    action = () => { };

    @bindable
    msg = "Are you sure";

    constructor(public dlg: DialogService) {

    }

    do() {
        this.dlg.open({
            viewModel: ConfirmDialog,
            model: this.msg
        }).then(result => {
            if (result.wasCancelled) return;
            this.action();
        });
    }
}
