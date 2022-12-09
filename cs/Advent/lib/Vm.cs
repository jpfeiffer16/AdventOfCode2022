namespace VM
{
    // This file contains classes for a basic vm implementation
    //
    // Example usage:
    // foreach (var instructionList in alternateInstructionLists)
    // {
    //     var vm = new Vm(instructionList);
    //     try
    //     {
    //         using var cancellationTokenSource = new CancellationTokenSource(3000);
    //         vm.Run(cancellationTokenSource.Token);
    //         Console.WriteLine("SUCCESS!");
    //         Console.WriteLine(vm.Accumulator);
    //     }
    //     catch (OperationCanceledException)
    //     {
    //         Console.WriteLine("ERROR");
    //     }
    // }

    public class Instruction
    {
        public string Name { get; }
        public bool ArgPositive { get; }
        public long Arg { get; }

        public Instruction(string name, bool argPositive, long arg)
        {
            Name = name;
            ArgPositive = argPositive;
            Arg = arg;
        }

        public Instruction(string line)
        {
            // TODO: this is old parsing logic from 2020 Day8
            var parts = line.Split(' ');
            var name = parts[0];
            var isPositive = parts[1][0] == '+';
            var arg = parts[1].Substring(1);
            Name = name;
            ArgPositive = isPositive;
            Arg = long.Parse(arg);
        }
    }

    public class Vm
    {
        public int InstructionPointer = 0;
        public long Accumulator = 0;

        private List<Instruction> _instructions;

        public Vm(List<Instruction> instructions)
        {
            _instructions = instructions;
        }

        public void Run(CancellationToken cancellationToken = default)
        {
            for (; InstructionPointer < _instructions.Count; InstructionPointer++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var instruction = _instructions[InstructionPointer];
                switch (instruction.Name)
                {
                    case "acc":
                        Accumulator += instruction.ArgPositive ? instruction.Arg : -1 * instruction.Arg;
                        break;
                    case "jmp":
                        InstructionPointer += (int)(instruction.ArgPositive ? instruction.Arg : -1 * instruction.Arg) - 1;
                        break;
                    case "nop":
                        break;
                }
            }
        }
    }
}
