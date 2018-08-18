using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Clears the ObservableCollection and re-fills it with the given items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        public static void Refill<T>(this ObservableCollection<T> source, IEnumerable<T> items)
        {
            source.Clear();
            foreach (T i in items) {
                source.Add(i);
            }
        }
    }
}
