define(["jquery"], function ($) {
    var getQuestions = function (callback) {
        $.getJSON("api/questions", callback);
    };

    // Async/await doesnot work for some reason. Didn't investigate.
    //   var getNamesWithFetchAsync = async function(callback) {
    //     var response = await fetch("api/questions");
    //     var data = await response.json();
    //     callback(data);
    //   };

    return { getQuestions };
});
