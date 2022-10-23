import * as Cookies from "./cookies.js";
let userInfo = null;
let navop=false;
function loadData()
{
    Cookies.checkCookie();
    document.getElementById("username").innerText = Cookies.getCookie("user");
    let request_UserInfo = new XMLHttpRequest();
    request_UserInfo.onreadystatechange = function ()
    {
        if (this.readyState === 4 && this.status === 200)
        {
            userInfo = JSON.parse(this.responseText);
            if (userInfo.user_photo != null)
            {

            }
            document.getElementById("first_name").value = userInfo.first_name;
            document.getElementById("second_name").value = userInfo.second_name;
            document.getElementById("middle_name").value = userInfo.middle_name;
            document.getElementById("email").value = userInfo.email;
            document.getElementById("phone").value = userInfo.phone;
            document.getElementById("category_access").value = userInfo.id_category_access.name;
            fillAcademicDegree();
            fillChair();
            fillRank();
        }
    };
    request_UserInfo.open("GET", "http://localhost:8080/userinfo/findByid?id=" + getCookie("id_user"));
    request_UserInfo.send();
}
function fillAcademicDegree()
{
    let request_Degree = new XMLHttpRequest();
    request_Degree.onreadystatechange = function ()
    {
        if (this.readyState === 4 && this.status === 200) {
            let answer = JSON.parse(this.responseText);
            document.getElementById("academic_degree").innerHTML = "<option>" + userInfo.id_academic_degree.name + "</option>"
            answer.forEach(function (item) {
                if (item.name !== userInfo.id_academic_degree.name) {
                    document.getElementById("academic_degree").innerHTML += "<option>" + item.name + "</option>"
                }
            });
        }
    }
    request_Degree.open("GET", "http://localhost:8080/degree/findAll");
    request_Degree.send();
}
function fillChair()
{
    let request_Chair = new XMLHttpRequest();
    request_Chair.onreadystatechange = function ()
    {
        if (this.readyState === 4 && this.status === 200) {
            let answer = JSON.parse(this.responseText);
            document.getElementById("chair").innerHTML = "<option>" + userInfo.id_chair.abbr + "</option>"
            answer.forEach(function (item) {
                if (item.abbr !== userInfo.id_chair.abbr) {
                    document.getElementById("chair").innerHTML += "<option>" + item.abbr + "</option>"
                }
            });
        }
    }
    request_Chair.open("GET", "http://localhost:8080/chair/findAll");
    request_Chair.send();
}
function fillRank()
{
    let request_Rank = new XMLHttpRequest();
    request_Rank.onreadystatechange = function ()
    {
        if (this.readyState === 4 && this.status === 200) {
            let answer = JSON.parse(this.responseText);
            document.getElementById("rank").innerHTML = "<option>" + userInfo.id_rank.name + "</option>"
            answer.forEach(function (item) {
                if (item.name !== userInfo.id_rank.name) {
                    document.getElementById("rank").innerHTML += "<option>" + item.name + "</option>"
                }
            });
        }
    }
    request_Rank.open("GET", "http://localhost:8080/rank/findAll");
    request_Rank.send();
}
function saveData()
{
    //if (document.getElementById("first_name").value.length < 3 || document.getElementById("second_name").value.length < 3
    //|| document.getElementById("middle_name").value.length < 3  || document.getElementById("email").value.length < 3) return;
    let rank;
    let request = new XMLHttpRequest();
    request.onreadystatechange = function ()
    {
        if (this.readyState === 4 && this.status === 200)
        {
            rank=JSON.parse(this.responseText);
            let degree;
            let request2 = new XMLHttpRequest();
            request2.onreadystatechange = function ()
            {
                if (this.readyState === 4 && this.status === 200)
                {
                    degree=JSON.parse(this.responseText);
                    let chair;
                    let request3 = new XMLHttpRequest();
                    request3.onreadystatechange = function ()
                    {
                        if (this.readyState === 4 && this.status === 200)
                        {
                            chair=JSON.parse(this.responseText);
                            let request_SaveUserInfo = new XMLHttpRequest();
                            request_SaveUserInfo.open("POST", "http://localhost:8080/userinfo/save");
                            request_SaveUserInfo.setRequestHeader("Content-Type", "application/json");
                            request_SaveUserInfo.onreadystatechange = function ()
                            {
                                if (this.readyState === 4 && this.status === 200){
                                    loadData();
                                }
                            }
                            request_SaveUserInfo.send(JSON.stringify(
                                {
                                    id: getCookie("id_user"),
                                    second_name: document.getElementById("second_name").value,
                                    first_name: document.getElementById("first_name").value,
                                    middle_name:  document.getElementById("middle_name").value,
                                    email:  document.getElementById("email").value,
                                    phone:  document.getElementById("phone").value,
                                    user_photo: userInfo.user_photo,
                                    delete_date: userInfo.delete_date,
                                    id_rank: rank,
                                    id_academic_degree: degree,
                                    id_category_access: userInfo.id_category_access,
                                    id_chair: chair,
                                }));
                        }
                    };
                    request3.open("GET", "http://localhost:8080/chair/findByAbbr?name=" + document.getElementById("chair").options[document.getElementById("chair").selectedIndex].text);
                    request3.send();
                }
            };
            request2.open("GET", "http://localhost:8080/degree/findByName?name=" + document.getElementById("academic_degree").options[document.getElementById("academic_degree").selectedIndex].text);
            request2.send();
        }
    };
    request.open("GET", "http://localhost:8080/rank/findByName?name=" + document.getElementById("rank").options[document.getElementById("rank").selectedIndex].text);
    request.send();
}

function userExit()
{
    if (confirm("Ви дійсно хочете вийти з системи?")) {
        deleteCookie("user");
        deleteCookie("id_user");
        window.location = "index.html";
    }
}
//sidebad js
$('body,html').click(function(e){
    if (navop===true){
        $('.menu').removeClass('menu_active');
        document.getElementById("logoimg").style.position ="sticky";
        navop=false;}
});
$('.menu').on('click', function(e) {
    e.stopPropagation();
})
$('.menu-btn').on('click', function(e) {
    e.stopPropagation();
    navop=true;
    $('.menu').toggleClass('menu_active');
    if(document.getElementById("logoimg").style.position==="fixed") document.getElementById("logoimg").style.position="sticky";
    else document.getElementById("logoimg").style.position ="fixed";
})