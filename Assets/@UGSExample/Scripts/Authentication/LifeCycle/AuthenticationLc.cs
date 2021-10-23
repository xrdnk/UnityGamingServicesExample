using Denicode.UGSExample.Authentication.Domain.Service;
using Denicode.UGSExample.Authentication.Presenter;
using Denicode.UGSExample.Authentication.UIView;
using Denicode.UGSExample.Shared.Progression;
using Zenject;

namespace Denicode.UGSExample.Authentication.LifeCycle
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