define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {

        var linkedPosts = ko.observable();

        ds.getQuestionByIdWithAnswers((data) => {
            if (data.answers) {
                linkedPosts(data.answers.$values[0].question.linkedPosts.$values);
            }
        });

        var selectQuestion = (data, linkedPost) => {
            store.dispatch(store.actions.selectQuestion(linkedPost.linkPostId));
            location.reload();
        }

        return { linkedPosts, selectQuestion };
    }
})