using System.Text;

public static class Day9
{
    private class Command
    {
        public Direction Direction { get; }
        public int Number { get; }

        public Command(Direction direction, int number)
        {
            Direction = direction;
            Number = number;
        }
    }

    private class Vec
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vec(int x, int y)
        {
            X = x;
            Y = y;
        }

        public (int, int) Tup => (X, Y);

        public static Vec operator -(Vec a, Vec b) =>
            new Vec(a.X - b.X, a.Y - b.Y);

        public static Vec operator +(Vec a, Vec b) =>
            new Vec(a.X + b.X, a.Y + b.Y);
    }

    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private class Grid
    {
        private int _pointer = 0;
        private int _current = 0;
        private Vec _origin = new Vec(0, 0);
        private readonly int _ropeSize;

        public List<Command> Commands { get; }
        public Vec Head { get; set; }
        public Vec Tail { get; set; }
        public Dictionary<ValueTuple<int, int>, bool> Visited { get; }
        public List<Vec> Knots { get; }

        public Grid(List<Command> commands, int ropeSize)
        {
            Head = new Vec(0, 0);
            Commands = commands;
            _ropeSize = ropeSize;
            Visited = new Dictionary<ValueTuple<int, int>, bool>
            {
                { _origin.Tup, true }
            };
            Knots = new List<Vec>();
            Knots.Add(Head);
            for (var i = 0; i < ropeSize; i++)
            {
                Knots.Add(new Vec(0, 0));
            }
            Tail = Knots.Last();
        }

        public Grid Sim()
        {
            while (Step()) {}
            return this;
        }

        public bool Step()
        {
            if (_pointer == Commands.Count) return false;

            var command = Commands[_pointer];
            if (_current == command.Number)
            {
                _current = 0;
                _pointer++;
                Step();
            }
            else
            {
                _current++;

                switch (command.Direction)
                {
                    case Direction.Left:
                        Head.X--;
                        break;
                    case Direction.Right:
                        Head.X++;
                        break;
                    case Direction.Up:
                        Head.Y++;
                        break;
                    case Direction.Down:
                        Head.Y--;
                        break;
                }

                for (var i = 1; i < Knots.Count; i++)
                {
                    var thisKnot = Knots[i];
                    var lastKnot = Knots[i - 1];

                    var diff = lastKnot - thisKnot;
                    var absx = Math.Abs(diff.X);
                    var absy = Math.Abs(diff.Y);
                    if (absx > 1 || absy > 1)
                    {
                        var delta = new Vec(
                            absx == 0 ? 0 : (diff.X / absx) * Math.Min(1, absx),
                            absy == 0 ? 0 : (diff.Y / absy) * Math.Min(1, absy));
                        var newLocation = thisKnot + delta;
                        Knots[i].X = newLocation.X;
                        Knots[i].Y = newLocation.Y;
                    }

                }

                Visited[Tail.Tup] = true;
            }

            return true;
        }

        public string Render()
        {
            var sb = new StringBuilder();
            for (var y = 0; y <= 20; y++)
            {
                for (var x = 0; x <= 20; x++)
                {
                    if (Head.Tup == (x, y))
                        sb.Append('H');
                    else if (Knots.Skip(0).Any(n => n.Tup == (x, y)))
                        sb.Append(((char)(Knots
                                            .Skip(0)
                                            .Select((n, i) => (n, i))
                                            .First(t => t.n.Tup == (x, y)).i + 48)));
                    else
                        sb.Append('.');
                }
                sb.AppendLine();
            }

            // We've all got to do what we've got to do sometimes.
            return string.Join('\n', sb.ToString().Split('\n').Reverse());
        }
    }

    public static void Run()
    {
        var commands = File.ReadAllLines(
                                        "./day9/input.txt"
                                         // "./day9/sample.txt"
                                         // "./day9/large-sample.txt"
                                        )
                        .Select(l => new Command(l[0] switch
                            {
                                'L' => Direction.Left,
                                'R' => Direction.Right,
                                'U' => Direction.Up,
                                'D' => Direction.Down,
                                _   => throw new Exception("Bad direction")
                            }, int.Parse(l[2..]))).ToList();

        Console.WriteLine(
                "Part 1: {0}",
                new Grid(commands, 1).Sim().Visited.Count());


        Console.WriteLine(
                "Part 2: {0}",
                new Grid(commands, 9).Sim().Visited.Count());
    }
}
