using Cysharp.Threading.Tasks;
using Denicode.UGSExample.Authentication.Domain.Service;
using Denicode.UGSExample.RelayService.Domain.Service;
using Denicode.UGSExample.RelayService.Presentation.View;
using Denicode.UGSExample.Shared.Progression;
using UniRx;

namespace Denicode.UGSExample.RelayService.Presentation.Presenter
{
    public sealed class RelayPresenter : IPeriod
    {
        readonly RelayDomainService _relayService;
        readonly AuthService _authService;
        readonly RelayView _relayView;
        readonly CompositeDisposable _cd;

        public RelayPresenter
        (
            RelayDomainService relayService,
            AuthService authService,
            RelayView relayView
        )
        {
            _relayService = relayService;
            _authService = authService;
            _relayView = relayView;
            _cd = new CompositeDisposable();
        }

        void IOrigination.Originate()
        {
            _relayView.OnGetRegionsAsObservable()
                .Subscribe(_ => UniTask.Void(async () => _relayView.SetRegions(await _relayService.GetRegionsAsync())))
                .AddTo(_cd);

            _relayView.OnCreateRelayAsObservable()
                .Subscribe(region =>
                    UniTask.Void(async () =>
                        _relayView.DisplayHostAllocationId(await _relayService.CreateAllocationAsync(RelayConstant.MAX_CONNECTIONS, region.Id))))
                .AddTo(_cd);

            _relayView.OnGetJoinCodeAsObservable()
                .Subscribe(_ => UniTask.Void(async () => _relayView.DisplayJoinCode(await _relayService.GetJoinCodeAsync())))
                .AddTo(_cd);

            _relayView.OnJoinRelayAsObservable()
                .Subscribe(_ => UniTask.Void(async () => _relayView.DisplayPlayerAllocationId(await _relayService.JoinAllocationAsync())))
                .AddTo(_cd);
        }

        void ITermination.Terminate()
        {
            _cd?.Dispose();
        }
    }

}