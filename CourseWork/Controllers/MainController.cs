using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Npgsql;

namespace CourseWork.Controllers
{
    public class MainController : Controller
    {


        #region Pages
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            if (Request.Cookies["user_priv"] != "true")
                return new UnauthorizedResult();
            else
                return View();

        }

        public IActionResult Chairs()
        {

            if (Request.Cookies["chair_priv"] != "true")
                return new UnauthorizedResult();
            else
                return View();
        }

        public IActionResult Institutes()
        {
            if (Request.Cookies["inst_priv"] != "true")
                return new UnauthorizedResult();
            else
                return View();
        }

        public IActionResult Faculties()
        {
            if (Request.Cookies["fac_priv"] != "true")
                return new UnauthorizedResult();
            else
                return View();
        }

        public IActionResult Exit(int id)
        {
            NpgsqlCommand command = new NpgsqlCommand($"call addlog({id}, 'unlogged', 'user', 'tb_users', '{Request.HttpContext.Connection.RemoteIpAddress}', 1)", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult(command.CommandText);
            }
            DataBase._connection.Close();
            return Redirect("../");
        }

        public IActionResult ChangeUser(int id)
        {
            if (Request.Cookies["user_priv"] != "true")
                return new UnauthorizedResult();
            else
            {
                ViewBag.UserId = id;
                return View();
            }
        }
        #endregion
        public IActionResult UpdateUser(string name, string surname, string middlename, string phone, string email)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(surname) || String.IsNullOrEmpty(middlename)
                || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email))
                return new BadRequestObjectResult("Поля не можуть бути пустими");
            name = name.Replace(" ", "");
            surname = surname.Replace(" ", "");
            middlename = middlename.Replace(" ", "");
            phone = phone.Replace(" ", "");
            email = email.Replace(" ", "");
            NpgsqlCommand command = new NpgsqlCommand($"BEGIN; " +
                $"UPDATE tb_users_info SET first_name = '{name}', second_name = '{surname}', " +
                $"middle_name = '{middlename}', email = '{email}', phone = '{phone}' " +
                $"WHERE id_user_info = {Request.Cookies["id_user"]}; " +
                $"CALL addlog({Request.Cookies["id_user"]}, 'changed', '{Request.Cookies["id_user"]}', 'tb_users_info', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT;", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("База даних відхилила запит");
            }
            DataBase._connection.Close();
            return new OkResult();
        }

        public object GetUser(int id)
        {
            NpgsqlCommand command = new NpgsqlCommand($"SELECT id_user_info, first_name, second_name, middle_name, email, phone, tb_rank.name_rank, tb_academic_degree.name_academic_degree, tb_chair.name_chair, tb_category_access.name_category_access, user_photo, delete_date " +
                $"FROM public.tb_users_info " +
                $"JOIN public.tb_category_access ON tb_category_access.id_category_access = tb_users_info.id_category_access " +
                $"JOIN public.tb_rank ON tb_rank.id_rank = tb_users_info.id_rank " +
                $"JOIN public.tb_academic_degree ON tb_academic_degree.id_academic_degree = tb_users_info.id_academic_degree " +
                $"JOIN public.tb_chair ON tb_chair.id_chair = tb_users_info.id_chair " +
                $"WHERE id_user_info='{id}'", DataBase._connection);
            DataBase._connection.Open();
            NpgsqlDataReader reader = command.ExecuteReader();
            object[] result = new object[12];
            while (reader.Read()) reader.GetValues(result);
            DataBase._connection.Close();
            return result;
        }
    }
}
