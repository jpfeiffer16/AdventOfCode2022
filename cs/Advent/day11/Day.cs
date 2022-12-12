public static class Day11
{
    private class Monkey
    {
        public int Number { get; }
        // public Stack<int> Items { get; }
        public List<int> Items { get; set; }
        public List<string> Operation { get; }
        public int TestValue { get; }
        public int TruePass { get; }
        public int FalsePass { get; }
        public int Inspected { get; set; } = 0;

        public Monkey(
            int number,
            List<int> items,
            List<string> operation,
            int testValue,
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
                "./day11/input.txt"
                // "./day11/sample-input.txt"
        );
        var monkeys = new List<Monkey>();
        for (var i = 0; i < input.Count(); i += 7)
        {
            var monkeyNumber = int.Parse(input[i].Split(' ')[1].TrimEnd(':'));
            var items = input[i + 1].Split(':')[1].Split(',').Select(n => int.Parse(n)).ToList();
            var opParts = input[i + 2].Split('=')[1].Trim().Split(' ').ToList();
            var op1 = opParts[0];
            var op = opParts[1];
            var op2 = opParts[2];
            // var opNumber = input[i + 2][25..];
            var test = int.Parse(input[i + 3][21..]);
            var t = int.Parse(input[i + 4][29..]);
            var f = int.Parse(input[i + 5][30..]);
            monkeys.Add(new Monkey(monkeyNumber, items, opParts, test, t, f));
        }

        for (var i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                foreach (var item in monkey.Items)
                {
                    monkey.Inspected++;
                    var oldWorryLevel = item;
                    var newWorryLevel = (int)Math.Floor(monkey.Operation[1] switch
                    {
                        "+" => (double)ParseVar(monkey.Operation[0], oldWorryLevel)
                            + (double)ParseVar(monkey.Operation[2], oldWorryLevel),
                        "*" => (double)ParseVar(monkey.Operation[0], oldWorryLevel)
                            * (double)ParseVar(monkey.Operation[2], oldWorryLevel),
                        _   => throw new NotImplementedException("Op not implmented")
                    } / 3d);

                    if (newWorryLevel % monkey.TestValue == 0)
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
        }

        var tp = monkeys.OrderByDescending(m => m.Inspected).Take(2).ToList();
        Console.WriteLine(tp[0].Inspected * tp[1].Inspected);
    }

    private static int ParseVar(string variable, int currentWorryLevel)
    {
        return variable switch
        {
            "old" => currentWorryLevel,
            _     => int.Parse(variable)
        };
    }
}
