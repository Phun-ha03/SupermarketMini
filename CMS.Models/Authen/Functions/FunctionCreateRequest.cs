using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Functions
{
    public class FunctionCreateRequest
    {

        [Display(Name = "Name")]
        public string? Name { set; get; }
        [Display(Name = "Description")]
        public string? Description { set; get; }
        [Display(Name = "Controler")]
        public string? Controler { set; get; }
        [Display(Name = "Action")]
        public string? Action { set; get; }
        [Display(Name = "Icon")]
        public string? Icon { set; get; }
        [Display(Name = "Order")]
        public short SortOrder { set; get; }
        [Display(Name = "Show")]
        public byte IsShow { set; get; }
        [Display(Name = "Parent function")]
        public int ParentFunctionId { set; get; }
        [Display(Name = "Level")]
        public byte LevelId { set; get; }
        [Display(Name = "Status")]
        public byte StatusId { set; get; }

        public bool IsShowBoolean { set; get; }
    }
}
