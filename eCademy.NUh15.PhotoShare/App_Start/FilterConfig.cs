﻿using System.Web;
using System.Web.Mvc;

namespace eCademy.NUh15.PhotoShare
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
