using FluentValidation;
using Hackathon.BL.Validation.User;
using Hackathon.Common.Models.FileStorage;

namespace Hackathon.BL.Validation.Common;

public class UploadImageValidator : AbstractValidator<FileImage>
{
    public UploadImageValidator()
    {
        RuleFor(x => x)
           .Custom((image, context) =>
           {
               var validResult = image.IsValid();

               if (!validResult.IsSuccess)
               {
                   var error = validResult.Errors.Values.FirstOrDefault().Value;
                   context.AddFailure(error);
               }
           });
    }
}
