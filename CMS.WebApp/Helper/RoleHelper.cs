using CMS.Models.Authen.Functions;

public class RoleHelper
{
    public static bool HasPermission(string actionName, string controlerName, object? functions)
    {
        var retVal = false;

        if (functions == null)
        {
            return retVal;
        }

        controlerName = controlerName.ToLower();
        actionName = actionName.ToLower();

        var funtionObjs = functions as List<FunctionViewModel>;
        foreach(var function in funtionObjs)
        {
            if(function.Controler.ToLower().Equals(controlerName)
                && function.Action.ToLower().Equals(actionName))
            {
                retVal = true;
                break;
            }
        }

        return retVal;
    }
}
