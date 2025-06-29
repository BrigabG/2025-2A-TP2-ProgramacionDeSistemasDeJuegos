using System;
using System.Collections.Generic;

namespace DebugConsole.Interfaces
{
    public interface ICommand<T>
    {
        T Name { get; }
        List<T> Aliases { get; }
        T Description { get; }
        void Execute(Action<T> log, params T[] args);
    }
}