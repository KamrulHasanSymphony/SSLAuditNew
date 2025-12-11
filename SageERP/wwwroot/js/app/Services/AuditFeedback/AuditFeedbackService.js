var AuditFeedbackService = function () {

    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditFeedback/CreateEdit',
            method: 'post',
            data: masterObj,

            processData: false,
            contentType: false,

        })
            .done(done)
            .fail(fail);

    };

    var FeedbackBranchSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/CreateEdit',
            method: 'post',
            data: masterObj,

            processData: false,
            contentType: false,

        })
            .done(done)
            .fail(fail);

    };

    var FeedbackBranchSendEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/BranchSendEmail',
            method: 'post',
            data: masterObj,

            processData: false,
            contentType: false,

        })
            .done(done)
            .fail(fail);

    };

    var deleteFile = function (obj, done, fail) {

        $.ajax({
            url: '/AuditFeedback/DeleteFile',
            type: 'POST',
            data: obj,
        })
            .done(done)
            .fail(fail);

    };

    var deleteFileBranch = function (obj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/DeleteFile',
            type: 'POST',
            data: obj,
        })
            .done(done)
            .fail(fail);

    };


    var AuditBranchFeedbackPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/MultiplePost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };


    var AuditFeedbackPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditFeedback/MultiplePost',
            method: 'post',
            data: masterObj

           

        })
            .done(done)
            .fail(fail);

    };


    var AuditBranchBranchFeedbackUnPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditBranchFeedback/MultipleUnPost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    var AuditFeedbackUnPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditFeedback/MultipleUnPost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };


    return {
        save, deleteFile, FeedbackBranchSave, deleteFileBranch, AuditBranchFeedbackPost,
        AuditBranchBranchFeedbackUnPost, AuditFeedbackPost, AuditFeedbackUnPost, FeedbackBranchSendEmail
    }


}();