import * as Cookies from "./cookies.js";
let userInfo = null;
let navop=false;

Cookies.checkCookie();

document.getElementById('exitbtn').addEventListener("click", function (e) {
    if (confirm("Ви дійсно хочете вийти з системи?")) {
        let id = Cookies.getCookie("id_user");
        Cookies.deleteCookie("user");
        Cookies.deleteCookie("id_user");
        Cookies.deleteCookie("user_priv");
        Cookies.deleteCookie("inst_priv");
        Cookies.deleteCookie("fac_priv");
        Cookies.deleteCookie("chair_priv");
        window.location = "Exit?id=1";
    }
});
try {
    document.getElementById('userbtn').addEventListener("click", function (e) {
        window.location = "Users";
    });
}
catch { };
//sidebad js

document.querySelector('#mainbtn').addEventListener("click", function() {
    window.location = "Index";
})

document.querySelector('body,html').addEventListener("click", function (e) {
    if (navop === true) {
        document.querySelector('.menu').classList.remove('menu_active');
        document.querySelector('.menu-btn span').classList.remove('activabutt');
        document.getElementById("logoimg").style.position = "sticky";
        navop = false;
    }
});
document.querySelector('.menu-btn').addEventListener("click", function (e) {
    e.stopPropagation();
    if (!navop) {
        document.querySelector('.menu').classList.add('menu_active');
        navop = true;
    }
    else {
        document.querySelector('.menu').classList.remove('menu_active');
        navop = false;
}

    if (document.getElementById("logoimg").style.position === "fixed") {
        document.querySelector('.menu-btn span').classList.remove('activabutt');
        document.getElementById("logoimg").style.position = "sticky";
    }
    else {
        document.querySelector('.menu-btn span').classList.add('activabutt');
        document.getElementById("logoimg").style.position = "fixed";
    }
});
document.querySelector('.menu').addEventListener("click", function (e) {
    e.stopPropagation();
});