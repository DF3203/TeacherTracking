import * as Cookies from "./cookies.js";
if (Cookies.getCookie("user") !== undefined && Cookies.getCookie("id_user") !== undefined)
    window.location = "Main/Index";

const button = document.querySelector(".login100-form-btn");

button.addEventListener("click", onLoginClick);

const login = document.querySelector('input[name="login"]');
const password = document.querySelector('input[name="pass"]');

const onLoginInput = () => {
    if (login.value.length < 6)
    {
        login.parentNode.setAttribute('data-validate', "Мінімум 6 символів");
        login.parentNode.classList.add('alert-validate');
    }
    else login.parentNode.classList.remove('alert-validate');
}

const onPasswordInput = () => {
    if (password.value.length < 8) {
        password.parentNode.setAttribute('data-validate', "Мінімум 8 символів");
        password.parentNode.classList.add('alert-validate');
    }
    else password.parentNode.classList.remove('alert-validate');
}

login.addEventListener("input", onLoginInput);
password.addEventListener("input", onPasswordInput);

 async function onLoginClick()
{
     const res = await fetch("https://localhost:7113/Home/LogIn?login=" + login.value + "&password=" + password.value);
     const data = await res.json();
     if (data == 0) {
         login.parentNode.setAttribute('data-validate', "Неправильний логін або пароль");
         login.parentNode.classList.add('alert-validate');
         password.parentNode.setAttribute('data-validate', "Неправильний логін або пароль");
         password.parentNode.classList.add('alert-validate');
     }
     else if (data == 2) {
         login.parentNode.setAttribute('data-validate', "Спробуйте пізніше");
         login.parentNode.classList.add('alert-validate');
         password.parentNode.setAttribute('data-validate', "Спробуйте пізніше");
         password.parentNode.classList.add('alert-validate');
     }
     else if (data == 1) {
         const res = await fetch("https://localhost:7113/Home/findByLogin?login=" + login.value);
         const user = await res.json();
         let date = new Date(Date.now() + 86400e3);
         date = date.toUTCString();
         Cookies.setCookie("user", login.value, { secure: true, 'expires': date });
         Cookies.setCookie("id_user", user, { secure: true, 'expires': date });
         const res2 = await fetch("https://localhost:7113/Home/GetAccess?id=" + user);
         const accesses = await res2.json();
         Cookies.setCookie("user_priv", accesses[0], { secure: true, 'expires': date });
         Cookies.setCookie("inst_priv", accesses[1], { secure: true, 'expires': date });
         Cookies.setCookie("fac_priv", accesses[2], { secure: true, 'expires': date });
         Cookies.setCookie("chair_priv", accesses[3], { secure: true, 'expires': date });
         window.location = "Main/Index";
     }
}