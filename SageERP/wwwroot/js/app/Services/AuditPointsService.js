var AuditPointsService = function () {
    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditPoints/CreateEdit',
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