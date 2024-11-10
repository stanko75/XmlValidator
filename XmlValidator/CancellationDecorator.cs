using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;

namespace XmlValidator
{
    public class CancellationDecorator<TCommand> : ICommandHandlerAsync<TCommand>
    {
        private readonly ICommandHandlerAsync<TCommand> _decoratedHandlerAsync;

        public CancellationDecorator(ICommandHandlerAsync<TCommand> decoratedHandlerAsync)
        {
            _decoratedHandlerAsync = decoratedHandlerAsync;
            CancellationDecorators.ListOfCancellationDecorators.Add(_decoratedHandlerAsync);
        }

        public CancellationTokenSource CancellationTokenSource
        {
            get => _decoratedHandlerAsync.CancellationTokenSource;
            set => _decoratedHandlerAsync.CancellationTokenSource = value;
        }

        public async Task Execute(TCommand command)
        {
            if (CancellationTokenSource is null)
            {
                CancellationTokenSource = new CancellationTokenSource();
            }

            await _decoratedHandlerAsync.Execute(command);
        }

        public void CancelOperation()
        {
            foreach (var listOfCancellationDecorator in CancellationDecorators.ListOfCancellationDecorators)
            {
                try
                {
                    dynamic dynamicDecorator = listOfCancellationDecorator;
                    dynamicDecorator.CancellationTokenSource.Cancel();
                    dynamicDecorator.CancellationTokenSource.Dispose();
                    dynamicDecorator.CancellationTokenSource = null;
                }
                catch (RuntimeBinderException)
                {
                }
            }
        }
    }

    public static class CancellationDecorators
    {
        public static readonly List<object> ListOfCancellationDecorators = new List<object>();
    }
}