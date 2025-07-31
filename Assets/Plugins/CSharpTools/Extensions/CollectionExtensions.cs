using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSharpTools.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IList<T> collection)
        {
            return collection is not { Count: > 0 };
        }
        
        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
