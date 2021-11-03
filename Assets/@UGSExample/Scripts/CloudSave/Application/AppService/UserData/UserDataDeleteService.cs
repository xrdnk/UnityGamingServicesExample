using Deniverse.UGSExample.CloudSave.Domain.Repository;

namespace Deniverse.UGSExample.CloudSave.Application.AppService
{
    /// <summary>
    /// ユーザデータ処理用のアプリケーションサービス
    /// </summary>
    public sealed class UserDataDeleteService
    {
        readonly IUserDataRepository _userDataRepository;

        public UserDataDeleteService
        (
            IUserDataRepository userDataRepository
        )
        {
            _userDataRepository = userDataRepository;
        }

        public void Handle(string key)
        {
            _ = _userDataRepository.Delete(key);
        }
    }
}