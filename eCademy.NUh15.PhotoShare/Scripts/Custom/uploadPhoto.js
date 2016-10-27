
$(function () {

    Dropzone.options.uploadForm = {
        autoProcessQueue: false,
        uploadMultiple: true,
        maxFiles: 1,

        init: function () {
            var myDropZone = this;

            $('#upload-form').on('submit', uploadPhoto);

            function uploadPhoto(e) {
                e.preventDefault();
                e.stopPropagation();
                myDropZone.processQueue();
            }

            myDropZone.on('success', function (file, response) {
                console.log(file, response);
                window.location.href = '/Photos/' + response.Id;
            });
            myDropZone.on('error', function (file, response) {
                console.error(file, response.ExceptionMessage);
            });
        }
    }

    

});