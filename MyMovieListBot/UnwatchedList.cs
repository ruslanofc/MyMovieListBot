using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieListBot
{
    public class UnwatchedList 
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Movie { get; set; }
    }

    public class UnwatchedListService : IUnwatchedList
    {
        public void Save(UnwatchedList entity)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=UnwatchedList";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO UnwatchedList (id, movie, sender_id) " +
                        "VALUES (@id, @movie, @sender_id)";
                    cmd.Parameters.AddWithValue("id", Convert.ToInt32(entity.Id));
                    cmd.Parameters.AddWithValue("movie", entity.Movie);
                    cmd.Parameters.AddWithValue("sender_id", Convert.ToInt32(entity.SenderId));
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public void Delete(string Movie)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=UnwatchedList";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM UnwatchedList WHERE Movie='" + Movie.ToString() + "'";
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public List<string> OpenUnwatchedList(int a)
        {
            var movieList = new List<string>();
            var current_id = a;
            var command = ($"SELECT movie FROM UnwatchedList WHERE sender_id = "+current_id+" ");
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=UnwatchedList";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(command, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var movie = reader[0].ToString();

                        movieList.Add(movie);
                    }
                }
            };

            return movieList;
        }

        //public bool FilmFactChecking(string a)
        //{
        //    var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=UnwatchedList";

        //    var current_film = a;

        //    using (var conn = new NpgsqlConnection(connString))
        //    {
        //        conn.Open();

        //        using (var cmd = new NpgsqlCommand())
        //        {
        //            cmd.Connection = conn;
        //            cmd.CommandText = ($"SELECT movie FROM UnwatchedList WHERE movie ="+ current_film +" ");
        //            cmd.
        //            cmd.ExecuteNonQuery();
        //        }
        //    };
        //}
    }
}
