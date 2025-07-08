using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public class Icon
    {
        public int IconId { get; set; }
        public string IconCode { get; set; }
        public byte IconTypeId { get; set; }
        public string IconTypeCode { get; set; }
        public byte StatusId { get; set; }
    }
}

