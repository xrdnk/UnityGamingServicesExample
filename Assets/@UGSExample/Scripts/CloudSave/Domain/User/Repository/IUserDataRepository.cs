using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Denicode.UGSExample.CloudSave.Domain.Repository
{
    /// <summary>
    /// ユーザデータ永続化のためのリポジトリクラス
    /// </summary>
    public interface IUserDataRepository
    {
        /// <summary>
        /// Read
        /// </summary>
        /// <returns>PlayerPrefsKey に該当するデータ</returns>
        UniTask<object> Read(string key);

        UniTask<List<object>> ReadAll();

        /// <summary>
        /// Create & Save
        /// </summary>
        /// <param name="key">PlayerPrefsKey</param>
        /// <param name="data">保存するデータ</param>
        UniTask Save(string key, object data);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="key">PlayerPrefsKey</param>
        UniTask Delete(string key);
    }
}