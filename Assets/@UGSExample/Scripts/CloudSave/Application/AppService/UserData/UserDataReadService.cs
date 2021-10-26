using Cysharp.Threading.Tasks;
using Denicode.UGSExample.CloudSave.Domain.Repository;

namespace Denicode.UGSExample.CloudSave.Application.AppService
{
    /// <summary>
    /// ユーザデータ取得用のアプリケーションサービス
    /// </summary>
    public sealed class UserDataReadService
    {
        readonly IUserDataRepository _userDataRepository;

        public UserDataReadService
        (
            IUserDataRepository userDataRepository
        )
        {
            _userDataRepository = userDataRepository;
        }

        public async UniTask<object> Handle(string key)
        {
            return await _userDataRepository.Read(key);
        }

        public async UniTask<object> Handle<T>(string key) where T : class
        {
            return await _userDataRepository.Read<T>(key);
        }
    }
}