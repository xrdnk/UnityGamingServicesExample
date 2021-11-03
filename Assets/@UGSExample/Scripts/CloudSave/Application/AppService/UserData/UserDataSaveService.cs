using Deniverse.UGSExample.CloudSave.Domain.Repository;

namespace Deniverse.UGSExample.CloudSave.Application.AppService
{
    /// <summary>
    /// ユーザデータ保存用のアプリケーションサービス
    /// Create と Update を兼ねる
    /// </summary>
    public sealed class UserDataSaveService
    {
        readonly IUserDataRepository _userDataRepository;

        public UserDataSaveService
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