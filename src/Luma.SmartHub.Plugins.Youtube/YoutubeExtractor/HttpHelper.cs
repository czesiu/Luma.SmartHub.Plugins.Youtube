﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Luma.SmartHub.Plugins.Youtube.YoutubeExtractor
{
    internal static class HttpHelper
    {
        public static string HtmlDecode(string value)
        {
            return System.Net.WebUtility.HtmlDecode(value);
        }

        public static IDictionary<string, string> ParseQueryString(Uri s)
        {
            return ParseQueryString(s.ToString());
        }

        public static IDictionary<string, string> ParseQueryString(string s)
        {
            // remove anything other than query string from url
            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
            }

            var dictionary = new Dictionary<string, string>();

            foreach (string vp in Regex.Split(s, "&"))
            {
                string[] strings = Regex.Split(vp, "=");
                dictionary.Add(strings[0], strings.Length == 2 ? UrlDecode(strings[1]) : string.Empty);
            }

            return dictionary;
        }

        public static string ReplaceQueryStringParameter(string currentPageUrl, string paramToReplace, string newValue)
        {
            var query = ParseQueryString(currentPageUrl);

            query[paramToReplace] = newValue;

            var resultQuery = new StringBuilder();
            bool isFirst = true;

            foreach (KeyValuePair<string, string> pair in query)
            {
                if (!isFirst)
                {
                    resultQuery.Append("&");
                }

                resultQuery.Append(pair.Key);
                resultQuery.Append("=");
                resultQuery.Append(pair.Value);

                isFirst = false;
            }

            var uriBuilder = new UriBuilder(currentPageUrl)
            {
                Query = resultQuery.ToString()
            };

            return uriBuilder.ToString();
        }

        public static string UrlDecode(string url)
        {
            return System.Net.WebUtility.UrlDecode(url);
        }
    }
}