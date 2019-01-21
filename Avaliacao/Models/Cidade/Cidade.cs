using Avaliacao.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avaliacao.Models
{
    [Table("Cidades")]
    //Particularmente prefiro município, por causa de emissão de notas fiscais eletrônicas
    public class Cidade
    {
        [Key]
        [Display(Description = "Código IBGE")]
        [RegularExpression(@"\d{7}", ErrorMessage = "{0} este campo deve ter somente 7 dígitos")]
        public string cMun { get; set; }

        [NotMapped]
        //id Virtual para as pesquisas
        [DisplayGridHeader(Descricao = "Código IBGE", FiltroPesquisa = true, OrdenaColuna = true, CampoVirtual = "cMun")]
        public string id { get { return cMun; } }

        [Display(Description = "Nome Cidade")]
        [MaxLength(UtilDB.CamposDescricao60, ErrorMessage = "{0}" + UtilDB.errorCampoDescricao60)]
        [DisplayGridHeader(OrdenaColuna =true, FiltroPesquisa =true)]
        public string xMun { get; set; }

        [Display(Description = "Estado")]
        [DisplayGridHeader(OrdenaColuna = true, FiltroPesquisa = true)]
        public string uf { get; set; }
                
        [Display(Description = "CEP")]
        [DisplayGridHeader(OrdenaColuna = true, FiltroPesquisa = true)]
        public string cep { get; set; }

    }
}