public static class Day4
{
    public static void Run()
    {
        var assignments = File
            .ReadAllLines("./day4/input.txt")
            .Select(l => l.Split(',')
                          .Select(r =>
                              r.Split('-')
                                  .Select(n => int.Parse(n)).ToList())
                          .ToList()
                          .Select(p => (from: p[0], to: p[1]))
                          .ToList()).ToList();

        var contain = assignments.Where(a =>
                (a[0].from >= a[1].from && a[0].to <= a[1].to)
                || (a[0].from <=a[1].from && a[0].to >= a[1].to));

        var overlap = assignments.Where(a =>
                ((a[0].from >= a[1].from && a[0].from <= a[1].to)
                    || (a[0].to >= a[1].from && a[0].to <= a[1].to))
                ||((a[1].from >= a[0].from && a[1].from <= a[0].to)
                    || (a[1].to >= a[0].from && a[1].to <= a[0].to)));

        Console.WriteLine("Contain: {0}", contain.Count());
        Console.WriteLine("Overlap: {0}", overlap.Count());
    }
}
