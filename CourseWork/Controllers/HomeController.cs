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
            NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM checkuser('{login}','{password}')", DataBase._connection);
            DataBase._connection.Open();
            NpgsqlDataReader reader = command.ExecuteReader();
            object result = new object();
            if (reader.HasRows)
            {
                while (reader.Read())
                    result = reader.GetValue(0);
            }
            else result = false;
            DataBase._connection.Close();
            _logger.LogInformation($"Attempt to log in with username {login} and password {password}");
            return String.IsNullOrEmpty(result.ToString())?false:result;
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