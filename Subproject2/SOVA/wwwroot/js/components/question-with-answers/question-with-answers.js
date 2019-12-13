define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var activeComponent = ko.observable("question-with-answers");
        var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);
        var showAnnotations = ko.observable(false);
        var textAreaValue = ko.observable();
        var annotationText = ko.observable();
        var response = ko.observable();
        var questionByIdWithAnswers = ko.observable();

        store.subscribe(function () {
            selectedQuestionId(store.getState().selectedQuestionId);
        });

        var toggleAnnotationbox = () => {
            console.log(showAnnotations())
            showAnnotations(!showAnnotations())
        };

        var selectQuestionsByTag = (data, tag) => {
            store.dispatch(store.actions.tagFilter(tag.tag.tagString));
        }

        ds.getQuestionByIdWithAnswers((data) => {
            questionByIdWithAnswers(data);
        });

        ds.getAnnotation((data) => {
            if (data.message && data.message.toLowerCase().includes("not found")) {
                annotationText(null)
            } else {
                annotationText(data)
            }
        });

        var cancelAnnotation = () => {
            showAnnotations(false)
        };

        var saveAnnotation = () => {
            console.log(annotationText());
            console.log(selectedQuestionId());
            annotationText(textAreaValue());
            ds.saveAnnotation(annotationText(), selectedQuestionId(), (data) => {
                response(data);
            })
            showAnnotations(false);
        }

        return {
            selectedQuestionId,
            activeComponent,
            questionByIdWithAnswers,
            selectQuestionsByTag,
            showAnnotations,
            toggleAnnotationbox,
            cancelAnnotation,
            annotationText,
            saveAnnotation,
            response,
            textAreaValue
        };

    };
});