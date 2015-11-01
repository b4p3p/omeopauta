using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omeopauta.context
{
    [Table("Appunti")]
    public class DBAppunto
    {
        public DBAppunto() { }

        public DBAppunto(string descriptions, string shortDescription)
        {
            this.ShortDescription = shortDescription;
            this.Description = descriptions;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string ShortDescription{ get; set; }

        public string Description { get; set; }

        public virtual ICollection<DBAppuntiTag> Tags { get; set; }

        public virtual ICollection<DBImage> Images { get; set; }

        [NotMapped]
        public string[] ListTags { get; set; }
        
        [NotMapped]
        public string SimpleText { get; set; }

        internal IEnumerable<DBImage> GetImages()
        {
            using (OmeopautaContext db = new OmeopautaContext())
            {
                db.Appunti.Attach(this);
                return this.Images;
            }
        }
    }
}
