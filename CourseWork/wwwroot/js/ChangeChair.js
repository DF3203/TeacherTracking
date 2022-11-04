import * as Toasties from "./common.js";
const name = document.querySelector('#name');
const faculty = document.querySelector('#faculty');
const chief = document.querySelector('#chief');
const paramsString = document.location.search; // ?page=4&limit=10&sortby=desc  
const searchParams = new URLSearchParams(paramsString);
document.querySelector('#saveBtn').addEventListener('click', onSaveClick);
document.querySelector('#reloadBtn').addEventListener('click', function () {
    window.location = '';
})

let id = searchParams.get("id"); // 4
let modal = new bootstrap.Modal(document.getElementById('operation_success'), { backdrop: true, keyboard: true, focus: true });

document.getElementById('operation_success').addEventListener('hidden.bs.modal', function (event) {
    window.location = "Chairs";
})

document.getElementById('confirmBtn').addEventListener('click', function () {
    modal.toggle();
    window.location = "Chairs";
})

async function onSaveClick() {
    if (id != -1) {
        const query = await fetch(`https://localhost:7113/Main/UpdateChair?id=${id}&name=${name.value}&faculty=${faculty.value}&chief=${chief.value}`);
        const status = query.status;
        if (status != 200) {
            const response = await query.text();
            Toasties.callToast(false, response);
        }
        else
            modal.toggle();
    }
    else {
        const query = await fetch(`https://localhost:7113/Main/AddChair?name=${name.value}&faculty=${faculty.value}&chief=${chief.value}`);
        const status = query.status;
        if (status != 200) {
            const response = await query.text();
            Toasties.callToast(false, response);
        }
        else
            modal.toggle();
    }
}