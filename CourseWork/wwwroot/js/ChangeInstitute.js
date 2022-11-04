import * as Toasties from "../js/common.js";
const name = document.querySelector('#name');
const abbr = document.querySelector('#abbr');
const director = document.querySelector('#director');
const paramsString = document.location.search; // ?page=4&limit=10&sortby=desc  
const searchParams = new URLSearchParams(paramsString);
document.querySelector('#saveBtn').addEventListener('click', onSaveClick);
document.querySelector('#reloadBtn').addEventListener('click', function () {
    window.location = '';
})

let id = searchParams.get("id"); // 4
let modal = new bootstrap.Modal(document.getElementById('operation_success'), { backdrop: true, keyboard: true, focus: true });

document.getElementById('operation_success').addEventListener('hidden.bs.modal', function (event) {
    window.location = "Institutes";
})

document.getElementById('confirmBtn').addEventListener('click', function () {
    modal.toggle();
    window.location = "Institutes";
})

async function onSaveClick() {
    if (id != -1) {
        const query = await fetch(`https://localhost:7113/Main/UpdateInstitute?id=${id}&name=${name.value}&abbreviation=${abbr.value}&chief=${director.value}`);
        const status = query.status;
        if (status != 200) {
            const response = await query.text();
            Toasties.callToast(false, response);
        }
        else
            modal.toggle();
    }
    else {
        const query = await fetch(`https://localhost:7113/Main/AddInstitute?name=${name.value}&abbreviation=${abbr.value}&chief=${director.value}`);
        const status = query.status;
        if (status != 200) {
            const response = await query.text();
            Toasties.callToast(false, response);
        }
        else
            modal.toggle();
    }
}