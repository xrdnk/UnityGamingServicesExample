using Cysharp.Threading.Tasks;
using Denicode.UGSExample.Authentication.Domain.Service;
using Denicode.UGSExample.Authentication.UIView;
using Denicode.UGSExample.Shared.Progression;
using UniRx;

namespace Denicode.UGSExample.Authentication.Presenter
{
    public sealed class AuthPresenter : IPeriod
    {
        readonly AuthService _authService;
        readonly AuthView _authView;
        readonly CompositeDisposable _cd;

        public AuthPresenter
        (
            AuthService authService,
            AuthView authView
        )
        {
            _authService = authService;
            _authView = authView;
            _cd = new CompositeDisposable();
        }

        void IOrigination.Originate()
        {
            UniTask.Void(async () =>
            {
                await UniTask.WaitUntil(() => _authService.IsInitialized);

                _authView.OnSignInTriggerAsObservable()
                    .Subscribe(_ => _authService.SignInAnonymously().Forget())
                    .AddTo(_cd);

                _authView.OnSignOutTriggerAsObservable()
                    .Subscribe(_ => _authService.SignOut())
                    .AddTo(_cd);

                _authService.OnSignedInAsObservable()
                    .Subscribe(tupleValue => _authView.DisplaySignedInResult(tupleValue.isSuccess, tupleValue.playerId).Forget())
                    .AddTo(_cd);

                _authService.OnSingedOutAsObservable()
                    .Subscribe(_ => _authView.DisplaySignedOutResult())
                    .AddTo(_cd);
            });
        }

        void ITermination.Terminate()
        {
            _cd?.Dispose();
        }
    }
}