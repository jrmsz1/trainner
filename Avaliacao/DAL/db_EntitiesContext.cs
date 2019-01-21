using Avaliacao.Models;
using Avaliacao.Models.Aluno;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Avaliacao.DAL
{
    public partial class db_EntitiesContext : DbContext
    {
        public db_EntitiesContext() : base(nameOrConnectionString: "AppDatabaseConnectionString")
        {
            //Não utilizo o CODE FIRST
            //Banco foi criado na mão
        }

        

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Cidade> Cidades { get; set; }

        public DbSet<UF> UFs { get; set; }

        public DbSet<Aluno> Alunos { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("public");
        }

    }
}