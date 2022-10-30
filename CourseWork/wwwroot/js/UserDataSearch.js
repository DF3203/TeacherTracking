import * as Cookies from "./cookies.js";
const res = await fetch("https://localhost:7113/Main/GetUser?id=" + Cookies.getCookie("user"));
const stats = await res.json();
if (stats[10] != null) {

}
document.getElementById("first_name").value = stats[1];
document.getElementById("second_name").value = stats[2];
document.getElementById("middle_name").value = stats[3];
document.getElementById("email").value = stats[4];
document.getElementById("phone").value = stats[5];
document.getElementById("rank").value = stats[6];
document.getElementById("academic_degree").value = stats[7];
document.getElementById("chair").value = stats[8];
document.getElementById("category_access").value = stats[9];