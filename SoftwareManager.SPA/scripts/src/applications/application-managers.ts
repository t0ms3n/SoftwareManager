import {EntityViewModel} from '../shared/entity-view-model';
import {ApplicationService, ApplicationManagerService} from "../services/index";
import {autoinject, computedFrom} from 'aurelia-framework';
import {ApplicationApplicationManagerEntity, ApplicationManagerEntity} from "../resources/models/index"
import {ApplicationManagerViewModel} from "./application-manager-vm"
import {DialogService} from "aurelia-dialog";
import {LookupSelectionDialog} from "../shared/dialogs/lookup-selection-dialog";


@autoinject
export class ApplicatioManagersViewModel {
    availableManagers: ApplicationManagerEntity[];
    entities: ApplicationManagerViewModel[] = [];
    selectionResult;

    constructor(private appliactionService: ApplicationService, private lookupService: ApplicationManagerService, private dialogService: DialogService) {
        this.lookupService.getApplicationManagers().then(data =>
            this.availableManagers = data
        );
    }

    activate(managerEntities) {
        if (typeof managerEntities !== "undefined" && managerEntities != null) {
            this.entities.splice(0);
            managerEntities.forEach((value, index, arr) => this.entities.push(new ApplicationManagerViewModel(value)));
        }
    }



    addManager() {
        this.selectManager().then((item) => {
            var newItem = new ApplicationApplicationManagerEntity();
            newItem.applicationManager = item;
            newItem.applicationManagerId = item.id;
            let vm = new ApplicationManagerViewModel(newItem);
            vm.shouldBeAdded = true;
            this.entities.push(vm);
        }, (error) => {
            console.log(error);
        });
    }

    selectManager(): Promise<any> {
        let promise: Promise<any>;
        let items = [];
        for (let index in this.availableManagers) {
            if (this.availableManagers.hasOwnProperty(index)) {
                let managerId = this.availableManagers[index].id;

                var found = this.entities.find((value) => {
                    return value.managerId === managerId;
                });
                if (typeof found == "undefined") {
                    items.push(this.availableManagers[index]);
                }
            }
        }

        if (items.length > 0) {
            promise = this.dialogService.open({ viewModel: LookupSelectionDialog, model: { title: "Select an application manager", items: items } }).then(response => {
                if (!response.wasCancelled) {
                    return response.output.selectedItem;
                }
                return "The selection was canceled";
            });
        } else {
            promise = Promise.reject("All managers have already been assigned");
        }

        return promise;
    }

    managerSelected() {
        console.log(this.selectionResult);
    }

    deleteManager(manager: ApplicationManagerViewModel) {
        if (manager.shouldBeAdded) {
            let index = this.entities.indexOf(manager);
            this.entities.splice(index, 1);
        } else {
            manager.shouldBeDeleted = true;
        }
    }

}