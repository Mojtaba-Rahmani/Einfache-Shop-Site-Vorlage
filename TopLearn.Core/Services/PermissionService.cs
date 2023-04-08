using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services
{
    public class PermissionService : IPermissionService
    {
        TopLearnContext _Context;
        public PermissionService(TopLearnContext Context)
        {
            _Context = Context;
        }

        public void AddRollesToUser(List<int> roleIds, int userId)
        {
            foreach (int roleId in roleIds)
            {
                _Context.UserRoles.Add(new UserRole()

                { 
                    UserId= userId,
                    RoleId= roleId
                });

            }
            _Context.SaveChanges();
        }

        public void EditRolleUser(List<int> roleIds, int UserId)
        {
            //Delete All Rolles
            _Context.UserRoles.Where(r => r.UserId == UserId).ToList().ForEach(r => _Context.UserRoles.Remove(r));
            AddRollesToUser(roleIds, UserId);
        }

        public List<Roles> GetRoles()
        {
            return _Context.Roles.ToList();
        }
    }
}
