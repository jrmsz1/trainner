using Avaliacao.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avaliacao.Models
{
    [Table("UFs")]
    public class UF
    {
        [Key]
        [Display(Description ="UF")]
        [RegularExpression(@"[A-Z]{2}", ErrorMessage = "Digite a Sigla da UF")]
        public string uf { get; set; }

        [Display(Description = "Nome da UF")]
        [MaxLength(UtilDB.CamposDescricao20, ErrorMessage = "{0}" + UtilDB.errorCampoDescricao20)]
        public string nome { get; set; }

        [Display(Description = "Código da UF")]
        public int cuf { get; set; }

        public virtual List<Cidade> municipios { get; set; }
    }

}