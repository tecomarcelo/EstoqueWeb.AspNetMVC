using EstoqueWeb.Infra.Data.Interfaces;
using EstoqueWeb.Infra.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurano para projeto MVC
builder.Services.AddControllersWithViews();

//habilitar uso de sessões no projeto parte01
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//habilitando o projeto para usar permissões de acesso parte01
builder.Services.Configure<CookiePolicyOptions>(options => { options.MinimumSameSitePolicy = SameSiteMode.None; });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

//capturar a connectionstring mapeada no "appsettings.json"
var connectionString =
builder.Configuration.GetConnectionString("EstoqueWeb");

//enviar a connectionstring para as classes do Repositorio (injeção de dependência).
builder.Services.AddTransient<IProdutoRepository>(map => new ProdutoRepository(connectionString));
builder.Services.AddTransient<IUsuarioRepository>(map => new UsuarioRepository(connectionString));

var app = builder.Build();

//habilitar uso de sessões no projeto parte02
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

//habilitando o projeto para usar permissões de acesso (autenticação e autorização) parte02
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}");

app.Run();
