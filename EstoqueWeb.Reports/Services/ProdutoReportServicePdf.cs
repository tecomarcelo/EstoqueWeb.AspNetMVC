using EstoqueWeb.Infra.Data.Entities;
using EstoqueWeb.Reports.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoqueWeb.Reports.Services
{
    /// <summary>
    /// Classe para geração do relatório em formato PDF
    /// </summary>
    public class ProdutoReportServicePdf : IProdutoReportService
    {
        /// <summary>
        /// Metodo para geração do relatorio
        /// </summary>
        /// <param name="nome">Criterio de pesquisa por nome</param>
        /// <param name="produtos">Lista de produtos</param>
        /// <returns>Arquivo em formato byte</returns>
        public byte[] Create(string? nome, int? ativo, List<Produto> produtos)
        {
            //criando o arquivo PDF
            var memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            //escrevendo o conteudo do arquivo
            using (var document = new Document(pdf))
            {
                //espaçamento entre linhas
                document.SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 1f));
                
                //titulo do documento
                document.Add(new Paragraph("Relatório de Produtos")
                    .AddStyle(new Style().SetFontSize(24)));
                document.Add(new Paragraph("Projeto - EstoqueWEB")
                    .AddStyle(new Style().SetFontSize(14)));

                //informações do relatório
                var table = new Table(2); //2 -> numero de colunas
                var status = ativo == 1 ? "ATIVO" : "INATIVO";

                table.AddCell("Pesquisa por nome:");
                table.AddCell(nome);

                table.AddCell("Somente Status:");
                table.AddCell(status);

                table.AddCell("Quantidade:");
                table.AddCell($"{produtos.Count} produtos(s) obtidos na pesquisa.");

                document.Add(table);

                document.Add(new Paragraph("\n"));
                document.Add(new LineSeparator(new SolidLine(1f)));
                document.Add(new Paragraph("\n"));


                //imprimindo produtos
                foreach (var item in produtos)
                {
                    document.Add(new Paragraph($"ID: \n").SetBold());
                    document.Add(new Paragraph($"{item.Id}"));
                    document.Add(new Paragraph($"Nome: \n").SetBold());
                    document.Add(new Paragraph($"{item.Nome}"));
                    document.Add(new Paragraph($"Preço: \n").SetBold());
                    document.Add(new Paragraph($"{item.Preco}"));
                    document.Add(new Paragraph($"Quantidade: \n").SetBold());
                    document.Add(new Paragraph($"{item.Quantidade}"));
                    document.Add(new Paragraph($"Descrição: \n").SetBold());
                    document.Add(new Paragraph($"{item.Descricao}"));
                    document.Add(new Paragraph($"Data de Inclusão:{((DateTime)item.DataInclusao).ToString("dd/MM/yyyy")}"));
                    document.Add(new Paragraph($"Ultima Alteração: {((DateTime)item.DataAlteracao).ToString("dd/MM/yyyy")}"));

                    document.Add(new Paragraph("\n"));
                    document.Add(new LineSeparator(new SolidLine(1f)));
                    document.Add(new Paragraph("\n"));
                }
            }
            //retornando o contiudo do arquivo
            return memoryStream.ToArray();
        }
    }
}
