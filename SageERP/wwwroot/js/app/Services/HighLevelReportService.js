var HighLevelReportService = function () {

    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/Tours/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    return {
        save: save,  
    }

}();