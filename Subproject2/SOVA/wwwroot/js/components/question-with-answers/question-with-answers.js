define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var activeComponent = ko.observable("question-with-answers");
        var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);
        var showAnnotations = ko.observable(false);
        var annotationText = ko.observable();
        var response = ko.observable();


        var questionByIdWithAnswers = ko.observable();

        store.subscribe(function () {
            selectedQuestionId(store.getState().selectedQuestionId);
        });

        var toggleAnnotationbox = () => {
            console.log(showAnnotations())
            showAnnotations(!showAnnotations())
        }
        ds.getQuestionByIdWithAnswers((data) => {
            questionByIdWithAnswers(data);
        });
        var cancelAnnotation = () => {
            showAnnotations(false)

        }
    
        var saveAnnotation = () => {
            console.log(annotationText())
            console.log(selectedQuestionId())
            ds.saveAnnotation(annotationText(), selectedQuestionId(), (data) => {
            response(data)})

        }

        return {
            selectedQuestionId,
            activeComponent,
            questionByIdWithAnswers,
            showAnnotations,
            toggleAnnotationbox,
            cancelAnnotation,
            annotationText,
            saveAnnotation,
            response

        };

    };
});