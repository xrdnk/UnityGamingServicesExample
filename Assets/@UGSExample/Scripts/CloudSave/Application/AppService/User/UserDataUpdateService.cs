using System;
using Cysharp.Threading.Tasks;
using Denicode.UGSExample.CloudSave.Domain.Repository;
using Denicode.UGSExample.CloudSave.Domain.Service;

namespace Denicode.UGSExample.CloudSave.Application.AppService
{
    /// <summary>
    /// ユーザ情報更新用のアプリケーションサービス
    /// </summary>
    public sealed class UserDataUpdateService
    {
        readonly UserDataService _userDataService;
        readonly IUserDataRepository _userDataRepository;

        public UserDataUpdateService
        (
            UserDataService userDataService,
            IUserDataRepository userDataRepository
        )
        {
            _userDataService = userDataService;
            _userDataRepository = userDataRepository;
        }

        public async UniTaskVoid Handle(string key, object data)
        {
            if (!await _userDataService.IsAlreadyDataExists(key))
            {
                throw new Exception($"{key}に関するデータは存在しないため，更新できませんでした．");
            }

            _ = _userDataRepository.Save(key, data);
        }
    }
}