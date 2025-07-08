using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Functions
{
    public class FunctionDeleteRequest
    {
        [Display(Name = "Id")]
        public int FunctionId { set; get; }
        [Display(Name = "Name")]
        public string? Name { set; get; }
        [Display(Name = "Description")]
        public string? Description { set; get; }
    }

}
