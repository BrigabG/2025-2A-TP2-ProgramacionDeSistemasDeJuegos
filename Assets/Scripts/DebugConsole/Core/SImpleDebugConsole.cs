using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DebugConsole.Interfaces;

namespace DebugConsole.Core 
{
    public class SimpleDebugConsole<T> : IDebugConsole<T>
    {
        private readonly Action<T> _log;
        private readonly Dictionary<T, ICommand<T>> _commandDictionary;

        public SimpleDebugConsole(Action<T> log, params ICommand<T>[] commands)
        {
            _log               = log;
            _commandDictionary = new Dictionary<T, ICommand<T>>();
            Commands           = new List <ICommand<T>>();

            foreach (var cmd in commands)
                AddCommand(cmd);
        }

        public List<ICommand<T>> Commands { get; set; }

        public void AddCommand(ICommand<T> command, bool throwIfDuplicate = true)
        {
            if (Commands.Contains(command))
            {
                if (throwIfDuplicate)
                    throw new DuplicateNameException($"Command {command.Name} has already been added");
                return;
            }
            Commands.Add(command);
        }

        public bool TryAddCommand(ICommand<T> command)
            => Commands.Contains(command) || TryAddToCommandDictionary(command);

        public void AddCommand(ICommand<T> command)
        {
            throw new NotImplementedException();
        }

        public bool IsValidCommand(T name)
            => _commandDictionary.ContainsKey(name);

        public void ExecuteCommand(T name, params T[] args)
        {
            if (_commandDictionary.TryGetValue(name, out var command))
                command.Execute(_log, args);
            else
                _log.Invoke((T)Convert.ChangeType($"Command not found: {name}", typeof(T)));
        }

        private void AddToCommandDictionary(ICommand<T> command)
        {
            // Nombre principal
            if (!_commandDictionary.ContainsKey(command.Name))
                _commandDictionary.Add(command.Name, command);
            else
                throw new DuplicateNameException($"Command {command.Name} already exists in dictionary");

            // Aliases
            foreach (var alias in command.Aliases)
            {
                if (_commandDictionary.TryGetValue(alias, out var dup))
                    throw new DuplicateNameException(
                        $"Alias '{alias}' already in use by '{dup.Name}'");
                _commandDictionary.Add(alias, command);
            }
        }

        private bool TryAddToCommandDictionary(ICommand<T> command)
        {
            if (!_commandDictionary.ContainsKey(command.Name))
            {
                _commandDictionary.Add(command.Name, command);
                return true;
            }

            foreach (var alias in command.Aliases)
            {
                if (!_commandDictionary.ContainsKey(alias))
                {
                    _commandDictionary.Add(alias, command);
                    return true;
                }
            }

            return false;
        }
    }
}
