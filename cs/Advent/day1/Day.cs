/// <summary>
/// Day1.
/// </summary>
public static class Day1 {
    /// <summary>
    /// Run.
    /// </summary>
    public static void Run() {
        var file = File.ReadAllLines("./day1/input.txt");
        var current = new List<int>();
        var list = new List<List<int>> { current };

        foreach (var line in file)
        {
            if (string.IsNullOrWhiteSpace(line.Trim()))
            {
                list.Add(current = new List<int>());
            }
            else
            {
                current.Add(int.Parse(line.Trim()));
            }
        }

        Console.WriteLine("Top 1: {0}", list.Select(g => g.Sum()).Max());
        Console.WriteLine("Top 3: {0}", list.Select(g => g.Sum()).OrderByDescending(i => i).Take(3).Sum());
    }
}
