using System;
using Cysharp.Threading.Tasks;
using Shared.UIViewBase;
using UniRx;
using UnityEngine.UI;
using UnityEngine;

namespace Denicode.UGSExample.Authentication.UIView
{
    public sealed class AuthView : UIViewBase
    {
        [SerializeField] Text _textAuthResult;
        [SerializeField] Button _buttonSignIn;
        [SerializeField] Button _buttonSignOut;

        readonly Subject<Unit> _signInSubject = new Subject<Unit>();
        public IObservable<Unit> OnSignInTriggerAsObservable() => _signInSubject;

        readonly Subject<Unit> _signOutSubject = new Subject<Unit>();
        public IObservable<Unit> OnSignOutTriggerAsObservable() => _signOutSubject;

        readonly Subject<Unit> _displayedSignedInSubject = new Subject<Unit>();
        public IObservable<Unit> OnDisplayedTriggerAsObservable() => _displayedSignedInSubject;

        readonly BoolReactiveProperty _isSignedInRp = new BoolReactiveProperty(false);

        protected override void Awake()
        {
            _buttonSignIn.OnClickAsObservable()
                .Subscribe(_ => _signInSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonSignOut.OnClickAsObservable()
                .Subscribe(_ => _signOutSubject.OnNext(Unit.Default))
                .AddTo(this);

            _isSignedInRp
                .Subscribe(isSignedIn =>
                {
                    _buttonSignIn.interactable = !isSignedIn;
                    _buttonSignOut.interactable = isSignedIn;
                })
                .AddTo(this);
        }

        public async UniTaskVoid DisplaySignedInResult(bool isSignedIn, string playerId)
        {
            _textAuthResult.text = isSignedIn
                ? $"<color=green>サインインしました\nID:{playerId}</color>"
                : "<color=red>サインインできませんでした</color>";
            _isSignedInRp.Value = isSignedIn;
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            _displayedSignedInSubject.OnNext(Unit.Default);
        }

        public void DisplaySignedOutResult()
        {
            _textAuthResult.text = "<color=green>サインアウトしました</color>";
            _isSignedInRp.Value = false;
        }
    }
}