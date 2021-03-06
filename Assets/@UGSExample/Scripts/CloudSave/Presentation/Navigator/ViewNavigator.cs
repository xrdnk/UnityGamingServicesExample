using Deniverse.UGSExample.Authentication.UIView;
using Deniverse.UGSExample.CloudSave.Presentation.UIView;
using Deniverse.UGSExample.Shared.Progression;
using UniRx;

namespace Deniverse.UGSExample.CloudSave.Presentation.Navigator
{
    public sealed class ViewNavigator : IPeriod
    {
        readonly AuthView _authView;
        readonly UserView _userView;
        readonly CompositeDisposable _cd;

        public ViewNavigator
        (
            AuthView authView,
            UserView userView
        )
        {
            _authView = authView;
            _userView = userView;
            _cd = new CompositeDisposable();
        }

        void IOrigination.Originate()
        {
            Init();

            _authView.OnDisplayedSignedInTriggerAsObservable()
                .Subscribe(_ => _userView.Show())
                .AddTo(_cd);

            _authView.OnDisplayedSignedOutTriggerAsObservable()
                .Subscribe(_ => _userView.Hide())
                .AddTo(_cd);
        }

        void ITermination.Terminate()
        {
            _cd?.Dispose();
        }

        void Init()
        {
            _authView.Show();
            _userView.Hide();
        }
    }
}