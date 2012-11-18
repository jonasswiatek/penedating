function addDeleteHobby(url, hobby, remove) {
    if (hobby == null || hobby == "")
        return;

    $.post(url, {
        hobby: hobby,
        remove: remove
    }, function (data) {
        $('#hobby-new').val("");
        $("#me-hobbies").html(data);
    });
}

function hug(url, userId) {
    $.post(url, {
            userId: userId
        }, function(data) {
            alert("Hug successfully sent");
        });
}

function ajax(url, container) {
    container.load(url);
}