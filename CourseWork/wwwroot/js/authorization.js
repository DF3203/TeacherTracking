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

 async function onLoginClick()
{
     const res = await fetch("https://localhost:7113/Home/LogIn?login=" + login.value + "&password=" + password.value);
     const data = await res.json();
     alert(data);
}