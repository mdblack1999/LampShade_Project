using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        private readonly IAuthHelper _authHelper;
        private readonly IRoleRepository _roleRepository;
        public AccountApplication(IAccountRepository accountRepository , IPasswordHasher passwordHasher ,
            IFileUploader fileUploader , IAuthHelper authHelper , IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
            _authHelper = authHelper;
            _roleRepository = roleRepository;
        }

        public OperationResult Register(RegisterAccount command)
        {
            var operation = new OperationResult();
            if (_accountRepository.Exists(x => x.Username == command.Username))
                return operation.Failed(ApplicationMessages.DuplicateAccount);

            if (_accountRepository.Exists(x => x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessages.DuplicateMobile);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessages.PasswordNotMatch);

            if (command.Password.Length < 6 || command.RePassword.Length < 6)
                return operation.Failed(ApplicationMessages.MinLengthPass);

            var password = _passwordHasher.Hash(command.Password);
            var path = $"profilePhotos";
            string picturePath;
            const string pathAvatarPhoto = "/avatar.png";

            if (command.ProfilePhoto != null)
            {
                picturePath = _fileUploader.Upload(command.ProfilePhoto , path);
            }
            else
            {
                picturePath = pathAvatarPhoto;
            }
            var account = new Account(command.FullName , command.Username , password , command.Mobile , command.RoleId ,
                picturePath);

            _accountRepository.Create(account);
            _accountRepository.SaveChanges();
            return operation.Succeeded(ApplicationMessages.SuccessRegister);
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);
            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_accountRepository.Exists(x =>
                    (x.Username == command.Username || x.Mobile == command.Mobile) && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var path = $"profilePhotos";
            var picturePath = _fileUploader.Upload(command.ProfilePhoto , path);
            account.Edit(command.FullName , command.Username , command.Mobile , command.RoleId , picturePath);
            _accountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessages.PasswordNotMatch);

            var password = _passwordHasher.Hash(command.Password);
            account.ChangePassword(password);

            _accountRepository.SaveChanges();
            return operation.Succeeded();

        }

        public OperationResult Login(Login command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(command.Username);
            if (account == null)
                return operation.Failed(ApplicationMessages.NotRegister);

            var result = _passwordHasher.Check(account.Password , command.Password);
            if (!result.Verified)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            var permissions = _roleRepository.Get(account.RoleId)
                .Permissions.Select(x => x.Code).ToList();

            var authViewModel = new AuthViewModel(account.Id , account.RoleId , account.FullName , account.Username ,
                account.ProfilePhoto , permissions);

            _authHelper.SignIn(authViewModel);
            return operation.Succeeded();
        }


        public EditAccount GetDetails(long id)
        {
            return _accountRepository.GetDetails(id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }

        public void Logout()
        {
            _authHelper.SignOut();
        }
    }
}
