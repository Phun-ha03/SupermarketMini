namespace CMS.BaseModels.Common
{

    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}
