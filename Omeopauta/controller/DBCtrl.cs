using Omeopauta.context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omeopauta.controls;
using System.Windows;

namespace Omeopauta.controller
{
    class DBCtrl
    {
        public const string FOLDER_IMAGES = "images";

        public static string[] GetTags(string query)
        {            
            if ( String.IsNullOrEmpty(query) )
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
                               where t.Tag.Contains(query)
                               select t.Tag;
                    return tags.ToArray();
                }
            }
        }

        public static string[] GetTags(OmeopautaContext db, DBAppunto appunto)
        {
            return (from DBAppuntiTag app in db.AppuntiTag
                   where app.Appunto.ID == appunto.ID
                   select app.Tag.Tag).ToArray();
        }

        public static DBAppunto[] GetAppunti(string q)
        {
            using (OmeopautaContext db = new OmeopautaContext())
            {
                DBAppunto[] appunti = String.IsNullOrEmpty(q)
                    ? (from DBAppunto a in db.Appunti
                       select a).ToArray()
                    : (from DBAppunto a in db.Appunti
                       where a.SimpleText.Contains(q) ||
                             a.ShortDescription.Contains(q)
                       select a).ToArray();

                //carica la lista dei tag (da usare con db offline)
                foreach (DBAppunto item in appunti)
                    item.ListTags = GetTags(db, item);

                return appunti.ToArray<DBAppunto>();
            }
        }

        internal static IEnumerable<DBAppunto> GetAppuntiByTag(string tag)
        {
            using (OmeopautaContext db = new OmeopautaContext())
            {
                DBTag dbTag = db.Tags.FirstOrDefault<DBTag>(t => t.Tag == tag);
                var apps = from DBAppuntiTag appTag in db.AppuntiTag
                           where appTag.Tag.Tag == tag
                           select appTag;
                var ris = from DBAppuntiTag app in apps
                          select app.Appunto;

                //carica la lista dei tag (da usare con db offline)
                foreach (DBAppunto item in ris)
                    item.ListTags = GetTags(db, item);

                return ris.ToArray<DBAppunto>();
            }
        }

        internal static void InsertOrUpdate(DBAppunto appunto)
        {
            using (OmeopautaContext db = new OmeopautaContext())
            {
                DBAppunto old = db.Appunti.FirstOrDefault<DBAppunto>(a => a.ID == appunto.ID);
                
                if ( old == null )  //aggiungo l'appunto
                {
                    db.Appunti.Add(appunto);
                    UpdateTags(db, appunto, null);
                }
                else                // modifico quello esistente
                {
                    string[] oldTags = null;

                    //stacco l'appunto di test
                    db.Entry(old).State = EntityState.Detached;

                    oldTags = GetTags(db, old);
                    db.Appunti.Attach(appunto);
                    db.Entry(appunto).State = EntityState.Modified;
                    UpdateTags(db, appunto, oldTags);
                }
                
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Aggiunge le immagini di un appunto
        /// </summary>
        internal static void AddImages(List<DBImage> listImage, DBAppunto appunto)
        {
            using (OmeopautaContext db = new OmeopautaContext())
            {
                db.Appunti.Attach(appunto);

                foreach (DBImage item in listImage)
                {
                    item.Appunto = appunto;
                    if (item.ID == 0)               //aggiungo le nuove immagini
                    {
                        moveImage(item, appunto);
                        db.Images.Add(item);
                    }
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Rimuove un appunto e le sue immagini
        /// </summary>
        /// <param name="appunti"></param>
        internal static void DeleteAppunto(List<DBAppunto> appunti)
        {
            using (OmeopautaContext db = new OmeopautaContext())
            {
                foreach (DBAppunto item in appunti)
                {
                    db.Appunti.Attach(item);
                    DeleteDBImages(db, item.Images);
                    RemoveTagsFrom(db, item);
                    db.Appunti.Remove(item);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// appunto deve essere stato precedentemente salvato per avere l'ID corretto
        /// </summary>
        private static void moveImage(DBImage item, DBAppunto appunto)
        {
            if (!Directory.Exists(FOLDER_IMAGES)) Directory.CreateDirectory(FOLDER_IMAGES);
            string newName = appunto.ID + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + item.Name;
            string newPath = FOLDER_IMAGES + "/" + newName;

            File.Copy(item.AbsPath, newPath);

            item.Name = newName;
        }

        private static void UpdateTags(OmeopautaContext db, DBAppunto newAppunto, 
                                       string[] _oldTags)
        {
            List<string> newTags = (newAppunto.ListTags != null && newAppunto.ListTags.Length > 0)
                ? newAppunto.ListTags.ToList<string>() : new List<string>();
            List<string> oldTags = (_oldTags != null && _oldTags.Length > 0)
                ? _oldTags.ToList<string>() : new List<string>();
            List<string> allTags = new List<string>();
            allTags.AddRange(newTags); allTags.AddRange(oldTags);

            foreach (string tag in allTags)
            {
                if (string.IsNullOrWhiteSpace(tag)) continue;
                if (newTags.Contains(tag) && oldTags.Contains(tag)) continue; //non è cambiato

                DBTag dbtag = db.Tags.FirstOrDefault<DBTag>(t => t.Tag == tag);

                if( newTags.Contains(tag))             //aggiunge o incrementa un tag
                {
                    AddTag(db, tag, newAppunto, dbtag);
                }
                else
                {
                    RemoveTag(db, newAppunto, dbtag);
                }
            }
        }

        

        internal static void DeleteImages(ImageGallery[] images)
        {
            using (OmeopautaContext db = new OmeopautaContext())
            {
                foreach (ImageGallery item in images)
                {
                    if (item.DBImg.ID == 0) continue;
                    db.Images.Attach(item.DBImg);
                    db.Images.Remove(item.DBImg);
                    if (item.DBImg.tmpPath == null)
                    {
                        item.DeleteImage();
                    }
                        
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Funzione usata quando si elimina direttamente un appunto
        /// </summary>
        private static void DeleteDBImages(OmeopautaContext db, ICollection<DBImage> images)
        {
            while(images.Count > 0)
            {
                DBImage item = images.First<DBImage>();
                DeletePictureAsync(item.AbsPath);
                db.Images.Remove(item);             //rimuove dal db
            }
        }

        private static void DeletePictureAsync(string path)
        {
            (new System.Threading.Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        GC.Collect();
                        File.Delete(path);
                        Console.WriteLine("DELETE FILE: " + path);
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("EXCEPTION DELETE: " + path);
                    }
                }
            })).Start();
        }

        private static void AddTag(OmeopautaContext db, string strTag, DBAppunto appunto, DBTag dbTag)
        {
            if (dbTag == null)
                dbTag = db.Tags.Add(new DBTag() {
                    ContUsed = 1,
                    Tag = strTag
                });
            else
                dbTag.ContUsed++;

            db.AppuntiTag.Add(new DBAppuntiTag()
            {
                Appunto = appunto,
                Tag = dbTag
            });
        }

        private static void RemoveTag(OmeopautaContext db, DBAppunto appunto, DBTag dbTag)
        {
            //cancello la tupla per il n-n
            //DBAppuntiTag dbApp = db.AppuntiTag.FirstOrDefault<DBAppuntiTag>(
            //    t => t.Appunto.ID == appunto.ID && t.Tag.Tag == dbTag.Tag);
            DBAppuntiTag dbApp = appunto.Tags.FirstOrDefault<DBAppuntiTag>( t =>  t.Tag.Tag == dbTag.Tag);
            db.AppuntiTag.Remove(dbApp);

            if (dbTag != null)
            {
                //decremento
                dbTag.ContUsed--;
                if (dbTag.ContUsed == 0)                //cancello se non ci sono piu tag
                    db.Tags.Remove(dbTag);
            }
        }

        private static void RemoveTagsFrom(OmeopautaContext db, DBAppunto appunto)
        {
            while(appunto.Tags.Count>0)
            {
                DBAppuntiTag item = appunto.Tags.First<DBAppuntiTag>();

                DBTag tag = item.Tag;           //prendo il tag da eliminare
                RemoveTag(db, appunto, tag);    //lo cancello
            }
        }
    }
}
