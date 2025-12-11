var FiscalYearsService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/Advances/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    var AdvancesMultiplePost = function (masterObj, done, fail) {

        $.ajax({
            url: '/Advances/MultiplePost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    var AdvancesMultipleUnPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/Advances/MultipleUnPost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    //var finalImageSave = function (url, masterObj, done, fail) {

    //    $.ajax({
    //        url: url,
    //        method: 'post',
    //        data: masterObj,
    //        processData: false,
    //        contentType: false,
    //        timeout: 60000
    //    })
    //        .done(done)
    //        .fail(fail);

    //};

    var finalImageSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/FiscalYear/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };




    return {
        save: save,
        AdvancesMultiplePost : AdvancesMultiplePost,
        AdvancesMultipleUnPost: AdvancesMultipleUnPost,
        finalImageSave: finalImageSave

    }
}();