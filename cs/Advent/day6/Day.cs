public static class Day6
{
    public static void Run()
    {
        var file = File.ReadAllText("./day6/input.txt");
        var i = 4;
        for (; i < file.Length; i++)
        {
            var substring = file.Substring(i - 4, 4);
            var result = CheckDifferent(substring);
            if (result) break;
        }

        Console.WriteLine("Start of packet: {0}", i);


        var j = 14;
        for (; j < file.Length; j++)
        {
            var substring = file.Substring(j - 14, 14);
            var result = CheckDifferent(substring);
            if (result) break;
        }

        Console.WriteLine("Start of message: {0}", j);
    }

    private static bool CheckDifferent(string input) =>
        input.Distinct().Count() == input.Length;
}
