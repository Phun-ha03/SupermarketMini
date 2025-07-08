using CMS.BaseModels.Common;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CMS.WebApp.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public UploadController(IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService) : base(functionService, httpContextAccessor, userService)
        {
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }


        //[Authorize(Roles = "Upload-UploadFiles")]
        [HttpPost]
        public ActionResult UploadFiles(IFormFile file)
        {
            ApiResult<string> retval = new ApiResult<string>();
            var fildir = "";
            //foreach (IFormFile file in data)
            {
                //Checking file is available to save.  
                if (file != null)
                {
                    var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}-{DateTime.Now.ToString("HHmmss")}{Path.GetExtension(originalFileName)}";
                    string subPath = $"{DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "\\")}";

                    var filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\" + ConstantHelper.AttachmentPath + subPath;

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    fildir = Path.Combine(subPath, fileName);

                    fileName = Path.Combine(filePath, fileName);
                    using var output = new FileStream(fileName, FileMode.Create);
                    file.OpenReadStream().CopyTo(output);
                    output.Close();
                }
            }
            return Content(fildir);
        }

        //[Authorize(Roles = "Upload-UploadFiles")]
        [HttpPost]
        public ActionResult DeleteFiles(string file_name)
        {
            ApiResult<string> retval = new ApiResult<string>();
            var fildir = "";
            //foreach (IFormFile file in data)
            {
                //Checking file is available to save.  
                file_name = Directory.GetCurrentDirectory() + "\\wwwroot\\" + ConstantHelper.AttachmentPath + file_name;
                //fileName = @"D:\DOTNET2022\ICJobMan\ICSoft.Jobman.WebApp\Uploads\Attachments\2022\07\27\10g-142034.jpg";
                if (System.IO.File.Exists(file_name))
                {
                    try
                    {
                        System.IO.File.Delete(file_name);
                        return Content("ok");
                    }
                    catch (Exception ex)
                    {
                        // Debug.WriteLine("Deletion of file failed: " + ex.Message);
                    }
                }


            }
            return Content(file_name);
        }

    }
}
