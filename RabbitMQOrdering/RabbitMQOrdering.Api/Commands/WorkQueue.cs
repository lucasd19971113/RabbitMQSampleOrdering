using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQOrdering.Api.Commands
{
    public class WorkQueue : IWorkQueue
    {
        public List<ICommand> Commands { get; set; } = new List<ICommand>();

        public WorkQueue()
        {
            
        }

        public async Task ProcessCommands()
        {
            if(Commands.AsEnumerable().Any())
            {
                var commandsCompleted = new List<ICommand>();
                foreach (var command in Commands)
                {
                    await command.ExecuteAsync();

                    commandsCompleted.Add(command);
                }

                commandsCompleted.ForEach(cp =>{
                    if(Commands.Contains(cp))
                        Commands.Remove(cp);
                    
                });

                commandsCompleted.RemoveRange(0, commandsCompleted.Count);
            }     
        }

        public void AddCommand(ICommand command){
            Commands.Add(command);
        }
    }

    public interface IWorkQueue
    {
        Task ProcessCommands();
        void AddCommand(ICommand command);
    }
}