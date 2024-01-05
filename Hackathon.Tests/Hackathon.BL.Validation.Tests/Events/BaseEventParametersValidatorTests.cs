using FluentValidation;
using FluentValidation.TestHelper;
using Hackathon.BL.Validation.Event;
using Hackathon.Common.Models.Event;
using Xunit;

namespace Hackathon.BL.Validation.Tests.Events;

public class BaseEventParametersValidatorTests
{
    private readonly IValidator<BaseEventParameters> _validator;
    public BaseEventParametersValidatorTests()
    {
        _validator = new BaseEventParametersValidator();
    }

    [Fact]
    public void Validate_Start()
    {
        var result = _validator.TestValidate(new EventModel
            {
                Start = DateTime.UtcNow
            });

        result.ShouldHaveValidationErrorFor(x => x.Start);
    }
}
