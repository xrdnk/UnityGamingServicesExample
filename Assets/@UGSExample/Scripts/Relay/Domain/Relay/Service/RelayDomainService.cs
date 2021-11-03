using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Deniverse.UGSExample.RelayService.Domain.Service
{
    public static class RelayConstant
    {
        public const int MAX_CONNECTIONS = 4;
    }

    public sealed class RelayDomainService
    {
        Guid _hostAllocationId;
        Guid _playerAllocationId;
        string _joinCode = null;

        /// <summary>
        /// リージョンリストの取得
        /// </summary>
        public async UniTask<List<Region>> GetRegionsAsync() => await Relay.Instance.ListRegionsAsync();

        /// <summary>
        /// アロケーション (Unity Relay の論理セッション) の生成
        /// </summary>
        /// <param name="maxConnections">最大接続数</param>
        /// <param name="regionId">リージョンID</param>
        /// <returns>アローケーションID</returns>
        public async UniTask<Guid> CreateAllocationAsync(int maxConnections = 4, string regionId = null)
        {
            // Important: Once the allocation is created, you have ten seconds to BIND
            var allocation = await Relay.Instance.CreateAllocationAsync(maxConnections, regionId);
            _hostAllocationId = allocation.AllocationId;

            Debug.Log($"Host Allocation ID: {_hostAllocationId}");
            return _hostAllocationId;
        }

        /// <summary>
        /// 入室コードの取得
        /// </summary>
        public async UniTask<string> GetJoinCodeAsync(Guid allocationId = default)
        {
            try
            {
                _joinCode = await Relay.Instance.GetJoinCodeAsync(allocationId == default ? _hostAllocationId : allocationId);
                Debug.Log("GetJoinCode: " + _joinCode);
            }
            catch (RelayServiceException ex)
            {
                var msg = ex.Message + "\n" + ex.StackTrace;
                Debug.LogError(msg);
                throw new RelayServiceException(ex.ErrorCode, ex.Message);
            }

            return _joinCode;
        }

        /// <summary>
        /// アロケーション入室処理
        /// </summary>
        public async UniTask<Guid> JoinAllocationAsync(string joinCode = null)
        {
            try
            {
                var joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode ?? _joinCode);
                _playerAllocationId = joinAllocation.AllocationId;
                Debug.Log($"ClientAllocationID: {_playerAllocationId}");
            }
            catch (RelayServiceException ex)
            {
                var msg = ex.Message + "\n" + ex.StackTrace;
                Debug.LogError(msg);
                throw new RelayServiceException(ex.ErrorCode, ex.Message);
            }

            return _playerAllocationId;
        }
    }
}