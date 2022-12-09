public static class EnumberableExtensions
{
    /// <summary>
    /// Tail inclusive inverse of TakeWhile.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="fn">The fn.</param>
    /// <typeparam name="T"></typeparam>
    public static IEnumerable<T> TakeTill<T>(
            this IEnumerable<T> input, Func<T, bool> fn)
    {
        var output = new List<T>();

        foreach (var item in input)
        {
            output.Add(item);
            if (fn(item)) break;
        }

        return output;
    }
}
