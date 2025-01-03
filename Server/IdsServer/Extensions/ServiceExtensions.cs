using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace IdsServer.Extensions;

/// <summary>
/// Validation extensions.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Validates options using FluentValidation.
    /// </summary>
    /// <typeparam name="TOptions">Options.</typeparam>
    /// <param name="optionsBuilder">Options builder.</param>
    /// <returns></returns>
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(x =>
            new FluentValidationOptions<TOptions>(optionsBuilder.Name, x.GetRequiredService<IValidator<TOptions>>()));
        return optionsBuilder;
    }
}

/// <summary>
/// Fluent validation options.
/// </summary>
/// <typeparam name="TOptions">Options.</typeparam>
public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions> _validator;

    /// <summary>
    /// The options name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentValidationOptions{TOptions}"/> class.
    /// </summary>
    /// <param name="name">Options name.</param>
    /// <param name="validator">Validator.</param>
    public FluentValidationOptions(string name, IValidator<TOptions> validator)
    {
        _validator = validator;
        Name = name;
    }

    /// <summary>
    /// Validates the options.
    /// </summary>
    /// <param name="name">Option name.</param>
    /// <param name="options">Options.</param>
    /// <returns></returns>
    public ValidateOptionsResult Validate(string name, TOptions options)
    {
        // Null name is used to configure all named options.
        if (Name != null && Name != name)
        {
            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }

        // Ensure options are provided to validate against
        ArgumentNullException.ThrowIfNull(options);

        ValidationResult validationResult = _validator.Validate(options);
        if (validationResult.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        IEnumerable<string> errors = validationResult.Errors.Select(x =>
            $"Options validation failed for '{x.PropertyName}' with error: '{x.ErrorMessage}'.");

        return ValidateOptionsResult.Fail(errors);
    }
}