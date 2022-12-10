using VM;

public static class Day10
{
    public static void Run()
    {
        var vm = new Vm(
                File.ReadAllLines(
                    "./day10/input.txt"
                    // "./day10/sample.txt"
                    // "./day10/small-sample.txt"
                ).Select(l => new Instruction(l)).ToList());

        using var cancellationTokenSource = new CancellationTokenSource(3000);
        vm.Run(cancellationTokenSource.Token);
        Console.WriteLine(
                vm.SignalStrengths.Select(kv => kv.Key * kv.Value).Sum());
        Console.WriteLine(vm.RenderCrt());
    }
}
