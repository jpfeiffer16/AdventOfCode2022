using System.Text;

public static class Day5
{
    public static void Run()
    {
        var lines = File.ReadAllLines("./day5/input.txt");

        var diagramLines = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Reverse();
        var piles = new Dictionary<int, (Stack<char> stack, List<char> list)>(
                diagramLines.First()
                            .Trim()
                            .Split(' ')
                            .Where(i => !string.IsNullOrWhiteSpace(i))
                            .Select(i =>
                                KeyValuePair.Create(
                                    int.Parse(i),
                                    (stack: new Stack<char>(), list: new List<char>()))));

        var stackLines = diagramLines.Skip(1).ToList();
        foreach (var line in stackLines)
        {
            foreach (var key in piles.Keys)
            {
                var item = line[((key - 1) * 4) + 1];
                if (item != ' ')
                {
                    piles[key].stack.Push(item);
                    piles[key].list.Add(item);
                }
            }
        }

        var moves = lines
            .SkipWhile(l => !string.IsNullOrWhiteSpace(l))
            .Skip(1)
            .Select(l => l.Replace("move", string.Empty)
                          .Replace("from", string.Empty)
                          .Replace("to", string.Empty)
                          .Trim()
                          .Split(' ')
                          .Where(i => !string.IsNullOrWhiteSpace(i))
                          .Select(i => int.Parse(i)).ToList())
            .Select(d => (number: d[0], from: d[1], to: d[2])).ToList();

        Console.WriteLine(moves);

        Console.WriteLine(RenderPilesList(piles));
        Console.WriteLine();

        foreach (var move in moves)
        {

            for (var i = 0; i < move.number; i++)
            {
                piles[move.to].stack.Push(piles[move.from].stack.Pop());
            }

            var take = piles[move.from].list
                .GetRange(piles[move.from].list.Count - move.number, move.number);
            piles[move.to].list
                .AddRange(take);
            piles[move.from].list
                .RemoveRange(piles[move.from].list.Count - move.number, move.number);

            Console.WriteLine(RenderPilesList(piles));
        }

        //TODO: Clean this up. Why can't I get using to work?
        var outputBuilder = new StringBuilder();
        foreach (var pile in piles)
        {
            outputBuilder.Append(pile.Value.stack.First());
        }

        Console.WriteLine("MOVE 1: {0}", outputBuilder.ToString());

        outputBuilder.Clear();
        foreach (var pile in piles)
        {
            var list = pile.Value.list;
            var first = list.First();
            var last = list.Last();
            outputBuilder.Append(pile.Value.list.Last());
        }

        Console.WriteLine("MOVE MANY: {0}", outputBuilder.ToString());
    }

    /// <summary>
    /// Renders piles list.
    /// Note that only the list is rendered here. Not the stack.
    /// </summary>
    /// <param name="piles">The piles.</param>
    private static string
        RenderPilesList(Dictionary<int, (Stack<char>, List<char>)> piles)
    {
        var outputList = new List<StringBuilder>();
        var firstLine = new StringBuilder();
        foreach (var key in piles.Keys)
        {
            firstLine.Append($"{key}    ");
        }
        outputList.Add(firstLine);

        for (var i = 0; i < piles.Values.Select(v => v.Item2.Count()).Max(); i++)
        {
            var newHead = new StringBuilder();
            foreach (var key in piles.Keys)
            {
                if (piles[key].Item2.Count > i)
                {
                    newHead.Append($"[{piles[key].Item2[i]}]  ");
                }
                else
                {
                    newHead.Append("     ");
                }
            }
            outputList.Add(newHead);
        }

        outputList.Reverse();

        return string.Join('\n', outputList.Select(l => l.ToString()));
    }
}
