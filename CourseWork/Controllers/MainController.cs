using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Npgsql;

namespace CourseWork.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            if (Request.Cookies["user_priv"] != "true")
                return BadRequest();
            else
                return View();

        }

        public IActionResult Exit()
        {
            NpgsqlCommand command = new NpgsqlCommand($"call addlog({Request.Cookies["id_user"]}, 'unlogged', 'user', 'tb_users', '{Request.HttpContext.Connection.RemoteIpAddress}', 1)", DataBase._connection);
            DataBase._connection.Open();
            command.ExecuteNonQuery();
            return Redirect("../");
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
