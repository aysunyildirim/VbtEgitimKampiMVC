using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VbtEgitimKampiMVC.Core.Application.Features.Role.Commands.Create;
using VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;
using VbtEgitimKampiMVC.Core.Application.Services.Repositories;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Context;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // SqlServer örneği

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssembly(typeof(CreateRoleCommandHandlerValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateUserCommandHandlerValidator).Assembly);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<CreateUserCommandRequest>(); // Komutların bulunduğu assembly'yi belirtiyoruz
    config.RegisterServicesFromAssemblyContaining<CreateRoleCommandRequest>(); // Komutların bulunduğu assembly'yi belirtiyoruz
});

var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
