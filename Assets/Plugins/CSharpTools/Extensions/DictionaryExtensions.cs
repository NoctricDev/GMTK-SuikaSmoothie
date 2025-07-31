using System.Collections.Generic;

namespace CSharpTools.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Merge<TSource, TOther>(this IDictionary<TSource, TOther> dictionary, IDictionary<TSource, TOther> other)
        {
            foreach (KeyValuePair<TSource, TOther> pair in other)
            {
                if (!dictionary.ContainsKey(pair.Key))
                {
                    dictionary.Add(pair);
                }
            }
        }
    }   
}