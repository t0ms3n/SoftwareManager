System.register([], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var ListViewModel;
    return {
        setters:[],
        execute: function() {
            ListViewModel = (function () {
                function ListViewModel(route, router, service) {
                    this.route = route;
                    this.router = router;
                    this.service = service;
                }
                ListViewModel.prototype.open = function (id) {
                    this.router.navigate(this.route + "/" + id);
                };
                return ListViewModel;
            }());
            exports_1("ListViewModel", ListViewModel);
        }
    }
});
//# sourceMappingURL=list-view-model.js.map