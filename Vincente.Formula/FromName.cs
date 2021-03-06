﻿using System;
using System.Collections.Generic;

namespace Vincente.Formula
{
    public class FromName
    {
        public static string GetShortName(string fullName, string epic)
        {
            var shortName = GetShortName(fullName);

            var match = string.Concat(epic, " - ");

            if (shortName.ToLower().StartsWith(match.ToLower()))
            {
                return shortName.Substring(match.Length);
            }

            return shortName;
        }

        public static string GetShortName(string fullName)
        {
            string name;

            var domId = Formula.FromName.GetDomID(fullName);

            if (domId == null)
            {
                name = fullName;
            }
            else
            {
                var pos = fullName.IndexOf(domId.Substring(1));

                if (pos == 0)
                {
                    name = "";
                }
                else
                {
                    name = fullName.Substring(pos + domId.Length);
                }
            }

            name = name.Replace("â€“", "-");
            name = name.Replace("â€˜", "'");
            name = name.Replace("â€™", "'");

            if (name.StartsWith("- ")) name = name.Substring(2);

            name = name.Trim();

            return name;
        }

        public static string GetDomID(string name)
        {
            if (name == null) return null;

            string[] words = name.Split(' ');

            foreach (string word in words)
            {

                if (word.StartsWith("20") && word.Contains("."))
                {
                    var decimalIndex = word.IndexOf(".");

                    if (decimalIndex != 8)
                    {
                        return null;
                    }

                    // Just a dot.
                    if (word.Length - decimalIndex < 2)
                    {
                        return null;
                    }

                    // Have a sensible max length for extensions, assume 99z is longest.
                    if (word.Length - decimalIndex > 4)
                    {
                        return null;
                    }

                    return String.Format("D{0}", word);
                }
            }

            return null;
        }
    }
}
