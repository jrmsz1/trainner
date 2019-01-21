using Avaliacao.DAL;
using System;
using System.ComponentModel.DataAnnotations;

namespace Avaliacao.Models.Aluno
{

    //Daria para utilizar o AutoMapper

    //(código, come, cpf, sexo, telefone,data de cadastro, cidade ).
    public class ListAlunos
    {
        [DisplayGridHeader("Código", FiltroPesquisa = true, OrdenaColuna = true)]
        public int id { get; set; }

        [DisplayGridHeader("",FiltroPesquisa = true, OrdenaColuna = true)]      
        public string Nome { get; set; }

        [DisplayGridHeader("", FiltroPesquisa = true, OrdenaColuna = true)]
        public string CPF { get; set; }

        [DisplayGridHeader(Descricao = "Data de Nascimento", FiltroPesquisa = true, OrdenaColuna = false, VisivelGrid = false)]        
        public DateTime? data_nascimento { get; set; }

        [DisplayGridHeader("Sexo", FiltroPesquisa = true, OrdenaColuna = true)]
        public string sexo{ get; set; }

        [DisplayGridHeader("Telefone", FiltroPesquisa = true, OrdenaColuna = true)]
        public string telefonemov { get; set; }

        [DisplayGridHeader("Data de cadastro", FiltroPesquisa = true, OrdenaColuna = true)]
        public DateTime? data_cadastro { get; set; }

        [DisplayGridHeader("Cidade", FiltroPesquisa = true, OrdenaColuna = true)]
        public string cidade { get; set; }

        

    }
}