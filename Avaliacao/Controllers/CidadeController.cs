using Avaliacao.DAL;
using Avaliacao.DAL.Repositorio;
using Avaliacao.Models;
using Avaliacao.wsCorreios;
using ExpressionBuilder.Common;
using ExpressionBuilder.Generics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;

namespace Avaliacao.Controllers
{
    public class CidadeController : Controller
    {
        // GET: Cidade / Lista                
        public async Task<ActionResult> Lista(string sortOrder="id", string strCriterio="", int? page = 1)
        {
            if (Session["usuarioId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ViewBag.Orders == null)
            {
                //Se tivesse mais tempo faria isto dinamicamente
                ViewBag.Orders = new List<string> { "id", "Nome", "xMun", "uf", "cep" };
            }

            if (!string.IsNullOrEmpty(strCriterio))
            {
                string[] valores = strCriterio.Split('|');
                if (valores.Count() == 3)
                {
            
                        page = 1;
                }
            }


            List<Cidade> listCidadeViewModel = new List<Cidade>();

            await Task.Factory.StartNew(() =>
            {
                using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
                {
                    Expression<Func<Cidade, bool>> expressao = null;
                    IQueryable<Cidade> registros;
                    

                    if (!string.IsNullOrEmpty(strCriterio))
                    {
                        var filter = new Filter<Cidade>();
                        string tipoCampo, nomeCampo, valorCampo;
                        tipoCampo = nomeCampo = valorCampo = string.Empty;

                        string[] valores = strCriterio.Split('|');
                        tipoCampo = valores[0];
                        nomeCampo = valores[1];
                        valorCampo = valores[2];

                        ViewBag.CurrentFilter = valorCampo;

                        //sortOrder.Replace("id", "cMun") - Força o mapeamento virtual
                        if (tipoCampo == "text")
                        {
                            filter.By(nomeCampo.Replace("id", "cMun").Replace(" desc", ""), Operation.StartsWith, valorCampo);
                            expressao = filter.GetExpression(filter);
                         
                        }                     
                        else if (tipoCampo == "number")
                        {
                            int valorInt = Convert.ToInt32(valorCampo);
                            filter.By(nomeCampo.Replace("id", "cMun").Replace(" desc", ""), Operation.EqualTo, valorInt);
                            expressao = filter.GetExpression(filter);
                        }
                        else if (tipoCampo == "date")
                        {
                            DateTime data = Convert.ToDateTime(valorCampo);
                            filter.By(nomeCampo.Replace("id", "cMun").Replace(" desc", ""), Operation.EqualTo, data);
                            expressao = filter.GetExpression(filter);
                        }

                        if (expressao!=null)
                            registros = cid.OrderBy(cid.Get(expressao), sortOrder.Replace("id", "cMun").Replace(" desc", ""), !sortOrder.EndsWith(" desc"));
                        else
                            registros = cid.OrderBy(cid.Get(), sortOrder.Replace("id", "cMun").Replace(" desc", ""), !sortOrder.EndsWith(" desc"));
                    }
                    else
                    {
                        //sortOrder.Replace("id", "cMun") - Força o mapeamento virtual
                        registros = cid.OrderBy(cid.Get(), sortOrder.Replace("id", "cMun").Replace(" desc", ""), !sortOrder.EndsWith(" desc"));
                    }

                    var registrosEnum = registros.GetEnumerator();

                    while (registrosEnum.MoveNext())
                        listCidadeViewModel.Add(registrosEnum.Current);

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


            /* ViewBag.page = (!string.IsNullOrEmpty(page) ? page : "0");
             ViewBag.sortOrder = (!string.IsNullOrEmpty(sortOrder) ? sortOrder : "ASC");
             ViewBag.CurrentFilter = (!string.IsNullOrEmpty(CurrentFilter) ? CurrentFilter : "");
             */

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(listCidadeViewModel.ToPagedList(pageNumber, pageSize));

            //return View(listCidadeViewModel);
        }
               
        //GET: /Cidade/ConsultaCEP
        public ActionResult ConsultaCEP()
        {
            return View();
        }

        //GET: /Cidade/FindCEP
        public async Task<ActionResult> FindCEP(string cepParam)
        {
            enderecoERP respostaCorreios = new enderecoERP();

            await Task.Factory.StartNew(() =>
            {
                var ws = new wsCorreios.AtendeClienteService();

                ws.Timeout = 1000 * 60;

                //Tratar
                //ws.Proxy

                try
                {
                    respostaCorreios = ws.consultaCEP(cepParam.RemoveMask());

                    /* resposta.end;
                     resposta.complemento + resposta.complemento2;
                     resposta.bairro;
                     resposta.cidade;
                     resposta.uf;*/
                }
                catch (Exception) //Daria para informar o motivo para o usuário
                {

                }
            });
            var resposta = JsonConvert.SerializeObject(respostaCorreios);
            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        //GET: /Cidade/FindEstado
        public ActionResult FindEstado(string estado)
        {
            return Json(new { uf = UtilDB.getUF(estado, "s") }, JsonRequestBehavior.AllowGet);
        }

        //GET: /Cidade/UpInsert
        public ActionResult UpInsert(string modeid = "")
        {

            if (Session["usuarioId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            //"INS|0", "UPD|558", "DEL|6664"

            Cidade cid;
            
            var valorId = modeid.Replace("%7C", "|");

            ViewBag.modeid = modeid;

            if (!string.IsNullOrEmpty(modeid))
            {
                if (modeid.StartsWith("INS"))
                {
                    cid = new Cidade();
                    ViewBag.Title = "Novo";
                    ViewData.Model = cid;

                    return View(cid);
                }
                else if (modeid.StartsWith("UPD"))
                {
                    string valor = valorId.Split('|')[1];
                    using (Repositorio<Cidade> rcid = new Repositorio<Cidade>())
                    {
                        cid = rcid.Primeiro(e => e.cMun == valor);
                        ViewBag.Title = "Editar";
                        ViewData.Model = cid;
                    }

                    return View(cid);
                }
                else if (modeid.StartsWith("DEL"))
                {
                    string valor = valorId.Split('|')[1];
                    using (Repositorio<Cidade> rcid = new Repositorio<Cidade>())
                    {
                        cid = rcid.Primeiro(e => e.cMun == valor);
                        ViewBag.Title = "Remover";
                        ViewData.Model = cid;
                    }

                    return View(cid);
                }
            }

            return RedirectToAction("Lista");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpInsert(string modeid, Cidade c)
        {
            //Daria para criptografar a URL

            var valorId = modeid.Replace("%7C", "|");
            if (ModelState.IsValid)
            {

                try
                {
                    using (Repositorio<Cidade> rcid = new Repositorio<Cidade>())
                    {
                        if (valorId.Split('|')[0] == "INS")
                        {

                            var registro = rcid.Primeiro(e => e.cMun == c.cMun);

                            if (registro == null)
                            {
                                //Adiciona o registro
                                rcid.Adicionar(c);
                                rcid.Commit();


                                return RedirectToAction("Lista");
                            }
                            else
                            {
                                ModelState.AddModelError("CustomError", string.Format("Esta Cidade com o código {0} já foi cadastrada no sistema!", c.cMun));

                            }
                        }
                        else if(valorId.Split('|')[0] == "UPD")
                        {
                            //Adiciona o registro
                            rcid.Atualizar(c);
                            rcid.Commit();

                            return RedirectToAction("Lista");
                        }else if (valorId.Split('|')[0] == "DEL")
                        {
                            //Adiciona o registro
                            rcid.Deletar(e=>e.cMun == valorId.Split('|')[1]);
                            rcid.Commit();

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

            return View(c);
        }

        [HttpGet]
        public ActionResult LoadCidades()
        {

            //Apago os registos das cidades
            using (Repositorio<Cidade> rcid = new Repositorio<Cidade>())
            {
                rcid.Deletar(e => e.uf != string.Empty);
                rcid.Commit();
            }

            //Apago os registos das ufs
            using (Repositorio<UF> ruf = new Repositorio<UF>())
            {
                ruf.Deletar(e => e.uf != string.Empty);
                ruf.Commit();
            }

            dynamic loaded = "";

            try
            {

                int cUF = 0;
                string[] linha;

                string filepath = Server.MapPath("~/App_Data/ScriptsBD/Municipios.csv");

                List<UF> ufs = new List<UF>();
                UF uf = new UF();
                List<Cidade> mun = new List<Cidade>();

                var linhas = System.IO.File.ReadAllLines(filepath);

                bool primeiraLinha = true;
                int contador = 0;

                foreach (var linhaArq in linhas)
                {

                    linha = linhaArq.Split(';');

                    if (primeiraLinha)
                    {
                        cUF = Convert.ToInt32(linha[0]);
                        uf.cuf = cUF;
                        uf.uf = UtilDB.getUF(cUF.ToString(), "s");
                        uf.nome = UtilDB.getUF(cUF.ToString(), "u");
                    }


                    if (linha != null)
                    {
                        if (cUF > 0 && (cUF.ToString() != linha[0]))
                        {

                            using (Repositorio<UF> rup = new Repositorio<UF>())
                            {
                                rup.Adicionar(uf);
                                rup.Commit();
                            }

                            uf = new UF();
                            cUF = Convert.ToInt32(linha[0]);
                            uf.cuf = cUF;
                            uf.uf = UtilDB.getUF(cUF.ToString(), "s");
                            uf.nome = UtilDB.getUF(cUF.ToString(), "u");

                            using (Repositorio<Cidade> rcid = new Repositorio<Cidade>())
                            {
                                rcid.AdicionarRange(mun);
                                rcid.Commit();
                            }

                            mun.Clear();

                        }

                        if (cUF > 0 && cUF.ToString() == linha[0])
                        {
                            mun.Add(new Cidade { uf = uf.uf, cMun = linha[2], xMun = linha[3], cep="" });
                            contador++;

                            //última linha lida
                            if (contador == linhas.Length)
                            {
                                uf = new UF();
                                uf.cuf = cUF;
                                uf.uf = UtilDB.getUF(cUF.ToString(), "s");
                                uf.nome = UtilDB.getUF(cUF.ToString(), "u");

                                using (Repositorio<UF> rup = new Repositorio<UF>())
                                {
                                    rup.Adicionar(uf);
                                    rup.Commit();
                                }

                                using (Repositorio<Cidade> rcid = new Repositorio<Cidade>())
                                {
                                    rcid.AdicionarRange(mun);
                                    rcid.Commit();
                                }
                            }
                        }
                    }
                    primeiraLinha = false;
                }

                loaded = new { Registros = "OK", Qtd = contador };
            }
            catch (Exception e)
            {
                loaded = new { Registros = "Erro ao gravar no BD", Qtd = 0, Motivo = e.InnerException };
            }

            return Json(loaded, JsonRequestBehavior.AllowGet);
        }
    }
}