import * as Cookies from "./cookies.js";
if (Cookies.getCookie("user") !== undefined && Cookies.getCookie("id_user") !== undefined)
window.location = "mainPage.cshtml";

const button = document.querySelector(".login100-form-btn");

button.addEventListener("click", onLoginClick);

const login = document.querySelector('input[name="login"]');
const password = document.querySelector('input[name="pass"]');

const onLoginInput = () => {
    if (login.value.length < 6) login.parentNode.classList.add('alert-validate');
    else login.parentNode.classList.remove('alert-validate');
}

const onPasswordInput = () => {
    if (password.value.length < 8) password.parentNode.classList.add('alert-validate');
    else password.parentNode.classList.remove('alert-validate');
}

login.addEventListener("input", onLoginInput);
password.addEventListener("input", onPasswordInput);

function onLoginClick()
{
    alert("1");
//    let request_CheckLogin = new XMLHttpRequest();
//    request_CheckLogin.onreadystatechange = function ()
//    {
//        if (this.readyState === 4 && this.status === 200)
//        {
//            let user = JSON.parse(this.responseText);
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