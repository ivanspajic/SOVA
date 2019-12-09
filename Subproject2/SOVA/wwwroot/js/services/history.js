define(["store"], function (store) {
    store.subscribe(function () {
        var state = store.getState();
        var stringifiedState = JSON.stringify(state);
        window.history.pushState(stringifiedState, null);
    });
    window.onpopstate = function (event) {
        if (event.state === null) {
            console.log("landing-page");
        } else {
            var parsedState = JSON.parse(event.state);
            console.log(parsedState.activeComponent);
            Object.keys(store.actions).forEach(key => {
                console.log(store.actions[key]);
            });
        }
    }
});