using System.Collections.Generic;
using Deniverse.UGSExample.Authentication.Domain.Service;
using Deniverse.UGSExample.Authentication.Presenter;
using Deniverse.UGSExample.Authentication.UIView;
using Deniverse.UGSExample.RelayService.Domain.Service;
using Deniverse.UGSExample.RelayService.Presentation.Navigator;
using Deniverse.UGSExample.RelayService.Presentation.Presenter;
using Deniverse.UGSExample.RelayService.Presentation.View;
using Deniverse.UGSExample.Shared.Progression;
using Zenject;

namespace Deniverse.UGSExample.RelayService.LifeCycle
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