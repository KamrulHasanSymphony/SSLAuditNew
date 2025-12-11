var BKFraudIrregularitiesInternalControlPreferenceCBSService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKFraudIrregularitiesInternalControlPreferenceCBS/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    var saveFraudIrregularitiesInternalControlreferenceIntegration = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKFraudIrregularitiesInternalControlPreferenceCBS/SaveFraudIrregularitiesInternalControlPreferenceIntegration',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };



    return {
        save: save,
        saveFraudIrregularitiesInternalControlreferenceIntegration: saveFraudIrregularitiesInternalControlreferenceIntegration

    }
}();