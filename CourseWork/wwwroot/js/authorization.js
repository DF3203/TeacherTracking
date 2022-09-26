
(function ($) {
    "use strict";

    /*==================================================================
    [ Validate ]*/
    var input = $('.validate-input .input100');

    $('.validate-form').on('button',function(){
        var check = true;

        for(var i=0; i<input.length; i++) {
            if(validate(input[i]) == false){
                showValidate(input[i]);
                check=false;
            }
        }

        return check;
    });


    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
           hideValidate(this);
        });
    });

    function validate (input) {
        if($(input).attr('type') == 'text' || $(input).attr('name') == 'email') {
            if($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
                return false;
            }
        }
        else {
            if($(input).val().trim() == ''){
                return false;
            }
        }
    }

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }
    console.log(1);
})(jQuery);

function checkUser()
{
    if (getCookie("user") !== undefined && getCookie("id_user") !== undefined)
 window.location = "mainPage.html";
}

function onLoginClick()
{
//    let login = document.getElementsByName("username")[0];
//    let password = document.getElementsByName("pass")[0];

//    let input = $('.validate-input .input100');


//    if (login.value.toString().length === 0 || (password.value.toString().length === 0))
//    {
//        $($(input).parent()).addClass('alert-validate');
//        login.value = "";
//        password.value = "";
//        return;
//    }

//    let request_CheckLogin = new XMLHttpRequest();
//    request_CheckLogin.onreadystatechange = function ()
//    {
//        if (this.readyState === 4 && this.status === 200)
//        {
//            let user = JSON.parse(this.responseText);
//            console.log(user);
//            if (user === false)
//            {
//                $($(input).parent()).addClass('alert-validate');
//                ($(input).parent()).dataset;
//                login.value = "";
//                password.value = "";
//            }
//            else
//            {
//                let request_GetID = new XMLHttpRequest();
//                request_GetID.onreadystatechange = function () {
//                    if (this.readyState === 4 && this.status === 200) {
//                        let userInfo = JSON.parse(this.responseText);
//                        let date = new Date(Date.now() + 86400e3);
//                        date = date.toUTCString();
//                        setCookie("user", login.value, {secure: true, 'expires': date});
//                        setCookie("id_user", userInfo.id_user, {secure: true, 'expires': date});
//                        window.location = "mainPage.html";
//                    }
//                }
//                request_GetID.open("GET", "http://localhost:8080/user/findByLogin?login=" + login.value);
//                request_GetID.send();
//            }
//        }
//    };

//    request_CheckLogin.open("GET", "http://localhost:8080/user/logIn?login=" + login.value + "&password=" + password.value);
//    request_CheckLogin.send();
}