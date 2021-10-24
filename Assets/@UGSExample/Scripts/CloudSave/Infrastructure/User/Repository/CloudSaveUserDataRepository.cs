using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Denicode.UGSExample.CloudSave.Domain.Repository;
using Unity.Services.CloudSave;
using UnityEngine;

namespace Denicode.UGSExample.CloudSave.Infrastructure.Repository
{
    public sealed class CloudSaveUserDataRepository : IUserDataRepository
    {
        public async UniTask<object> Read(string key)
        {
            // PlayerPrefsKey を持つ辞書データの読み込み
            var savedData = await SaveData.LoadAsync(new HashSet<string> {key});
            // 読み込んだ辞書データから Key に対応する JSON データを読み込む
            var jsonData = savedData[key];
            // デシリアライズ処理を行い，データを取得する (Read の場合は自分でデシリアライズする必要がある)
            var data = JsonUtility.FromJson<object>(jsonData);
            return data;
        }

        public async UniTask<List<object>> ReadAll()
        {
            var savedDataDict = await SaveData.LoadAllAsync();
            return Enumerable
                    .Select(savedDataDict, savedData => Read(savedData.Key))
                    .Cast<object>().ToList();
        }

        public async UniTask Save(string key, object data)
        {
            // PlayerPrefsKey と保存データで辞書化
            var savingData = new Dictionary<string, object>{ { key, data } };
            try
            {
                // Cloud Save 上で保存する(この時内部で一緒にシリアライズ処理も行ってくれる)
                await SaveData.ForceSaveAsync(savingData);
            }
            catch (CloudSaveValidationException ex)
            {
                Debug.LogError(ex);
            }
            catch (CloudSaveException ex)
            {
                Debug.LogError(ex);
            }
        }

        public async UniTask Delete(string key)
        {
            await SaveData.ForceDeleteAsync(key);
        }
    }
}