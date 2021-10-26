using System.Collections.Generic;
using Denicode.UGSExample.Authentication.Domain.Service;
using Denicode.UGSExample.Authentication.Presenter;
using Denicode.UGSExample.Authentication.UIView;
using Denicode.UGSExample.CloudSave.Application.AppService;
using Denicode.UGSExample.CloudSave.Domain.Repository;
using Denicode.UGSExample.CloudSave.Infrastructure.Repository;
using Denicode.UGSExample.CloudSave.Presentation.Navigator;
using Denicode.UGSExample.CloudSave.Presentation.Presenter;
using Denicode.UGSExample.CloudSave.Presentation.UIView;
using Denicode.UGSExample.Shared.Progression;
using Zenject;

namespace Denicode.UGSExample.Authentication.LifeCycle
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

        List<IPeriod> _periods = new List<IPeriod>();

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