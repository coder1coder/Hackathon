using FluentValidation;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.FileStorage;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Validation.ImageFile;

public class FileImageValidator : AbstractValidator<IFileImage>
{
    private string[] _allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

    public int MinWidthProfileImage => 150;
    public int MinHeightProfileImage => 150;

    public int MinWidthEventImage => 500;
    public int MinHeightEventImage => 500;

    public FileImageValidator(IOptions<FileSettings> fileSettings)
    {
        var _fileSettings = fileSettings?.Value ?? new FileSettings();

        //Правила валидации для изображений профиля
        RuleSet("ProfileImage", () =>
        {
            RuleFor(x => x.Width)
                .Must(width => width >= MinWidthProfileImage)
                .WithMessage(GetErrorSizeMessage(MinWidthProfileImage, MinHeightProfileImage));

            RuleFor(x => x.Height)
                .Must(height => height >= MinHeightProfileImage)
                .WithMessage(GetErrorSizeMessage(MinWidthProfileImage, MinHeightProfileImage));

            RuleFor(x => x.Length)
                .Must(length => length >= _fileSettings.ProfileFileImage.MinLength && length <= _fileSettings.ProfileFileImage.MaxLength)
                .WithMessage(GetErrorLengthMessage(_fileSettings.ProfileFileImage.MinLength, _fileSettings.ProfileFileImage.MaxLength));

            RuleFor(x => x.Extension)
                .Must(extension => _allowedExtensions.Contains(extension))
                .WithMessage(GetErrorExtensionMessage());
        });

        //Правила валидации для изображений события
        RuleSet("EventImage", () =>
        {
            RuleFor(x => x.Width)
                .Must(width => width >= MinWidthEventImage)
                .WithMessage(GetErrorSizeMessage(MinWidthEventImage, MinHeightEventImage));

            RuleFor(x => x.Height)
                .Must(height => height >= MinHeightEventImage)
                .WithMessage(GetErrorSizeMessage(MinWidthEventImage, MinHeightEventImage));

            RuleFor(x => x.Length)
                .Must(length => length >= _fileSettings.EventFileImage.MinLength && length <= _fileSettings.EventFileImage.MaxLength)
                .WithMessage(GetErrorLengthMessage(_fileSettings.EventFileImage.MinLength, _fileSettings.EventFileImage.MaxLength));

            RuleFor(x => x.Extension)
                .Must(extension => _allowedExtensions.Contains(extension))
                .WithMessage(GetErrorExtensionMessage());
        });
    }

    private string GetErrorSizeMessage(int minWidth, int minHeight) => string.Format(FileImageErrorMessages.ErrorSizeMessage,
        minWidth, minHeight);
    private string GetErrorLengthMessage(long minLength, long maxLength) => string.Format(FileImageErrorMessages.ErrorLengthMessage, 
        minLength, GetMB(maxLength));
    private string GetErrorExtensionMessage() => string.Format(FileImageErrorMessages.ErrorExtensionMessage,
        string.Join(" ", _allowedExtensions));

    private double GetMB(long maxLength)
    {
        return Math.Round(maxLength / 1024d / 1000d);
    }
}
