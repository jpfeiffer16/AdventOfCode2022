public static class Day3
{
    public static void Run()
    {
        var backpacks = File
            .ReadAllLines("./day3/input.txt")
            .Select(l =>
                    (first: l.Substring(0, l.Length / 2),
                     second: l.Substring(l.Length / 2),
                     everything: l))
            .ToList();

        var priorities = backpacks.Select(b =>
        {
            char duplicateItem = (char)0;
            foreach (var item in b.first)
            {
                duplicateItem = item;
                if (b.second.Contains(item))
                {
                    break;
                }
            }

            return GetPriority(duplicateItem);
        }).ToList();

        Console.WriteLine("Sum: {0}", priorities.Sum());

        var groups = new List<List<string>>();
        var counted = 0;
        while (counted != backpacks.Count)
        {
            groups.Add(backpacks
                       .Skip(counted)
                       .Take(3)
                       .Select(b => b.everything).ToList());
            counted += 3;
        }

        var badgePriorities = groups.Select(g =>
                g.OrderBy(d => d.Length).ToList()).ToList()
                .Select(g =>
                {
                    var duplicateItem = (char)0;
                    foreach (var item in g.First())
                    {
                        duplicateItem = item;
                        if (g[1].Contains(item)
                            && g[2].Contains(item))
                        {
                            break;
                        }
                    }
                    return GetPriority(duplicateItem);
                }).ToList();

        Console.WriteLine("Badges sum: {0}", badgePriorities.Sum());
    }

    private static int GetPriority(char letter)
    {
        var asciiCode = (int)letter;
        if (asciiCode >= 97 && asciiCode <= 122)
            return asciiCode - 96;
        if (asciiCode >= 65 && asciiCode <= 90)
            return asciiCode - 38;

        throw new Exception(
                $"Unable to get priority for letter: {letter}");
    }
}
