using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Walet;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExisUserName(string UserName);
        bool IsExisEmail(string Email);
        
        int AddUser(User User);

        User LoginUser(LoginViewModel Login);
        User GetUserByEmail (string Email); 
        User GetUserByActiveCode(string ActiveCode);
        User GetUserByUserName(string UserName);
        User GetUserByUserName(int Userid);
        User GetUserByUserById(int Userid);
        int GetUserIdByUserName(string UserName);
       // User GetUserByCurrentUserName(string UserName);
        void UpdateUser(User user);
        bool ActiveAccouant(string ActiveCode);

        #region UserPanel
        InformationUserViewModel GetUserInformation(int username);
        SideBarUserPanelViewModel GetSideBarUserPanelData(string UserName);
        EditProfileVieModel GetDataForEditProfileUser(string UserName);
        void EditProfile(int UserName , EditProfileVieModel Profile);

        bool CompareOldPassword(string OldPassword, string UserName);
        void ChangeUserPassword(string UserName , string NewPassword);
        #endregion

        #region Walet
        int BalanceUserWalet(string UserName);

        List<WalletViewModel> GetWalletUser(string UserName);
        int ChargeWallet(string UserName , int Amount , string Description, bool isPey = false);

        int AddWallet(Walet Wallet);

        Walet GetWalletByWalletId(int WalletId);   
        
        void UpdateWallet(Walet Wallet);
        #endregion


        #region Admin Panel

        UserForAdminViewModel GetUsers(int PageId = 1, string FilterEmail = "", string FilterUserName="");
        int AddUserFromAdmin(CreateUsersViewModel CreateUsersViewModel);

        UserEditViewModel GetUserForShowInEditMode(int userId);

        void EditUserFromAdmin(UserEditViewModel UserEditViewModel);
        #endregion
    }
}
