using Denicode.UGSExample.CloudSave.Domain.Repository;

namespace Denicode.UGSExample.CloudSave.Application.AppService
{
    /// <summary>
    /// ユーザデータ登録用のアプリケーションサービス
    /// </summary>
    public sealed class UserDataCreateService
    {
        readonly IUserDataRepository _userDataRepository;

        public UserDataCreateService
        (
            IUserDataRepository userDataRepository
        )
        {
            _userDataRepository = userDataRepository;
        }

        public void Handle(string key, object data)
        {
            _ = _userDataRepository.Save(key, data);
        }
    }
}