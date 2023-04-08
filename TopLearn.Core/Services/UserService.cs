using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Walet;

namespace TopLearn.Core.Services
{
    public class UserService : IUserService
    {
        private TopLearnContext _Context;
        public UserService(TopLearnContext context)
        {
            _Context = context;
        }
        public bool IsExisEmail(string Email)
        {
            return _Context.Users.Any(x => x.Email == Email);
        }

        public bool IsExisUserName(string UserName)
        {

            return _Context.Users.Any(x => x.UserName == UserName);

        }

        public int AddUser(User user)
        {
            _Context.Users.Add(user);
            _Context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel Login)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(Login.Password);
            string email = FixedText.FixedEmail(Login.Email);
            return _Context.Users.SingleOrDefault(x => x.Email == email && x.Password == hashPassword);
        }

        public bool ActiveAccouant(string activeCode)
        {
            var user = _Context.Users.SingleOrDefault(x => x.ActiveCode == activeCode);
            if (user == null || user.IsActive == null)
                return false;
            //if (user.IsActive == null)
            //    return false;

            user.IsActive = true;
            user.ActiveCode = NameGenerator.GeneratUniqCode();
            _Context.SaveChanges();

            return true;
        }

        public User GetUserByEmail(string Email)
        {
            return _Context.Users.SingleOrDefault(u => u.Email == Email);
        }

        public User GetUserByActiveCode(string ActiveCode)
        {
            return _Context.Users.SingleOrDefault(u => u.ActiveCode == ActiveCode);
        }

        public void UpdateUser(User user)
        {
            _Context.Update(user);
            _Context.SaveChanges();
        }

        public InformationUserViewModel GetUserInformation(int username)
        {
            var user = GetUserByUserName(username);
            InformationUserViewModel Information = new InformationUserViewModel();
            Information.UserName = user.UserName;
            Information.Email = user.Email;
            Information.RegisterDate = user.RegisterDate;
            Information.Walet = BalanceUserWalet(user.UserName);

            return Information;
        }

        public User GetUserByUserName(int UserId)
        {

            return _Context.Users.SingleOrDefault(u => u.UserUniqId == UserId.ToString());
        }
        public User GetUserByUserName(string UserName)
        {
            return _Context.Users.SingleOrDefault(u => u.UserName == UserName);
        }
        public User GetUserByCurrentUserName(string UserName)
        {
            return _Context.Users.SingleOrDefault(u => u.UserName == UserName);
        }
        public SideBarUserPanelViewModel GetSideBarUserPanelData(string UserName)
        {
            return _Context.Users.Where(u => u.UserUniqId == UserName).Select(u => new SideBarUserPanelViewModel
            {
                UserName = u.UserName,
                ImageName = u.UserAvatar,
                RegisterDate = u.RegisterDate
            }).Single();

        }

        public EditProfileVieModel GetDataForEditProfileUser(string Userid)
        {
            return _Context.Users.Where(u => u.UserUniqId == Userid.ToString()).Select(u => new EditProfileVieModel
            {
                AvatarName = u.UserAvatar,
                Email = u.Email,
                UserName = u.UserName,


            }).Single();


        }

        public void EditProfile(int Userid, EditProfileVieModel Profile)
        {

            if (Profile.UserAvatar != null)
            {
                string imgPath = "";
                if (Profile.AvatarName != "Par.jpeg") // ax az ghabl dashte bashe
                {
                    imgPath = Path.Combine(Directory.GetCurrentDirectory(), "WWWroot/UserAvatar", Profile.AvatarName);
                    if (File.Exists(imgPath))
                    {
                        File.Delete(imgPath);
                    }

                }
                Profile.AvatarName = NameGenerator.GeneratUniqCode() + Path.GetExtension(Profile.UserAvatar.FileName);
                imgPath = Path.Combine(Directory.GetCurrentDirectory(), "WWWroot/UserAvatar", Profile.AvatarName);

                using (var stram = new FileStream(imgPath, FileMode.Create))
                {
                    Profile.UserAvatar.CopyTo(stram);
                }
            }

            var User = GetUserByUserName(Userid);
            User.UserName = Profile.UserName;
            User.Email = Profile.Email;
            User.UserAvatar = Profile.AvatarName;

            UpdateUser(User);



        }

        public bool CompareOldPassword(string OldPassword, string UserName)
        {
            string HashOldPassword = PasswordHelper.EncodePasswordMd5(OldPassword);
            return _Context.Users.Any(p => p.Password == HashOldPassword && p.UserName == UserName);




        }

        public void ChangeUserPassword(string UserName, string NewPassword)
        {
            var User = GetUserByUserName(UserName);
            User.Password = PasswordHelper.EncodePasswordMd5(NewPassword);
            UpdateUser(User);
        }
        public int GetUserIdByUserName(string UserName)
        {
            return _Context.Users.Single(u => u.UserName == UserName).UserId;
        }

        public int BalanceUserWalet(string UserName)
        {
            int userId = GetUserIdByUserName(UserName);

            var enter = _Context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPay)
                .Select(w => w.Amount).ToList();

            var exit = _Context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 2)
               .Select(w => w.Amount).ToList();

            return (enter.Sum() - exit.Sum());

        }

        public List<WalletViewModel> GetWalletUser(string UserName)
        {
            int userid = GetUserIdByUserName(UserName);

            return _Context.Wallets
                .Where(w => w.UserId == userid && w.IsPay == true)
                .Select(w => new WalletViewModel()
                {
                    Amount = w.Amount,
                    Description = w.Description,
                    TypeId = w.TypeId,
                    CreateDate = w.CreateDate,
                    IsPay = w.IsPay
                })
                .ToList();
        }

        public int ChargeWallet(string UserName, int Amount, string Description, bool isPey)
        {
            int userid = GetUserIdByUserName(UserName);



            Walet wallet = new Walet()
            {
                Amount = Amount,
                CreateDate = DateTime.Now,
                Description = Description,
                IsPay = isPey,
                TypeId = 1,
                UserId = userid

            };

            return AddWallet(wallet);

        }

        public int AddWallet(Walet Wallet)
        {
            _Context.Wallets.Add(Wallet);
            _Context.SaveChanges();
            return Wallet.WalletId;
        }

        public Walet GetWalletByWalletId(int WalletId)
        {
            return _Context.Wallets.Find(WalletId);

        }

        public void UpdateWallet(Walet Wallet)
        {
            _Context.Wallets.Update(Wallet);
            _Context.SaveChanges();
        }

        public UserForAdminViewModel GetUsers(int PageId = 1, string FilterEmail = "", string FilterUserName = "")
        {
            IQueryable<User> result = _Context.Users;

            if (!string.IsNullOrEmpty(FilterEmail))
            {
                result = result.Where(u => u.Email.Contains(FilterEmail)); // dar ef core jadid az like estefade mishavad
            }
            if (!string.IsNullOrEmpty(FilterUserName))
            {
                result = result.Where(u => u.UserName.Contains(FilterUserName));
            }

            // show item in page


            int take = 2;
            int skip = (PageId - 1) * take;

            //skip = (PageId - 1) * take;
            UserForAdminViewModel listUser = new UserForAdminViewModel();
            listUser.CurrentPage = PageId;
            listUser.PageCount = result.Count() - 1;// /  take;
            listUser.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();


            return listUser;
        }
        public void SaveAvatar(SaveAvatarViewModel SaveAvatar, User user)
        {


            user.UserAvatar = NameGenerator.GeneratUniqCode() + Path.GetExtension(SaveAvatar.UserAvatar.FileName);
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "WWWroot/UserAvatar", user.UserAvatar);

            using (var stram = new FileStream(imgPath, FileMode.Create))
            {
                SaveAvatar.UserAvatar.CopyTo(stram);
            }
        }


        public int AddUserFromAdmin(CreateUsersViewModel CreateUsersViewModel)
        {
            SaveAvatarViewModel SaveAvatarModel = new SaveAvatarViewModel();
            SaveAvatarModel.UserAvatar = CreateUsersViewModel.UserAvatar;
            SaveAvatarModel.UserId = GetUserIdByUserName(CreateUsersViewModel.UserName);

            User user = new User();
            user.Password = PasswordHelper.EncodePasswordMd5(CreateUsersViewModel.Password);
            user.ActiveCode = NameGenerator.GeneratUniqCode();
            user.Email = CreateUsersViewModel.Email;
            user.UserName = CreateUsersViewModel.UserName;
            user.IsActive = true;
            user.RegisterDate = DateTime.Now;

            #region SaveAvatar
            if (CreateUsersViewModel.UserAvatar != null)
            {
                SaveAvatar(SaveAvatarModel, user);
            }
            #endregion

            return AddUser(user);
        }



        public UserEditViewModel GetUserForShowInEditMode(int userId)
        {
            return _Context.Users.Where(u => u.UserId == userId).Select(u => new UserEditViewModel
            {
                UserId = u.UserId,
                Email = u.Email,
                AvatarName = u.UserAvatar,
                UserName = u.UserName,
                UserRoles = u.UserRoles.Select(r => r.RoleId).ToList()
            }).Single();
        }

        public void EditUserFromAdmin(UserEditViewModel UserEdit)
        {
           

            User user = GetUserByUserById(UserEdit.UserId);
            user.Email = UserEdit.Email;
            if (!string.IsNullOrEmpty(UserEdit.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(UserEdit.Password);
            }
            if (UserEdit.UserAvatar != null)
            {
                //Delete Old Ims
                if (UserEdit.AvatarName == "/Par.jpeg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "WWWroot/UserAvatar", UserEdit.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                    //Save New Img

                }
                SaveAvatarViewModel SaveAvatarModel = new SaveAvatarViewModel();
                SaveAvatarModel.UserAvatar = UserEdit.UserAvatar;
                SaveAvatarModel.UserId = GetUserIdByUserName(user.UserName);

                SaveAvatar(SaveAvatarModel, user);

            }
            _Context.Users.Update(user);
            _Context.SaveChanges();
        }

        public User GetUserByUserById(int Userid)
        {
            return _Context.Users.Find(Userid);
        }
    }
}
