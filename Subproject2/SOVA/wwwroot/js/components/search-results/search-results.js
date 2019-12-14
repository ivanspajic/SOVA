define(['knockout', 'dataService', 'store', 'jqcloud'], function (ko, ds, store) {
    return function (params) {
        var results = ko.observable();

        var width = params.width || 250;
        var height = params.height || 250;
        var words = ko.observable();

        var prevPage = ko.observable();
        var nextPage = ko.observable();

        var searchedTerm = ko.observable(localStorage.getItem("searchTerm"));

        store.subscribe(() => {
            searchedTerm(store.getState().searchTerm);
            ds.search((data) => {
                results(data.items.$values);
                nextPage(data.next);
                prevPage(data.prev);
            }),
                ds.getWord2Words((data) => {
                    words(data.items.$values);
                    $('#cloud').jQCloud(words,
                        {
                            width: width,
                            height: height
                        });
                    $('#cloud').jQCloud('update', words());
                });
        });

        ds.search((data) => {
            results(data.items.$values);
            nextPage(data.next);
            prevPage(data.prev);
        });

        var searchNext = function () {
            ds.getOtherPages(nextPage, (data) => {
                results(data.items.$values);
                nextPage(data.next);
                prevPage(data.prev);
            });
        };

        var searchPrev = function () {
            ds.getOtherPages(prevPage, (data) => {
                results(data.items.$values);
                nextPage(data.next);
                prevPage(data.prev);
            });
        };

        ds.getWord2Words((data) => {
            words(data.items.$values);
            $('#cloud').jQCloud(words(),
                {
                    width: width,
                    height: height
                });
        });

        var selectPost = (data, id, isQuestion) => {
            store.dispatch(store.actions.selectPost(id, isQuestion.isQuestion));
        };

        return function () {
            return {
                selectPost,
                nextPage,
                prevPage,
                searchNext,
                searchPrev,
                results,
                searchedTerm,
                words
            };
        };
    };
});
