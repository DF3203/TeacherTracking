import * as Toasties from "./common.js";
const name = document.querySelector('#name');
const abbr = document.querySelector('#abbr');
const institute = document.querySelector('#institute');
const dean = document.querySelector('#dean');
const deputy = document.querySelector('#deputy');
const paramsString = document.location.search; 
const searchParams = new URLSearchParams(paramsString);
document.querySelector('#saveBtn').addEventListener('click', onSaveClick);
document.querySelector('#reloadBtn').addEventListener('click', function () {
    window.location = '';
})

let id = searchParams.get("id"); // 4
let modal = new bootstrap.Modal(document.getElementById('operation_success'), { backdrop: true, keyboard: true, focus: true });

document.getElementById('operation_success').addEventListener('hidden.bs.modal', function (event) {
    window.location = "Faculties";
})

document.getElementById('confirmBtn').addEventListener('click', function () {
    modal.toggle();
    window.location = "Faculties";
})

async function onSaveClick() {
    if (id != -1) {
        const query = await fetch(`https://localhost:7113/Main/UpdateFaculty?id=${id}&name=${name.value}&abbreviation=${abbr.value}&institute=${institute.value}&dean=${dean.value}&deputy=${deputy.value}`);
        const status = query.status;
        if (status != 200) {
            const response = await query.text();
            Toasties.callToast(false, response);
        }
        else
            modal.toggle();
    }
    else {
        const query = await fetch(`https://localhost:7113/Main/AddFaculty?name=${name.value}&abbreviation=${abbr.value}&institute=${institute.value}&dean=${dean.value}&deputy=${deputy.value}`);
        const status = query.status;
        if (status != 200) {
            const response = await query.text();
            Toasties.callToast(false, response);
        }
        else
            modal.toggle();
    }
}