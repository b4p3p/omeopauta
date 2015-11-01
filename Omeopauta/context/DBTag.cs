using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omeopauta.context
{
    [Table("Tag")]
    public class DBTag
    {

        [Key]
        public string Tag { get; set; }

        public int ContUsed { get; set; }
    }
}
