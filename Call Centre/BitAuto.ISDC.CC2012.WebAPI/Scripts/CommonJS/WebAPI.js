/* File Created: 九月 4, 2014 */

var WebApi = {};
WebApi.Msg = function (header, content,second) {
    if (!content) {
        content = header;
        header = "Message";
    }
    if (!second) {
        second = 2000;
    }
    if ($('#msg-div').length == 0) {
        $('body').prepend('<div id="msg-div"></div>');
        WebApi.Msg.timeout = 0;
    }
    var tmpMsg$ = $('<div class="msg"><h3>' + header + '</h3><p>' + content + '</p></div>');
    $('#msg-div').append(tmpMsg$);
    clearTimeout(WebApi.Msg.timeout);
    tmpMsg$.css({ 'top': '-50px', 'display': 'block', 'position': 'relative', 'opacity': '0' })
        .animate({ 'top': '0px', 'opacity': '1' }, 'slow').delay(second)
        .animate({ 'top': '-50px', 'opacity': '0' }, 'slow')
        .queue(function () {
            $(this).empty();
            clearTimeout(WebApi.Msg.timeout);
            WebApi.Msg.timeout = setTimeout(function () {
                $('#msg-div').empty();
            }, 800);
        });
}