using UnityEngine;
using System;
using System.Collections.Generic;
using DebugConsole.Interfaces;

namespace DebugConsole.Commands
{
    public abstract class CommandSO : ScriptableObject, ICommand<string>
    {
        public abstract string Name { get; }
        public abstract List<string> Aliases { get; }
        public abstract string Description { get; }
        public abstract void Execute(Action<string> writeToConsole, params string[] args);
    }
}

