using Npgsql;

namespace CourseWork.Models
{
    public static class DataBase
    {
        public static readonly string _connectionString = "Server=localhost;User id=postgres; Password=1304;Port=5432;Database=Teachers";
        public static readonly NpgsqlConnection _connection = new NpgsqlConnection(_connectionString);
    }
}
