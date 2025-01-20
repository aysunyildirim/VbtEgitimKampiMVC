using FluentValidation;

namespace VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;

public class CreateUserCommandHandlerValidator : AbstractValidator<CreateUserCommandRequest>
{
    public CreateUserCommandHandlerValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("İsim Soyisim alanı gereklidir.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("İsim Soyisim sadece harf ve boşluklardan oluşmalıdır.")
            .Length(2, 100).WithMessage("İsim Soyisim 2 ile 100 karakter arasında olmalıdır.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta alanı gereklidir.")
            .EmailAddress().WithMessage("Geçersiz e-posta formatı.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon numarası gereklidir.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Telefon numarası geçerli olmalı ve uluslararası kod içerebilir.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı gereklidir.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter uzunluğunda olmalıdır.")
            .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Rol ID'si sıfırdan büyük olmalıdır.")
            .LessThan(1000).WithMessage("Rol ID'si 1000'den küçük olmalıdır.");

    }
}
