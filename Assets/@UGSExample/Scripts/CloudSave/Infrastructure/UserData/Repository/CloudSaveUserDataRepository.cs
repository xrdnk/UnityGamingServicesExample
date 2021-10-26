using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Denicode.UGSExample.CloudSave.Domain.Repository;
using Unity.Services.CloudSave;
using UnityEngine;

namespace Denicode.UGSExample.CloudSave.Infrastructure.Repository
{
    public sealed class CloudSaveUserDataRepository : IUserDataRepository
    {
        /// <summary>
        /// プリミティブ型の読込処理
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async UniTask<object> Read(string key)
        {
            Dictionary<string, string> savedData;
            try
            {
                // PlayerPrefsKey を持つ辞書データの読み込み
                savedData = await SaveData.LoadAsync(new HashSet<string> { key });
            }
            // バリデーションエラー
            catch (CloudSaveValidationException csve)
            {
                Debug.LogError($"{csve.ErrorCode} : {csve.Reason}");
                return null;
            }
            // Cloud Save の汎用エラー
            catch (CloudSaveException cse)
            {
                Debug.LogError($"{cse.ErrorCode} : {cse.Reason}");
                return null;
            }

            // 読み込んだ辞書データから Key に対応する生データを読み込む
            var rawData = savedData[key];
            Debug.Log($"RawData Read: [{key}, {rawData}]");
            return rawData;
        }

        /// <summary>
        /// シリアライズデータの読込処理
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<object> Read<T>(string key) where T : class
        {
            Dictionary<string, string> savedData;
            try
            {
                // PlayerPrefsKey を持つ辞書データの読み込み
                savedData = await SaveData.LoadAsync(new HashSet<string> { key });
            }
            // バリデーションエラー
            catch (CloudSaveValidationException csve)
            {
                Debug.LogError($"{csve.ErrorCode} : {csve.Reason}");
                return null;
            }
            // Cloud Save の汎用エラー
            catch (CloudSaveException cse)
            {
                Debug.LogError($"{cse.ErrorCode} : {cse.Reason}");
                return null;
            }

            // 読み込んだ辞書データから Key に対応する JSON データを読み込む
            var jsonData = savedData[key];
            Debug.Log($"JsonData Read: [{key}, {jsonData}]");
            // デシリアライズ処理を行い，データを取得する
            // NOTE: Read の場合は自分でデシリアライズする必要がある
            var data = JsonUtility.FromJson<T>(jsonData);
            Debug.Log($"RawData Read: [{key}, {data}]");
            return data;
        }

        public async UniTask Save(string key, object data)
        {
            // PlayerPrefsKey と保存データで辞書化
            var savingData = new Dictionary<string, object>{ { key, data } };
            try
            {
                // Cloud Save 上で保存処理
                // NOTE: この時，渡すデータがシリアライズデータの場合，内部で一緒にシリアライズ処理も行ってくれる
                await SaveData.ForceSaveAsync(savingData);
                Debug.Log($"Saved: [{key},{data}]");
            }
            // バリデーションエラー
            catch (CloudSaveValidationException csve)
            {
                Debug.LogError($"{csve.ErrorCode} : {csve.Reason}");
            }
            // Cloud Save の汎用エラー
            catch (CloudSaveException cse)
            {
                Debug.LogError($"{cse.ErrorCode} : {cse.Reason}");
            }
        }

        public async UniTask Delete(string key)
        {
            try
            {
                // Cloud Save 上で削除処理
                await SaveData.ForceDeleteAsync(key);
                Debug.Log($"Deleted: [{key}]");
            }
            // バリデーションエラー
            catch (CloudSaveValidationException csve)
            {
                Debug.LogError($"{csve.ErrorCode} : {csve.Reason}");
            }
            // Cloud Save の汎用エラー
            catch (CloudSaveException cse)
            {
                Debug.LogError($"{cse.ErrorCode} : {cse.Reason}");
            }
        }
    }
}