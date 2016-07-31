﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftwareManager.WebApi.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterToUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}