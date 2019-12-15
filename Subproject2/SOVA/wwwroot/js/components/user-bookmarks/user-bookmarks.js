define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var activeComponent = ko.observable(localStorage.getItem("activeComponent"));
        var username = ko.observable(localStorage.getItem("username"));
        var userBookmarks = ko.observableArray([]);

        var prevPage = ko.observable();
        var nextPage = ko.observable();

        ds.getUserBookmarks((data) => {
            console.log(data);
            userBookmarks(data.items.$values);
            nextPage(data.next);
            prevPage(data.prev);
        });

        var getNext = function () {
            ds.getOtherPages(nextPage, (data) => {
                userBookmarks(data.items.$values);
                nextPage(data.next);
                prevPage(data.prev);
            });
        };

        var getPrev = function () {
            ds.getOtherPages(prevPage, (data) => {
                userBookmarks(data.items.$values);
                nextPage(data.next);
                prevPage(data.prev);
            });
        };

        var selectQuestion = (data, submission) => {
            store.dispatch(store.actions.selectQuestion(submission.submissionId));
        }

        return {
            activeComponent,
            username,
            userBookmarks,
            selectQuestion,
            nextPage,
            prevPage,
            getNext,
            getPrev
        };
    };
});