using System.Collections.ObjectModel;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;
using QuantumCryptoCram.Presentation.ViewModels;
using QuantumCryptoCram.Presentation.ViewModels.Protocol;

namespace QuantumCryptoCram.Presentation.Tests
{
    public class AutoCheckTests
    {
        private AliceNotebookEntry _firstEntry;
        private AliceViewModel _aliceVm;

        [SetUp]
        public void Setup()
        {
            _firstEntry = new AliceNotebookEntry(0);
            IAlice alice = Substitute.For<IAlice>();
            ISimulationManager simulationManager = Substitute.For<ISimulationManager>();
            alice.MyNotebook.Returns(new ObservableCollection<AliceNotebookEntry>()
            {
                _firstEntry
            });
            _aliceVm = new AliceViewModel(Substitute.For<INavigationController>(), alice, simulationManager, Substitute.For<IDialogViewModelFactory>());
        }

        [TestCase(Polarisation.Diagonal, Polarisation.Diagonal, true)]
        [TestCase(Polarisation.Rectilinear, Polarisation.Rectilinear, true)]
        [TestCase(Polarisation.Diagonal, Polarisation.Rectilinear, false)]
        [TestCase(Polarisation.Rectilinear, Polarisation.Diagonal, false)]
        [TestCase(Polarisation.Rectilinear, null, false)]
        [TestCase(Polarisation.Diagonal, null, false)]
        public void AutoCheckPolarisationTest(Polarisation myPolarisation, Polarisation? polarisationPartner, bool expectedReturn)
        {
            // ### Arrange
            _firstEntry.MyPolarisation = myPolarisation;
            _firstEntry.PolarisationPartner = polarisationPartner;

            // ### Act
            _aliceVm.AutoCheckPolarisationCommand();

            // ### Assert
            _firstEntry.IsPolarisationMatching.Should().Be(expectedReturn);
        }

        [TestCase(DataBit.One, DataBit.One, true)]
        [TestCase(DataBit.Zero, DataBit.Zero, true)]
        [TestCase(DataBit.Zero, DataBit.One, false)]
        [TestCase(DataBit.One, DataBit.Zero, false)]
        public void AutoCheckPreKeyBits(DataBit myDataBit, DataBit? dataBitPartner, bool expectedReturn)
        {
            // ### Arrange
            _firstEntry.MyData = myDataBit;
            _firstEntry.PreKeyPartner = dataBitPartner;

            // ### Act
            _aliceVm.AutoCheckPreKeyBitsCommand();

            // ### Assert
            _firstEntry.IsPreKeyMatching.Should().Be(expectedReturn);
        }
    }
}