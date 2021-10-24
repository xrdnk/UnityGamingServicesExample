using Denicode.UGSExample.Authentication.UIView;
using Denicode.UGSExample.CloudSave.Presentation.UIView;
using Denicode.UGSExample.Shared.Progression;
using UniRx;

namespace Denicode.UGSExample.CloudSave.Application.Navigator
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

            _authView.OnDisplayedTriggerAsObservable()
                .Subscribe(_ =>
                {
                    _authView.Hide();
                    _userView.Show();
                })
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