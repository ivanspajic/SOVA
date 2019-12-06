﻿define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var activeComponent = ko.observable("question-with-answers");
        var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);

        var questionByIdWithAnswers = ko.observable();

        store.subscribe(function () {
            selectedQuestionId(store.getState().selectedQuestionId);
        });

        ds.getQuestionByIdWithAnswers((data) => {
            console.log(data);
            questionByIdWithAnswers(data);
        });

        return {
            selectedQuestionId,
            activeComponent,
            questionByIdWithAnswers,
        };

    };
});