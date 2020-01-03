using System;
using System.Collections.Generic;
using System.Linq;
namespace Storage.QueryMethods
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">
        /// type of value or reference which need finding
        /// </typeparam>
        /// <param name="list">Source list</param>
        /// <param name="selector">
        /// A transform function to apply to each element
        /// </param>
        /// <param name="obj">
        /// value or reference which need finding
        /// </param>
        /// <param name="compare">
        /// Delegate to compare obj1 to obj2</param>
        /// <returns>index of element whose selector = obj </returns>
        public static int BinarySearch<T,TElement>(IList<TElement> list, Func<TElement, T> selector, T obj, Func<T, T, int> compare)
        {
            int start = 0;
            int end = list.Count - 1;
            int middle, _compare;
            while (start <= end)
            {
                middle = (start + end) / 2;
                _compare = compare(selector(list[middle]), obj);
                if (_compare == 0) return middle;
                else if (_compare < 0)
                    start = middle + 1;
                else end = middle - 1;
            }
            if (~start >= 0) throw new Exception("result != -1");
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="obj">
        /// The object to locate in the list. The value can be null for reference types.</param>
        /// <param name="compare">
        /// Delegate to compare object1 to object2
        /// </param>
        /// <returns>
        /// index, list[index-1] \<= list[index] \<= list[index+1] 
        /// </returns>
        public static int BinarySearchRelatively<T>(IList<T> list, T obj, Func<T, T, int> compare)
        {
            int start = 0;
            int end = list.Count - 1;
            if (list.Count == 0) return 0;
            if (compare(obj, list[end]) > 0) return list.Count;
            if (compare(obj, list[start]) < 0) return 0;
            int middle, _compare;
            while (start < end - 1)
            {
                middle = (start + end) / 2;
                _compare = compare(list[middle], obj);
                if (_compare > 0)
                    end = middle;
                else if (_compare < 0)
                    start = middle;
                else return middle;
            }
            return end;
        }
    }
}
