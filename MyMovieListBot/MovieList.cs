using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace MyMovieListBot
{
    public class MovieList
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Movie { get; set; }
        public string Rating { get; set; }
    }

    public class MovieListService : IDataService
    {
        public void Save(MovieList entity)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=MyMovieList";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO MyMovieList (id, movie, rating, sender_id) " +
                        "VALUES (@id, @movie, @rating, @sender_id)";
                    cmd.Parameters.AddWithValue("id", Convert.ToInt32(entity.Id));
                    cmd.Parameters.AddWithValue("movie", entity.Movie);
                    cmd.Parameters.AddWithValue("rating", Convert.ToInt32(entity.Rating));
                    cmd.Parameters.AddWithValue("sender_id", Convert.ToInt32(entity.SenderId));
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public void Update(int id)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=MyMovieList";

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
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=MyMovieList";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM MyMovieList WHERE Movie='" + Movie.ToString() + "'";
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public void Open(MovieList entity)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=MyMovieList";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM MyMovieList ";
                    cmd.ExecuteNonQuery();
                }
            };
        }
    }
}