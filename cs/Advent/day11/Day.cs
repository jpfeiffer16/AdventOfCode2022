public static class Day11
{
    private class Monkey
    {
        public int Number { get; }
        // public Stack<int> Items { get; }
        public List<long> Items { get; set; }
        public List<string> Operation { get; }
        public long TestValue { get; }
        public int TruePass { get; }
        public int FalsePass { get; }
        public long Inspected { get; set; } = 0;

        public Monkey(
            int number,
            List<long> items,
            List<string> operation,
            long testValue,
            int truePass,
            int falsePass)
        {
            Number = number;
            Items = items;
            Operation = operation;
            TestValue = testValue;
            TruePass = truePass;
            FalsePass = falsePass;
        }
    }

    public static void Run()
    {
        var input = File.ReadAllLines(
                // "./day11/input.txt"
                "./day11/sample-input.txt"
        );
        var monkeys = new List<Monkey>();
        for (var i = 0; i < input.Count(); i += 7)
        {
            var monkeyNumber = int.Parse(input[i].Split(' ')[1].TrimEnd(':'));
            var items = input[i + 1].Split(':')[1].Split(',').Select(n => long.Parse(n)).ToList();
            var opParts = input[i + 2].Split('=')[1].Trim().Split(' ').ToList();
            var op1 = opParts[0];
            var op = opParts[1];
            var op2 = opParts[2];
            // var opNumber = input[i + 2][25..];
            var test = long.Parse(input[i + 3][21..]);
            var t = int.Parse(input[i + 4][29..]);
            var f = int.Parse(input[i + 5][30..]);
            monkeys.Add(new Monkey(monkeyNumber, items, opParts, test, t, f));
        }

        // for (var i = 0; i < 20; i++)
        long round = 0;
        for (var i = 0; i < 10_000; i++)
        {
            round++;
            foreach (var monkey in monkeys)
            {
                foreach (var item in monkey.Items)
                {
                    monkey.Inspected++;
                    var oldWorryLevel = item;
                    var a = ParseVar(monkey.Operation[0], oldWorryLevel);
                    var b = ParseVar(monkey.Operation[2], oldWorryLevel);
                    var com = LowestCommonMultiple(a, b, monkey.TestValue);
                    var newWorryLevel = monkey.Operation[1] switch
                    {
                        "+" => (a / com)
                            + (b / com),
                        "*" => (a / com)
                            * (b / com),
                        _   => throw new NotImplementedException("Op not implmented")
                    };

                    // Console.WriteLine("New worry level: {0}", newWorryLevel);

                    if (newWorryLevel % (monkey.TestValue / com) == 0)
                    {
                        monkeys[monkey.TruePass].Items.Add(newWorryLevel);
                    }
                    else
                    {
                        monkeys[monkey.FalsePass].Items.Add(newWorryLevel);
                    }
                }
                monkey.Items.Clear();
            }
            var temp = monkeys.Select(m => m.Inspected).ToList();
            if (temp.First() == 5204)
                System.Diagnostics.Debugger.Break();
            if (temp.First() == 52166)
                System.Diagnostics.Debugger.Break();
            if (round == 20 || round == 1000)
            {
                // var temp = monkeys.Select(m => m.Inspected).ToList();
                System.Diagnostics.Debugger.Break();
            }
        }

        var tp = monkeys.OrderByDescending(m => m.Inspected).Take(2).ToList();
        Console.WriteLine(tp[0].Inspected);
        Console.WriteLine(tp[1].Inspected);
        Console.WriteLine(tp[0].Inspected * tp[1].Inspected);
    }

    private static long LowestCommonMultiple(params long[] numbers)
    {
        var largest = 1L;
        for (var i = 1; i <= numbers.Min(); i++)
        {
            if (numbers.All(n => n % i == 0))
                largest = i;
        }

        return largest;
    }

    private static long ParseVar(
            string variable, long currentWorryLevel) => variable switch
        {
            "old" => currentWorryLevel,
            _     => long.Parse(variable)
        };
}
