using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        public UnhandledExceptionBehavior(ILogger<TRequest> Logger) 
        {
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
        }
        public async Task<TResponse> Handle(TRequest Request, RequestHandlerDelegate<TResponse> Next, CancellationToken CancellationToken)
        {
            try
            {
                return await Next();
            }
            catch(Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, $"Application Request: Unhandled Exception for request {requestName}");
                throw;
            }
        }
    }
}
