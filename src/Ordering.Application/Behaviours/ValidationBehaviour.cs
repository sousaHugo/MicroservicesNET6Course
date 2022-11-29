using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> Validators)
    {
        _validators = Validators ?? throw new ArgumentNullException(nameof(Validators));
    }
    public async Task<TResponse> Handle(TRequest Request, RequestHandlerDelegate<TResponse> Next, CancellationToken CancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(Request);
            var validatonResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, CancellationToken)));
            var failures = validatonResults.SelectMany(e => e.Errors).Where(f => f != null).ToList();
           
            if(failures.Any())
                throw new ValidationException(failures);
        }

        return await Next();
    }
}
