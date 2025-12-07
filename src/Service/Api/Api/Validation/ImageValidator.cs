using FluentValidation;

namespace Api.Validation;

public class ImageValidator : AbstractValidator<IFormFile>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".svg"];
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

    public ImageValidator()
    {
        RuleFor(file => file.FileName)
            .Must(HaveValidExtension)
            .WithMessage("Разрешены только изображения с расширениями .jpg, .jpeg, .png или .svg.");

        RuleFor(file => file.Length)
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage($"Размер файла не должен превышать {MaxFileSize / 1024 / 1024} MB.");

        RuleFor(file => file.ContentType)
            .Must(ct => ct.StartsWith("image/"))
            .WithMessage("Файл должен быть изображением.");
    }

    private bool HaveValidExtension(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return AllowedExtensions.Contains(ext);
    }
}