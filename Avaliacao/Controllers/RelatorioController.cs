using Avaliacao.DAL.Repositorio;
using Avaliacao.Models;
using Avaliacao.Models.Aluno;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Avaliacao.Controllers
{


    //https://www.devmedia.com.br/como-gerar-relatorios-no-asp-net-mvc/33921
    //http://www.macoratti.net/18/04/mvc_relatpdf1.htm

    public class RelatorioController : Controller
    {
        // GET: Relatorio
        public ActionResult Aluno()
        {
            if (Session["usuarioId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        // GET: Relatorio
        public ActionResult Cidade()
        {
            if (Session["usuarioId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // GET: Relatorio
        public ActionResult AluCid()
        {
            if (Session["usuarioId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }


        [HttpPost]
        [ActionName("Alunos")]
        public ActionResult Alunos()
        {
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            //Top Heading
            Chunk chunk = new Chunk("Relatórios de alunos", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            pdfDoc.Add(chunk);

            //Horizontal Line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);


            //Table
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell
            PdfPCell cell = new PdfPCell();
            chunk = new Chunk("Listagem");
            cell.AddElement(chunk);
            cell.Colspan = 5;
            cell.BackgroundColor = BaseColor.GRAY;
            table.AddCell(cell);

            table.AddCell("Código");
            table.AddCell("Nome");
            table.AddCell("CPF");
            table.AddCell("Sexo");
            table.AddCell("Email");
            int count = 0;
            using(Repositorio<Aluno> alu = new Repositorio<Aluno>())
            {
                var todos = alu.GetTodos();
                var a = todos.GetEnumerator();
                while (a.MoveNext())
                {
                    table.AddCell(a.Current.id.ToString());
                    table.AddCell(a.Current.nome.ToString());
                    table.AddCell(a.Current.cpf.ToString());
                    table.AddCell(a.Current.sexo.ToString());
                    table.AddCell(a.Current.email.ToString());

                    count++;
                }
            }                  

            pdfDoc.Add(table);
            //Horizontal Line
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            Paragraph para = new Paragraph();
            para.Add(DateTime.Now.ToString() + " - Registros: " + count.ToString("000000"));
            para.SpacingBefore = 10f;
            para.SpacingAfter = 10f;
            pdfDoc.Add(para);
                    
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Alunos.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return View();
        }


        [HttpPost]
        [ActionName("Cidades")]
        public async Task<ActionResult> Cidades(string uf="")
        {
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            //Top Heading
            Chunk chunk = new Chunk("Relatórios de Cidades", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            pdfDoc.Add(chunk);

            //Horizontal Line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);


            //Table
            PdfPTable table = new PdfPTable(3);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell
            PdfPCell cell = new PdfPCell();
            chunk = new Chunk("Listagem");
            cell.AddElement(chunk);
            cell.Colspan = 3;
            cell.BackgroundColor = BaseColor.GRAY;
            table.AddCell(cell);

            table.AddCell("Código IBGE");
            table.AddCell("Nome Cidade");
            table.AddCell("UF");
          
            int count = 0;
            using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
            {
                List<Cidade> cidades = new List<Cidade>();
                if (!string.IsNullOrEmpty(uf))
                {
                    cidades = await cid.GetTodosAsync(c => c.uf == uf);
                }
                else
                {
                    cidades = await cid.GetTodosAsync();
                }

                foreach (var item in cidades)
                {
                    table.AddCell(item.cMun.ToString());
                    table.AddCell(item.xMun.ToString());
                    table.AddCell(item.uf.ToString());

                    count++;
                }

            }

            pdfDoc.Add(table);
            //Horizontal Line
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            Paragraph para = new Paragraph();
            para.Add(DateTime.Now.ToString() + " - Registros: " + count.ToString("000000"));
            para.SpacingBefore = 10f;
            para.SpacingAfter = 10f;
            pdfDoc.Add(para);

            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Cidades.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return View();
        }

        [HttpPost]
        [ActionName("AluCidRel")]
        public async Task<ActionResult> AluCidRel(string uf = "")
        {
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            //Top Heading
            Chunk chunk = new Chunk("Relatórios de Alunos por Cidade " + (string.IsNullOrEmpty(uf)?"- TODAS CIDADES": "- " + uf), FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            pdfDoc.Add(chunk);

            //Horizontal Line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);


            //Table
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell
            PdfPCell cell = new PdfPCell();
            chunk = new Chunk("Listagem");
            cell.AddElement(chunk);
            cell.Colspan = 3;
            cell.BackgroundColor = BaseColor.BLUE;
            table.AddCell(cell);
            pdfDoc.Add(table);

            int count = 0;
            using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
            {
                List<Cidade> cidades = new List<Cidade>();
                if (!string.IsNullOrEmpty(uf))
                {
                    cidades = await cid.GetTodosAsync(c => c.uf == uf);
                }
                else
                {
                    cidades = await cid.GetTodosAsync();
                }

                foreach (var item in cidades)
                {
                    using (Repositorio<Aluno> alu = new Repositorio<Aluno>())
                    {
                        var alunos = await alu.GetTodosAsync(a => a.codibgecidade == item.cMun);

                        if (alunos.Count > 0)
                        {
                            chunk = new Chunk(item.uf + " - " + item.cMun + " - " + item.xMun, FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK));
                            line = new Paragraph(chunk);
                           pdfDoc.Add(line);
                        }

                        foreach (var aluno in alunos)
                        {
                            chunk = new Chunk("         " + aluno.cpf + " - " + aluno.nome, FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK));
                            line = new Paragraph(chunk);
                            pdfDoc.Add(line);
                        }
                        if (alunos.Count > 0)
                        {
                            //Horizontal Line
                            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                            line.SpacingBefore = 2f;
                            line.SpacingAfter = 2f;
                            pdfDoc.Add(line);
                            count += alunos.Count;
                            chunk = new Chunk("Registros: " + alunos.Count.ToString("000000"), FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK));
                            line = new Paragraph(chunk);
                            line.SpacingAfter = 8f;
                            pdfDoc.Add(line);
                        }


                    }
                }
            }
            
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            Paragraph para = new Paragraph();
            para.Add(DateTime.Now.ToString() + " - Registros Geral: " + count.ToString("000000"));
            para.SpacingBefore = 10f;
            para.SpacingAfter = 10f;
            pdfDoc.Add(para);

            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Alunos por Cidade.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return View();
        }

    }
}