(function () {

    window.mfeNotify = function (message) {
        var inIframe = window.self !== window.top;

        if (inIframe) {
            window.top.$(window.top).trigger('notify', { message: message });
        } else {
            internalNotify(message);
        }
    }

    $(window).on('notify', function (e, d) {
        internalNotify('posted: ' + d.message);
    });

    function internalNotify(message) {
        console.log(message);
    }

}());
