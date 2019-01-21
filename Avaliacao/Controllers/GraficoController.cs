using Avaliacao.DAL.Repositorio;
using Avaliacao.Models;
using Avaliacao.Models.Aluno;
using Avaliacao.Models.Relatorio;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Avaliacao.Controllers
{
    public class GraficoController : Controller
    {
        // GET: Graficos
        public ActionResult Index()
        {
            if (Session["usuarioId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

       
        //Retorna dados JSON
        public JsonResult AluCidGrafico()
        {
            List<AlunosCidView> listaAlunos = new List<AlunosCidView>();

            using (Repositorio<Cidade> rcid = new Repositorio<Cidade>())
            {
                var todasCidades = rcid.GetTodos();

                foreach (var cidade in todasCidades)
                {
                    using (Repositorio<Aluno> ralu = new Repositorio<Aluno>())
                    {
                        var total = ralu.Contar(e => e.codibgecidade == cidade.cMun);

                        if (total > 0)
                        {
                            listaAlunos.Add(new AlunosCidView { cidade = cidade.cMun + " - " + cidade.xMun.Trim() + " - Registros: " + total, qtoalunos = total });
                        }

                    }
                }
            }
            
            return Json(listaAlunos, JsonRequestBehavior.AllowGet);

        }
    }
}