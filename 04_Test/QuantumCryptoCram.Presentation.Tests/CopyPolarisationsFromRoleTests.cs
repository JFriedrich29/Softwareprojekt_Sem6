using System.Collections.ObjectModel;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using QuantumCryptoCram.Application;
using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;
using QuantumCryptoCram.Presentation.ViewModels;
using QuantumCryptoCram.Presentation.ViewModels.Protocol;

namespace QuantumCryptoCram.Presentation.Tests
{
    class CopyPolarisationsFromRoleTests
    {
        private const Polarisation PolarisationAliceValue = Polarisation.Diagonal;
        private const Polarisation PolarisationBobValue = Polarisation.Rectilinear;

        private EveViewModel _eveViewModel;

        [SetUp]
        public void Setup()
        {
            var _firstEntryEve = new EveNotebookEntry(0)
            {
                PolarisationAlice = PolarisationAliceValue,
                PolarisationBob = PolarisationBobValue
            };
            var eveNotebook = new ObservableCollection<EveNotebookEntry>()
            {
                _firstEntryEve
            };
            IQuantumPipeReceiveEndpoint quantumPipe = Substitute.For<IQuantumPipeReceiveEndpoint>();
            // NSubstitute normally returns an IPhoton-Substitute which causes the copied Polarisation to measure
            // values immediately, which we don't want since in the test there should be no sent photons.
            _ = quantumPipe.DequeuePhoton().Returns(_ => null);

            IEve eve = new Eve(
                eveNotebook,
                Substitute.For<IPublicNetwork>(),
                quantumPipe,
                Substitute.For<IQuantumPipeSendEndpoint>(),
                null);
            _eveViewModel = new EveViewModel(
                Substitute.For<INavigationController>(),
                eve,
                Substitute.For<ISimulationManager>(),
                Substitute.For<IDialogViewModelFactory>());
        }

        [Test]
        public void CopyPolarisationsFromAlice()
        {
            // ### Act
            _eveViewModel.CopyPolarisationsFromRole(ProtocolRoleType.Alice);

            // ### Assert
            _eveViewModel.InternalProtocolNotebook[0].MyPolarisation.HasValue.Should().Be(true);
            _eveViewModel.InternalProtocolNotebook[0].MyPolarisation.Value.Should().Be(PolarisationAliceValue);
        }

        [Test]
        public void CopyPolarisationsFromBob()
        {
            // ### Act
            _eveViewModel.CopyPolarisationsFromRole(ProtocolRoleType.Bob);

            // ### Assert
            _eveViewModel.InternalProtocolNotebook[0].MyPolarisation.HasValue.Should().Be(true);
            _eveViewModel.InternalProtocolNotebook[0].MyPolarisation.Value.Should().Be(PolarisationBobValue);
        }

        [Test]
        public void CopyPolarisationsFromBoth()
        {
            // ### Act
            _eveViewModel.CopyPolarisationsFromRole(ProtocolRoleType.Alice);
            _eveViewModel.CopyPolarisationsFromRole(ProtocolRoleType.Bob);

            // ### Assert
            _eveViewModel.InternalProtocolNotebook[0].MyPolarisation.HasValue.Should().Be(true);
            _eveViewModel.InternalProtocolNotebook[0].MyPolarisation.Value.Should().Be(PolarisationBobValue);
        }
    }
}
