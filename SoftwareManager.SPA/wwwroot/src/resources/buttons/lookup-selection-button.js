System.register(["aurelia-framework", "aurelia-dialog", "../../shared/dialogs/lookup-selection-dialog"], function(exports_1, context_1) {
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
    var aurelia_framework_1, aurelia_dialog_1, lookup_selection_dialog_1;
    var LookupSelectionButton;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_dialog_1_1) {
                aurelia_dialog_1 = aurelia_dialog_1_1;
            },
            function (lookup_selection_dialog_1_1) {
                lookup_selection_dialog_1 = lookup_selection_dialog_1_1;
            }],
        execute: function() {
            LookupSelectionButton = (function () {
                function LookupSelectionButton(dlg) {
                    this.dlg = dlg;
                    this.title = "Select an item";
                    this.action = function () { };
                    this.items = [];
                }
                LookupSelectionButton.prototype.do = function () {
                    var _this = this;
                    this.dlg.open({
                        viewModel: lookup_selection_dialog_1.LookupSelectionDialog,
                        model: { items: this.items, title: this.title }
                    }).then(function (result) {
                        if (result.wasCancelled)
                            return;
                        _this.action();
                    });
                };
                __decorate([
                    aurelia_framework_1.bindable, 
                    __metadata('design:type', Object)
                ], LookupSelectionButton.prototype, "title", void 0);
                __decorate([
                    aurelia_framework_1.bindable, 
                    __metadata('design:type', Object)
                ], LookupSelectionButton.prototype, "action", void 0);
                __decorate([
                    aurelia_framework_1.bindable, 
                    __metadata('design:type', Object)
                ], LookupSelectionButton.prototype, "items", void 0);
                LookupSelectionButton = __decorate([
                    aurelia_framework_1.autoinject, 
                    __metadata('design:paramtypes', [aurelia_dialog_1.DialogService])
                ], LookupSelectionButton);
                return LookupSelectionButton;
            }());
            exports_1("LookupSelectionButton", LookupSelectionButton);
        }
    }
});
//# sourceMappingURL=lookup-selection-button.js.map