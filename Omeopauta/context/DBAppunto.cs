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
            this.Descriptions = descriptions;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string ShortDescription{ get; set; }

        public string Descriptions { get; set; }

        public virtual ICollection<DBAppuntiTag> Tags { get; set; }
    }
}
