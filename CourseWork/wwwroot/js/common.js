export function setCookie(name, value, options = {}) {

    options = {
        path: '/',
        // при необходимости добавьте другие значения по умолчанию
        ...options
    };

    if (options.expires instanceof Date) {
        options.expires = options.expires.toUTCString();
    }

    let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

    for (let optionKey in options) {
        updatedCookie += "; " + optionKey;
        let optionValue = options[optionKey];
        if (optionValue !== true) {
            updatedCookie += "=" + optionValue;
        }
    }

    document.cookie = updatedCookie;
}

export function callToast(headerresult, message) {
    let toastget = "<div class=\"toast \" style=\"background-color:white;border-radius:13px;\">" +
        "<div class=\"toast-header text-white \" style=\"border-radius:10px 10px 0 0;background-color:" + ((headerresult === true) ? "#2C9AD8" : "#DC3545") + "\">" +
        "<p style=\"margin-bottom: 0;padding-top:0.25rem;padding-bottom:0.25rem;\">" + ((headerresult === true) ? "Операція успішна" : "Виникла помилка") + "</p>" +
        "<button type=\"button\" class=\"btn-close btn-close-white me-2 m-auto\" data-bs-dismiss=\"toast\" aria-label=\"Close\"></button>" +
        "</div>" +
        "<div class=\"toast-body\">" + message +
        "</div></div>";
    document.querySelector('.toast-container').innerHTML = toastget;
    new bootstrap.Toast(document.querySelector('.toast'), { delay: 2000, animation: true, autohide: true }).show();

}
export function deleteCookie(name) {
    setCookie(name, "", {
        'max-age': -1
    })
}

export function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

export function checkCookie()
{
    if (getCookie("user") === undefined || getCookie("id_user") === undefined)
        window.location = "https://localhost:7113/";
}