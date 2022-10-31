import * as Toasties from "../js/common.js";
const f_name = document.querySelector('#first_name');
const s_name = document.querySelector('#second_name');
const m_name = document.querySelector('#middle_name');
const phone = document.querySelector('#phone');
const email = document.querySelector('#email');
document.querySelector('#saveBtn').addEventListener('click', onSaveClick)




async function onSaveClick() { 
    const query = await fetch(`https://localhost:7113/Main/UpdateUser/LogIn?name=${f_name.value}&surname=${s_name.value}&middlename=${m_name.value}&phone=${phone.value}&email=${email.value}`);
    const status = query.status;
    if (status != 200)
    {
        const response = await query.text();
        Toasties.callToast(false,response);
    }
    else
        Toasties.callToast(true,"Зміни внесено");
}