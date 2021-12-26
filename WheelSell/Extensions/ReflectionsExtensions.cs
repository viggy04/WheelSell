using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelSell.Extensions
{
    public static class ReflectionsExtensions
    {
        public static string GetPropertyValue<T>(this T Item, string propertyName)
        {
            return Item.GetType().GetProperty(propertyName).GetValue(Item, null).ToString();
        }
    }
}
