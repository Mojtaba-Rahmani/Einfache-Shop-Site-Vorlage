using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Roles
        List<Roles> GetRoles();
        void AddRollesToUser(List<int> roleIds, int UserId);

        void EditRolleUser(List<int> roleIds, int UserId);  
        #endregion
    }
}
