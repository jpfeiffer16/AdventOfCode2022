public static class Day8
{
    public static void Run()
    {
        var forest = File.ReadAllLines("./day8/input.txt")
            .Select(l => l.ToList()
                          .Select(i => i - 48)
                          .ToList()).ToList();

        Console.WriteLine(
                "Total Visible: {0}",
                forest.SelectMany((r, y) =>
                    r.Where((_, x) =>
                        IsVisible((x, y), forest))).Count());

        Console.WriteLine(
                "Total Scores: {0}",
                forest.SelectMany((r, y) => 
                    r.Select((_, x) => 
                        ScoreTree((x, y), forest))).Max());
    }

#region Part2
    
    private static int ScoreTree(
            (int x, int y) tree, List<List<int>> forest) =>
        (ScoreUp(tree, forest))
        * (ScoreLeft(tree, forest))
        * (ScoreRight(tree, forest))
        * (ScoreDown(tree, forest));

    private static int ScoreUp(
            (int x, int y) tree, List<List<int>> forest) =>
        forest
            .Take(tree.y)
            .Select(r => r[tree.x])
            .Reverse()
            .TakeTill(t => t >= forest[tree.y][tree.x])
            .Count();

    private static int ScoreLeft(
            (int x, int y) tree, List<List<int>> forest) =>
        forest[tree.y]
            .Take(tree.x)
            .Reverse()
            .TakeTill(t => t >= forest[tree.y][tree.x])
            .Count();

    private static int ScoreRight(
            (int x, int y) tree, List<List<int>> forest) =>
        forest[tree.y]
            .Skip(tree.x + 1)
            .TakeTill(t => t >= forest[tree.y][tree.x])
            .Count();

    private static int ScoreDown(
            (int x, int y) tree, List<List<int>> forest) =>
        forest
            .Skip(tree.y + 1)
            .Select(r => r[tree.x])
            .TakeTill(t => t >= forest[tree.y][tree.x])
            .Count();

#endregion

#region Part1

    private static bool IsVisible(
            (int x, int y) tree,
            List<List<int>> forest) => tree switch
        {
            var (x, y) when x == 0 || y == 0 => true,
            var (x, y) when x == forest.First().Count() - 1
                         || y == forest.Count() - 1 => true,
            var t when CheckUp(tree, forest)
                    || CheckLeft(t, forest)
                    || CheckRight(t, forest)
                    || CheckDown(t, forest) => true,
            _ => false
        };

    private static bool CheckUp(
            (int x, int y) tree, List<List<int>> forest) =>
        forest
            .Take(tree.y)
            .Select(r => r[tree.x])
            .All(l => l < forest[tree.y][tree.x]);

    private static bool CheckLeft(
            (int x, int y) tree, List<List<int>> forest) =>
        forest[tree.y]
            .Take(tree.x)
            .All(l => l < forest[tree.y][tree.x]);

    private static bool CheckRight(
            (int x, int y) tree, List<List<int>> forest) =>
        forest[tree.y]
            .Skip(tree.x + 1)
            .All(l => l < forest[tree.y][tree.x]);

    private static bool CheckDown(
            (int x, int y) tree, List<List<int>> forest) =>
        forest
            .Skip(tree.y + 1)
            .Select(r => r[tree.x])
            .All(l => l < forest[tree.y][tree.x]);

#endregion
}
