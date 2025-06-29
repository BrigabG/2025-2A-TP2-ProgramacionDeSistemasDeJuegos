using System;
using System.Collections.Generic;
using System.Linq;
using DebugConsole.Interfaces;

namespace DebugConsole.Commands
{
    public class HelpCommand : ICommand<string>
    {
        private readonly IDebugConsole<string> _console;

        public HelpCommand(IDebugConsole<string> console)
            => _console = console;

        public string Name => "help";

        public List<string> Aliases
            => new List<string>()
            {
                "h", "Help", "HELP", "?",
                "h?",
                "help?",
                "help!"
            };

        public string Description => "Shows all commands information";

        public void Execute(Action<string> log, params string[] args)
        {
            const string header = "<color=green>name\t\t\tdescription\n</color>";
            var info = _console.Commands
                .Aggregate(header,
                    (cur, cmd) => cur + $"{cmd.Name}\t\t{cmd.Description}\n");

            log?.Invoke(info);
        }
    }
}