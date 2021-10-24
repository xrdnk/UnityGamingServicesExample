using Cysharp.Threading.Tasks;
using Denicode.UGSExample.CloudSave.Domain.Repository;

namespace Denicode.UGSExample.CloudSave.Domain.Service
{
    /// <summary>
    /// ユーザ管理用のサービスクラス
    /// </summary>
    public sealed class UserDataService
    {
        readonly IUserDataRepository _userDataRepository;

        public UserDataService(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }

        /// <summary>
        /// 既に重複確認
        /// </summary>
        public async UniTask<bool> IsAlreadyDataExists(string key)
        {
            return await _userDataRepository.Read(key) != null;
        }
    }
}