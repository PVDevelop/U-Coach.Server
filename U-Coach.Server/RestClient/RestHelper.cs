using System;
using System.Collections.Generic;

namespace PVDevelop.UCoach.Server.RestClient
{
    public static class RestHelper
    {
        public static string FormatUri(string uri, params object[] segments)
        {
            if(segments.Length == 0)
            {
                return uri;
            }

            var chars = new List<char>(uri.Length);
            bool isOpen = false;
            var argsEnumerator = segments.GetEnumerator();
            foreach (var ch in uri)
            {
                if (ch == '{')
                {
                    if(isOpen)
                    {
                        throw new FormatException("Скобка открыта дважды");
                    }
                    isOpen = true;
                }
                else if (ch == '}')
                {
                    if(!argsEnumerator.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException("Недостаточно аргументов");
                    }
                    chars.AddRange(argsEnumerator.Current.ToString());
                    isOpen = false;
                }
                else if(!isOpen)
                {
                    chars.Add(ch);
                }
            }

            return new string(chars.ToArray());
        }
    }
}
