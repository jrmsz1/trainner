using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Avaliacao.DAL
{
    public static class Validacao
    {

        public static String SenhaCripto(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }


        /// <summary>
        /// Formatar uma string CNPJ
        /// </summary>
        /// <param name="CNPJ">string CNPJ sem formatacao</param>
        /// <returns>string CNPJ formatada</returns>
        /// <example>Recebe '99999999999999' Devolve '99.999.999/9999-99'</example>

        public static string FormatCNPJ(this string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>

        public static string FormatCPF(this string CPF)
        {
            return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }
        /// <summary>
        /// Retira a Formatacao de uma string CNPJ/CPF
        /// </summary>
        /// <param name="Codigo">string Codigo Formatada</param>
        /// <returns>string sem formatacao</returns>
        /// <example>Recebe '99.999.999/9999-99' Devolve '99999999999999'</example>

        public static string RemoveMask(this string Codigo)
        {
            return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }        

        /// <summary>
        /// Valida o CPF informado, removendo a máscara automaticamente
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns>Se é válido ou não o CPF informado</returns>
        public static bool ValidaCPF(this string cpf)
        {


            //Remove a máscara
            cpf = cpf.RemoveMask();

            //Expressão regular
            Match match = Regex.Match(cpf, @"^\d{11}$", RegexOptions.IgnoreCase);

            // Here we check the Match instance.
            if (match.Success)
            {


                var digitoDigitado = cpf[9].ToString() + cpf[10].ToString();
                int soma1 = 0, soma2 = 0;
                int vlr = 11;

                for (int i = 0; i < 9; i++)
                {
                    soma1 += int.Parse(cpf[i].ToString()) * (vlr - 1);
                    soma2 += int.Parse(cpf[i].ToString()) * vlr;
                    vlr--;
                }
                soma1 = (((soma1 * 10) % 11) == 10 ? 0 : ((soma1 * 10) % 11));
                soma2 = (((soma2 + (2 * soma1)) * 10) % 11);

                var digitoGerado = (soma1 * 10) + soma2;
                if (digitoGerado != Convert.ToInt32(digitoDigitado))
                    return false;

                return true;
            }
            return false;
        }

    }
}