using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omeopauta.context
{
    internal sealed class Configuration : DbMigrationsConfiguration<OmeopautaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OmeopautaContext context) { }
    }

    public class DbInitializer : CreateDatabaseIfNotExists<OmeopautaContext>
    {

    }

    //public class OmeopautaContext : DbContext

    public class OmeopautaContext : DbContext
    {
        public OmeopautaContext() : base("OmeopautaConnection")
        {

        }

        public OmeopautaContext(string nameOrConnectionString) 
            : base(nameOrConnectionString)
        {

        }

        static OmeopautaContext()
        {
            // Database initialize
            //Database.SetInitializer<OmeopautaContext>(new DbInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OmeopautaContext, Configuration>());

            //var migrator = new DbMigrator(new Configuration());
            //migrator.Update();

            using (OmeopautaContext db = new OmeopautaContext())
                db.Database.Initialize(false);
        }


        public DbSet<DBAppunto> Appunti { get; set; }
        public DbSet<DBAppuntiTag> AppuntiTag { get; set; }
        public DbSet<DBTag> Tags { get; set; }
        public DbSet<DBImage> Images { get; set; }
    }

    public class MigrationsContextFactory : IDbContextFactory<OmeopautaContext>
    {
        //public OmeopautaContext Create()
        //{
        //    return new OmeopautaContext("OmeopautaConnection");
        //}
        public OmeopautaContext Create()
        {
            throw new NotImplementedException();
        }
    }
}
