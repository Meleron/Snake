using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    static class ListExtensions
    {
        public static List<T> Swap<T>(this List<T> source, T firstElement, T secondElement)
        {
            //T temp = secondElement;
            source[source.IndexOf(secondElement)] = firstElement;
            source[source.IndexOf(firstElement)] = secondElement;
            return source;
        }
    }
}
