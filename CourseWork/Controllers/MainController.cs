using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Npgsql;
using System.Numerics;
using System.Xml.Linq;
using System.IO;

namespace CourseWork.Controllers
{
    public class MainController : Controller
    {
        public IActionResult LoadBackup(DateTime date)
        {
            if ((Request.Cookies["chair_priv"] != "true") || (Request.Cookies["fac_priv"] != "true") || (Request.Cookies["inst_priv"] != "true") || (Request.Cookies["user_priv"] != "true"))
                return new BadRequestObjectResult("Немає прав");
            if (!System.IO.File.Exists($"C:\\Users\\viach\\Desktop\\{date.Day}.{date.Month}.{date.Year}.backup"))
                return new BadRequestObjectResult($"Копії бази даних за {date.Day}.{date.Month}.{date.Year} немає");
            string command = $"\"C:\\Program Files\\PostgreSQL\\11\\bin\\pg_restore.exe\" --clean --disable-triggers --format=c --dbname=postgresql://postgres:1304@localhost:5432/Teachers < C:\\Users\\viach\\Desktop\\{date.Day}.{date.Month}.{date.Year}.backup";
            System.Diagnostics.Process.Start("cmd.exe", "/C " + command);
            return new OkResult();
        }

        #region Pages
        public IActionResult Index()
        {
            if(!System.IO.File.Exists($"C:\\Users\\viach\\Desktop\\{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}.backup"))
            {
                string command = $"\"C:\\Program Files\\PostgreSQL\\11\\bin\\pg_dump.exe\" --format=c --dbname=postgresql://postgres:1304@localhost/Teachers " +
                    $"> C:\\Users\\viach\\Desktop\\{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}.backup";
                System.Diagnostics.Process.Start("cmd.exe", "/C " + command);
            }
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
        public IActionResult ChangeInstitute(int id)
        {
            if (Request.Cookies["inst_priv"] != "true")
                return new UnauthorizedResult();
            else
            {
                ViewBag.InstituteId = id;
                return View();
            }
        }
        public IActionResult ChangeFaculty(int id)
        {
            if (Request.Cookies["fac_priv"] != "true")
                return new UnauthorizedResult();
            else
            {
                ViewBag.FacultyId = id;
                return View();
            }
        }
        public IActionResult ChangeChair(int id)
        {
            if (Request.Cookies["chair_priv"] != "true")
                return new UnauthorizedResult();
            else
            {
                ViewBag.ChairId = id;
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

        public IActionResult ChangeUserAdmin(int id, string name, string surname, string middlename, string phone, 
            string email, string rank, string degree, string chair, string level)
        {
            if ((Request.Cookies["user_priv"] != "true"))
                return new UnauthorizedResult();
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(surname) || String.IsNullOrEmpty(middlename)
                || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email))
                return new BadRequestObjectResult("Поля не можуть бути пустими");
            name = name.Replace(" ", "");
            surname = surname.Replace(" ", "");
            middlename = middlename.Replace(" ", "");
            phone = phone.Replace(" ", "");
            email = email.Replace(" ", "");
            NpgsqlCommand command = new NpgsqlCommand($"BEGIN; " +
                $"update tb_users_info " +
                $"set first_name = '{name}', second_name = '{surname}', middle_name = '{middlename}', email = '{email}', " +
                $"phone = '{phone}', id_rank = (select id_rank from tb_rank where name_rank = '{rank}'), " +
                $"id_academic_degree = (select id_academic_degree from tb_academic_degree where name_academic_degree = '{degree}'), " +
                $"id_chair = (select id_chair from tb_chair where name_chair = '{chair}'), " +
                $"id_category_access = (select id_category_access from tb_category_access where name_category_access = '{level}') " +
                $"where id_user_info = {id}; " +
                $"CALL addlog({Request.Cookies["id_user"]}, 'changed_user', '{id}', 'tb_users_info', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
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

        public IActionResult ActivateUser(int id, string login, string password)
        {
            if ((Request.Cookies["user_priv"] != "true"))
                return new UnauthorizedResult();
            NpgsqlCommand command = new NpgsqlCommand($"BEGIN; " +
               $"UPDATE tb_users_info " +
               $"SET delete_date = NULL " +
               $"where id_user_info = {id}; " +
               $"CALL insertuser({id}, '{login}', '{password}'); " +
               $"call addlog({Request.Cookies["id_user"]}, 'activate_user', '{id}', 'tb_users', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
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
        public IActionResult AddUser(string name, string surname, string middlename, string phone,
            string email, string rank, string degree, string chair, string level)
        {
            if ((Request.Cookies["user_priv"] != "true"))
                return new UnauthorizedResult();
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(surname) || String.IsNullOrEmpty(middlename)
                || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email))
                return new BadRequestObjectResult("Поля не можуть бути пустими");
            name = name.Replace(" ", "");
            surname = surname.Replace(" ", "");
            middlename = middlename.Replace(" ", "");
            phone = phone.Replace(" ", "");
            email = email.Replace(" ", "");
            NpgsqlCommand command = new NpgsqlCommand($"BEGIN; " +
                $"insert into tb_users_info (first_name, second_name, middle_name, email, phone, id_rank, id_academic_degree, id_chair, id_category_access) " +
                $"values ('{name}', '{surname}', '{middlename}', '{email}', '{phone}', " +
                $"(select id_rank from tb_rank where name_rank = '{rank}'), " +
                $"(select id_academic_degree from tb_academic_degree where name_academic_degree = '{degree}'), " +
                $"(select id_chair from tb_chair where name_chair = '{chair}'), " +
                $"(select id_category_access from tb_category_access where name_category_access = '{level}')); " +
                $"call addlog({Request.Cookies["id_user"]}, 'add_user', 'new_user', 'tb_users_info', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
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

        public IActionResult AddChair(string name, string faculty, string chief)
        {
            if ((Request.Cookies["chair_priv"] != "true"))
                return new UnauthorizedResult();
            int user_id = -1;
            NpgsqlCommand command = new NpgsqlCommand($"SELECT id_user_info FROM tb_users_info where second_name || ' ' || first_name || ' ' ||  middle_name = '{chief}';", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    user_id = Convert.ToInt16(reader.GetValue(0));
                if (user_id == -1)
                    throw new Exception("Невірний користувач");
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("Такого користувача не існує");
            }
            DataBase._connection.Close();
            command = new NpgsqlCommand($"BEGIN; " +
                $"INSERT INTO tb_chair (name_chair, id_faculty, id_user_chief) values ('{name}', " +
                $"(SELECT id_faculty FROM tb_faculty WHERE abbreviation_faculty = '{faculty}'), {user_id}); " +
                $"call addlog({Request.Cookies["id_user"]}, 'add_chair', 'new_chair', 'tb_chair', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT; ", DataBase._connection);
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

        public IActionResult UpdateChair(string id, string name, string faculty, string chief)
        {
            if ((Request.Cookies["chair_priv"] != "true"))
                return new UnauthorizedResult();
            int user_id = -1;
            NpgsqlCommand command = new NpgsqlCommand($"SELECT id_user_info FROM tb_users_info where second_name || ' ' || first_name || ' ' ||  middle_name = '{chief}';", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    user_id = Convert.ToInt16(reader.GetValue(0));
                if (user_id == -1)
                    throw new Exception("Невірний користувач");
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("Такого користувача не існує");
            }
            DataBase._connection.Close();
            command = new NpgsqlCommand($"BEGIN; " +
                $"UPDATE tb_chair " +
                $"SET name_chair = '{name}', " +
                $"id_faculty = (SELECT id_faculty FROM tb_faculty WHERE abbreviation_faculty = '{faculty}'),  id_user_chief = {user_id}" +
                $"WHERE id_chair = {id}; " +
                $"call addlog({Request.Cookies["id_user"]}, 'change_chair', '{user_id}', 'tb_chair', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT; ", DataBase._connection);
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

        #region Faculty
        public IActionResult DeleteFaculty(int id)
        {
            if ((Request.Cookies["fac_priv"] != "true"))
                return new UnauthorizedResult();
            NpgsqlCommand command = new NpgsqlCommand($"BEGIN; " +
                $"DELETE FROM tb_faculty WHERE id_faculty = {id}; " +
                $"call addlog({Request.Cookies["id_user"]}, 'delete_faculty', '{id}', 'tb_faculty', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
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

        public IActionResult UpdateFaculty(int id, string name, string abbreviation,
            string institute, string dean, string deputy)
        {

            if ((Request.Cookies["fac_priv"] != "true"))
                return new UnauthorizedResult();
            int _dean = -1;
            int _deputy_dean = -1;
            NpgsqlCommand command = new NpgsqlCommand($"SELECT id_user_info FROM tb_users_info where second_name || ' ' || first_name || ' ' ||  middle_name = '{dean}';", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    _dean = Convert.ToInt16(reader.GetValue(0));
                if (_dean == -1)
                    throw new Exception("Невірний користувач");
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("Такого користувача не існує");
            }
            DataBase._connection.Close();
            command = new NpgsqlCommand($"SELECT id_user_info FROM tb_users_info where second_name || ' ' || first_name || ' ' ||  middle_name = '{deputy}';", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    _deputy_dean = Convert.ToInt16(reader.GetValue(0));
                if (_deputy_dean == -1)
                    throw new Exception("Невірний користувач");
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("Такого користувача не існує");
            }
            DataBase._connection.Close();
            command = new NpgsqlCommand($"BEGIN; " +
                $"UPDATE tb_faculty " +
                $"SET name_faculty = '{name}', abbreviation_faculty = '{abbreviation}', " +
                $"id_institute = (select id_institute from tb_institute where abbreviation_institute = '{institute}'), id_user_dean = {_dean}, " +
                $"id_user_deputy_dean = {_deputy_dean} " +
                $"WHERE id_faculty = {id}; " +
                $"call addlog({Request.Cookies["id_user"]}, 'change_faculty', '{id}', 'tb_faculty', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT; ", DataBase._connection);
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

        public IActionResult AddFaculty(string name, string abbreviation,
            string institute, string dean, string deputy)
        {

            if ((Request.Cookies["fac_priv"] != "true"))
                return new UnauthorizedResult();
            int _dean = -1;
            int _deputy_dean = -1;
            NpgsqlCommand command = new NpgsqlCommand($"SELECT id_user_info FROM tb_users_info where second_name || ' ' || first_name || ' ' ||  middle_name = '{dean}';", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    _dean = Convert.ToInt16(reader.GetValue(0));
                if (_dean == -1)
                    throw new Exception("Невірний користувач");
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("Такого користувача не існує");
            }
            DataBase._connection.Close();
            command = new NpgsqlCommand($"SELECT id_user_info FROM tb_users_info where second_name || ' ' || first_name || ' ' ||  middle_name = '{deputy}';", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    _deputy_dean = Convert.ToInt16(reader.GetValue(0));
                if (_deputy_dean == -1)
                    throw new Exception("Невірний користувач");
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("Такого користувача не існує");
            }
            DataBase._connection.Close();
            command = new NpgsqlCommand($"BEGIN; " +
                $"insert into tb_faculty (name_faculty, abbreviation_faculty, id_institute, id_user_dean, id_user_deputy_dean) " +
                $"values ('{name}', '{abbreviation}', (select id_institute from tb_institute where abbreviation_institute = '{institute}'), " +
                $"{_dean}, {_deputy_dean}); " +
                $"call addlog({Request.Cookies["id_user"]}, 'add_faculty', 'new_faculty', 'tb_faculty', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT; ", DataBase._connection);
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

        #region Institute

        public IActionResult DeleteInstitute(int id)
        {
            if ((Request.Cookies["inst_priv"] != "true"))
                return new UnauthorizedResult();
            NpgsqlCommand command = new NpgsqlCommand($"BEGIN; " +
                $"DELETE FROM tb_institute WHERE id_institute = {id}; " +
                $"call addlog({Request.Cookies["id_user"]}, 'delete_institute', '{id}', 'tb_institute', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
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

        public IActionResult AddInstitute(string name, string abbreviation, string chief)
        {
            if ((Request.Cookies["inst_priv"] != "true"))
                return new UnauthorizedResult();
            int user_id = -1;
            NpgsqlCommand command = new NpgsqlCommand($"SELECT id_user_info FROM tb_users_info where second_name || ' ' || first_name || ' ' ||  middle_name = '{chief}';", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) 
                    user_id = Convert.ToInt16(reader.GetValue(0));
                if (user_id == -1)
                    throw new Exception("Невірний користувач");
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("Такого користувача не існує");
            }
            DataBase._connection.Close();
            command = new NpgsqlCommand($"BEGIN; " +
                $"INSERT INTO tb_institute (name_institute, abbreviation_institute, id_user_chief) values ('{name}', '{abbreviation}', {user_id}); " +
                $"call addlog({Request.Cookies["id_user"]}, 'add_institute', 'new_institute', 'tb_institute', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT; ", DataBase._connection);
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

        public IActionResult UpdateInstitute(string id,string name, string abbreviation, string chief)
        {
            if ((Request.Cookies["inst_priv"] != "true"))
                return new UnauthorizedResult();
            int user_id = -1;
            NpgsqlCommand command = new NpgsqlCommand($"SELECT id_user_info FROM tb_users_info where second_name || ' ' || first_name || ' ' ||  middle_name = '{chief}';", DataBase._connection);
            DataBase._connection.Open();
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    user_id = Convert.ToInt16(reader.GetValue(0));
                if (user_id == -1)
                    throw new Exception("Невірний користувач");
            }
            catch
            {
                DataBase._connection.Close();
                return new BadRequestObjectResult("Такого користувача не існує");
            }
            DataBase._connection.Close();
            command = new NpgsqlCommand($"BEGIN; " +
                $"UPDATE tb_institute " +
                $"SET name_institute = '{name}', abbreviation_institute = '{abbreviation}', id_user_chief = {user_id} " +
                $"WHERE id_institute = {id}; " +
                $"call addlog({Request.Cookies["id_user"]}, 'change_institute', '{user_id}', 'tb_institute', '{Request.HttpContext.Connection.RemoteIpAddress}', 1); " +
                $"COMMIT; ", DataBase._connection);
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
