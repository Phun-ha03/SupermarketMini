using CMS.Models.Authen.Roles;
using CMS.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Helper
{
    public class RoleHelper
    {
        public static List<RoleViewModel> BuildHỉerachy(List<RoleViewModel> roles)
        {
            List<RoleViewModel> trees = new List<RoleViewModel>();
            try
            {
                if (roles == null || roles.Count <= 0) return trees;

                var min_level = roles.Min(m => m.LevelId);

                var stack = roles.Where(m => m.LevelId == min_level).OrderByDescending(m => m.SortOrder).ToList();

                while (stack.Count > 0)
                {
                    var role = stack[stack.Count - 1];
                    trees.Add(role);
                    stack.RemoveAt(stack.Count - 1);

                    stack.AddRange(roles.Where(m => m.ParentRoleId == role.Id).OrderByDescending(m => m.SortOrder).ToList());
                }

                var leading = ":--";

                foreach (var role in trees)
                {
                    var i = 1;
                    while (i < role.LevelId)
                    {
                        role.Name = leading + role.Name;
                        role.Description = leading + role.Description;
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

        public static List<RoleViewModel> BuildHỉerachy(List<RoleViewModel> roles, int parentRoleId)
        {
            List<RoleViewModel> trees = new List<RoleViewModel>();
            try
            {
                if (roles == null || roles.Count <= 0) return trees;

                var min_level = roles.Where(m => parentRoleId <= 0 || m.ParentRoleId == parentRoleId)
                    .Min(m => m.LevelId);

                var stack = roles.Where(m => parentRoleId <= 0 && m.LevelId == min_level
                                            || parentRoleId > 0 && m.LevelId == min_level && m.ParentRoleId == parentRoleId)
                    .OrderByDescending(m => m.SortOrder).ToList();

                while (stack.Count > 0)
                {
                    var role = stack[stack.Count - 1];
                    trees.Add(role);
                    stack.RemoveAt(stack.Count - 1);

                    stack.AddRange(roles.Where(m => m.ParentRoleId == role.Id).OrderByDescending(m => m.SortOrder).ToList());
                }

                var leading = ":--";

                foreach (var role in trees)
                {
                    var i = 1;
                    while (i < role.LevelId)
                    {
                        role.Name = leading + role.Name;
                        role.Description = leading + role.Description;
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
