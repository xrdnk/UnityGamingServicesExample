using CloudSave.Application.Enumerate;
using Deniverse.UGSExample.CloudSave.Application.AppService;
using Deniverse.UGSExample.CloudSave.Domain.Entity;
using Deniverse.UGSExample.CloudSave.Presentation.UIView;
using Deniverse.UGSExample.Shared.Progression;
using UniRx;

namespace Deniverse.UGSExample.CloudSave.Presentation.Presenter
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
                    _ = dataType == DataType.User ? _userDataReadService.Handle<UserData>(dataType.ToString()) : _userDataReadService.Handle(dataType.ToString());
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