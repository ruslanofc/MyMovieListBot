using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MyMovieListBot
{
    class MovieListService<T> : IDataService<T> where T : MovieList
    {
        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=postgres";

        public void Save(T MovieList)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO MyMovieList (id, Movie, Rating) " +
                        "VALUES (@id, @Movie, @Rating)";
                    cmd.Parameters.AddWithValue("Id", MovieList.Id.ToString());
                    cmd.Parameters.AddWithValue("Movie", MovieList.Movie);
                    cmd.Parameters.AddWithValue("Rating", MovieList.Rating.ToString());              
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public void Update(long id, T MyMovieList)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE MyMovieList SET class= '" + MyMovieList.Id.ToString() + "', Movie='"
                        + MyMovieList.Movie + "', Rating='" + MyMovieList.Rating.ToString() + "'";
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public void Delete(long id)
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
    }
}

