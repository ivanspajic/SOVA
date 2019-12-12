define(['knockout', 'dataService', 'store', 'jqcloud'], function (ko, ds, store) {
    return function (params) {
        var results = ko.observable();

        var width = params.width || 250;
        var height = params.height || 250;
        var words = [];

        store.subscribe(() => {
            ds.search((data) => {
                results(data.items.$values);
            }),
            ds.getWord2Words((data) => {
                words = data.items.$values;
                $('#cloud').jQCloud(words,
                    {
                        width: width,
                        height: height
                    });
                $('#cloud').jQCloud('update', words);
            });
        });

        ds.search((data) => {
            results(data.items.$values);
        });
        ds.getWord2Words((data) => {
            words = data.items.$values;
            $('#cloud').jQCloud(words,
                {
                    width: width,
                    height: height
                });
        });

        var selectPost = (data, id, isQuestion) => {
            store.dispatch(store.actions.selectPost(id, isQuestion.isQuestion));
        }

        return function () {
            return {
                selectPost,
                results
            };
        };
    };
});
