$(document).ready(function () {
    $("#success").hide();
    $("#register-form").submit((event) => {
        event.preventDefault();
        const data = {
            username: $("#username").val(),
            password: $("#password").val(),
            fullName: $("#fullname").val(),
        };
        $.post("registerUser", data).then((response) => {
            $("#success").show(500);
        });
    });

    $("#login-form").submit((event) => {
        event.preventDefault();
        const data = {
            username: $("#username").val(),
            password: $("#password").val()
        };
        $.post("loginUser", data).then((response) => {
            if (response.status == "success") {
                window.location.replace("/home/index");
            }
        });
    });

    $("#login-trans").on('click', () => {
        window.location.replace("/user/login")
    })
});
