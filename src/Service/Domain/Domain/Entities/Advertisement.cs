using Ardalis.GuardClauses;
using Domain.Validation;
using FluentValidation;

namespace Domain.Entities;

public class Advertisement
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Номер объявления
    /// </summary>
    public int Number { get; private set; }

    /// <summary>
    /// ID пользователя, который создал объявление
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Основной текст объявления
    /// </summary>
    public string Text { get; private set; }

    /// <summary>
    /// Путь к картинке
    /// </summary>
    public string Image { get; private set; }

    /// <summary>
    /// Рейтинг от 0 до X
    /// </summary>
    public double Rating { get; private set; }

    /// <summary>
    /// Дата создания объявления
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Дата окончания публикации
    /// </summary>
    public DateTime EndDate { get; private set; }

    protected Advertisement()
    {
    }

    public Advertisement(
        Guid userId,
        int number,
        string text,
        string image,
        DateTime endDate)
    {
        Guard.Against.NullOrWhiteSpace(text, nameof(Text));
        Guard.Against.NullOrWhiteSpace(image, nameof(Image));
        Guard.Against.Default(endDate, nameof(EndDate));
        
        Id = Guid.NewGuid();
        Number = number;
        UserId = userId;
        Text = text;
        Image = SetImage(image);
        StartDate = DateTime.UtcNow;
        EndDate = endDate;
        
        var validator = new AdvertisementValidator();
        var result = validator.Validate(this);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }

    private static string SetImage(string imageUrl)
    {
        var validator = new ImageValidator();

        var result = validator.Validate(imageUrl);
        if (!result.IsValid)
        {
            throw new ArgumentException(result.Errors.First().ErrorMessage);
        }

        return imageUrl;
    }

    public void Update(string text, string? image, DateTime endDate)
    {
        Guard.Against.NullOrWhiteSpace(text, nameof(Text));
        Guard.Against.Default(endDate, nameof(EndDate));
        Text = text;
        if (image != null)
            Image = SetImage(image);
        EndDate = endDate;
        
        var validator = new AdvertisementValidator();
        var result = validator.Validate(this);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}