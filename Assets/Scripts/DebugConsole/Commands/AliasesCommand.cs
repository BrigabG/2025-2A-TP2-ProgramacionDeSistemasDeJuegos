using System;
using System.Collections.Generic;
using System.Linq;
using DebugConsole.Interfaces;


namespace DebugConsole.Commands
{
    public class AliasesCommand : ICommand<string>
    {
        private readonly IDebugConsole<string> _console;

        public AliasesCommand(IDebugConsole<string> console)
            => _console = console;

        public string Name => "aliases";

        public List<string> Aliases
            => new List<string>() { "a", "alias", "ALIASES" };

        public string Description => "Shows all aliases for a command";

        public void Execute(Action<string> log, params string[] args)
        {
            if (args.Length == 0)
            {
                log?.Invoke("Usage: aliases <commandName>");
                return;
            }

            var cmdName = args[0];
            if (!_console.IsValidCommand(cmdName))
            {
                log?.Invoke($"Unknown command: {cmdName}");
                return;
            }

            var cmd = _console.Commands
                .First(c => c.Name == cmdName || c.Aliases.Contains(cmdName));
            var list = string.Join(", ", cmd.Aliases);
            log?.Invoke($"Aliases for {cmd.Name}: {list}");
        }
    }
}