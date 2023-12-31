﻿using FluentValidation;
using MediatR;

namespace Trickle.Directory.Application.Validators;

internal class ValidatorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await Task.WhenAll(_validators.Select(v => v.ValidateAndThrowAsync(request, cancellationToken)));
        return await next();
    }
}
