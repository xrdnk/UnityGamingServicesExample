using Deniverse.UGSExample.Authentication.UIView;
using Deniverse.UGSExample.RelayService.Presentation.View;
using Deniverse.UGSExample.Shared.Progression;
using UniRx;

namespace Deniverse.UGSExample.RelayService.Presentation.Navigator
{
    public sealed class RelayNavigator : IPeriod
    {
        readonly AuthView _authView;
        readonly RelayView _relayView;
        readonly CompositeDisposable _cd;

        public RelayNavigator
        (
            AuthView authView,
            RelayView relayView
        )
        {
            _authView = authView;
            _relayView = relayView;
            _cd = new CompositeDisposable();
        }

        void IOrigination.Originate()
        {
            Init();

            _authView.OnDisplayedSignedInTriggerAsObservable()
                .Subscribe(_ => _relayView.Show())
                .AddTo(_cd);

            _authView.OnDisplayedSignedOutTriggerAsObservable()
                .Subscribe(_ => _relayView.Hide())
                .AddTo(_cd);
        }

        void ITermination.Terminate()
        {
            _cd?.Dispose();
        }

        void Init()
        {
            _authView.Show();
            _relayView.Hide();
        }
    }
}