var ReportController = function () {
    var init = function () {

        $('#printButton').on('click', function () {

            debugger;
            var gdImage = new Image();
            gdImage.src = "/images/d.png";
            //gdImage.src = "/images/BracbankLogo.png";
            gdImage.alt = "SSL Image";
            gdImage.className = "gd-image";
            gdImage.width = 200;
            gdImage.height = 100;

            var gdImage2 = new Image();
            gdImage2.src = "/images/d.png";
            gdImage2.alt = "SSL Image";
            gdImage2.className = "gd-image";
            gdImage2.width = 200;
            gdImage2.height = 100;

            var gdImageClone = gdImage.cloneNode(true);

            gdImage.onload = function () {
                debugger;

                $('#gdLabelContainer').prepend(gdImage);

                $('a').not('.no-print a').each(function () {
                    var pTag = $('<b>').html($(this).html());
                    $(this).replaceWith(pTag);
                });

                window.print();
                gdImage.remove();
         

                


            };
        });
    }

    return {
        init: init
    }

}();








