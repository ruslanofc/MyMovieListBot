using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieListBot
{
    interface IDataService
    {
        void Save(MovieList entity);
        void Delete(string Movie);
        Dictionary<string, string> OpenList();
    }
}
