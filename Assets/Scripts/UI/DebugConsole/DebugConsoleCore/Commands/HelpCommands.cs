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

        public string DetailedHelp =>
            "help [commandName]\n" +
            "    Shows all commands or detailed help for specific command.\n" +
            "    Examples: help | help playanimation";

        public void Execute(Action<string> log, params string[] args)
        {
            if (args.Length > 0)
            {
                var commandName = args[0];

                var command = _console.Commands
                    .FirstOrDefault(c => c.Name == commandName || c.Aliases.Contains(commandName));

                if (command == null)
                {
                    log?.Invoke($"Command not found: {commandName}");
                    return;
                }

                log?.Invoke(command.DetailedHelp);
                return;
            }

            const string header = "<color=green>name\t\t\tdescription\n</color>";
            var info = _console.Commands
                .Aggregate(header,
                    (cur, cmd) => cur + $"{cmd.Name}\t\t{cmd.Description}\n");

            log?.Invoke(info);
        }
    }
}