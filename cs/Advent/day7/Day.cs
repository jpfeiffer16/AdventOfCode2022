public static class Day7
{
    private const int TOTAL_MACHINE_SPACE = 70_000_000;
    private const int SPACE_NEEDED_FOR_UPDATE = 30_000_000;

    private class Command
    {
        public Command(
            string type, List<string> args)
        {
            Type = type;
            Args = args;
            Result = new List<string>();
        }

        public string Type { get; }
        public List<string> Args { get; }
        public List<string> Result { get; }
    }

    private class MachineFileSystem
    {
        public MachineFileSystem()
        {
             Root = new MachineDir("/", null);
             Cwd = Root;
        }

        public MachineDir Root { get; }
        public MachineDir Cwd { get; private set; }


        public void Cd(string location)
        {
            if (location == "/") Cwd = Root;
            // NOTE: Maybe we should throw here
            else if (location == "..") Cwd = Cwd.Parent ?? Root;
            else Cwd = Cwd.Dirs.Find(d => d.Name == location)
                ?? throw new Exception($"Directory not found: {location}");
        }

        public void Ls(List<string> lsOutput)
        {
            // NOTE should we parse here or somewhere else?
            var nodes = lsOutput
                .Select(l => l.Split(' '))
                .ToList()
                .Select(p => (IMachineFileSystemNode)(p.FirstOrDefault() == "dir"
                             ? new MachineDir(p[1], Cwd)
                             : new MachineFile(p[1], int.Parse(p[0]))));
            Cwd.Dirs.AddRange(nodes.Where(n => n.GetType() == typeof(MachineDir)).Cast<MachineDir>());
            Cwd.Files.AddRange(nodes.Where(n => n.GetType() == typeof(MachineFile)).Cast<MachineFile>());
        }

        public void WalkDirs(Action<MachineDir> fn)
        {
            Root.WalkDirs(fn);
        }
    }


    private interface IMachineFileSystemNode
    {
        public string Name { get; }
        public int Size { get; }
    }

    private class MachineFile : IMachineFileSystemNode
    {
        public MachineFile(string name, int size)
        {
            Name = name;
            Size = size;
        }

        public int Size { get; }
        public string Name { get; }
    }

    private class MachineDir : IMachineFileSystemNode
    {

        public MachineDir(string name, MachineDir? parent)
        {
            Name = name;
            Parent = parent;
            Dirs = new List<MachineDir>();
            Files = new List<MachineFile>();
        }

        public MachineDir? Parent { get; }
        public List<MachineFile> Files { get; }
        public List<MachineDir> Dirs { get; }
        public string Name { get; }

        // NOTE: Cache this if it gets expensive
        public int Size => Files
            .Select(f => f.Size)
            .Union(Dirs.Select(d => d.Size))
            .Sum();

        public void WalkDirs(Action<MachineDir> fn)
        {
            Dirs.ForEach(d => d.WalkDirs(fn));
            fn(this);
        }
    }

    public static void Run()
    {
        var fs = new MachineFileSystem();
        var lines = File.ReadAllLines("day7/input.txt").ToList();
        var commands = new List<Command>();
        for (var i = 0; i < lines.Count; i++)
        {
            (i, var command) = ParseCommand(i, lines);
            commands.Add(command);

            if (command.Type == "cd")
                fs.Cd(command.Args[0]);
            else if (command.Type == "ls")
                // fs.Ls(ParseLsOutput(command.Result));
                fs.Ls(command.Result);
            else
                throw new Exception($"Command type not impl'd: {command.Type}");
        }

        var dirs = new List<MachineDir>();
        fs.WalkDirs(d => dirs.Add(d));
        Console.WriteLine("Total up to 100_000: {0}",
                dirs.Where(d => d.Size <= 100_000).Select(d => d.Size).Sum());
        Console.WriteLine("Figuring out what we need to delete...");

        var rootDirSize = fs.Root.Size;
        var currentFree = TOTAL_MACHINE_SPACE - rootDirSize;
        var minimalDirSizeToDelete = SPACE_NEEDED_FOR_UPDATE - currentFree;
        Console.WriteLine("Have: {0} free.", currentFree);
        Console.WriteLine("Need: {0} free.", SPACE_NEEDED_FOR_UPDATE);
        Console.WriteLine("Finding directory at least {0} sized...", minimalDirSizeToDelete);
        var dirToDelete = dirs.OrderBy(d => d.Size)
            .First(d => d.Size >= minimalDirSizeToDelete);

        Console.WriteLine("Dir to delete:");
        Console.WriteLine("{0}: {1}", dirToDelete.Name, dirToDelete.Size);
    }

    private static (int index, Command command) ParseCommand(int index, List<string> lines)
    {
        var line = lines[index];
        if (!line.TrimStart().StartsWith("$"))
            throw new Exception("Command line doesn't start with $");

        var commandParts = line.Split(' ');
        var command = new Command(
            commandParts[1], commandParts.Skip(2).ToList());

        while (++index < lines.Count
               && !lines[index].TrimStart().StartsWith("$"))
        {
            command.Result.Add(lines[index]);
        }

        return (--index, command);
    }
}
