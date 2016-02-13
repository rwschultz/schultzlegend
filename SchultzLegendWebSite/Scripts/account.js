//Register user
$('#btn-register').on('click', function (e) {
    var email = document.getElementById('tb-email').value;
    var password = document.getElementById('tb-password').value;
    var confpassword = document.getElementById('tb-confpassword').value;
    var token = document.getElementsByName("__RequestVerificationToken")[0].value

    document.getElementById('error').textContent = '&nbsp;';

    var register = Register(email, password, confpassword, token);
    if (register.Error == "none") {
        $("#register").addClass("hidden");
        $("#check-email").removeClass("hidden");

    }
    else if(register == false)
    {
        document.getElementById('error').textContent = 'You are not authorized to create an account';
    }
    else if (register.Error.split(':')[0] == "email") {
        if (register.Error.split(':')[1] == "invalid")
        {
            document.getElementById('error').textContent = 'Please enter a valid email address';
        }
        if (register.Error.split(':')[1] == "long") {
            document.getElementById('error').textContent = 'Please enter a shorter email address';
        }
    }
    else if (register.Error.split(':')[0] == "password") {
        if (register.Error.split(':')[1] == "short") {
            document.getElementById('error').textContent = 'Please enter a password that is 8 or more characters long';
        }
        else if (register.Error.split(':')[1] == "number") {
            document.getElementById('error').textContent = 'Please enter a password containing a number';
        }
        else if (register.Error.split(':')[1] == "upper") {
            document.getElementById('error').textContent = 'Please enter a password containing an uppercase letter';
        }
        else if (register.Error.split(':')[1] == "lower") {
            document.getElementById('error').textContent = 'Please enter a password containing a lowercase letter';
        }
        else if (register.Error.split(':')[1] == "mismatch") {
            document.getElementById('error').textContent = 'The password and confirm password fields do not match';
        }
    }

});
function Register(email, password, confpassword, token) {
    var ajaxresponse;
    $.ajax({
        type: "POST",
        url: '/Register',
        dataType: "json",
        async: false,
        data: { email: email, password: password, confpassword: confpassword, __RequestVerificationToken: token },
        success: function (response) {
            ajaxresponse = response;
        },
        error: function () {
            ajaxresponse = false;
        }
    });
    return ajaxresponse;
}

//Login user
$('#btn-login').on('click', function (e) {
    var email = document.getElementById('tb-email').value;
    var password = document.getElementById('tb-password').value;
    var token = document.getElementsByName("__RequestVerificationToken")[0].value
    var remember = $('#cb-remember-me');

    var login = Login(email, password, token, remember[0].checked);
    if (login.Error == 'none') {
        window.location = "/";
    }
    else if(login.Error == 'confirm') {
        window.location = "/ConfirmEmail?token=1";
    }
    else {
        document.getElementById('error').textContent = 'The email or password is invalid';
    }
});
function Login(email, password, token, remember) {
    var ajaxresponse;
    $.ajax({
        type: "POST",
        url: '/Login',
        dataType: "json",
        async: false,
        data: { email: email, password: password, __RequestVerificationToken: token, remember: remember },
        success: function (response) {
            ajaxresponse = response;
        },
        error: function () {
            ajaxresponse = false;
        }
    });
    return ajaxresponse;
}

//Change Password
$('#btn-change-password').on('click', function (e) {
    var curpassword = document.getElementById('tb-curpassword').value;
    var newpassword = document.getElementById('tb-newpassword').value;
    var confpassword = document.getElementById('tb-confpassword').value;
    var token = document.getElementsByName("__RequestVerificationToken")[0].value

    document.getElementById('error').textContent = ' ';

    var register = ChangePassword(curpassword, newpassword, confpassword, token);
    if (register.Error == "none") {
        window.location = '/Account';

    }
    else if (register == false) {
        document.getElementById('error').textContent = 'An Error has occurred changing your password';
    }
    else if (register.Error.split(':')[0] == "password") {
        if (register.Error.split(':')[1] == "invalid") {
            document.getElementById('error').textContent = 'The current password is invalid';
        }
        if (register.Error.split(':')[1] == "short") {
            document.getElementById('error').textContent = 'Please enter a password that is 8 or more characters long';
        }
        else if (register.Error.split(':')[1] == "number") {
            document.getElementById('error').textContent = 'Please enter a password containing a number';
        }
        else if (register.Error.split(':')[1] == "upper") {
            document.getElementById('error').textContent = 'Please enter a password containing an uppercase letter';
        }
        else if (register.Error.split(':')[1] == "lower") {
            document.getElementById('error').textContent = 'Please enter a password containing a lowercase letter';
        }
        else if (register.Error.split(':')[1] == "mismatch") {
            document.getElementById('error').textContent = 'The password and confirm password fields do not match';
        }
    }

});
function ChangePassword(curpassword, newpassword, confpassword, token) {
    var ajaxresponse;
    $.ajax({
        type: "POST",
        url: '/ChangePassword',
        dataType: "json",
        async: false,
        data: { curpassword: curpassword, newpassword: newpassword, confpassword: confpassword, __RequestVerificationToken: token },
        success: function (response) {
            ajaxresponse = response;
        },
        error: function () {
            ajaxresponse = false;
        }
    });
    return ajaxresponse;
}

//Forgot Password
$('#btn-forgot').on('click', function (e) {
    var email = document.getElementById('tb-email').value;

    ForgotPassword(email);
    $("#send-email").addClass("hidden");
    $("#check-email").removeClass("hidden");

});
function ForgotPassword(email) {
    $.ajax({
        type: "POST",
        url: '/ForgotPassword',
        dataType: "json",
        async: false,
        data: { email: email}  
    });
}
