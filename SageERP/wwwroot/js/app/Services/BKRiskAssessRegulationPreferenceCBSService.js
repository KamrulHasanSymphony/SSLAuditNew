var BKRiskAssessRegulationPreferenceCBSService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKRiskAssessRegulationPreferenceCBS/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };


    var saveRiskAssessRegulationPreferenceIntegration = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKRiskAssessRegulationPreferenceCBS/SaveRiskAssessRegulationPreferenceIntegration',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };



    return {
        save: save,
        saveRiskAssessRegulationPreferenceIntegration: saveRiskAssessRegulationPreferenceIntegration

    }
}();