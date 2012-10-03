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