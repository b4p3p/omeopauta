using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omeopauta.context
{
    class OmeopautaContext : DbContext
    {
        internal OmeopautaContext() : base("OmeopautaConnection")
        {

        }

        static OmeopautaContext()
        {
            // Database initialize
            Database.SetInitializer<OmeopautaContext>(new DbInitializer());

            using (OmeopautaContext db = new OmeopautaContext())
                db.Database.Initialize(false);
        }

        class DbInitializer : CreateDatabaseIfNotExists<OmeopautaContext>
        {

        }

        public DbSet<DBAppunto> Appunti { get; set; }
        public DbSet<DBAppuntiTag> AppuntiTag { get; set; }
        public DbSet<DBTag> Tags { get; set; }
    }
}
