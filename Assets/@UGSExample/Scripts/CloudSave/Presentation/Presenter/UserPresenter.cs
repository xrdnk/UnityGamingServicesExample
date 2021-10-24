using Cysharp.Threading.Tasks;
using Denicode.UGSExample.CloudSave.Application.AppService;
using Denicode.UGSExample.CloudSave.Application.Constant;
using Denicode.UGSExample.CloudSave.Presentation.UIView;
using Denicode.UGSExample.Shared.Progression;
using UniRx;

namespace CloudSave.Presentation.Presenter
{
    public sealed class UserPresenter : IPeriod
    {
        readonly UserDataCreateService _userDataCreateService;
        readonly UserDataReadService _userDataReadService;
        readonly UserDataUpdateService _userDataUpdateService;
        readonly UserDataDeleteService _userDataDeleteService;
        readonly UserView _userView;
        readonly CompositeDisposable _cd;

        public UserPresenter
        (
            UserDataCreateService userDataCreateService,
            UserDataReadService userDataReadService,
            UserDataUpdateService userDataUpdateService,
            UserDataDeleteService userDataDeleteService,
            UserView userView
        )
        {
            _userDataCreateService = userDataCreateService;
            _userDataReadService = userDataReadService;
            _userDataUpdateService = userDataUpdateService;
            _userDataDeleteService = userDataDeleteService;
            _userView = userView;
            _cd = new CompositeDisposable();
        }

        void IOrigination.Originate()
        {
            _userView.OnCreateTriggerAsObservable()
                .Subscribe(name => _userDataCreateService.Handle(CloudSaveConstants.USER_NAME_KEY, name))
                .AddTo(_cd);

            _userView.OnDeleteTriggerAsObservable()
                .Subscribe(_ => _userDataDeleteService.Handle(CloudSaveConstants.USER_NAME_KEY))
                .AddTo(_cd);
        }

        void ITermination.Terminate()
        {
            _cd?.Dispose();
        }
    }
}