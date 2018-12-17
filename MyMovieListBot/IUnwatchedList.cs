using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieListBot
{
    interface IUnwatchedList
    {
        void Save(UnwatchedList entity);
        void Delete(string Movie);
        List<string> OpenUnwatchedList();
    }
}
