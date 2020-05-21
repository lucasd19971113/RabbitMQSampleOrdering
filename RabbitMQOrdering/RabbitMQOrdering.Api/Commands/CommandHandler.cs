using System;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Services.IServices;

namespace RabbitMQOrdering.Api.Commands
{
    public class CommandHandler
    {
        private IWorkQueue _workQueue;

        private IServiceProvider _serviceProvider;

        public CommandHandler(IWorkQueue workQueue, IServiceProvider serviceProvider)
        {
            _workQueue = workQueue;
            _serviceProvider = serviceProvider;
        }

        public void SetCommand(string command, ProductQueue productQueue)
        {
            switch (command)
            {
                case string s when s.ToLower().Contains("preparing"):
                    productQueue._serviceScopefactory = _serviceProvider;
                    _workQueue.AddCommand(new PrepareProductOrder(productQueue));
                    break;

                case string s when s.ToLower().Contains("done"):
                    productQueue._serviceScopefactory = _serviceProvider;
                    _workQueue.AddCommand(new ProductDone(productQueue));
                    break;
            }
        }

        public void ProcessCommands()
        {
            _workQueue.ProcessCommands();
        }
    }
}