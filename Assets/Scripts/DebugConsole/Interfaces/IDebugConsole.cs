using System.Collections.Generic;

namespace DebugConsole.Interfaces
{
    public interface IDebugConsole<T>
    {
        List<ICommand<T>> Commands { get; set; }
        void AddCommand(ICommand<T> command);
        bool IsValidCommand(T name);
        void ExecuteCommand(T name, params T[] args);
        bool TryAddCommand(ICommand<T> command);
    }
}