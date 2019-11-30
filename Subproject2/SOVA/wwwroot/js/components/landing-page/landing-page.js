﻿define(["knockout", "dataService"], function (ko, ds) {
    return function () {
        var questions = ko.observableArray([]);
        ds.getQuestions(function (data) {
            questions(data.items.$values);
        });
        return { questions };
    };
});
