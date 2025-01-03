using FluentValidation;
using JetBrains.Annotations;

namespace IdsServer.Validators;

/// <summary>
/// Application options validator.
/// </summary>
[UsedImplicitly]
public class AppOptionsValidator : AbstractValidator<AppOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppOptionsValidator"/> class.
    /// </summary>
    public AppOptionsValidator()
    {
       
    }
}