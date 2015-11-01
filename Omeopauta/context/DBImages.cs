using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Omeopauta.context
{
    [Table("Images")]
    public class DBImage
    {
        public string tmpPath = null;

        public DBImage() { }

        public DBImage(DBAppunto appunto, string name, string tmpPath)
        {
            Appunto = appunto;
            Name = name;
            this.tmpPath = tmpPath;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public virtual DBAppunto Appunto { get; set; }

        public string Name { get; set; }

        public string Descrizione { get; set; }

        [NotMapped]
        public string AbsPath {
            get
            {
                if (tmpPath != null) return tmpPath; //path temporaneo prima del salvataggio

                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(folder, "images", Name);
            }
        }
    }
}
