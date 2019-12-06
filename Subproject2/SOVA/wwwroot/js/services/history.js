define(["store"], function (store) {
    store.subscribe(function () {
        var state = store.getState();
        window.history.pushState(state, "");
    });
});