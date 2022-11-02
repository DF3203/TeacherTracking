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

        #region User
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
            catch (Exception ex)
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("База даних відхилила запит");
            }
            DataBase._connection.Close();
            return new OkResult();
        }

        public IActionResult DeleteUser(int id)
        {
            if ((Request.Cookies["user_priv"] != "true"))
                return new UnauthorizedResult();
            NpgsqlCommand command = new NpgsqlCommand($"BEGIN; " +
                $"DELETE FROM tb_users WHERE id_user = {id}; " +
                $"UPDATE tb_users_info SET delete_date = '{DateTime.Now.Date}' WHERE id_user_info = {id}; " +
                $"call addlog({Request.Cookies["id_user"]}, 'delete_user', '{id}', 'tb_users', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT;", DataBase._connection);
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
            return new OkResult();
        }

        #endregion

        #region Chair
        public IActionResult DeleteChair(int id)
        {
            if ((Request.Cookies["chair_priv"] != "true"))
                return new UnauthorizedResult();
            NpgsqlCommand command = new NpgsqlCommand($"BEGIN; " +
                $"DELETE FROM tb_chair WHERE id_chair = {id}; " +
                $"call addlog({Request.Cookies["id_user"]}, 'delete_chair', '{id}', 'tb_chair', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT;", DataBase._connection);
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
            return new OkResult();
        }
        #endregion


    }
}
