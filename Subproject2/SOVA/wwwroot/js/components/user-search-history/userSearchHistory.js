define("store", function (store) {
    return function () {
        store.subscribe(function () {
            var state = store.getState();
        });
        return {

        };
    };
});