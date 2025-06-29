using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using DebugConsole.Commands;
using DebugConsole.Interfaces;

namespace DebugConsole.Core

{
    [CreateAssetMenu(menuName = "Debug/ConsoleWrapper")]
    public class ConsoleWrapper : ScriptableObject, IDebugConsole<string>, ILogHandler
    {
        [SerializeField] protected List<CommandSO> commands;
        [SerializeField] protected char[] separators = new[] { ' ', '\t' };

        protected IDebugConsole<string> DebugConsole;
        public Action<string> Log = delegate { };

        private ILogHandler _originalLogHandler;

        protected void OnEnable()
        {
            DebugConsole = new SimpleDebugConsole<string>(
              str => Log(str),
              commands.Cast<ICommand<string>>().ToArray()
            );

            DebugConsole.AddCommand(new AliasesCommand(DebugConsole));
            DebugConsole.AddCommand(new HelpCommand(DebugConsole));

            _originalLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = this;
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            _originalLogHandler.LogFormat(logType, context, format, args);
            Log?.Invoke(string.Format(format, args));
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            _originalLogHandler.LogException(exception, context);
            Log?.Invoke(exception.ToString());
        }

        // ===== IDebugConsole<string> =====
        public List<ICommand<string>> Commands { get; set; }

        public void AddCommand(ICommand<string> command)
            => DebugConsole.AddCommand(command);

        public bool TryAddCommand(ICommand<string> command)
            => DebugConsole.TryAddCommand(command);

        public bool IsValidCommand(string name)
            => DebugConsole.IsValidCommand(name);

        public void ExecuteCommand(string name, params string[] args)
            => DebugConsole.ExecuteCommand(name, args);

        /// <summary>
        /// Parsea la línea de texto, valida el comando y lo ejecuta.
        /// </summary>
        public bool TryUseInput(string input)
        {
            var parts      = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var commandName = parts[0];
            if (!DebugConsole.IsValidCommand(commandName))
            {
                Log?.Invoke($"command not found: {commandName}");
                return false;
            }

            var args = parts.Skip(1).ToArray();
            DebugConsole.ExecuteCommand(commandName, args);
            return true;
        }
    }
}