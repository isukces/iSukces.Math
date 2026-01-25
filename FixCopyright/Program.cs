namespace FixCopyright;

internal class Program
{
    public static void Main(string[] args)
    {
        var folder = SearchFoldersUntilFileExists(new DirectoryInfo(Directory.GetCurrentDirectory()), "iSukces.Math.sln");

        var ms = Path.Combine(folder!.FullName,  "iSukces.Mathematics", "_ms");
        Console.WriteLine(ms);
        foreach (var i in Directory.GetFiles(ms, "*.cs", SearchOption.AllDirectories))
        {
            var before = File.ReadAllText(Path.Combine(ms, i));
            var lines  = GetLines(before);
            var after  = string.Join("\r\n", lines) + "\r\n";
            if (before != after)
                File.WriteAllText(i, after);
        }
    }

    private static IEnumerable<string> GetLines(string before)
    {

        var year = DateTime.Now.Year;
        yield return $"// Copyright © Internet Sukces Piotr Stęclik 2017-{year}. All rights reserved.";
        yield return "// Licensed under the MIT license.";
        yield return "//";
        yield return "// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).";
        yield return "// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.";

        before = before.Replace("\r\n", "\n").Trim();
        var wasNormalLine = false;
        foreach (var i in before.Split('\n'))
        {
            if (wasNormalLine)
            {
                yield return i;
                continue;
            }
            
            if (string.IsNullOrWhiteSpace(i) || i.Trim().StartsWith("//"))
                continue;
            yield return "";
            yield return i;
            wasNormalLine = true;
        }
    }

    public static DirectoryInfo? SearchFoldersUntilFileExists(DirectoryInfo di, string fileName)
    {
        for (; di != null && di.Exists; di = di.Parent)
        {
            if (File.Exists(Path.Combine(di.FullName, fileName)))
                return di;
        }
        return (DirectoryInfo) null;
    }
}