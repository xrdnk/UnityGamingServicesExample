using CloudSave.Application.Enumerate;
using Denicode.UGSExample.CloudSave.Application.AppService;
using Denicode.UGSExample.CloudSave.Domain.Entity;
using Denicode.UGSExample.CloudSave.Presentation.UIView;
using Denicode.UGSExample.Shared.Progression;
using UniRx;

namespace Denicode.UGSExample.CloudSave.Presentation.Presenter
{
    public sealed class UserPresenter : IPeriod
    {
        readonly UserDataSaveService _userDataSaveService;
        readonly UserDataReadService _userDataReadService;
        readonly UserDataDeleteService _userDataDeleteService;
        readonly UserView _userView;
        readonly CompositeDisposable _cd;

        public UserPresenter
        (
            UserDataSaveService userDataSaveService,
            UserDataReadService userDataReadService,
            UserDataDeleteService userDataDeleteService,
            UserView userView
        )
        {
            _userDataSaveService = userDataSaveService;
            _userDataReadService = userDataReadService;
            _userDataDeleteService = userDataDeleteService;
            _userView = userView;
            _cd = new CompositeDisposable();
        }

        void IOrigination.Originate()
        {
            _userView.OnCreateTriggerAsObservable()
                .Subscribe(valueTuple => _userDataSaveService.Handle(valueTuple.Item1.ToString(), valueTuple.Item2))
                .AddTo(_cd);

            _userView.OnReadTriggerAsObservable()
                .Subscribe(dataType =>
                {
                    if (dataType == DataType.User)
                    {
                        _ = _userDataReadService.Handle<UserData>(dataType.ToString());
                    }
                    else
                    {
                        _ = _userDataReadService.Handle(dataType.ToString());
                    }
                })
                .AddTo(_cd);

            _userView.OnDeleteTriggerAsObservable()
                .Subscribe(dataType => _userDataDeleteService.Handle(dataType.ToString()))
                .AddTo(_cd);
        }

        void ITermination.Terminate()
        {
            _cd?.Dispose();
        }
    }
}