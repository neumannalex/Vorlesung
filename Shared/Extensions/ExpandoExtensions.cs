using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;

namespace Vorlesung.Shared.Extensions
{
    public static class ExpandoExtensions
    {
        public static T FirstOrDefault<T>(this ExpandoObject obj, string key)
        {
            object item = obj.FirstOrDefault(x => x.Key == key).Value;
            return (item is T) ? (T)item : default(T);
        }
    }
}
