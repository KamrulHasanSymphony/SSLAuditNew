var BKAuditOfficePreferenceCBSService = function () {


    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKAuditOfficePreferenceCBS/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    var AuditOfficePreferenceIntegration = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKAuditOfficePreferenceCBS/AuditOfficePreferenceIntegration',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    var saveAuditOfficePreferenceIntegration = function (masterObj, done, fail) {

        $.ajax({
            url: '/BKAuditOfficePreferenceCBS/SaveAuditOfficePreferenceIntegration',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };



    return {
        save: save,
        AuditOfficePreferenceIntegration: AuditOfficePreferenceIntegration,
        saveAuditOfficePreferenceIntegration: saveAuditOfficePreferenceIntegration


    }
}();