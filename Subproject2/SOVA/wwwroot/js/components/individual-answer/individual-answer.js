define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var selectedAnswerId = ko.observable(store.getState().selectedPostId);
    var answerById = ko.observable();

    store.subscribe(function () {
        selectedAnswerId(store.getState().selectedPostId);
    });

    ds.getAnswerById((data) => {
        answerById(data);
    });

    return function () {
        return {
            selectedAnswerId,
            answerById,
        };
    };
});