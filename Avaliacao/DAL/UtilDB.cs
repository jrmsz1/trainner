using System;
using System.Linq;
using System.Reflection;

namespace Avaliacao.DAL
{
    public static class UtilDB
    {
        public const int CamposDescricao60 = 60;

        public const int CamposDescricao20 = 20;

        public const int CPF = 11;

        public const int CODIBGE = 7;

        public const string errorCampoDescricao60 = " este campo não pode ser superior a 60 caracteres";

        public const string errorCampoDescricao20 = " este campo não pode ser superior a 20 caracteres";



        public static class TipoContato
        {
            public const string Pai = "Pai";
            public const string Mae = "Mãe";
            public const string Irmao = "Irmã(o)";
            public const string Amigo = "Amigo(a)";
        }

        /// <summary>
        /// Formato saída: modoSaida: s - sigla, c - codUF, u - Unidade Federativa
        /// </summary>
        /// <param name="chave"></param>
        /// <param name="modoSaida"></param>
        /// <returns></returns>
        public static string getUF(string chave, string modoSaida)
        {

            if (chave.ToUpper() == "AC" || chave.Equals("12"))
                return (modoSaida == "c" ? "12" : (modoSaida == "s" ? "AC" : "Acre"));
            else if (chave.ToUpper() == "AL" || chave.Equals("27"))
                return (modoSaida == "c" ? "27" : (modoSaida == "s" ? "AL" : "Alagoas"));
            else if (chave.ToUpper() == "AM" || chave.Equals("13"))
                return (modoSaida == "c" ? "13" : (modoSaida == "s" ? "AM" : "Amazonas"));
            else if (chave.ToUpper() == "AP" || chave.Equals("16"))
                return (modoSaida == "c" ? "16" : (modoSaida == "s" ? "AP" : "Amapá"));
            else if (chave.ToUpper() == "BA" || chave.Equals("29"))
                return (modoSaida == "c" ? "29" : (modoSaida == "s" ? "BA" : "Bahia"));
            else if (chave.ToUpper() == "CE" || chave.Equals("23"))
                return (modoSaida == "c" ? "23" : (modoSaida == "s" ? "CE" : "Ceará"));
            else if (chave.ToUpper() == "DF" || chave.Equals("53"))
                return (modoSaida == "c" ? "53" : (modoSaida == "s" ? "DF" : "Distrito Federal"));
            else if (chave.ToUpper() == "ES" || chave.Equals("32"))
                return (modoSaida == "c" ? "32" : (modoSaida == "s" ? "ES" : "Espírito Santo"));
            else if (chave.ToUpper() == "GO" || chave.Equals("52"))
                return (modoSaida == "c" ? "52" : (modoSaida == "s" ? "GO" : "Goiás"));
            else if (chave.ToUpper() == "MA" || chave.Equals("21"))
                return (modoSaida == "c" ? "21" : (modoSaida == "s" ? "MA" : "Maranhão"));
            else if (chave.ToUpper() == "MG" || chave.Equals("31"))
                return (modoSaida == "c" ? "31" : (modoSaida == "s" ? "MG" : "Minas Gerais"));
            else if (chave.ToUpper() == "MS" || chave.Equals("50"))
                return (modoSaida == "c" ? "50" : (modoSaida == "s" ? "MS" : "Mato Grosso do Sul"));
            else if (chave.ToUpper() == "MT" || chave.Equals("51"))
                return (modoSaida == "c" ? "51" : (modoSaida == "s" ? "MT" : "Mato Grosso"));
            else if (chave.ToUpper() == "PA" || chave.Equals("15"))
                return (modoSaida == "c" ? "15" : (modoSaida == "s" ? "PA" : "Pará"));
            else if (chave.ToUpper() == "PB" || chave.Equals("25"))
                return (modoSaida == "c" ? "25" : (modoSaida == "s" ? "PB" : "Paraíba"));
            else if (chave.ToUpper() == "PE" || chave.Equals("26"))
                return (modoSaida == "c" ? "26" : (modoSaida == "s" ? "PE" : "Pernambuco"));
            else if (chave.ToUpper() == "PI" || chave.Equals("22"))
                return (modoSaida == "c" ? "22" : (modoSaida == "s" ? "PI" : "Piauí"));
            else if (chave.ToUpper() == "PR" || chave.Equals("41"))
                return (modoSaida == "c" ? "41" : (modoSaida == "s" ? "PR" : "Paraná"));
            else if (chave.ToUpper() == "RJ" || chave.Equals("33"))
                return (modoSaida == "c" ? "33" : (modoSaida == "s" ? "RJ" : "Rio de Janeiro"));
            else if (chave.ToUpper() == "RN" || chave.Equals("24"))
                return (modoSaida == "c" ? "24" : (modoSaida == "s" ? "RN" : "Rio Grande do Norte"));
            else if (chave.ToUpper() == "RO" || chave.Equals("11"))
                return (modoSaida == "c" ? "11" : (modoSaida == "s" ? "RO" : "Rondônia"));
            else if (chave.ToUpper() == "RR" || chave.Equals("14"))
                return (modoSaida == "c" ? "14" : (modoSaida == "s" ? "RR" : "Roraima"));
            else if (chave.ToUpper() == "RS" || chave.Equals("43"))
                return (modoSaida == "c" ? "43" : (modoSaida == "s" ? "RS" : "Rio Grande do Sul"));
            else if (chave.ToUpper() == "SC" || chave.Equals("42"))
                return (modoSaida == "c" ? "42" : (modoSaida == "s" ? "SC" : "Santa Catarina"));
            else if (chave.ToUpper() == "SE" || chave.Equals("28"))
                return (modoSaida == "c" ? "28" : (modoSaida == "s" ? "SE" : "Sergipe"));
            else if (chave.ToUpper() == "SP" || chave.Equals("35"))
                return (modoSaida == "c" ? "35" : (modoSaida == "s" ? "SP" : "São Paulo"));
            else if (chave.ToUpper() == "TO" || chave.Equals("17"))
                return (modoSaida == "c" ? "17" : (modoSaida == "s" ? "TO" : "Tocantins"));
            else if (chave.ToUpper() == "EX" || chave.Equals("99"))
                return (modoSaida == "c" ? "99" : (modoSaida == "s" ? "EX" : "Exterior"));
            else if (chave.ToUpper() == "SVC" || chave.Equals("91"))
                return (modoSaida == "c" ? "91" : (modoSaida == "s" ? "SV" : "Servidor Virtual de Contingência"));
            else
                return string.Empty;

            /*
            string input = "RO11Rondônia,";
            string pat = @"(?<linha>(?<sigla>\w{2})(?<coduf>\d{2})(?<uf>\w+)\,)";

            if (chave.Length> 2)
            {
                

                // Instantiate the regular expression object.
                
                Regex expression = new Regex(pat, RegexOptions.IgnoreCase);

                // ... See if we matched.
                Match match = expression.Match(input);
                if (match.Success)
                {
                    // ... Get group by name.
                    string result = match.Groups["linha"].Value;
                    Console.WriteLine("Middle: {0}", result);
                }
                // Done.

            }
            else { }
         */

        }

        public static string GetVersionAssembly(string assemblyName)
        {
            var version = "";
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().Select(x => x.FullName).ToList();
            foreach (var asm in assemblies)
            {
                var fragments = asm.Split(new char[] { ',', '{', '}' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                if (string.Compare(fragments[0], assemblyName, true) == 0)
                {
                    var subfragments = fragments[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    version = subfragments[1];
                    break;
                }
            }
            return version;
        }
    }
}