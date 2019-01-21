using Avaliacao.DAL;
using Avaliacao.Models;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Avaliacao.Controllers.Util
{
    public static class HelperUtil
    {



        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string formId)
        {
            return htmlHelper.BeginForm(null, null, FormMethod.Post, new { id = formId });
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string formId, FormMethod method)
        {
            return htmlHelper.BeginForm(null, null, method, new { id = formId });
        }

        public static MvcHtmlString SpanFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var valueGetter = expression.Compile();
            var value = valueGetter(helper.ViewData.Model);

            var span = new TagBuilder("span");
            span.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (value != null)
            {
                span.SetInnerText(value.ToString());
            }

            return MvcHtmlString.Create(span.ToString());
        }

        /*
             ViewBag.FleetTypeList = new List<SelectListItem>{
            new SelectListItem { Text="17 Truck", Value="1"},
            new SelectListItem { Text="20 Truck", Value="2"},
            new SelectListItem { Text="Something else", Value="0"}
        };
        and in the view

        @Html.DropDownListFor(m => m.FleetType, ViewBag.FleetTypeList as List<SelectListItem>
                    , new { @class = "btn btn-primary btn-lg dropdown-toggle" }) 
         */

        public static MvcHtmlString DropDownListModel<TModel>(this HtmlHelper<IEnumerable<TModel>> helper, SelectList ListaValores = null, object htmlAttributes = null)
        {

            //a ideia aqui é dado um selectList / list montar a lista e retornar; caso contrário, procurar os atributos customizados
            //if (ListaValores != null)
            //{

            //}

            PropertyInfo[] Props = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);


            //Get column headers
            bool isDisplayGridHeader = false;
            bool isDisplayAttributeDefined = false;

            List<string> nomesPropriedades = new List<string>();

            StringBuilder tags = new StringBuilder();

            TagBuilder tagBuilder = new TagBuilder("select");
            tagBuilder.Attributes.Add("name", "pesquisaDropList");
            tagBuilder.Attributes.Add("id", "pesquisaDropListId");

            StringBuilder options = new StringBuilder();
            options.AppendLine("<option value='-1'> Selecione uma opção </option>");


            foreach (PropertyInfo prop in Props)
            {

                nomesPropriedades.Add(prop.Name);
                var displayName = "";

                isDisplayAttributeDefined = Attribute.IsDefined(prop, typeof(DisplayAttribute));

                if (isDisplayAttributeDefined)
                {
                    DisplayAttribute dna = (DisplayAttribute)Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute));
                    if (dna != null)
                        displayName = dna.Description;
                }
                else
                    displayName = prop.Name;

                isDisplayGridHeader = Attribute.IsDefined(prop, typeof(DisplayGridHeader));

                if (isDisplayGridHeader)
                {
                    DisplayGridHeader dgh = (DisplayGridHeader)Attribute.GetCustomAttribute(prop, typeof(DisplayGridHeader));
                    if (dgh != null)
                    {

                        if (!string.IsNullOrEmpty(dgh.Descricao))
                            displayName = dgh.Descricao;

                        if (dgh.FiltroPesquisa)
                        {
                            string dynamicTypeJSfilter = "text";

                            if (prop.PropertyType == typeof(int))
                                dynamicTypeJSfilter = "number";
                            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                                dynamicTypeJSfilter = "date";


                            string singleOption = "<option value = '" + dynamicTypeJSfilter + '|' + prop.Name + "'>" + displayName + "</option>";
                            options.AppendLine(singleOption);
                        }

                    }
                }
            }

            tagBuilder.InnerHtml = options.ToString();
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(htmlAttributes))
            {
                tagBuilder.MergeAttribute(prop.Name.Replace('_', '-'), prop.GetValue(htmlAttributes).ToString(), true);
            }

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        public static MvcHtmlString DropDownListModel<TModel>(this HtmlHelper<IPagedList<TModel>> helper, SelectList ListaValores = null, object htmlAttributes = null)
        {

            //a ideia aqui é dado um selectList / list montar a lista e retornar; caso contrário, procurar os atributos customizados
            //if (ListaValores != null)
            //{

            //}

            PropertyInfo[] Props = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);


            //Get column headers
            bool isDisplayGridHeader = false;
            bool isDisplayAttributeDefined = false;

            List<string> nomesPropriedades = new List<string>();

            StringBuilder tags = new StringBuilder();

            TagBuilder tagBuilder = new TagBuilder("select");
            tagBuilder.Attributes.Add("name", "pesquisaDropList");
            tagBuilder.Attributes.Add("id", "pesquisaDropListId");

            StringBuilder options = new StringBuilder();
            options.AppendLine("<option value='-1'> Selecione uma opção </option>");


            foreach (PropertyInfo prop in Props)
            {

                nomesPropriedades.Add(prop.Name);
                var displayName = "";

                isDisplayAttributeDefined = Attribute.IsDefined(prop, typeof(DisplayAttribute));

                if (isDisplayAttributeDefined)
                {
                    DisplayAttribute dna = (DisplayAttribute)Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute));
                    if (dna != null)
                        displayName = dna.Description;
                }
                else
                    displayName = prop.Name;

                isDisplayGridHeader = Attribute.IsDefined(prop, typeof(DisplayGridHeader));

                if (isDisplayGridHeader)
                {
                    DisplayGridHeader dgh = (DisplayGridHeader)Attribute.GetCustomAttribute(prop, typeof(DisplayGridHeader));
                    if (dgh != null)
                    {

                        if (!string.IsNullOrEmpty(dgh.Descricao))
                            displayName = dgh.Descricao;

                        if (dgh.FiltroPesquisa)
                        {
                            string dynamicTypeJSfilter = "text";

                            if (prop.PropertyType == typeof(int))
                                dynamicTypeJSfilter = "number";
                            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                                dynamicTypeJSfilter = "date";


                            string singleOption = "<option value = '" + dynamicTypeJSfilter + '|' + prop.Name + "'>" + displayName + "</option>";
                            options.AppendLine(singleOption);
                        }

                    }
                }
            }

            tagBuilder.InnerHtml = options.ToString();
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(htmlAttributes))
            {
                tagBuilder.MergeAttribute(prop.Name.Replace('_', '-'), prop.GetValue(htmlAttributes).ToString(), true);
            }

            return MvcHtmlString.Create(tagBuilder.ToString());
        }


        // Método de montar as listagens com o atributo customizado que criei
        // e já preenche a listagem
        //  Apresentação
        public static MvcHtmlString DisplayGridModel<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> helper, TValue valores, string classTable = "", string classTr = "", string classTh = "", string classTd = "")
        {

            var url = new UrlHelper(helper.ViewContext.RequestContext);

            var controller = helper.ViewContext.RouteData.Values["controller"].ToString();


            var listaOrder = (List<string>) helper.ViewBag.Orders;


          


            

            //System.Reflection.MemberInfo info = typeof(TModel);
            //Get properties
            PropertyInfo[] Props = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //Get column headers
            bool isDisplayGridHeader = false;
            bool isDisplayAttributeDefined = false;


            List<string> nomesPropriedades = new List<string>();

            StringBuilder tags = new StringBuilder();

            tags.Append("<table").Append(classTable.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTable) : "").Append(">");
            tags.Append("<tr").Append(classTr.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTr) : "").Append(">");
                       
            foreach (PropertyInfo prop in Props)
            {

                var displayName = "";

                isDisplayAttributeDefined = Attribute.IsDefined(prop, typeof(DisplayAttribute));

                if (isDisplayAttributeDefined)
                {

                    DisplayAttribute dna = (DisplayAttribute)Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute));
                    if (dna != null)
                        displayName = dna.Description;
                }
                else
                    displayName = prop.Name;

                isDisplayGridHeader = Attribute.IsDefined(prop, typeof(DisplayGridHeader));
                
                if (isDisplayGridHeader)
                {
                    DisplayGridHeader dgh = (DisplayGridHeader)Attribute.GetCustomAttribute(prop, typeof(DisplayGridHeader));
                    if (dgh != null)
                    {
                        if (dgh.VisivelGrid)
                        {
                            if (!string.IsNullOrEmpty(dgh.Descricao))
                                displayName = dgh.Descricao;

                            if (!dgh.OrdenaColuna)
                            {
                                tags.Append("<th").Append(classTh.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTh) : "").Append(">").Append(displayName).Append("</th>");
                            }
                            else
                            {
                                var ordemColumn = "";
                                for (int i = 0; i < listaOrder.Count; i++)
                                {
                                    if (listaOrder[i].StartsWith(prop.Name))
                                    {
                                        ordemColumn = listaOrder[i];
                                        break;
                                    }
                                }

                                if (!string.IsNullOrEmpty(ordemColumn)) {
                                    var anchorBuilderEdit = new TagBuilder("a");
                                    //Importante toda lista deve ter um id!!!!
                                    anchorBuilderEdit.MergeAttribute("href", url.Action("Lista", controller, new { sortOrder = ordemColumn }));
                                    anchorBuilderEdit.SetInnerText(displayName);
                                    var linkOrder = anchorBuilderEdit.ToString(TagRenderMode.Normal);

                                    tags.Append("<th").Append(classTh.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTh) : "").Append(">").Append(linkOrder).Append("</th>");
                                }else
                                    tags.Append("<th").Append(classTh.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTh) : "").Append(">").Append(displayName).Append("</th>");
                            }

                            nomesPropriedades.Add(prop.Name);
                        }
                    }
                }
            }

            tags.Append("<th></th>");


            //Montagem da grid
            dynamic lista = valores;




            foreach (var item in lista)
            {
                tags.Append("<tr").Append(classTr.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTr) : "").Append(">");

                foreach (var column in nomesPropriedades)
                {
                    dynamic conteudo = item.GetType().GetProperty(column).GetValue(item, null);
                    if (conteudo != null)
                        tags.Append("<td").Append(classTd.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTd) : "").Append(">").Append(conteudo).Append("</td>");
                    else
                        tags.Append("<td").Append(classTd.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTd) : "").Append(">").Append("</td>");
                }

                var anchorBuilderEdit = new TagBuilder("a");
                //Importante toda lista deve ter um id!!!!
                anchorBuilderEdit.MergeAttribute("href", url.Action("UpInsert", controller, new { modeid = "UPD|" +item.id }));
                anchorBuilderEdit.SetInnerText("Editar");
                var linkEdit = anchorBuilderEdit.ToString(TagRenderMode.Normal);

                tags.Append("<td").Append(classTd.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTd) : "").Append(">").Append(linkEdit).Append("</td>");

                var anchorBuilderDel = new TagBuilder("a");
                //Importante toda lista deve ter um id!!!!
                anchorBuilderDel.MergeAttribute("href", url.Action("UpInsert", controller, new { modeid = "DEL|"+item.id }));
                anchorBuilderDel.SetInnerText("Remover");
                var linkRemover = anchorBuilderDel.ToString(TagRenderMode.Normal);

                tags.Append("<td").Append(classTd.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTd) : "").Append(">").Append(linkRemover).Append("</td>");

                tags.Append("</tr>");
            }

            tags.Append("</tr>");
            tags.Append("</table>");

            return MvcHtmlString.Create(tags.ToString());
        }


        public static MvcHtmlString DisplayGridModel<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> helper, TValue valores, string classTable = "", string classTr = "", string classTh = "", string classTd = "")
        {

            var url = new UrlHelper(helper.ViewContext.RequestContext);

            var controller = helper.ViewContext.RouteData.Values["controller"].ToString();


            var listaOrder = (List<string>)helper.ViewBag.Orders;







            //System.Reflection.MemberInfo info = typeof(TModel);
            //Get properties
            PropertyInfo[] Props = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //Get column headers
            bool isDisplayGridHeader = false;
            bool isDisplayAttributeDefined = false;


            List<string> nomesPropriedades = new List<string>();

            StringBuilder tags = new StringBuilder();

            tags.Append("<table").Append(classTable.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTable) : "").Append(">");
            tags.Append("<tr").Append(classTr.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTr) : "").Append(">");

            foreach (PropertyInfo prop in Props)
            {

                var displayName = "";

                isDisplayAttributeDefined = Attribute.IsDefined(prop, typeof(DisplayAttribute));

                if (isDisplayAttributeDefined)
                {

                    DisplayAttribute dna = (DisplayAttribute)Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute));
                    if (dna != null)
                        displayName = dna.Description;
                }
                else
                    displayName = prop.Name;

                isDisplayGridHeader = Attribute.IsDefined(prop, typeof(DisplayGridHeader));

                if (isDisplayGridHeader)
                {
                    DisplayGridHeader dgh = (DisplayGridHeader)Attribute.GetCustomAttribute(prop, typeof(DisplayGridHeader));
                    if (dgh != null)
                    {
                        if (dgh.VisivelGrid)
                        {
                            if (!string.IsNullOrEmpty(dgh.Descricao))
                                displayName = dgh.Descricao;

                            if (!dgh.OrdenaColuna)
                            {
                                tags.Append("<th").Append(classTh.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTh) : "").Append(">").Append(displayName).Append("</th>");
                            }
                            else
                            {
                                var ordemColumn = "";
                                for (int i = 0; i < listaOrder.Count; i++)
                                {
                                    if (listaOrder[i].StartsWith(prop.Name))
                                    {
                                        ordemColumn = listaOrder[i];
                                        break;
                                    }
                                }

                                if (!string.IsNullOrEmpty(ordemColumn))
                                {
                                    var anchorBuilderEdit = new TagBuilder("a");
                                    //Importante toda lista deve ter um id!!!!
                                    anchorBuilderEdit.MergeAttribute("href", url.Action("Lista", controller, new { sortOrder = ordemColumn }));
                                    anchorBuilderEdit.SetInnerText(displayName);
                                    var linkOrder = anchorBuilderEdit.ToString(TagRenderMode.Normal);

                                    tags.Append("<th").Append(classTh.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTh) : "").Append(">").Append(linkOrder).Append("</th>");
                                }
                                else
                                    tags.Append("<th").Append(classTh.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTh) : "").Append(">").Append(displayName).Append("</th>");
                            }

                            nomesPropriedades.Add(prop.Name);
                        }
                    }
                }
            }

            tags.Append("<th></th>");


            //Montagem da grid
            dynamic lista = valores;




            foreach (var item in lista)
            {
                tags.Append("<tr").Append(classTr.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTr) : "").Append(">");

                foreach (var column in nomesPropriedades)
                {
                    dynamic conteudo = item.GetType().GetProperty(column).GetValue(item, null);
                    if (conteudo != null)
                        tags.Append("<td").Append(classTd.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTd) : "").Append(">").Append(conteudo).Append("</td>");
                    else
                        tags.Append("<td").Append(classTd.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTd) : "").Append(">").Append("</td>");
                }

                var anchorBuilderEdit = new TagBuilder("a");
                //Importante toda lista deve ter um id!!!!
                anchorBuilderEdit.MergeAttribute("href", url.Action("UpInsert", controller, new { modeid = "UPD|" + item.id }));
                anchorBuilderEdit.SetInnerText("Editar");
                var linkEdit = anchorBuilderEdit.ToString(TagRenderMode.Normal);

                tags.Append("<td").Append(classTd.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTd) : "").Append(">").Append(linkEdit).Append("</td>");

                var anchorBuilderDel = new TagBuilder("a");
                //Importante toda lista deve ter um id!!!!
                anchorBuilderDel.MergeAttribute("href", url.Action("UpInsert", controller, new { modeid = "DEL|" + item.id }));
                anchorBuilderDel.SetInnerText("Remover");
                var linkRemover = anchorBuilderDel.ToString(TagRenderMode.Normal);

                tags.Append("<td").Append(classTd.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTd) : "").Append(">").Append(linkRemover).Append("</td>");

                tags.Append("</tr>");
            }

            tags.Append("</tr>");
            tags.Append("</table>");

            return MvcHtmlString.Create(tags.ToString());
        }


        public static MvcHtmlString SpanDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            //var valueGetter = expression.Compile();
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            //var value = valueGetter(helper.ViewData.Model);
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            string value = metadata.Description ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            var span = new TagBuilder("span");
            span.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (value != null)
            {
                span.SetInnerText(value.ToString());
            }

            return MvcHtmlString.Create(span.ToString());
        }

        public static MvcHtmlString SpanDisplay<TModel, TProperty>(this HtmlHelper<TModel> helper, string value = "", object htmlAttributes = null)
        {         

            var span = new TagBuilder("span");
            span.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (value != null)
            {
                span.SetInnerText(value.ToString());
            }

            return MvcHtmlString.Create(span.ToString());
        }



        //public static List<string> resolveListOrder(object TModel)
        //{
        //    //System.Reflection.MemberInfo info = typeof(TModel);
        //    //Get properties
        //    PropertyInfo[] Props = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    //Get column headers
        //    bool isDisplayGridHeader = false;
        //    bool isDisplayAttributeDefined = false;


        //    List<string> nomesPropriedades = new List<string>();

        //    StringBuilder tags = new StringBuilder();

        //    tags.Append("<table").Append(classTable.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTable) : "").Append(">");
        //    tags.Append("<tr").Append(classTr.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTr) : "").Append(">");

        //    Dictionary<string, bool> colunasVisiveis = new Dictionary<string, bool>();

        //    foreach (PropertyInfo prop in Props)
        //    {

        //        var displayName = "";

        //        isDisplayAttributeDefined = Attribute.IsDefined(prop, typeof(DisplayAttribute));

        //        if (isDisplayAttributeDefined)
        //        {

        //            DisplayAttribute dna = (DisplayAttribute)Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute));
        //            if (dna != null)
        //                displayName = dna.Description;
        //        }
        //        else
        //            displayName = prop.Name;

        //        isDisplayGridHeader = Attribute.IsDefined(prop, typeof(DisplayGridHeader));

        //        if (isDisplayGridHeader)
        //        {
        //            DisplayGridHeader dgh = (DisplayGridHeader)Attribute.GetCustomAttribute(prop, typeof(DisplayGridHeader));
        //            if (dgh != null)
        //            {
        //                if (dgh.VisivelGrid)
        //                {
        //                    if (!dgh.OrdenaColuna)
        //                    {
        //                        if (!string.IsNullOrEmpty(dgh.Descricao))
        //                            tags.Append("<th").Append(classTh.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTh) : "").Append(">").Append(dgh.Descricao).Append("</th>");
        //                        else
        //                            tags.Append("<th").Append(classTh.Length > 0 ? string.Format(" {0}\"{1}\"", "class=", classTh) : "").Append(">").Append(displayName).Append("</th>");
        //                    }
        //                    else
        //                    {

        //                        var anchorBuilderEdit = new TagBuilder("a");
        //                        //Importante toda lista deve ter um id!!!!
        //                        anchorBuilderEdit.MergeAttribute("href", url.Action("Lista", controller, new { sortOrder = helper.ViewBag.Orders }));
        //                        anchorBuilderEdit.SetInnerText("Editar");
        //                        var linkEdit = anchorBuilderEdit.ToString(TagRenderMode.Normal);
        //                    }

        //                    nomesPropriedades.Add(prop.Name);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
        /**public static MvcHtmlString LabelFor<TModel, TValue>(
       this HtmlHelper<TModel> html,
       Expression<Func<TModel, TValue>> expression,
       object htmlAttributes
   )
        {
            return LabelHelper(
                html,
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                ExpressionHelper.GetExpressionText(expression),
                htmlAttributes
            );
        }

        private static MvcHtmlString LabelHelper(
            HtmlHelper html,
            ModelMetadata metadata,
            string htmlFieldName,
            object htmlAttributes
        )
        {
            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (string.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.SetInnerText(resolvedLabelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }/
    }
}*/