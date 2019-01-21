using Avaliacao.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avaliacao.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("usuarioId")]
        public int id { get; set; }

        [Display(Description = "Nome")]
        [MaxLength(UtilDB.CamposDescricao60, ErrorMessage = "Nome " + UtilDB.errorCampoDescricao60)]
        public string nome { get; set; }

        [Display(Description = "CPF")]
        [Required]
        [RegularExpression(@"\d{11}", ErrorMessage = "Digite o CPF do usuário.")]
        public string cpf { get; set; }

        //Poderia ter um status

        [Display(Description = "Sexo")]
        [RegularExpression(@"[M|F]", ErrorMessage = "Selecione o sexo do usuário")]
        public string sexo { get; set; }

        
        [Display(Description ="Telefone")]
        [Required]
        public string telefone { get; set; }

        public DateTime? data_cadastro { get; set; }

        [Display(Description = "UF")]
        [Required]
        public string ufusuario { get; set; }

        [Display(Description = "Cidade")]
        [Required]
        [MaxLength(UtilDB.CamposDescricao60, ErrorMessage = "{0}" + UtilDB.errorCampoDescricao60)]
        public string cidade { get; set; }              
        
        public string codibgecidade { get; set; }

        [Display(Description ="E-mail")]
        public string email { get; set; }


        [Required]
        [StringLength(128, ErrorMessage = "{0} deve ter no mínimo {2} caracteres", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string senhacripto { get; set; }

        /*[DataType(DataType.Password)]
        [Display(Name = "Confirmação da senha")]
        [Compare("senhacripto", ErrorMessage = "A senha e a confirmação da senha não são iguais!")]
        public string ConfirmPassword { get; set; }*/                

    }
}