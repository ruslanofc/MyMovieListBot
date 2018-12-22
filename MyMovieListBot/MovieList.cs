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
        public string Movie { get; set; }
        public string Rating { get; set; }
        public int SenderId { get; set; }
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
                    cmd.CommandText = "INSERT INTO MyMovieList (movie, rating, sender_id) " +
                        "VALUES (@movie, @rating, @sender_id)";
                    cmd.Parameters.AddWithValue("movie", entity.Movie);
                    cmd.Parameters.AddWithValue("rating", Convert.ToInt32(entity.Rating));
                    cmd.Parameters.AddWithValue("sender_id", Convert.ToInt32(entity.SenderId));
                    cmd.ExecuteNonQuery();
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

        public Dictionary<string, string> OpenList(int a)
        {
            var movieList = new Dictionary<string, string>();
            var current_id = a;
            var command = ($"SELECT movie, rating FROM MyMovieList WHERE sender_id =" + current_id + " ");
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=MyMovieList";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(command, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    foreach (var i in reader)
                    {
                        movieList.Add(reader[0].ToString(), reader[1].ToString());
                    }
                }
            };

            return movieList;

        }

        //public Dictionary<string, string> ListWithRating(int a, string b)
        //{
        //    var movieList = new Dictionary<string, string>();
        //    var current_id = a;
        //    var current_rating = b;
        //    var command = ($"SELECT movie, rating FROM MyMovieList WHERE sender_id =" + current_id + " and rating ="+ current_rating.ToString() +" ");
        //    var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=MyMovieList";

        //    using (var conn = new NpgsqlConnection(connString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(command, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            foreach (var i in reader)
        //            {
        //                movieList.Add(reader[0].ToString(), reader[1].ToString());
        //            }
        //        }
        //    };

        //    return movieList;
        //}
    }
}