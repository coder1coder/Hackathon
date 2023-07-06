using FluentValidation;
using Hackathon.Common.Models.FileStorage;

namespace Hackathon.BL.Validation.ImageFile;

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
