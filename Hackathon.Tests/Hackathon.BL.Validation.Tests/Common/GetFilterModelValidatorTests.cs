using AutoFixture;
using FluentValidation.TestHelper;
using Hackathon.BL.Validation.Common;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Xunit;

namespace Hackathon.BL.Validation.Tests.Common
{
    public class GetFilterModelValidatorTests
    {
        [Fact]
        public async Task Validate_WithValidPagination_Should_Success()
        {
            //arrange
            var parameters = new Fixture()
                .Build<GetListParameters<EventModel>>()
                .Without(x=>x.Filter)
                .With(x=>x.Limit, 1)
                .With(x=>x.Offset, 0)
                .Create();

            //act
            var result = await new GetListParametersValidator<EventModel>().TestValidateAsync(parameters);

            //assert
            result.ShouldNotHaveValidationErrorFor(x=>x.Limit);
            result.ShouldNotHaveValidationErrorFor(x=>x.Offset);
        }
    }
}
