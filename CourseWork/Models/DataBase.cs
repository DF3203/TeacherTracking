using Npgsql;

namespace CourseWork.Models
{
    public class DataBase
    {
        public static readonly string _connectionString = "Server=localhost;User id=postgres; Password=1304;Port=5433;Database=Teachers";
        public static readonly NpgsqlConnection _connection = new NpgsqlConnection(_connectionString);
        public NpgsqlCommand Command = _connection.CreateCommand();


    }
}
