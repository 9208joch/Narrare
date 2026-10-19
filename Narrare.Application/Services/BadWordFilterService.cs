using BogaNet.BWF;
using System;
using System.Collections.Generic;
using System.Text;
using BogaNet.BWF.Filter;
using BogaNet.BWF.Enum;

namespace Narrare.Application.Services;

public class BadWordFilterService
{
    public BadWordFilterService()
    {
        var filter = BadWordFilter.Instance;

        // Ersätt varje träff med asterisker.
        filter.ReplaceCharacters = new[] { '*' };

        // Ladda svenska och engelska ordlistor.
        filter.LoadFiles(
            true,
            new Tuple<string, string>(
                "sv",
                Path.Combine(
                    AppContext.BaseDirectory,
                    "Resources",
                    "sv.txt")),
            new Tuple<string, string>(
                "en",
                Path.Combine(
                    AppContext.BaseDirectory,
                    "Resources",
                    "en.txt")));
    }

    public string Filter(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var filter = BadWordFilter.Instance;

        if (!filter.IsLoaded)
        {
            throw new InvalidOperationException(
                "Ordlistan för fula ord har inte laddats.");
        }

        var badWords = filter.GetAll(text);

        foreach (var badWord in badWords)
        {
            text = System.Text.RegularExpressions.Regex.Replace(
                text,
                System.Text.RegularExpressions.Regex.Escape(badWord),
                "*********",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        return text;
    }
}