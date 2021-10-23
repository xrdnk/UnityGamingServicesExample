using System;
using UniRx;
using UnityEngine.UI;
using UnityEngine;

namespace Denicode.UGSExample.Authentication.UIView
{
    public class AuthView : MonoBehaviour
    {
        [SerializeField] Text _textAuthResult;
        [SerializeField] Button _buttonSignIn;
        [SerializeField] Button _buttonSignOut;

        readonly Subject<Unit> _signInSubject = new Subject<Unit>();
        public IObservable<Unit> OnSignInTriggerAsObservable() => _signInSubject;

        readonly Subject<Unit> _signOutSubject = new Subject<Unit>();
        public IObservable<Unit> OnSignOutTriggerAsObservable() => _signOutSubject;

        readonly BoolReactiveProperty _isSignedInRp = new BoolReactiveProperty(false);

        void Awake()
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

        public void DisplaySignedInResult(bool isSignedIn, string playerId)
        {
            _textAuthResult.text = isSignedIn
                ? $"<color=green>サインインしました\nID:{playerId}</color>"
                : "<color=red>サインインできませんでした</color>";
            _isSignedInRp.Value = isSignedIn;
        }

        public void DisplaySignedOutResult()
        {
            _textAuthResult.text = "<color=green>サインアウトしました</color>";
            _isSignedInRp.Value = false;
        }
    }
}