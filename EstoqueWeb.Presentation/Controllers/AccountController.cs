using EstoqueWeb.Infra.Data.Interfaces;
using EstoqueWeb.Infra.Data.Entities;
using EstoqueWeb.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using EstoqueWeb.Infra.Data.Util;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace EstoqueWeb.Presentation.Controllers
{
    public class AccountController : Controller
    {
        //atributo
        private readonly IUsuarioRepository _usuarioRepository;

        //construtor para inicialização do atributo
        public AccountController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(AccountLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //procurar o usuario no BD atraves do email e senha
                    var usuario = _usuarioRepository
                        .GetByEmailESenha(model.Email, CriptografiaUtil.GetMD5(model.Senha));

                    //verificar se o usuario foi encontrado
                    if(usuario != null)
                    {
                        TempData["MensagemSucesso"] = $"Parabéns, {usuario.Nome}! Acesso ao sistema realizado com sucesso.";

                        //gravar o nome do usuario atutenticado na sessão
                        var userIdentityModel = new userIdentityModel
                        {
                            Id = usuario.Id,
                            Nome = usuario.Nome,
                            Email = usuario.Email,
                            DataInclusao = usuario.DataInclusao,
                            DataHoraAcesso = DateTime.Now
                        };
                        //converter o objeto em JSON
                        var json = JsonConvert.SerializeObject(userIdentityModel);
                        HttpContext.Session.SetString("usuario", json);

                        #region Criando permissão de acesso a usuário

                        var autorizacao = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario.Id.ToString()) },
                                            CookieAuthenticationDefaults.AuthenticationScheme);

                        var claimPrincipal = new ClaimsPrincipal(autorizacao);
                                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);

                        #endregion

                        //redirecionar para a pagina inicial do projeto
                        return RedirectToAction("Index", "Home"); //home/Index                        
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Acesso negado. Usuário ou senha inválido.";
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }                
            }

            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //verificar se o email informado já esta cadastrado no BD
                    if (_usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["MensagemErro"] = $"O email informado já está cadastrado. Tente outro.";
                    }
                    else
                    {
                        var usuario = new Usuario();

                        usuario.Id = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = CriptografiaUtil.GetMD5(model.Senha);
                        usuario.DataInclusao = DateTime.Now;

                        //gravando no banco de dados
                        _usuarioRepository.Create(usuario);

                        TempData["MensagemSucesso"] = $"Usuário {usuario.Nome}, cadastrado com sucesso!";
                        ModelState.Clear(); //limpando os campos do formulario (model)
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }            
            return View();
        }

        [Authorize]
        public IActionResult UserData()
        {
            return View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            //apagar os dados do usuario da sessao
            HttpContext.Session.Remove("usuario");

            //apagar a permissão data ao usuario atenticado
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //redirecionar o usuario para  a pagina de login
            return RedirectToAction("Login");
        }
    }
}
