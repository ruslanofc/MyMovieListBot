using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace MyMovieListBot
{
    public class UnwatchedList
    {
        public int Id { get; set; }
        public string Movie { get; set; }
        public int SenderId { get; set; }
    }

    public class UnwatchedListService : 
    {
        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=postgres";

        public void Save(MovieList entity)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO MyMovieList (id, Movie) " +
                        "VALUES (@id, @Movie)";
                    cmd.Parameters.AddWithValue("Id", entity.Id.ToString());
                    cmd.Parameters.AddWithValue("Movie", entity.Movie);
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public void Update(int id)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    //cmd.connection = conn;
                    //cmd.commandtext = "update mymovielist set class= '" + mymovielist.id.tostring() + "', movie='"
                    //    + mymovielist.movie + "', rating='" + mymovielist.rating.tostring() + "'";
                    //cmd.executenonquery();
                }
            };
        }

        public void Delete(string Movie)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM MyMovieList WHERE id='" + id.ToString() + "'";
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public void Open(MovieList entity)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM MovieList ";
                    cmd.ExecuteNonQuery();
                }
            };
        }
    }
}
