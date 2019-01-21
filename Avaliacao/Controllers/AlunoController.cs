using Avaliacao.DAL;
using Avaliacao.DAL.Repositorio;
using Avaliacao.Models;
using Avaliacao.Models.Aluno;
using ExpressionBuilder.Common;
using ExpressionBuilder.Generics;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Avaliacao.Controllers
{
    public class AlunoController : Controller
    {
        // GET: Aluno / Lista

        //https://docs.microsoft.com/pt-br/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
        //https://stackoverflow.com/questions/2728340/how-can-i-do-an-orderby-with-a-dynamic-string-parameter
        //http://jasonwatmore.com/post/2014/07/16/dynamic-linq-using-strings-to-sort-by-properties-and-child-object-properties
        //http://www.macoratti.net/12/01/aspn_mvc32.htm
        public async Task<ActionResult> Lista(string sortOrder = "id", string strCriterio = "", int? page = 1)
        {

            if (Session["usuarioId"] == null)
            {              
                return RedirectToAction("Login","Home");
            }

            if (ViewBag.Orders == null)
            {
                //Se tivesse mais tempo faria isto dinamicamente
                ViewBag.Orders = new List<string> { "id", "Nome", "CPF", "data_nascimento", "sexo", "telefonemov", "data_cadastro", "cidade" };
            }

            if (!string.IsNullOrEmpty(strCriterio))
            {
                string[] valores = strCriterio.Split('|');
                if (valores.Count() == 3)
                {
                    page = 1;
                }
            }

            List<ListAlunos> listAlunoViewModel = new List<ListAlunos>();

            await Task.Factory.StartNew(() =>
            {
                using (Repositorio<Aluno> ralu = new Repositorio<Aluno>())
                {
                    Expression<Func<Aluno, bool>> expressao = null;
                    IQueryable<Aluno> registros;


                    if (!string.IsNullOrEmpty(strCriterio))
                    {
                        var filter = new Filter<Aluno>();
                        string tipoCampo, nomeCampo, valorCampo;
                        tipoCampo = nomeCampo = valorCampo = string.Empty;

                        string[] valores = strCriterio.Split('|');
                        tipoCampo = valores[0];
                        nomeCampo = valores[1];
                        valorCampo = valores[2];

                        ViewBag.CurrentFilter = valorCampo;


                        if (tipoCampo == "text")
                        {
                            filter.By(nomeCampo.Replace(" desc", ""), Operation.StartsWith, valorCampo);
                            expressao = filter.GetExpression(filter);

                        }
                        else if (tipoCampo == "number")
                        {
                            int valorInt = Convert.ToInt32(valorCampo);
                            filter.By(nomeCampo.Replace(" desc", ""), Operation.EqualTo, valorInt);
                            expressao = filter.GetExpression(filter);
                        }                            
                        else if(tipoCampo == "date")
                        {
                            DateTime data = Convert.ToDateTime(valorCampo);
                            filter.By(nomeCampo.Replace(" desc", ""), Operation.EqualTo, data);
                            expressao = filter.GetExpression(filter);
                        }

                        if (expressao != null)
                            registros = ralu.OrderBy(ralu.Get(expressao), sortOrder.Replace(" desc", ""), !sortOrder.EndsWith(" desc"));
                        else
                            registros = ralu.OrderBy(ralu.Get(), sortOrder.Replace(" desc", ""), !sortOrder.EndsWith(" desc"));
                    }
                    else
                    {
                        //sortOrder.Replace("id", "cMun") - Força o mapeamento virtual
                        registros = ralu.OrderBy(ralu.Get(), sortOrder.Replace(" desc", ""), !sortOrder.EndsWith(" desc"));
                    }

                    var registrosEnum = registros.GetEnumerator();

                    //Se mais tempo seria AutoMapper
                    while (registrosEnum.MoveNext())
                        listAlunoViewModel.Add(new ListAlunos
                        {
                            id = registrosEnum.Current.id,
                            Nome = registrosEnum.Current.nome,
                            CPF = registrosEnum.Current.cpf,
                            data_nascimento = registrosEnum.Current.data_nascimento,
                            sexo = registrosEnum.Current.sexo,
                            telefonemov = registrosEnum.Current.telefonemov,
                            cidade = registrosEnum.Current.cidade,
                            data_cadastro = registrosEnum.Current.data_cadastro
                        });


                }
            });

            var listaOrder = (List<string>)ViewBag.Orders;

            for (int i = 0; i < listaOrder.Count; i++)
            {
                if (listaOrder[i] == sortOrder)
                {
                    if (listaOrder[i].EndsWith(" desc"))
                        listaOrder[i] = listaOrder[i].Replace(" desc", "");

                    else
                        listaOrder[i] = listaOrder[i] + " desc";
                }
            }

            ViewBag.Orders = listaOrder;




            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(listAlunoViewModel.ToPagedList(pageNumber, pageSize));
        }


        //GET: /Aluno/UpInsert
        public ActionResult UpInsert(string modeid = "")
        {

            if (Session["usuarioId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            //"INS|0", "UPD|558", "DEL|6664"

            Aluno alu;

            var valorId = modeid.Replace("%7C", "|");

            ViewBag.modeid = modeid;


            var cities = new System.Collections.Generic.List<CidadeViewDropList>();



            if (!string.IsNullOrEmpty(modeid))
            {
                if (modeid.StartsWith("INS"))
                {
                    alu = new Aluno();
                    alu.uf = "MS";
                    alu.codibgecidade = string.Empty;
                    alu.data_cadastro = DateTime.Now;
                    ViewBag.Title = "Novo";
                    ViewData.Model = alu;

                    using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
                    {
                        var lista = cid.Get(e => e.uf == alu.uf).GetEnumerator();

                        while (lista.MoveNext())
                            cities.Add(new CidadeViewDropList { cMun = lista.Current.cMun, xMun = lista.Current.xMun });
                    }

                    ViewBag.ListaCidade = cities;

                    return View(alu);
                }
                else if (modeid.StartsWith("UPD"))
                {
                    string valor = valorId.Split('|')[1];
                    using (Repositorio<Aluno> ralu = new Repositorio<Aluno>())
                    {
                        int valorint = Convert.ToInt32(valor);
                        alu = ralu.Primeiro(e => e.id == valorint);
                        ViewBag.Title = "Editar";
                        ViewData.Model = alu;
                    }
                    using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
                    {
                        var lista = cid.Get(e => e.uf == alu.uf).GetEnumerator();

                        while (lista.MoveNext())
                            cities.Add(new CidadeViewDropList { cMun = lista.Current.cMun, xMun = lista.Current.xMun });
                    }

                    ViewBag.ListaCidade = cities;
                    return View(alu);
                }
                else if (modeid.StartsWith("DEL"))
                {
                    string valor = valorId.Split('|')[1];
                    using (Repositorio<Aluno> rcid = new Repositorio<Aluno>())
                    {
                        int valorint = Convert.ToInt32(valor);

                        alu = rcid.Primeiro(e => e.id == valorint);
                        ViewBag.Title = "Remover";
                        ViewData.Model = alu;
                    }
                    using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
                    {
                        var lista = cid.Get(e => e.uf == alu.uf).GetEnumerator();

                        while (lista.MoveNext())
                            cities.Add(new CidadeViewDropList { cMun = lista.Current.cMun, xMun = lista.Current.xMun });
                    }

                    ViewBag.ListaCidade = cities;
                    return View(alu);
                }
            }

            return RedirectToAction("Lista");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpInsert(string modeid, Aluno a)
        {
            //Daria para criptografar a URL
            bool erro = false;

            if (ModelState.IsValid)
            {
                if (!a.cpf.ValidaCPF())
                {
                    ModelState.AddModelError("CustomError", "O valor informado no campo CPF é inválido.");
                    erro = true;
                }
            }
            else
                erro = true;

            if (!erro)
            {


                var valorId = modeid.Replace("%7C", "|");


                try
                {
                    using (Repositorio<Aluno> ralu = new Repositorio<Aluno>())
                    {
                        if (valorId.Split('|')[0] == "INS")
                        {

                            var registro = ralu.Primeiro(e => e.cpf == a.cpf);

                            if (registro == null)
                            {
                                a.ultmodificacao = DateTime.Now.ToUniversalTime();
                                if (!string.IsNullOrEmpty(Session["usuarioId"] as string))
                                    a.usuarioIdmod = Convert.ToInt32(Session["usuarioId"].ToString());

                                //Adiciona o registro
                                ralu.Adicionar(a);
                                ralu.Commit();


                                return RedirectToAction("Lista");
                            }
                            else
                            {
                                ModelState.AddModelError("CustomError", string.Format("Esta Aluno com o CPF {0} já foi cadastrada no sistema!", a.cpf));

                            }
                        }
                        else if (valorId.Split('|')[0] == "UPD")
                        {
                            //Adiciona o registro

                            a.ultmodificacao = DateTime.Now.ToUniversalTime();
                            if (!string.IsNullOrEmpty(Session["usuarioId"] as string))
                                a.usuarioIdmod = Convert.ToInt32(Session["usuarioId"].ToString());

                            ralu.Atualizar(a);
                            ralu.Commit();

                            return RedirectToAction("Lista");
                        }
                        else if (valorId.Split('|')[0] == "DEL")
                        {
                            //Adiciona o registro
                            ralu.Deletar(e => e.id == Convert.ToInt32(valorId.Split('|')[1]));
                            ralu.Commit();

                            return RedirectToAction("Lista");
                        }
                        else
                        {
                            ModelState.AddModelError("CustomError", "Modo de operação inválido!");
                        }
                    }
                }
                catch (Exception ex) //ERRO não amigável 
                {
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }

            var cities = new System.Collections.Generic.List<CidadeViewDropList>();

            using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
            {
                var lista = cid.Get(e => e.uf == a.uf).GetEnumerator();

                while (lista.MoveNext())
                    cities.Add(new CidadeViewDropList { cMun = lista.Current.cMun, xMun = lista.Current.xMun });
            }

            ViewBag.ListaCidade = cities;

            return View(a);
        }
    }
}