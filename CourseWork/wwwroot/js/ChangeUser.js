import * as Toasties from "../js/common.js";
const surname = document.querySelector('#second_name');
const name = document.querySelector('#first_name');
const middlename = document.querySelector('#middle_name');
const email = document.querySelector('#email');
const phone = document.querySelector('#phone');
const rank = document.querySelector('#rank');
const degree = document.querySelector('#academic_degree');
const chair = document.querySelector('#chair');
const access = document.querySelector('#category_access');
const paramsString = document.location.search; // ?page=4&limit=10&sortby=desc  
const searchParams = new URLSearchParams(paramsString);
document.querySelector('#saveBtn').addEventListener('click', onSaveClick);
document.querySelector('#reloadBtn').addEventListener('click', function () {
    window.location = '';
})

let id = searchParams.get("id"); // 4
let modal = new bootstrap.Modal(document.getElementById('operation_success'), { backdrop: true, keyboard: true, focus: true });

document.getElementById('operation_success').addEventListener('hidden.bs.modal', function (event) {
    window.location = "Users";
})

document.getElementById('confirmBtn').addEventListener('click', function () {
    modal.toggle();
    window.location = "Users";
})

async function onSaveClick() {
    if (id != -1) {
        const query = await fetch(`https://localhost:7113/Main/ChangeUserAdmin?id=${id}&name=${name.value}&surname=${surname.value}&middlename=${middlename.value}&phone=${phone.value}&email=${email.value}&rank=${rank.value}&degree=${degree.value}&chair=${chair.value}&level=${access.value}`);
        const status = query.status;
        if (status != 200) {
            const response = await query.text();
            Toasties.callToast(false, response);
        }
        else
            modal.toggle();
    }
    else {
        const query = await fetch(`https://localhost:7113/Main/AddUser?name=${name.value}&surname=${surname.value}&middlename=${middlename.value}&phone=${phone.value}&email=${email.value}&rank=${rank.value}&degree=${degree.value}&chair=${chair.value}&level=${access.value}`);
        const status = query.status;
        if (status != 200) {
            const response = await query.text();
            Toasties.callToast(false, response);
        }
        else
            modal.toggle();
    }
}