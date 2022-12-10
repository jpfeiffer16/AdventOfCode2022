using System.Text;

namespace VM
{
    /// <summary>
    /// Instruction.
    /// </summary>
    public class Instruction
    {
        public string Name { get; }
        public int Arg { get; }

        public Instruction(string line)
        {
            var parts = line.Split(' ');
            Name = parts[0];
            if (Name == "addx")
                Arg = int.Parse(parts[1]);
        }
    }

    /// <summary>
    /// Vm.
    /// </summary>
    public class Vm
    {
        private readonly bool[,] Crt = new bool[40, 6];
        private readonly List<Instruction> _instructions;
        private int InstructionPointer = 0;

        public int X { get; private set; } = 1;
        public int Cycle { get; private set; } = 0;
        public Dictionary<int, int> SignalStrengths { get; }
            = new Dictionary<int, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Vm"/> class.
        /// </summary>
        /// <param name="instructions">The instructions.</param>
        public Vm(List<Instruction> instructions)
        {
            _instructions = instructions;
        }

        /// <summary>
        /// Run.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public void Run(
                CancellationToken cancellationToken = default)
        {
            for (; InstructionPointer < _instructions.Count; InstructionPointer++)
            {
                IncrCycle();

                cancellationToken.ThrowIfCancellationRequested();
                var instruction = _instructions[InstructionPointer];
                switch (instruction.Name)
                {
                    case "addx":
                        IncrCycle();
                        X += instruction.Arg;
                        break;
                    case "nop":
                        break;
                }
            }
        }

        /// <summary>
        /// Renders crt.
        /// </summary>
        public string RenderCrt()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < 6; y++)
            {
                for (var x = 0; x < 40; x++)
                {
                    sb.Append(Crt[x, y] ? "#" : ".");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private void IncrCycle()
        {
            ++Cycle;
            if (Cycle == 20
                || (Cycle > 20
                    && (Cycle - 20) % 40 == 0))
            {
                SignalStrengths[Cycle] = X;
            }
            // Draw crt pixel
            var crtx = (Cycle - 1) % 40;
            var crty = (Cycle - 1) / 40;
            Crt[crtx, crty] = crtx >= X - 1 && crtx <= X + 1;
        }
    }
}
