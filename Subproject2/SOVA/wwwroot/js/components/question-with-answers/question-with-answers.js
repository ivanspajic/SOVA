define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var activeComponent = ko.observable("question-with-answers");
        var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);
        var showAnnotations = ko.observable(false);
        var textAreaValue = ko.observable();
        var annotationText = ko.observable();
        var response = ko.observable();
        var questionByIdWithAnswers = ko.observable();
        var isBookmarked = ko.observable(false);
        var errorMessage = ko.observable();
        var successMessage = ko.observable();

        store.subscribe(function () {
            selectedQuestionId(store.getState().selectedQuestionId);
        });

        var toggleAnnotationField = () => {
            showAnnotations(!showAnnotations());
        };

        ds.getQuestionByIdWithAnswers((data) => {
            questionByIdWithAnswers(data);
        });

        ds.getAnnotation((data) => {
            if (data.message && data.message.toLowerCase().includes("not found")) {
                annotationText(null);
            } else {
                annotationText(data);
            }
        });

        var cancelAnnotation = () => {
            showAnnotations(false);
        };

        var saveAnnotation = () => {
            annotationText(textAreaValue());
            ds.saveAnnotation(annotationText(),
                selectedQuestionId(),
                (data) => {
                    response(data);
                });
            showAnnotations(false);
        }

        var showSnackBar = () => {
            // Get the snackBar DIV
            var snackBar = document.getElementById("snackBar");

            if (!!errorMessage()) {
                // Add the "showError" class to DIV if error
                snackBar.className = "showError";
            };

            if (!!successMessage()) {
                // Add the "showSuccess" class to DIV if success
                snackBar.className = "showSuccess";
            };

        }

        var toggleBookmark = () => {
            ds.toggleBookmarkStatus((data) => {
                if (data.message.toLowerCase().includes("not authorized")) {
                    errorMessage("🙊 Please log in or sign up to bookmark this post.");
                    setTimeout(function () {
                        errorMessage("");
                    }, 3500);
                } else {
                    isBookmarked(!isBookmarked());
                    if (isBookmarked() === true) {
                        successMessage("💾 Bookmarked. You can find it under your profile.");
                        setTimeout(function () {
                            successMessage("");
                        }, 3500);
                    } else {
                        successMessage("🙈 Bookmark removed.");
                        setTimeout(function () {
                            successMessage("");
                        }, 3500);
                    }
                }
                showSnackBar();
            });
        }

        return {
            selectedQuestionId,
            activeComponent,
            questionByIdWithAnswers,
            showAnnotations,
            toggleAnnotationField,
            cancelAnnotation,
            annotationText,
            saveAnnotation,
            response,
            textAreaValue,
            isBookmarked,
            toggleBookmark,
            errorMessage,
            successMessage
        };

    };
});