System.register(["aurelia-framework", "aurelia-dialog"], function(exports_1, context_1) {
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
    var aurelia_framework_1, aurelia_dialog_1;
    var LookupSelectionDialog;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_dialog_1_1) {
                aurelia_dialog_1 = aurelia_dialog_1_1;
            }],
        execute: function() {
            LookupSelectionDialog = (function () {
                function LookupSelectionDialog(controller) {
                    this.controller = controller;
                    this.items = [];
                    this.result = { selecteItem: {} };
                }
                LookupSelectionDialog.prototype.activate = function (data) {
                    this.items = data.items;
                    this.title = data.title;
                };
                LookupSelectionDialog = __decorate([
                    aurelia_framework_1.autoinject, 
                    __metadata('design:paramtypes', [aurelia_dialog_1.DialogController])
                ], LookupSelectionDialog);
                return LookupSelectionDialog;
            }());
            exports_1("LookupSelectionDialog", LookupSelectionDialog);
        }
    }
});
//# sourceMappingURL=lookup-selection-dialog.js.map