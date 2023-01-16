using EstoqueWeb.Infra.Data.Interfaces;
using EstoqueWeb.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EstoqueWeb.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //atributo
        private readonly IProdutoRepository _produtoRepository;
        
        public HomeController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index()
        {
            var model = new DashbordViewModel();
            
            try
            {
                //definindo as datas..
                var dataAtual = DateTime.Now;
                model.DataInicio = new DateTime(dataAtual.Year, 1, 1);
                model.DataFim = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month));
                
                //converter status
                var ativo = Convert.ToInt32(model.TotalAtivos);

                //ler o conteudo JSON gravado na sessão
                var json = HttpContext.Session.GetString("usuario");
                //deserializar o conteudo do JSON
                var usuario = JsonConvert.DeserializeObject<userIdentityModel>(json);

                //realizando a consulta dos produtos
                var produtosAtivos = _produtoRepository.GetByDatas(model.Nome, 1, usuario.Id);
                var produtosInativos = _produtoRepository.GetByDatas(model.Nome, 2, usuario.Id);
                var produtosTotais = _produtoRepository.GetByDatas(model.Nome, (1 + 2), usuario.Id);

                //calculando informação da model
                model.TotalAtivos = produtosAtivos.Count();
                model.TotalInativos = produtosInativos.Count();
                model.TotalGeral = (produtosAtivos.Count(e => e.Ativo == 1)+produtosInativos.Count(e => e.Ativo == 2));
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(model);
        }
    }
}
