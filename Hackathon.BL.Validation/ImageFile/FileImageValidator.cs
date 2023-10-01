using FluentValidation;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.FileStorage;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Validation.ImageFile;

public class FileImageValidator : AbstractValidator<IFileImage>
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

    public const int MinWidthProfileImage = 150;
    public const int MinHeightProfileImage = 150;

    public const int MinWidthEventImage = 500;
    public const int MinHeightEventImage = 500;

    public FileImageValidator(IOptions<FileSettings> fileSettingsOptions)
    {
        var fileSettings = fileSettingsOptions?.Value ?? new FileSettings();

        //Правила валидации для изображений профиля
        RuleSet(FileImageValidatorRuleSets.ProfileImage, () =>
        {
            RuleFor(x => x.Width)
                .Must(width => width >= MinWidthProfileImage)
                .WithMessage(GetErrorSizeMessage(MinWidthProfileImage, MinHeightProfileImage));

            RuleFor(x => x.Height)
                .Must(height => height >= MinHeightProfileImage)
                .WithMessage(GetErrorSizeMessage(MinWidthProfileImage, MinHeightProfileImage));

            RuleFor(x => x.Length)
                .Must(length => length >= fileSettings.ProfileFileImage.MinLength && length <= fileSettings.ProfileFileImage.MaxLength)
                .WithMessage(GetErrorLengthMessage(fileSettings.ProfileFileImage.MinLength, fileSettings.ProfileFileImage.MaxLength));

            RuleFor(x => x.Extension)
                .Must(extension => _allowedExtensions.Contains(extension))
                .WithMessage(GetErrorExtensionMessage());
        });

        //Правила валидации для изображений события
        RuleSet(FileImageValidatorRuleSets.EventImage, () =>
        {
            RuleFor(x => x.Width)
                .Must(width => width >= MinWidthEventImage)
                .WithMessage(GetErrorSizeMessage(MinWidthEventImage, MinHeightEventImage));

            RuleFor(x => x.Height)
                .Must(height => height >= MinHeightEventImage)
                .WithMessage(GetErrorSizeMessage(MinWidthEventImage, MinHeightEventImage));

            RuleFor(x => x.Length)
                .Must(length => length >= fileSettings.EventFileImage.MinLength && length <= fileSettings.EventFileImage.MaxLength)
                .WithMessage(GetErrorLengthMessage(fileSettings.EventFileImage.MinLength, fileSettings.EventFileImage.MaxLength));

            RuleFor(x => x.Extension)
                .Must(extension => _allowedExtensions.Contains(extension))
                .WithMessage(GetErrorExtensionMessage());
        });
    }

    private static string GetErrorSizeMessage(int minWidth, int minHeight) => string.Format(FileImageErrorMessages.ErrorSizeMessage,
        minWidth, minHeight);
    private static string GetErrorLengthMessage(long minLength, long maxLength) => string.Format(FileImageErrorMessages.ErrorLengthMessage,
        minLength, GetMegabytesFromBytes(maxLength));
    private string GetErrorExtensionMessage() => string.Format(FileImageErrorMessages.ErrorExtensionMessage,
        string.Join(" ", _allowedExtensions));

    private static double GetMegabytesFromBytes(long bytes) => Math.Round(bytes / 1024d / 1000d);
}
