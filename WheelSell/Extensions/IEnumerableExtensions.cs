﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WheelSell.Models;

namespace WheelSell.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> Items)
        {
            List<SelectListItem> List = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem
            {
                Text = "---Select---",
                Value = "0"
            };

            List.Add(sli);
            foreach(var Item in Items)
            {
                sli = new SelectListItem
                {
                    //Text = Item.GetType().GetProperty("Name").GetValue(Item,null).ToString(),
                    Text = Item.GetPropertyValue("Name"),
                    Value = Item.GetPropertyValue("Id")
                    //Value = Item.GetType().GetProperty("Id").GetValue(Item,null).ToString() 
                };
                List.Add(sli);

            }
            return List;

        }
    }
}
