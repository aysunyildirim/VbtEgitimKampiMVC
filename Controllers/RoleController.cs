using MediatR;
using Microsoft.AspNetCore.Mvc;
using VbtEgitimKampiMVC.Core.Application.Features.Role.Commands.Create;
using VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;

namespace VbtEgitimKampiMVC.Controllers;

public class RoleController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }
    // Kullanıcı oluşturmak için bir POST metodu ekliyoruz.
    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleCommandRequest request)
    {
        // Model validasyonu burada yapılabilir.

        if (ModelState.IsValid)
        {
            // MediatR ile komutu gönderiyoruz.
            var response = await _mediator.Send(request);

            if (response != null && response.Data != null)
            {
                // Başarılı bir şekilde kullanıcı oluşturulduğunda yönlendirme yapıyoruz.
                return View();
            }
            else
            {
                // Hata durumunda mesaj verilebilir.
                ModelState.AddModelError("", "Bir hata oluştu.");
            }
        }

        // Eğer model geçerli değilse tekrar formu gösteriyoruz.
        return View();
    }
}
