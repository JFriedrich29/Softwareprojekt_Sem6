using System;

using QuantumCryptoCram.Application;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Common.Encryption;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Config;
using QuantumCryptoCram.Presentation.Utility.Navigation;
using QuantumCryptoCram.Presentation.ViewModels;
using QuantumCryptoCram.Presentation.ViewModels.EncryptionTest;
using QuantumCryptoCram.Presentation.ViewModels.Protocol;

using Stylet;

using StyletIoC;

namespace QuantumCryptoCram.Presentation.Startup
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        /// <summary>
        /// Configures the Stylet dependency injection (aka. Ioc) container.
        /// </summary>
        /// <param name="builder">Ioc builder.</param>
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            RegisterCommonDependencies(builder);

            RegisterGlobalConfigurationDependencies(builder);

            RegisterApplicationDependencies(builder);

            RegisterViewModels(builder);
        }

        /// <summary>
        /// Register the application dependencies (application project) that hold the main application logic.
        /// </summary>
        /// <param name="builder">Ioc builder.</param>
        protected void RegisterApplicationDependencies(IStyletIoCBuilder builder)
        {
            // Instantiating the main model classes is outsourced in the simulation manager,
            // since they need to be instantiated at runtime after the user has chosen simulation options.
            builder.Bind<ISimulationManager>().To<SimulationManager>().InSingletonScope();
            builder.Bind<IAlice>().ToFactory(container => container.Get<ISimulationManager>().Alice);
            builder.Bind<IEve>().ToFactory(container => container.Get<ISimulationManager>().Eve);
            builder.Bind<IBob>().ToFactory(container => container.Get<ISimulationManager>().Bob);
        }

        /// <summary>
        /// Register external dependencies (common project), so their concrete implementation can be switched easily.
        /// </summary>
        /// <param name="builder">Ioc builder.</param>
        protected void RegisterCommonDependencies(IStyletIoCBuilder builder)
        {
            builder.Bind<IEncryptionService>().To<XorEncryptionService>();

            // Register Random Generator as delegate function, so every time a random generator is needed a new instance gets created.
            builder.Bind<Func<IRandomGenerator>>().ToFactory<Func<IRandomGenerator>>(c => () => new RandomGenerator());
        }

        /// <summary>
        /// Register a single instance of configuration/options classes for the whole application (singleton pattern).
        /// </summary>
        /// <param name="builder">Ioc builder.</param>
        protected void RegisterGlobalConfigurationDependencies(IStyletIoCBuilder builder)
        {
            builder.Bind<SimulationOptions>().ToSelf().InSingletonScope();
            builder.Bind<CredentialsManager>().ToSelf().InSingletonScope();
        }

        /// <summary>
        /// Register all view models that are used in the application, so Stylet can create them dynamically, when the user navigates to the dedicated view.
        /// </summary>
        /// <param name="builder">Ioc builder.</param>
        protected void RegisterViewModels(IStyletIoCBuilder builder)
        {
            builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();

            // https://github.com/canton7/Stylet/issues/24
            builder.Bind<Func<SimulationOverviewViewModel>>().ToFactory<Func<SimulationOverviewViewModel>>(c => () => c.Get<SimulationOverviewViewModel>());
            builder.Bind<Func<AliceViewModel>>().ToFactory<Func<AliceViewModel>>(c => () => c.Get<AliceViewModel>());
            builder.Bind<Func<AliceEncryptionTestViewModel>>().ToFactory<Func<AliceEncryptionTestViewModel>>(c => () => c.Get<AliceEncryptionTestViewModel>());
            builder.Bind<Func<BobViewModel>>().ToFactory<Func<BobViewModel>>(c => () => c.Get<BobViewModel>());
            builder.Bind<Func<BobEncryptionTestViewModel>>().ToFactory<Func<BobEncryptionTestViewModel>>(c => () => c.Get<BobEncryptionTestViewModel>());
            builder.Bind<Func<EveViewModel>>().ToFactory<Func<EveViewModel>>(c => () => c.Get<EveViewModel>());
            builder.Bind<Func<EveEncryptionTestViewModel>>().ToFactory<Func<EveEncryptionTestViewModel>>(c => () => c.Get<EveEncryptionTestViewModel>());
            builder.Bind<Func<MainViewModel>>().ToFactory<Func<MainViewModel>>(c => () => c.Get<MainViewModel>());
            builder.Bind<Func<ResultViewModel>>().ToFactory<Func<ResultViewModel>>(c => () => c.Get<ResultViewModel>());
            builder.Bind<Func<LocalModeViewModel>>().ToFactory<Func<LocalModeViewModel>>(c => () => c.Get<LocalModeViewModel>());
            builder.Bind<Func<NetworkModeViewModel>>().ToFactory<Func<NetworkModeViewModel>>(c => () => c.Get<NetworkModeViewModel>());
            builder.Bind<Func<ProtocolAnalysisViewModel>>().ToFactory<Func<ProtocolAnalysisViewModel>>(c => () => c.Get<ProtocolAnalysisViewModel>());

            // Register Dialogs for custom creation
            builder.Bind<IDialogViewModelFactory>().ToAbstractFactory();
        }

        protected override void OnLaunch()
        {
            // There's a circular dependency, where ShellViewModel -> HeaderViewModel -> NavigationController -> ShellViewModel
            // We break this by assigning the ShellViewModel to the NavigationController after constructing it
            NavigationController navigationController = Container.Get<NavigationController>();
            navigationController.Delegate = RootViewModel;
            navigationController.NavigateToMain();
        }
    }
}