System.register(["../services/index", 'aurelia-framework', "../resources/models/index", "./application-manager-vm", "aurelia-dialog", "../shared/dialogs/lookup-selection-dialog"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var index_1, aurelia_framework_1, index_2, application_manager_vm_1, aurelia_dialog_1, lookup_selection_dialog_1;
    var ApplicatioManagersViewModel;
    return {
        setters:[
            function (index_1_1) {
                index_1 = index_1_1;
            },
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (index_2_1) {
                index_2 = index_2_1;
            },
            function (application_manager_vm_1_1) {
                application_manager_vm_1 = application_manager_vm_1_1;
            },
            function (aurelia_dialog_1_1) {
                aurelia_dialog_1 = aurelia_dialog_1_1;
            },
            function (lookup_selection_dialog_1_1) {
                lookup_selection_dialog_1 = lookup_selection_dialog_1_1;
            }],
        execute: function() {
            ApplicatioManagersViewModel = (function () {
                function ApplicatioManagersViewModel(appliactionService, lookupService, dialogService) {
                    var _this = this;
                    this.appliactionService = appliactionService;
                    this.lookupService = lookupService;
                    this.dialogService = dialogService;
                    this.entities = [];
                    this.lookupService.getApplicationManagers().then(function (data) {
                        return _this.availableManagers = data;
                    });
                }
                ApplicatioManagersViewModel.prototype.activate = function (managerEntities) {
                    var _this = this;
                    if (typeof managerEntities !== "undefined" && managerEntities != null) {
                        this.entities.splice(0);
                        managerEntities.forEach(function (value, index, arr) { return _this.entities.push(new application_manager_vm_1.ApplicationManagerViewModel(value)); });
                    }
                };
                ApplicatioManagersViewModel.prototype.addManager = function () {
                    var _this = this;
                    this.selectManager().then(function (item) {
                        var newItem = new index_2.ApplicationApplicationManagerEntity();
                        newItem.applicationManager = item;
                        newItem.applicationManagerId = item.id;
                        var vm = new application_manager_vm_1.ApplicationManagerViewModel(newItem);
                        vm.shouldBeAdded = true;
                        _this.entities.push(vm);
                    }, function (error) {
                        console.log(error);
                    });
                };
                ApplicatioManagersViewModel.prototype.selectManager = function () {
                    var promise;
                    var items = [];
                    var _loop_1 = function(index) {
                        if (this_1.availableManagers.hasOwnProperty(index)) {
                            var managerId_1 = this_1.availableManagers[index].id;
                            found = this_1.entities.find(function (value) {
                                return value.managerId === managerId_1;
                            });
                            if (typeof found == "undefined") {
                                items.push(this_1.availableManagers[index]);
                            }
                        }
                    };
                    var this_1 = this;
                    var found;
                    for (var index in this.availableManagers) {
                        _loop_1(index);
                    }
                    if (items.length > 0) {
                        promise = this.dialogService.open({ viewModel: lookup_selection_dialog_1.LookupSelectionDialog, model: { title: "Select an application manager", items: items } }).then(function (response) {
                            if (!response.wasCancelled) {
                                return response.output.selectedItem;
                            }
                            return "The selection was canceled";
                        });
                    }
                    else {
                        promise = Promise.reject("All managers have already been assigned");
                    }
                    return promise;
                };
                ApplicatioManagersViewModel.prototype.managerSelected = function () {
                    console.log(this.selectionResult);
                };
                ApplicatioManagersViewModel.prototype.deleteManager = function (manager) {
                    if (manager.shouldBeAdded) {
                        var index = this.entities.indexOf(manager);
                        this.entities.splice(index, 1);
                    }
                    else {
                        manager.shouldBeDeleted = true;
                    }
                };
                ApplicatioManagersViewModel = __decorate([
                    aurelia_framework_1.autoinject, 
                    __metadata('design:paramtypes', [index_1.ApplicationService, index_1.ApplicationManagerService, aurelia_dialog_1.DialogService])
                ], ApplicatioManagersViewModel);
                return ApplicatioManagersViewModel;
            }());
            exports_1("ApplicatioManagersViewModel", ApplicatioManagersViewModel);
        }
    }
});
//# sourceMappingURL=application-managers.js.map