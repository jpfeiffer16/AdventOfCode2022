using System.Diagnostics;

public static class Day12
{
    private static Vec EndPos = new Vec(0, 0);
    private static List<List<int>> Grid = new List<List<int>>();
    private static List<(int x, int y)> BeenThere =  new List<(int, int)>();
    private static List<List<(int x, int y)>> Routes = new List<List<(int x, int y)>>();

    [DebuggerDisplay("({X}, {Y})")]
    private class Vec
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vec(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vec((int x, int y) tup)
        {
            X = tup.x;
            Y = tup.y;
        }

        public (int, int) Tup => (X, Y);

        public static Vec operator -(Vec a, Vec b) =>
            new Vec(a.X - b.X, a.Y - b.Y);

        public static Vec operator +(Vec a, Vec b) =>
            new Vec(a.X + b.X, a.Y + b.Y);

        public override string ToString() => $"({X}, {Y})";
    }

    public static void Run()
    {
        Vec startPos = new Vec(0, 0);
        EndPos = new Vec(0, 0);
        Grid = File.ReadAllLines(
                // "./day12/sample-input.txt"
                "./day12/input.txt"
        ).Select((l, y) => l.Select((c, x) => {
                if (c == 'S')
                {
                    startPos = new Vec(x, y);
                    c = 'a';
                }
                if (c == 'E')
                {
                    EndPos = new Vec(x, y);
                    c = 'z';
                }
                return c - 97;
        }).ToList()).ToList();

        Move(startPos, Grid);
        var test = Routes;
        var shortestRoute = Routes.OrderBy(r => r.Count).First();
        Console.WriteLine("Found {0} top-level solutions", Routes.Count());
        while (Refine(shortestRoute, Grid)) { }
        Console.WriteLine(shortestRoute.Count());
        RenderRoute(EndPos, shortestRoute, Grid.First().Count(), Grid.Count());
        // var atEnd = false;
        // while (!atEnd)
        // {
        //     atEnd = Move(startPos, grid);
        // }
    }

    private static bool Refine(List<(int x, int y)> route, List<List<int>> grid)
    {
        for (var i = 0; i < route.Count - 1; i++)
        {
            var pos = route[i];
            var nextPost = route[i + 1];
            // This is super bad. Fix it
            BeenThere.Clear();
            var moves = CanMove(new Vec(pos), grid);
            List<(Vec, Vec)> goodMoves;
            if ((goodMoves = moves.Where(m => m.pos.Tup != nextPost && route.IndexOf(m.pos.Tup) > i).ToList()).Count > 0)
            {
                // if (goodMoves.Count() > 1) throw new Exception("This is to good. I don't know what to do about it.");
                if (goodMoves.Count() > 1)
                {
                    goodMoves = goodMoves.Skip(1).ToList();
                    Console.WriteLine("Found {0} goodMoves", goodMoves.Count());
                }
                // Console.WriteLine(goodMoves.Count());
                foreach (var move in goodMoves)
                {
                    route.RemoveRange(i + 1, route.IndexOf(move.Item1.Tup) - (i + 1));
                    return true;
                }
            }
        }
        return false;
    }

    private static void Move(Vec pos, List<List<int>> grid, List<(int x, int y)>? previous = null)
    {
        if (previous == null) previous = new List<(int, int)>();

        previous.Add(pos.Tup);
        BeenThere.Add(pos.Tup);
        if (pos.Tup == EndPos.Tup)
        {
            Routes.Add(previous);
            Console.WriteLine("Found it");
        }

        var moves = CanMove(pos, grid);

        var test = moves.OrderBy(m => Math.Abs(m.delta.X) + Math.Abs(m.delta.Y)).ToList();
        var ordering = test.Select(t => t.delta.Tup).ToList();
        foreach (var move in test)
        {
            Move(move.pos, grid, new List<(int x, int y)>(previous));
        }
    }

    private static List<(Vec pos, Vec delta)> CanMove(Vec pos, List<List<int>> grid)
    {
        var canMove = new List<(Vec pos, Vec delta)>();
        var current = grid[pos.Y][pos.X];
        var newPos = new Vec(pos.X - 1, pos.Y);

        // LEFT
        if (pos.X !=0
            && grid[pos.Y][pos.X - 1] < current + 2
            && !BeenThere.Contains(newPos.Tup))
        {
            canMove.Add((newPos, EndPos - newPos));
        }
        // RIGHT
        if (pos.X < grid[pos.Y].Count - 1
            && grid[pos.Y][pos.X + 1] < current + 2
            && !BeenThere.Contains((newPos = new Vec(pos.X + 1, pos.Y)).Tup))
        {
            canMove.Add((newPos, EndPos - newPos));
        }
        // UP
        if (pos.Y != 0
            && grid[pos.Y - 1][pos.X] < current + 2
            && !BeenThere.Contains((newPos = new Vec(pos.X, pos.Y - 1)).Tup))
        {
            canMove.Add((newPos, EndPos - newPos));
        }
        // DOWN
        if (pos.Y < grid.Count - 1
            && grid[pos.Y + 1][pos.X] < current + 2
            && !BeenThere.Contains((newPos = new Vec(pos.X, pos.Y + 1)).Tup))
        {
            canMove.Add((newPos, EndPos - newPos));
        }

        return canMove;
    }
 
    private static void RenderRoute(Vec end, List<(int x, int y)> route, int gridWidth = 8, int gridHeight = 5)
    {
        var map = new char[gridWidth, gridHeight];
        for (var x = 0; x < gridWidth; x++)
        {
            for (var y = 0; y < gridHeight; y++)
            {
                map[x, y] = '.';
            }
        }

        for (var i = 0; i < route.Count - 1; i++)
        {
            var thisPos = route[i];
            var nextPos = route[i + 1];
            var diff = new Vec(nextPos) - new Vec(thisPos);
            map[thisPos.x, thisPos.y] = diff.Tup switch
            {
                (1, 0)  => '>',
                (-1, 0) => '<',
                (0, 1)  => 'v',
                (0, -1) => '^',
                _       => '?'
            };
        }

        map[end.X, end.Y] = 'E';

        for (var y = 0; y < gridHeight; y++)
        {
            for (var x = 0; x < gridWidth; x++)
            {
                Console.Write(map[x, y]);
            }
            Console.WriteLine();
        }
    }
}
