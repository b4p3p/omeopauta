using Omeopauta.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omeopauta.model;

namespace Omeopauta.controller
{
    class DBCtrl
    {
        public static string[] GetTags(string q)
        {            
            if ( String.IsNullOrEmpty(q) )
            {
                using (OmeopautaContext db = new OmeopautaContext())
                {
                    var tags = from DBTag t in db.Tags
                               select t.Tag;
                    return tags.ToArray();
                }
            }
            else
            {
                using (OmeopautaContext db = new OmeopautaContext())
                {
                    var tags = from DBTag t in db.Tags
                               select t.Tag;
                    return tags.ToArray();
                }
            }
        }

        internal static void Save(AppuntoModel appunto)
        {
            using (OmeopautaContext db = new OmeopautaContext())
            {
                db.Appunti.Add(new DBAppunto(appunto.FlowDescrizione, appunto.ShortDescrizione));
                db.SaveChanges();
            }
        }

        internal static DBAppunto[] GetAppunti(string q)
        {
            if (String.IsNullOrEmpty(q))
            {
                using (OmeopautaContext db = new OmeopautaContext())
                {
                    var appunti = from DBAppunto a in db.Appunti select a;
                    return appunti.ToArray<DBAppunto>();
                }
            }
            else
            {
                using (OmeopautaContext db = new OmeopautaContext())
                {
                    var appunti = from DBAppunto a in db.Appunti select a;
                    return appunti.ToArray<DBAppunto>();
                }
            }
        }
    }
}
