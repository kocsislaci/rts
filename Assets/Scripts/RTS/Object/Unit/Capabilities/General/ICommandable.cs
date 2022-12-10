using System.Collections.Generic;
using RTS.Player.Commands;

namespace RTS.Object.Unit.Capabilities.General
{
    public interface ICommandable
    {
        public List<CommandDto> ReceivedCommands { get; set; }

        public void AddCommandToOverwrite(CommandDto command)
        {
            ClearEveryCommand();
            AddCommandToQueue(command);
        }
        
        public void AddCommandToQueue(CommandDto command)
        {
            ReceivedCommands.Add(command);
        }
        
        public void ClearEveryCommand()
        {
            if (ReceivedCommands.Count > 0)
                ReceivedCommands.Clear();
        }
        
        public void FinishCommand()
        {
            if (ReceivedCommands.Count > 0)
                ReceivedCommands.RemoveAt(0);
        }
    }
}
