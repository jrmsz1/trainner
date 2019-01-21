using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Avaliacao.Models
{
    public class UsuarioLoginView
    {

        public string email { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "{0} deve ter no mínimo {2} caracteres", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string senhacripto { get; set; }
    }
}