using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BaseModels.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { set; get; }
        public string Message { set; get; }
        public int Extension { set; get; }
        public T ResultObj { set; get; }
        public bool IsSuccess { get; set; }
    }
}

