using EstoqueWeb.Infra.Data.Interfaces;
using EstoqueWeb.Infra.Data.Entities;
using EstoqueWeb.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using EstoqueWeb.Reports.Interfaces;
using EstoqueWeb.Reports.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EstoqueWeb.Presentation.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {
        //atributo
        private readonly IProdutoRepository _produtoRepository;

        //construtor para inicializar o atributo
        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost] //Annotation indica que o metodo ser executado no SUBMIT
        public IActionResult Cadastro(ProdutoCadastroViewModel model)
        {
            //Verificar se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    //ler o conteudo JSON gravado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    //deserializar o conteudo do JSON
                    var usuario = JsonConvert.DeserializeObject<userIdentityModel>(json);

                    var produto = new Produto
                    {
                        Id = Guid.NewGuid(),
                        Nome = model.Nome,
                        Preco = Convert.ToDecimal(model.Preco),
                        Quantidade = Convert.ToInt32(model.Quantidade),
                        Descricao = model.Descricao,
                        Ativo = Convert.ToInt32(model.Ativo),
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now,
                        IdUsuario = usuario.Id //Foreign key (chave estrangeira)
                    };

                    //gravando no banco de dados
                    _produtoRepository.Create(produto);

                    TempData["MensagemSucesso"] = $"Produto {produto.Nome}, Cadastrado com sucesso!";
                    ModelState.Clear(); //limpando os campos do formulario (model)
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }

            }

            else
            {
                TempData["MensagemAlerta"]
                    = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            return View();
        }
        
        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost] //Annotation indica que o metodo será executado no SUBMIT
        public IActionResult Consulta(ProdutoConsultaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //converter status
                    var ativo = Convert.ToInt32(model.Ativo);

                    //ler o conteudo JSON gravado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    //deserializar o conteudo do JSON
                    var usuario = JsonConvert.DeserializeObject<userIdentityModel>(json);

                    //realizando a consulta dos produtos
                    model.Produtos = _produtoRepository.GetByDatas(model.Nome, model.Ativo, usuario.Id);
                    
                    if (model.Produtos.Count > 0)
                    {
                        TempData["MensagemSucesso"] = $"{model.Produtos.Count} produto(s) obtido(s) para a pesquisa";
                    }

                    else
                    {
                        TempData["MensagemAlerta"] = "Nenhum produto foi encontrado para pesquisa realizada.";
                    }
                }

                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message; 
                }
            }
            else
            {
                TempData["MesagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            //voltando para página de consulta
            return View(model);
        }

        public IActionResult Edicao(Guid id)
        {
            var model = new ProdutoEdicaoViewModel();

            try
            {
                //consultar o evento no banco de dados pelo ID
                var produto = _produtoRepository.GetById(id);

                //Preencher os dados da classe model com as informações do produto
                model.Id = produto.Id;
                model.Nome = produto.Nome;
                model.Preco = produto.Preco;
                model.Quantidade = produto.Quantidade;
                model.Descricao = produto.Descricao;
                model.Ativo = produto.Ativo;
                model.DataAlteracao = Convert.ToDateTime(produto.DataAlteracao).ToString();
                model.DataInclusao = Convert.ToDateTime(produto.DataInclusao).ToString();
            }

            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //enviando o model para a pagina
            return View(model);
        }
        //Metodo para receber o SUBMIT da pagina de EDIÇÃO (POST)
        [HttpPost]
        public IActionResult Edicao(ProdutoEdicaoViewModel model)
        {
            //verificar se todos os campos passaram nas regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    //obtendo os dados do produto no banco de dados
                    var produto = _produtoRepository.GetById(model.Id);

                    //ler o conteudo JSON gravado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    //deserializar o conteudo do JSON
                    var usuario = JsonConvert.DeserializeObject<userIdentityModel>(json);

                    //modificando os dados do produto
                    produto.Nome = model.Nome;
                    produto.Preco = Convert.ToDecimal(model.Preco);
                    produto.Quantidade = Convert.ToInt32(model.Quantidade);
                    produto.Descricao = model.Descricao;
                    produto.Ativo = Convert.ToInt32(model.Ativo);
                    produto.DataAlteracao = DateTime.Now;
                    produto.IdUsuario = usuario.Id;

                    //atualizando no banco de dados
                    _produtoRepository.Update(produto);

                    TempData["MensagemSucesso"] = $"Produto '{produto.Nome}', atualizado com sucesso! ";

                    //redirecionamento para a pagina de consulta
                    return RedirectToAction("Consulta");
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            return View();
        }


        public IActionResult Exclusao(Guid id)
        {
            try
            {
                //buscar produto no banco de dados
                var produto = _produtoRepository.GetById(id);

                //excluindo produto
                _produtoRepository.Delete(produto);

                TempData["MensagemSucesso"] = $"Produto '{produto.Nome}', excluido com sucesso.";
            }

            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //redirecionar de volta para a pagina de consulta
            return RedirectToAction("Consulta");
        }

        public IActionResult Relatorio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Relatorio(ProdutoRelatorioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //capturar o nome enviado
                    string? nome = model.Nome;
                    int? ativo = Convert.ToInt32(model.Ativo);

                    //ler o conteudo JSON gravado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    //deserializar o conteudo do JSON
                    var usuario = JsonConvert.DeserializeObject<userIdentityModel>(json);

                    //Consultar os produtos no banco atraves do nome
                    var produtos = _produtoRepository.GetByDatas(nome, ativo, usuario.Id);

                    //verificar se algum produto foi obtido
                    if(produtos.Count > 0)
                    {
                        //criando objeto para interface
                        IProdutoReportService produtoReportService = null;  //vazio

                        //variaveis para definir os parametros de download
                        var contentType = string.Empty;  //MIME TYPE
                        var fileName = string.Empty;

                        switch (model.Formato)
                        {
                            case 0: //polimorfismo
                                produtoReportService = new ProdutoReportServicePdf();
                                contentType = "application/pdf";
                                fileName = $"produtos_{DateTime.Now.ToString("ddMMyyyy")}.pdf";

                                break;

                            case 1: //polimorfismo
                                produtoReportService = new ProdutoReportServiceExcel();
                                contentType = "application/vnd.openxml";
                                fileName = $"produtos_{DateTime.Now.ToString("ddMMyyyy")}.xlsx";

                                break;
                        }
                        //gerando o arquivo do relatório
                        var arquivo = produtoReportService.Create(nome, ativo, produtos);

                        //Download do arquivo
                        return File(arquivo, contentType, fileName);
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Nenhum produto foi obtido para a pesquisa informada.";
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }        
    }
}
