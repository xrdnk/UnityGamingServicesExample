using System.Collections.Generic;
using Deniverse.UGSExample.Authentication.Domain.Service;
using Deniverse.UGSExample.Authentication.Presenter;
using Deniverse.UGSExample.Authentication.UIView;
using Deniverse.UGSExample.CloudSave.Application.AppService;
using Deniverse.UGSExample.CloudSave.Domain.Repository;
using Deniverse.UGSExample.CloudSave.Infrastructure.Repository;
using Deniverse.UGSExample.CloudSave.Presentation.Navigator;
using Deniverse.UGSExample.CloudSave.Presentation.Presenter;
using Deniverse.UGSExample.CloudSave.Presentation.UIView;
using Deniverse.UGSExample.Shared.Progression;
using Zenject;

namespace Deniverse.UGSExample.Authentication.LifeCycle
{
    public class CloudSaveLc : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Domain
            Container.BindInterfacesAndSelfTo<AuthService>().AsSingle();

            Container.Bind<IUserDataRepository>().To<CloudSaveUserDataRepository>().AsSingle();

            // Application
            Container.Bind<UserDataSaveService>().AsSingle();
            Container.Bind<UserDataReadService>().AsSingle();
            Container.Bind<UserDataDeleteService>().AsSingle();

            // Presentation
            Container.BindInterfacesAndSelfTo<ViewNavigator>().AsSingle();

            Container.BindInterfacesAndSelfTo<AuthPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserPresenter>().AsSingle();

            Container.Bind<AuthView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UserView>().FromComponentInHierarchy().AsSingle();
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