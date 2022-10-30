const f_name = document.querySelector('#first_name');
const s_name = document.querySelector('#second_name');
const m_name = document.querySelector('#middle_name');
const phone = document.querySelector('#phone');
const email = document.querySelector('#email');
document.querySelector('#saveBtn').addEventListener('click', function () {
    const response = fetch(`https://localhost:7113/Main/UpdateUser/LogIn?name=${f_name.value}&surname=${s_name.value}&middlename=${m_name.value}&phone=${phone.value}&email=${email.value}`);
    if (!response.ok)
        alert("Not ok");
    else
        alert("ok");
})