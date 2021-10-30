using System.Collections.Generic;
using Denicode.UGSExample.Authentication.Domain.Service;
using Denicode.UGSExample.Authentication.Presenter;
using Denicode.UGSExample.Authentication.UIView;
using Denicode.UGSExample.RelayService.Domain.Service;
using Denicode.UGSExample.RelayService.Presentation.Navigator;
using Denicode.UGSExample.RelayService.Presentation.Presenter;
using Denicode.UGSExample.RelayService.Presentation.View;
using Denicode.UGSExample.Shared.Progression;
using Zenject;

namespace Denicode.UGSExample.RelayService.LifeCycle
{
    public class RelayLc : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AuthService>().AsSingle();
            Container.Bind<RelayDomainService>().AsSingle();

            Container.BindInterfacesAndSelfTo<AuthPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<RelayNavigator>().AsSingle();
            Container.BindInterfacesAndSelfTo<RelayPresenter>().AsSingle();

            Container.Bind<AuthView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RelayView>().FromComponentInHierarchy().AsSingle();
        }

        List<IPeriod> _periods = new();

        void Awake()
        {
            _periods = Container.Resolve<List<IPeriod>>();

            _periods.ForEach(x => x.Originate());
        }

        void OnDestroy()
        {
            _periods.ForEach(x => x.Terminate());
        }
    }
}