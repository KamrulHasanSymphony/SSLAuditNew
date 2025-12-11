var BKFinancePreformPreferenceCBSService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKFinancePreformPreferenceCBS/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };


    var saveFinancePreformPreferenceIntegration = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKFinancePreformPreferenceCBS/SaveFinancePreformPreferenceIntegration',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };



    return {
        save: save,
        saveFinancePreformPreferenceIntegration: saveFinancePreformPreferenceIntegration

    }
}();