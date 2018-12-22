using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieListBot
{
    class ListService
    {
        public string unwatchedMovieListService(List<string> unwatchedList)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var e in unwatchedList)
            {
                sb.Append($"{e}");
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public string watchedMovieListService(Dictionary<string,string> watchedList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Фильм    Оценка");
            sb.Append("\n");
            string[] columnSize = new string[] { "                    ","        "};
            foreach (var e in watchedList)
            {
                sb.Append($"{e.Key}{columnSize[0]}{e.Value}{columnSize[1]}");
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
