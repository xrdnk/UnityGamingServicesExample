using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Deniverse.UGSExample.LobbyService.Domain.Service
{
    public sealed class LobbyDomainService
    {
        const int MIN_HEARTBEAT_DURATION = 6;

        /// <summary>
        /// ロビー情報の作成処理 (CREATE)
        /// </summary>
        /// <param name="lobbyName">ロビー名</param>
        /// <param name="maxPlayers">ロビー内の最大プレイヤー人数</param>
        /// <param name="options">ロビー作成設定</param>
        /// <returns>ロビーオブジェクト</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<Lobby> CreateLobbyAsync
        (
            string lobbyName,
            int maxPlayers,
            CreateLobbyOptions options = null
        )
        {
            try
            {
                return await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            }
            catch (LobbyServiceException lse)
            {
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// ロビー情報の更新処理 (UPDATE)
        /// </summary>
        /// <param name="lobbyId">更新するロビー情報のID</param>
        /// <param name="options">ロビー更新情報</param>
        /// <returns>更新されたロビー</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<Lobby> UpdateLobbyAsync
        (
            string lobbyId,
            UpdateLobbyOptions options
        )
        {
            try
            {
                return await Lobbies.Instance.UpdateLobbyAsync(lobbyId, options);
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// 指定ロビーのプレイヤー情報の更新処理 (UPDATE)
        /// </summary>
        /// <param name="lobbyId">指定ロビーID</param>
        /// <param name="playerId">更新するプレイヤーのID</param>
        /// <param name="options"></param>
        /// <returns>プレイヤー情報が更新されたロビー</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<Lobby> UpdatePlayerAsync
        (
            string lobbyId,
            string playerId,
            UpdatePlayerOptions options
        )
        {
            try
            {
                return await Lobbies.Instance.UpdatePlayerAsync(lobbyId, playerId, options);
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// ロビーにハートビートを送信する処理
        /// </summary>
        /// <param name="lobbyId">指定ロビーID</param>
        /// <param name="heartBeatDuration">ハートビート送信間隔秒数</param>
        /// <param name="token">CancellationToken</param>
        public async UniTask HeartbeatLobbyAsync
        (
            string lobbyId,
            float heartBeatDuration,
            CancellationToken token
        )
        {
            // ハートビート送信は30秒間の間で5回以上送った場合，429エラーが発生する
            // よって，429エラーが発生しないようにクランプを行うとよい
            if (heartBeatDuration <= MIN_HEARTBEAT_DURATION)
            {
                heartBeatDuration = MIN_HEARTBEAT_DURATION;
            }

            while (!token.IsCancellationRequested)
            {
                await Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
                await UniTask.Delay(TimeSpan.FromSeconds(heartBeatDuration), cancellationToken: token);
            }
        }

        /// <summary>
        /// ロビーを指定した条件で検索する (QUERY)
        /// </summary>
        /// <param name="options">検索条件</param>
        /// <returns>検索にヒットしたロビーリスト</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<List<Lobby>> QueryLobbyAsync(QueryLobbiesOptions options = null)
        {
            try
            {
                return (await Lobbies.Instance.QueryLobbiesAsync(options)).Results;
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// 指定ロビーIDのロビー情報を取得する (READ)
        /// </summary>
        /// <param name="lobbyId">ロビーID</param>
        /// <returns>ロビー情報</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<Lobby> GetLobbyAsync(string lobbyId)
        {
            try
            {
                return await Lobbies.Instance.GetLobbyAsync(lobbyId);
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// 過去に入室したことがあるロビーID一覧の取得処理
        /// </summary>
        /// <returns>過去に入室したことがあるロビーID一覧</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<List<string>> GetJoinedLobbiesAsync()
        {
            try
            {
                return await Lobbies.Instance.GetJoinedLobbiesAsync();
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// ロビーIDを用いた入室処理 (JOIN LOBBY)
        /// </summary>
        /// <param name="lobbyId">ロビーID</param>
        /// <returns>入室したロビー情報</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<Lobby> JoinLobbyByIdAsync(string lobbyId)
        {
            try
            {
                return await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// 入室用コードを用いた入室処理 (JOIN LOBBY)
        /// </summary>
        /// <param name="joinCode">入室用コード</param>
        /// <returns>入室したロビー情報</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<Lobby> JoinLobbyByCodeAsync(string joinCode)
        {
            try
            {
                return await Lobbies.Instance.JoinLobbyByCodeAsync(joinCode);
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// 特定の条件下において素早く入室する処理 (QUICK JOIN)
        /// </summary>
        /// <returns>入室したロビー情報</returns>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask<Lobby> QuickJoinLobbyAsync(QuickJoinLobbyOptions options = null)
        {
            try
            {
                return await Lobbies.Instance.QuickJoinLobbyAsync(options);
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// ロビー削除処理 (DELETE)
        /// </summary>
        /// <param name="lobbyId">削除するロビーID</param>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask DeleteLobbyAsync(string lobbyId)
        {
            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(lobbyId);
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }

        /// <summary>
        /// ロビーからプレイヤーを退出する処理 (REMOVE)
        /// </summary>
        /// <param name="lobbyId">指定ロビーID</param>
        /// <param name="playerId">対象のプレイヤーID</param>
        /// <exception cref="LobbyServiceException">LobbyServiceException</exception>
        public async UniTask RemovePlayerAsync(string lobbyId, string playerId)
        {
            try
            {
                await Lobbies.Instance.RemovePlayerAsync(lobbyId, playerId);
            }
            catch (LobbyServiceException lse)
            {
                Debug.LogError(lse);
                throw new LobbyServiceException(lse);
            }
        }
    }
}