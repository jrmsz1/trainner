using Avaliacao.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Avaliacao.Models.Aluno
{
    [Table("Alunos")]
    public class Aluno
    {

        [Key]
        [Column("alunoId")]
        public int id { get; set; }

        [Display(Description = "Nome")]        
        [MaxLength(UtilDB.CamposDescricao60, ErrorMessage = "Nome " + UtilDB.errorCampoDescricao60)]
        [Required]
        public string nome { get; set; }


        [Display(Description = "RG")]                
        public string rg { get; set; }


        [Display(Description = "CPF")]   
        [Required]
        [RegularExpression(@"\d{11}", ErrorMessage = "Digite o CPF do usuário.")]
        public string cpf { get; set; }


        [Display(Description ="Data de nascimento")]
        [Required]
        public DateTime? data_nascimento { get; set; }

        [Display(Description ="Logradouro")]
        [Required]
        public string logradouro { get; set; }

        [Display(Description = "Número")]
        public string nro { get; set; }

        [Display(Description = "Complemento")]
        public string complemento { get; set; }

        [Display(Description = "Bairro")]
        public string bairro { get; set; }

        [Display(Description = "Código IBGE")]
        public string codibgecidade { get; set; }

        [Display(Description = "Cidade")]

        public string cidade { get; set; }

        [Display(Description = "UF")]
        public string uf { get; set; }

        [Display(Description = "CEP")]
        public string cep { get; set; }

        [Display(Description = "Cod Pais")]
        public string codpais { get; set; }

        [Display(Description = "Matricula")]
        [Required]
        public string matricula { get; set; }           

        [Display(Description = "Sexo")]
        [RegularExpression(@"[M|F]", ErrorMessage = "Selecione o sexo do usuário")]
        public string sexo { get; set; }

        [Display(Description = "E-mail")]
        [EmailAddress]
        public string email { get; set; }

        [Display(Description = "Telefone Movel")]
        [Required]
        public string telefonemov { get; set; }

        [Display(Description = "Telefone Comercial")]     
        public string telefonecom { get; set; }
        
        [Display(Description = "Telefone Residencial")]
        public string telefoneres { get; set; }
        
        public DateTime data_cadastro { get; set; }

        //Gostaria de testar o tipo JSON no postgres
        //porém poderia aumentar mais o prazo da entrega do projeto;
        public string pessoascontatojson { get; set; }
                
        [NotMapped]
        public List<PessoaContato> PessoasContato
        {
            get
            {
                if (!string.IsNullOrEmpty(pessoascontatojson))
                {
                    return JsonConvert.DeserializeObject<List<PessoaContato>>(pessoascontatojson);
                }
                return new List<PessoaContato>();
            }
            set
            {
                if (value != null)
                    pessoascontatojson = JsonConvert.SerializeObject(value);

            }
        }

        public DateTime? ultmodificacao { get; set; }

        public int usuarioIdmod { get; set; }

    }
}