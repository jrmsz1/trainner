namespace Avaliacao.DAL
{

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DisplayGridHeader : System.Attribute
    {
        

        private string descricao;
        public string Descricao
        {
            get
            {
                if (!string.IsNullOrEmpty(descricao))
                    return descricao;

                return string.Empty;

            }
            set
            {

                if (!string.IsNullOrEmpty(value))
                {
                    descricao = value;
                }
            }
        }

        private string campoVirtual;
        public string CampoVirtual
        {
            get
            {
                if (!string.IsNullOrEmpty(campoVirtual))
                    return campoVirtual;

                return string.Empty;

            }
            set
            {

                if (!string.IsNullOrEmpty(value))
                {
                    campoVirtual = value;
                }
            }
        }


        private bool filtroPesquisa = false;

        public bool FiltroPesquisa
        {
            get
            {
                return filtroPesquisa;
            }
            set { filtroPesquisa = value; }
        }

        private bool visivelGrid = true;

        public bool VisivelGrid
        {
            get
            {
                return visivelGrid;
            }
            set { visivelGrid = value; }
        }


        private bool ordenaColuna = false;

        public bool OrdenaColuna
        {
            get
            {
                return ordenaColuna;
            }

            set { ordenaColuna = value; }
        }

        public DisplayGridHeader()
        {

        }

        //Daria para fazer também o InputMask

        public DisplayGridHeader(string DescricaoColunaGrid = "", bool OrdenaColunaGrid = false, bool FiltroPesquisa = false, bool VisivelGrid = true, string CampoVirtual = "")
        {
            this.descricao = DescricaoColunaGrid;
            this.ordenaColuna = OrdenaColuna;
            this.filtroPesquisa = FiltroPesquisa;
            this.visivelGrid = VisivelGrid;
            this.campoVirtual = CampoVirtual;
        }

        public DisplayGridHeader(bool OrdenaColunaGrid = false, bool FiltroPesquisa = false, bool VisivelGrid = true)
        {
            this.ordenaColuna = OrdenaColuna;
            this.filtroPesquisa = FiltroPesquisa;
            this.visivelGrid = VisivelGrid;
        }
       
    }
}