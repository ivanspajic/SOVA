define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);
        var showAnnotations = ko.observable(false);
        var textAreaValue = ko.observable();
        var annotationText = ko.observable();
        var response = ko.observable();
        var questionByIdWithAnswers = ko.observable();
        var isBookmarked = ko.observable(false);
        var errorMessage = ko.observable();
        var successMessage = ko.observable();
        var isUserLoggedIn = ko.observable(!!localStorage.getItem("username"));

        store.subscribe(function () {
            selectedQuestionId(store.getState().selectedQuestionId);
        });

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

        var toggleAnnotationField = () => {
            if (!textAreaValue() || annotationText()) {
                textAreaValue(annotationText());
            }
            if (!isUserLoggedIn()) {
                errorMessage("⛔ Please log in to add your annotation.");
                setTimeout(function () {
                    errorMessage("");
                }, 3500);
            } else {
                showAnnotations(!showAnnotations());
            }
            showSnackBar();
        };

        var selectQuestionsByTag = (data, tag) => {
            store.dispatch(store.actions.tagFilter(tag.tag.tagString));
        }

        ds.getQuestionByIdWithAnswers((data) => {
            questionByIdWithAnswers(data);
        });

        ds.checkIfBookmarked((data) => {
            if (data.message.toLowerCase().includes("not bookmarked")) {
                isBookmarked(false);
            } else if (data.message.toLowerCase().includes("already bookmarked")) {
                isBookmarked(true);
            }
        });


        ds.getAnnotation((data) => {
            if (data.message && data.message.toLowerCase().includes("not found")) {
                annotationText(null);
            } else {
                annotationText(data.annotationString);
            }
        });


        var cancelAnnotation = () => {
            showAnnotations(false);
        };

        var saveAnnotation = () => {
            annotationText(textAreaValue());
            if (textAreaValue()) {
                ds.saveAnnotation(annotationText(),
                    (data) => {
                        if (data.message && data.message.toLowerCase().includes("not authorized")) {
                            errorMessage("⛔ Please log to add your annotation.");
                            setTimeout(function () {
                                errorMessage("");
                            }, 3500);
                        }
                        response(data);
                    });
                successMessage("📝 Annotation saved successfully. You can find it under your profile.");
                setTimeout(function () {
                    successMessage("");
                }, 3500);
            } else {
                toggleAnnotationField();
            }
            showAnnotations(false);
            showSnackBar();
        }

        var updateAnnotation = () => {
            if (!textAreaValue()) {
                textAreaValue(annotationText());
            }
            annotationText(textAreaValue());
            if (textAreaValue()) {
                ds.updateAnnotation(annotationText(),
                    (data) => {
                        response(data);
                    });
                successMessage("👍 Annotation updated successfully.");
                setTimeout(function () {
                    successMessage("");
                }, 3500);
            } else {
                ds.deleteAnnotation((data) => {
                    response(data);
                });
                successMessage("🏷 Annotation deleted.");
                setTimeout(function () {
                    successMessage("");
                }, 3500);
            }
            showAnnotations(false);
            showSnackBar();
        }

        var deleteAnnotation = () => {
            annotationText(textAreaValue());
            ds.deleteAnnotation((data) => {
                response(data);
            });
            successMessage("🏷 Annotation deleted.");
            setTimeout(function () {
                successMessage("");
            }, 3500);
            textAreaValue("");
            annotationText("");
            showAnnotations(false);
            showSnackBar();
        }

        var toggleBookmark = () => {
            ds.toggleBookmarkStatus((data) => {
                if (data.message.toLowerCase().includes("not authorized")) {
                    errorMessage("⛔ Please log in to bookmark this post.");
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
                        successMessage("📑 Bookmark removed.");
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
            questionByIdWithAnswers,
            selectQuestionsByTag,
            showAnnotations,
            toggleAnnotationField,
            cancelAnnotation,
            updateAnnotation,
            annotationText,
            saveAnnotation,
            response,
            textAreaValue,
            isBookmarked,
            toggleBookmark,
            errorMessage,
            successMessage,
            isUserLoggedIn,
            deleteAnnotation
        };
    };
});