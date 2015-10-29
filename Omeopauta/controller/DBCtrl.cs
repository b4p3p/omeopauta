using Omeopauta.controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omeopauta.controller
{
    class DBCtrl
    {
        private static Tag[] lstTag = 
        {
           new Tag("Ciccio"), new Tag("Pippo"), new Tag("Paperino")
        };

        public static Tag[] GetTags(string q)
        {            
            if ( String.IsNullOrEmpty(q) )
            {
                return lstTag;
            }
            else
            {
                return (from Tag t in lstTag
                        where t.Text.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0
                        select t).ToArray<Tag>();
            }
        }
    }
}
