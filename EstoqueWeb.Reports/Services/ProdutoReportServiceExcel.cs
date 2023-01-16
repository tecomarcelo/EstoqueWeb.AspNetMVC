using EstoqueWeb.Infra.Data.Entities;
using EstoqueWeb.Reports.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoqueWeb.Reports.Services
{
    /// <summary>
    /// Classe para geração do relatorio em formato EXCEL
    /// </summary>
    public class ProdutoReportServiceExcel : IProdutoReportService
    {
        /// <summary>
        /// Metodo para geração do relatório
        /// </summary>
        /// <param name="nome">Criterio de pesquisa por nome</param>
        /// <param name="produtos">Lista de produtos</param>
        /// <returns>Arquivo em formato byte</returns>
        public byte[] Create(string? nome, int? ativo, List<Produto> produtos)
        {
            //configurando a biblioteca para uso não comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //Criando a planilha excel
            using (var excelPackage = new ExcelPackage())
            {
                                
                //criando a planilha
                var sheet = excelPackage.Workbook.Worksheets.Add("Produtos");

                //Titulo da planilha
                sheet.Cells["A1"].Value = "Relatòrio de Produtos";
                var titulo = sheet.Cells["A1:H1"];
                titulo.Merge = true;
                titulo.Style.Font.Size = 18;
                titulo.Style.Font.Bold = true;

                sheet.Cells["A2"].Value = "Projeto EstoqueWEB - Produtos";
                var subtitulo = sheet.Cells["A2:H2"];
                subtitulo.Merge = true;
                subtitulo.Style.Font.Size = 14;
                subtitulo.Style.Font.Bold = true;

                //Inserindo os campos de pesquisa
                sheet.Cells["A4"].Value = $"Pesquisar por Nome: {nome}";

                //Cabeçalho das colunas para impressão dos eventos
                sheet.Cells["A7"].Value = "ID do Produto";
                sheet.Cells["B7"].Value = "Nome do Produto";
                sheet.Cells["C7"].Value = "Preço";
                sheet.Cells["D7"].Value = "Quantidade";
                sheet.Cells["E7"].Value = "Descrição";
                sheet.Cells["F7"].Value = "Status";
                sheet.Cells["G7"].Value = "Data Inclusão";
                sheet.Cells["H7"].Value = "Data Alteração";

                var cabecalho = sheet.Cells["A7:H7"];
                cabecalho.Style.Font.Bold = true;
                cabecalho.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cabecalho.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#7FFFD4"));

                var linha = 8;

                //varrendo e imprimento os produtos
                foreach (var item in produtos)
                {
                    sheet.Cells[$"A{linha}"].Value = item.Id;
                    sheet.Cells[$"B{linha}"].Value = item.Nome;
                    sheet.Cells[$"C{linha}"].Value = ((Decimal)item.Preco).ToString("C");
                    sheet.Cells[$"D{linha}"].Value = item.Quantidade;
                    sheet.Cells[$"E{linha}"].Value = item.Descricao;
                    sheet.Cells[$"F{linha}"].Value = item.Ativo == 1 ? "ATIVO" : "INATIVO";
                    sheet.Cells[$"G{linha}"].Value = ((DateTime)item.DataInclusao).ToString("dd/MM/yyyy");
                    sheet.Cells[$"H{linha}"].Value = ((DateTime)item.DataAlteracao).ToString("dd/MM/yyyy");

                    linha++;
                }

                //ajustar a largura das colunas
                sheet.Cells["A:A,B:B,F:H"].AutoFitColumns();

                //Borda no grid
                var grid = sheet.Cells[$"A7:H{linha - 1}"];

                //retornar a planilha execel em formato de arquivo
                return excelPackage.GetAsByteArray();
            }
        }
    }
}
