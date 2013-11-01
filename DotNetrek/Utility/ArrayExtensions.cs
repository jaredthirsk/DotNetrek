using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionFire.Netrek
{
    public static class ArrayExtensions
    {
        public static T Get<T>(this ArrayList list, int id)
            where T : class, new()
        {
            while (list.Count <= id)
            {
                list.Add(null);
            }

            T item = list[id] as T;
            if (item == null)
            {
                item = new T();
                list[id] = item;
            }
            return item;
        }

        public static T Get<T>(this IList<T> list, int id, Func<int, T> createMethod = null)
            where T : class, new()
        {
            while (list.Count <= id)
            {
                list.Add(null);
            }

            T item = list[id] as T;
            if (item == null)
            {
                if (createMethod != null)
                {
                    item = createMethod(id);
                }
                else
                {
                    item = new T();
                }
                
                list[id] = item;
            }
            return item;
        }

    }
}
