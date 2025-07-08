using CMS.Data.Entities.Authen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Functions
{
    public class FunctionViewModel
    {

        [Display(Name = "Id")]
        public int FunctionId { set; get; }
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
        [Display(Name = "Created time")]
        public DateTime CrDateTime { set; get; }
        [Display(Name = "Status")]
        public byte StatusId { set; get; }


        [Display(Name = "Chọn")]
        public bool Selected { set; get; }

        public FunctionViewModel() { }
        public FunctionViewModel(Function function)
        {
            FunctionId = function.FunctionId;
            Name = function.Name;
            Description = function.Description;
            Controler = function.Controler;
            Action = function.Action;
            Icon = function.Icon;
            SortOrder = function.SortOrder;
            IsShow = function.IsShow;
            ParentFunctionId = function.ParentFunctionId;
            LevelId = function.LevelId;
            CrDateTime = function.CrDateTime;
            StatusId = function.StatusId;
        }
    }
}
