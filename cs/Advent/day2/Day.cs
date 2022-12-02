
/// <summary>
/// Day2.
/// </summary>
public static class Day2
{
    /// <summary>
    /// Shapes.
    /// </summary>
    private enum Shape
    {
        ROCK = 1,
        PAPER = 2,
        SCISSORS = 3
    }

    /// <summary>
    /// Strategy type.
    /// </summary>
    private enum StrategyType
    {
        LOOSE = 1,
        DRAW = 2,
        WIN = 3
    }

    /// <summary>
    /// Run.
    /// </summary>
    public static void Run()
    {
        var strategies = File
            .ReadAllLines("./day2/input.txt")
            .Select(l => l.Split(' '))
            .Select(s => (play: ParseShape(s[0]),
                          response: ParseShape(s[1])));

        var playedScore = 0;
        foreach (var strategy in strategies)
        {
            playedScore += PlayStrategy(
                    ((Shape)strategy.play,
                     (Shape)strategy.response));
        }
        Console.WriteLine("Played Strategies: {0}", playedScore);

        var appliedScore = 0;
        foreach (var strategy in strategies)
        {
            appliedScore += ApplyStrategy(
                    ((Shape)strategy.play,
                     (StrategyType)strategy.response));
        }
        Console.WriteLine("Applied Strategies: {0}", appliedScore);
    }

    /// <summary>
    /// Parses shape.
    /// </summary>
    /// <param name="shapeString">The shape string.</param>
    /// <exception cref="Exception">Exception error.</exception>
    private static int ParseShape(string shapeString)
    {
        switch(shapeString)
        {
            case "A":
                return 1;
            case "B":
                return 2;
            case "C":
                return 3;
            case "X":
                return 1;
            case "Y":
                return 2;
            case "Z":
                return 3;
        }
        
        throw new Exception($"Unknown shape: {shapeString}");
    }

    /// <summary>
    /// Plays strategy.
    /// </summary>
    /// <param name="strategy">The strategy.</param>
    private static int PlayStrategy(
            (Shape play, Shape response) strategy) =>
        strategy switch
        {
            var (play, response) when play == response => 3,
            var (play, response) when play == Shape.ROCK =>
                response == Shape.PAPER ? 6 : 0,
            var (play, response) when play == Shape.PAPER =>
                response == Shape.SCISSORS ? 6 : 0,
            var (play, response) when play == Shape.SCISSORS =>
                response == Shape.ROCK ? 6 : 0,
            _ => throw new Exception("Something went wrong here")
        } + (int)strategy.response;

    /// <summary>
    /// Applies strategy.
    /// </summary>
    /// <param name="strategy">The strategy.</param>
    private static int ApplyStrategy(
            (Shape play, StrategyType response) strategy) =>
        PlayStrategy((strategy.play, strategy switch
        {
            var (play, response) when response == StrategyType.DRAW =>
                play,
            var (play, response) when response == StrategyType.LOOSE =>
                play switch
                {
                    Shape.ROCK => Shape.SCISSORS,
                    Shape.PAPER => Shape.ROCK,
                    Shape.SCISSORS => Shape.PAPER,
                    _ => throw new Exception("Can't LOOSE")
                },
            var (play, response) when response == StrategyType.WIN =>
                play switch
                {
                    Shape.ROCK => Shape.PAPER,
                    Shape.PAPER => Shape.SCISSORS,
                    Shape.SCISSORS => Shape.ROCK,
                    _ => throw new Exception("Can't WIN")
                },
            _ => throw new Exception("Something went wrong here.")
        }));
}
