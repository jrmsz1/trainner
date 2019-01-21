using Avaliacao.DAL;
using Avaliacao.DAL.Repositorio;
using Avaliacao.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Avaliacao.Controllers
{
    //http://www.dotnetawesome.com/2016/07/advance-master-details-entry-form-in-mvc.html
    //https://www.codeproject.com/Articles/531916/Articles/531916/Master-Details-using-ASP-NET-MVC#_articleTop
    public class HomeController : Controller
    {
                 

        public ActionResult Login()
        {            
            return View();
        }

        public ActionResult Index()
        {
            
            if (Session["usuarioId"] != null)
            {
                ViewBag.EntityFramework = UtilDB.GetVersionAssembly("EntityFramework");
                ViewBag.Postgres = UtilDB.GetVersionAssembly("Npgsql");
                ViewBag.MVC = typeof(Controller).Assembly.GetName().Version;// = UtilDB.GetVersionAssembly("System.Web.Mvc");              

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

            

        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Inscrever()
        {
            Usuario novo = new Usuario();

            novo.ufusuario = "MS";

            var cities = new System.Collections.Generic.List<CidadeViewDropList>();

            using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
            {
                var lista = cid.Get(e => e.uf == novo.ufusuario).GetEnumerator();

                while (lista.MoveNext())
                    cities.Add(new CidadeViewDropList { cMun = lista.Current.cMun, xMun = lista.Current.xMun });
            }

            novo.codibgecidade = "";

            ViewBag.ListaCidade = cities;

            ViewData.Model = novo;

            return View();
        }

       
                        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inscrever(Usuario u)
        {
            var erro = false;

            if (ModelState.IsValid)
            {
                if (!u.cpf.ValidaCPF())
                {
                    ModelState.AddModelError("CustomError", "O valor informado no campo CPF é inválido.");
                    erro = true;
                }
            }
            else
                erro = true;

            if (!erro)
            {
                try
                {
                    Usuario v = null;
                    
                    using (Repositorio<Usuario> User = new Repositorio<Usuario>())
                    {                      

                        //Faz o Login automático no sistema
                         v = User.Primeiro(e => e.cpf == u.cpf || e.email == u.email);

                        if (v == null)
                        {
                            //Adiciona o registro
                            u.senhacripto = Validacao.SenhaCripto(u.senhacripto);
                            u.data_cadastro = DateTime.Now;
                            User.Adicionar(u);
                            User.Commit();

                            v = User.Primeiro(e => e.cpf == u.cpf);

                            Session["usuarioId"] = v.id.ToString();
                            Session["nomeUser"] = v.nome;

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if(v.cpf == u.cpf)
                                ModelState.AddModelError("CustomError", string.Format("Usuário com o CPF {0} já cadastrado no sistema.", v.cpf ));
                            else if (v.email == u.email)
                                ModelState.AddModelError("CustomError", string.Format("Usuário com o EMAIL {0} já cadastrado no sistema.", v.email));

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
                var lista = cid.Get(e => e.uf == u.ufusuario).GetEnumerator();

                while (lista.MoveNext())
                    cities.Add(new CidadeViewDropList { cMun = lista.Current.cMun, xMun = lista.Current.xMun });
            }

            ViewBag.ListaCidade = cities;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsuarioLoginView u)
        {
            // esta action trata o post (login)
            if (ModelState.IsValid) //verifica se é válido
            {
                if (u.email == "admin@admin" && u.senhacripto == "admin")
                {
                    Session["usuarioId"] = "0"; //0 = ADMIN
                    Session["nomeUser"] = "Admin";

                    return RedirectToAction("Index");
                }
                else
                {                

                    using (Repositorio<Usuario> dc = new Repositorio<Usuario>())
                    {
                        var senhaCripto = Validacao.SenhaCripto(u.senhacripto);                      

                        var v = dc.Primeiro(e => e.email.TrimEnd() == u.email && senhaCripto == e.senhacripto);

                        if (v != null)
                        {
                            //Gravar COOKIE 
                            // OU SESSION
                            //cada um tem as suas vantagens e desvantages

                            //Adicionei no web config 20 min
                            Session["usuarioId"] = v.id.ToString();
                            Session["nomeUser"] = v.nome;

                            //https://www.codeproject.com/articles/244904/%2fArticles%2f244904%2fCookies-in-ASP-NET
                            /*Response.Cookies["LoginCookie"]["userId"] = v.id.ToString();
                            Response.Cookies["LoginCookie"]["nomeUser"] = v.nome;
                            Response.Cookies["LoginCookie"].Expires = DateTime.Now.AddDays(1);*/


                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            ViewBag.Message = "Usuario ou senha inválidos!";
            return View(u);
        }




        //GET: /Home/FillCity
        public async Task<ActionResult> FillCity(string estado)
        {
            var cities = new System.Collections.Generic.List<CidadeViewDropList>();

            if (!string.IsNullOrEmpty(estado))
            {

                await Task.Run(() =>
                {
                    using (Repositorio<Cidade> cid = new Repositorio<Cidade>())
                    {
                        var lista = cid.Get(e => e.uf == estado).GetEnumerator();

                        while (lista.MoveNext())
                            cities.Add(new CidadeViewDropList { cMun = lista.Current.cMun, xMun = lista.Current.xMun });
                    }
                });
            }

            //Municipios
            /*var cities = new System.Collections.Generic.List<Municipio>()
            {                
                    new Municipio (   (estado == "MS" ? "233" : "444"), (estado == "MS" ? "Campo Grande": "Maceio")),
                    new Municipio("233", "OutraCidade")
            };*/
            
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
       

        public ActionResult Contato()
        {
            ViewBag.Message = "Oziel Vilalba Junior";

            return View();
        }
    }
}