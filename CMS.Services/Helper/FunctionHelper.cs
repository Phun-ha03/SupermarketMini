using CMS.Models.Authen.Functions;
using CMS.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Helper
{
    public class FunctionHelper
    {
        public static List<FunctionViewModel> BuildHỉerachy(List<FunctionViewModel> functions)
        {
            List<FunctionViewModel> trees = new List<FunctionViewModel>();
            try
            {
                if (functions == null || functions.Count <= 0) return trees;

                var min_level = functions.Min(m => m.LevelId);

                var stack = functions.Where(m => m.LevelId == min_level).OrderByDescending(m => m.SortOrder).ToList();

                while (stack.Count > 0)
                {
                    var function = stack[stack.Count - 1];
                    trees.Add(function);
                    stack.RemoveAt(stack.Count - 1);

                    stack.AddRange(functions.Where(m => m.ParentFunctionId == function.FunctionId).OrderByDescending(m => m.SortOrder).ToList());
                }

                var leading = ":--";

                foreach (var function in trees)
                {
                    var i = 1;
                    while (i < function.LevelId)
                    {
                        function.Name = leading + function.Name;
                        function.Description = leading + function.Description;
                        i++;
                    }
                }

                return trees;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public static List<FunctionViewModel> BuildHỉerachy(List<FunctionViewModel> functions, int parentFunctionId)
        {
            List<FunctionViewModel> trees = new List<FunctionViewModel>();
            try
            {
                if (functions == null || functions.Count <= 0) return trees;

                var min_level = functions.Where(m => parentFunctionId <= 0 || m.ParentFunctionId == parentFunctionId)
                    .Min(m => m.LevelId);

                var stack = functions.Where(m => parentFunctionId <= 0 && m.LevelId == min_level
                                            || parentFunctionId > 0 && m.LevelId == min_level && m.ParentFunctionId == parentFunctionId)
                    .OrderByDescending(m => m.SortOrder).ToList();

                while (stack.Count > 0)
                {
                    var function = stack[stack.Count - 1];
                    trees.Add(function);
                    stack.RemoveAt(stack.Count - 1);

                    stack.AddRange(functions.Where(m => m.ParentFunctionId == function.FunctionId).OrderByDescending(m => m.SortOrder).ToList());
                }

                var leading = ":--";

                foreach (var function in trees)
                {
                    var i = 1;
                    while (i < function.LevelId)
                    {
                        function.Name = leading + function.Name;
                        function.Description = leading + function.Description;
                        i++;
                    }
                }

                return trees;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
    }
}
