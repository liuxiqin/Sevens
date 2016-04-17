using System.Runtime.InteropServices;

namespace Seven.Commands
{
    public interface ICommandProssor
    {
        void Execute(ICommand command);
    }
}
