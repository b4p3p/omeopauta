using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Omeopauta.context
{
    [Table("AppuntiTags")]
    public class DBAppuntiTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public virtual DBAppunto Appunto { get; set; }

        public virtual DBTag Tag { get; set; }
    }
}
