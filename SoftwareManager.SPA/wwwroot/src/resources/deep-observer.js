/**
 * Created by Jason on 29/02/2016.
 *
 * constructor(public deepobserver:DeepObserver) {
 *   this.obsDisposer = this.deepobserver.observe(this, 'target', (n,o,p)=>
 *                                                    { console.log('DATA CHANGED:', p, ':', o,'===>', n ); }
 *  }
 *  @bindable target:Object;
 *
 *  To remove bindings, call this.obsDisposer();
 * https://gist.github.com/jsobell/6240c0ba3da55214b5bb
 */
System.register(["aurelia-framework", "aurelia-dependency-injection"], function(exports_1, context_1) {
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
    var aurelia_framework_1, aurelia_dependency_injection_1;
    var DeepObserver;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_dependency_injection_1_1) {
                aurelia_dependency_injection_1 = aurelia_dependency_injection_1_1;
            }],
        execute: function() {
            DeepObserver = (function () {
                function DeepObserver(bindingEngine) {
                    this._bindingEngine = bindingEngine;
                }
                DeepObserver.prototype.observe = function (target, property, callback) {
                    var _this = this;
                    var subscriptions = { root: null, children: [] };
                    subscriptions.root = (this._bindingEngine.propertyObserver(target, property)
                        .subscribe(function (n, o) {
                        _this.disconnect(subscriptions.children);
                        var path = property;
                        _this.recurse(target, property, subscriptions.children, callback, path);
                    }));
                    return function () { _this.disconnect(subscriptions.children); subscriptions.root.dispose(); };
                };
                DeepObserver.prototype.disconnect = function (subscriptions) {
                    while (subscriptions.length) {
                        subscriptions.pop().dispose();
                    }
                };
                DeepObserver.prototype.recurse = function (target, property, subscriptions, callback, path) {
                    var sub = target[property];
                    if (typeof sub === "object") {
                        for (var p in sub)
                            if (sub.hasOwnProperty(p)) {
                                this.recurse(sub, p, subscriptions, callback, "" + path + (sub instanceof Array ? '[' + p + ']' : '.' + p));
                            }
                    }
                    if (target != property) {
                        subscriptions.push(this._bindingEngine.propertyObserver(target, property).subscribe(function (n, o) { return callback(n, o, path); }));
                    }
                };
                ;
                DeepObserver = __decorate([
                    aurelia_dependency_injection_1.autoinject(), 
                    __metadata('design:paramtypes', [aurelia_framework_1.BindingEngine])
                ], DeepObserver);
                return DeepObserver;
            }());
            exports_1("DeepObserver", DeepObserver);
        }
    }
});
//# sourceMappingURL=deep-observer.js.map