using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Npgsql;

namespace CourseWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public object LogIn(string login, string password)
        {
            NpgsqlCommand command = new NpgsqlCommand($"SELECT public.checkuser('{login}','{password}','{Request.HttpContext.Connection.RemoteIpAddress}')", DataBase._connection);
            DataBase._connection.Open();
            NpgsqlDataReader reader = command.ExecuteReader();
            object result = new object();
            while (reader.Read()) result = reader.GetValue(0);
            DataBase._connection.Close();
            _logger.LogInformation($"Attempt to log in with username {login} and password {password} and ip {Request.HttpContext.Connection.RemoteIpAddress}");
            return result;
        }
        public object findByLogin(string login)
        {
            NpgsqlCommand command = new NpgsqlCommand($"SELECT id_user FROM tb_users WHERE login='{login}'", DataBase._connection);
            DataBase._connection.Open();
            NpgsqlDataReader reader = command.ExecuteReader();
            object result = new object();
            while (reader.Read()) result = reader.GetValue(0);
            DataBase._connection.Close();
            return result;
        }

        public object getAccess(int id)
        {
            NpgsqlCommand command = new NpgsqlCommand($"SELECT users_priv, institute_priv, faculrty_priv, chair_priv " +
                $"FROM public.tb_category_access " +
                $"JOIN public.tb_users_info ON tb_users_info.id_category_access = tb_category_access.id_category_access " +
                $"WHERE tb_users_info.id_user_info={id}", DataBase._connection);
            DataBase._connection.Open();
            NpgsqlDataReader reader = command.ExecuteReader();
            object[] result = new object[4];
            while (reader.Read()) reader.GetValues(result);
            DataBase._connection.Close();
            return result;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}