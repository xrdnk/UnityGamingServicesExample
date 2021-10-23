using System;
using Cysharp.Threading.Tasks;
using Denicode.UGSExample.Shared.Progression;
using UniRx;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Denicode.UGSExample.Authentication.Domain.Service
{
    /// <summary>
    /// UGS を用いた認証サービス
    /// </summary>
    public sealed class AuthService : IPeriod
    {
        public bool IsInitialized => UnityServices.State == ServicesInitializationState.Initialized;
        public bool IsSignedIn => AuthenticationService.Instance.IsSignedIn;
        public string PlayerId => AuthenticationService.Instance.PlayerId;
        public string AccessToken => AuthenticationService.Instance.AccessToken;

        readonly Subject<(bool isSuccess, string playerId)> _signedInSubject = new Subject<(bool, string)>();
        public IObservable<(bool isSuccess, string playerId)> OnSignedInAsObservable() => _signedInSubject;

        readonly Subject<Unit> _signedOutSubject = new Subject<Unit>();
        public IObservable<Unit> OnSingedOutAsObservable() => _signedOutSubject;

        /// <summary>
        /// 初期化処理
        /// </summary>
        void IOrigination.Originate()
        {
            UniTask.Void(async () =>
            {
                // Unity Game Service の初期化処理
                await UnityServices.InitializeAsync();
            });
        }

        /// <summary>
        /// 匿名サインイン処理
        /// </summary>
        public async UniTask<bool> SignInAnonymously()
        {
            if (!IsInitialized)
            {
                Debug.Log("まだ初期化処理が完了していません．");
                return false;
            }

            if (IsSignedIn)
            {
                Debug.Log("すでにサインイン処理が完了しています．");
                return true;
            }

            // サインイン成功・失敗，サインアウト時のコールバック登録
            AuthenticationService.Instance.SignedIn += SignedInCallback;
            AuthenticationService.Instance.SignInFailed += SignedInFailedCallback;
            AuthenticationService.Instance.SignedOut += SignedOutCallback;

            // 匿名サインイン処理
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            // 認証エラー発生時の例外処理
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
                Debug.LogError($"エラーコード: {ex.ErrorCode}");
            }
            // リクエストエラー発生時の例外処理
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                Debug.LogError($"エラーコード: {ex.ErrorCode}");
            }

            _signedInSubject.OnNext((IsSignedIn, PlayerId));
            return IsSignedIn;
        }

        /// <summary>
        /// サインアウト処理
        /// </summary>
        public void SignOut()
        {
            if (!IsInitialized)
            {
                Debug.Log("まだ初期化処理が完了していません．");
                return;
            }

            if (!IsSignedIn)
            {
                Debug.Log("サインインしていません．");
                return;
            }

            // サインアウト処理
            _signedOutSubject.OnNext(Unit.Default);
            AuthenticationService.Instance.SignOut();

            // コールバック解除
            AuthenticationService.Instance.SignedIn -= SignedInCallback;
            AuthenticationService.Instance.SignInFailed -= SignedInFailedCallback;
            AuthenticationService.Instance.SignedOut -= SignedOutCallback;
        }

        void SignedInCallback() => Debug.Log($"サインインに成功しました．プレイヤーID: [{PlayerId}]，トークン: [{AccessToken}]");

        void SignedInFailedCallback(RequestFailedException ex) => Debug.LogError($"{ex}: サインインに失敗しました．");

        void SignedOutCallback() => Debug.Log("サインアウトしました.");

        /// <summary>
        /// 終端処理
        /// </summary>
        void ITermination.Terminate()
        {
            // コールバック解除
            AuthenticationService.Instance.SignedIn -= SignedInCallback;
            AuthenticationService.Instance.SignInFailed -= SignedInFailedCallback;
            AuthenticationService.Instance.SignedOut -= SignedOutCallback;
        }
    }
}