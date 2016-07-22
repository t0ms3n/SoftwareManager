export class ListViewModel {
    router;
    route;
    service;

    constructor(route, router, service) {
        this.route = route;
        this.router = router;
        this.service = service;
    }

    open(id) {
        this.router.navigate(this.route + "/" + id);
    }
}