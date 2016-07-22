System.register([], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var EntityViewModel;
    return {
        setters:[],
        execute: function() {
            EntityViewModel = (function () {
                function EntityViewModel(service, router, observer) {
                    var _this = this;
                    this.router = router;
                    this.observer = observer;
                    this.changedEntityProperties = [];
                    this.service = service;
                    this.router = router;
                    // ChangeTracking der Properties der Entity
                    this.observerDispose = this.observer.observe(this, "entity", function (n, o, p) {
                        var property = p.replace("entity.", "");
                        if (property !== "entity" && _this.changedEntityProperties.indexOf(property) < 0) {
                            _this.changedEntityProperties.push(property);
                        }
                    });
                }
                EntityViewModel.prototype.detached = function () {
                    if (this.observerDispose)
                        this.observerDispose();
                };
                EntityViewModel.prototype.activate = function (info) {
                    var _this = this;
                    return this.loadOrCreateNew(info.id).then(function (result) {
                        _this.entity = result;
                    });
                };
                EntityViewModel.prototype.hasEntityChanges = function () {
                    return this.changedEntityProperties.length > 0;
                };
                Object.defineProperty(EntityViewModel.prototype, "isExisting", {
                    get: function () {
                        return this.entity.id > 0;
                    },
                    enumerable: true,
                    configurable: true
                });
                EntityViewModel.prototype.delete = function () {
                    var _this = this;
                    var result = this.service.deleteEntity(this.entity);
                    result.then(function (res) {
                        if (res) {
                            _this.router.navigate("");
                        }
                    });
                };
                Object.defineProperty(EntityViewModel.prototype, "canRevert", {
                    get: function () {
                        return this.entity.id > 0 && this.hasEntityChanges();
                    },
                    enumerable: true,
                    configurable: true
                });
                EntityViewModel.prototype.revert = function () {
                    var _this = this;
                    return this.loadOrCreateNew(this.entity.id).then(function (result) {
                        _this.entity = result;
                        _this.resetChangeTracking();
                    });
                };
                EntityViewModel.prototype.save = function () {
                    var _this = this;
                    var promise;
                    if (this.entity.id <= 0)
                        promise = this.service.insertEntity(this.entity);
                    else
                        promise = this.service.updateEntity(this.entity, this.changedEntityProperties);
                    promise.then(function (result) {
                        if (result["statusCode"]) {
                            var statusCode = result.statusCode;
                            if (statusCode === 204) {
                                _this.resetChangeTracking();
                            }
                            else if (statusCode === 412) {
                                _this.revert();
                            }
                        }
                        else if (result["id"]) {
                            _this.router.navigate(result["id"]);
                        }
                    });
                };
                EntityViewModel.prototype.loadOrCreateNew = function (id) {
                    var promise;
                    if (id > 0) {
                        promise = this.service.loadExisting(id);
                    }
                    else {
                        promise = this.service.createNew();
                    }
                    return promise;
                };
                EntityViewModel.prototype.resetChangeTracking = function () {
                    // ChangeTracking zurücksetzen
                    this.changedEntityProperties.splice(0);
                };
                return EntityViewModel;
            }());
            exports_1("EntityViewModel", EntityViewModel);
        }
    }
});
//# sourceMappingURL=entity-view-model.js.map