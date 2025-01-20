using FluentValidation;
using VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;

namespace VbtEgitimKampiMVC.Core.Application.Features.Role.Commands.Create;

public class CreateRoleCommandHandlerValidator : AbstractValidator<CreateRoleCommandRequest>
{
    public CreateRoleCommandHandlerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Rolun adı alanı gereklidir.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("İsim Soyisim sadece harf ve boşluklardan oluşmalıdır.")
            .Length(2, 100).WithMessage("İsim Soyisim 2 ile 100 karakter arasında olmalıdır.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("E-posta alanı gereklidir.")
            .EmailAddress().WithMessage("Geçersiz e-posta formatı.");

    }
}
