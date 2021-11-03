using Deniverse.UGSExample.Authentication.Domain.Service;
using Deniverse.UGSExample.Authentication.Presenter;
using Deniverse.UGSExample.Authentication.UIView;
using Deniverse.UGSExample.Shared.Progression;
using Zenject;

namespace Deniverse.UGSExample.Authentication.LifeCycle
{
    public class AuthenticationLc : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AuthService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AuthPresenter>().AsSingle();
            Container.Bind<AuthView>().FromComponentInHierarchy().AsSingle();
        }

        IPeriod _authService;
        IPeriod _authPresenter;

        void Awake()
        {
            _authService = Container.Resolve<AuthService>();
            _authPresenter = Container.Resolve<AuthPresenter>();

            _authService.Originate();
            _authPresenter.Originate();
        }

        void OnDestroy()
        {
            _authService.Terminate();
            _authPresenter.Terminate();
        }
    }
}