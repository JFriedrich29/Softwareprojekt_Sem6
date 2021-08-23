using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using QuantumCryptoCram.Application;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Common.Extensions;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Presentation.Utility.Navigation;

using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the ProtocolAnalysis.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class ProtocolAnalysisViewModel : Screen
    {
        private readonly INavigationController _navigationController;
        private readonly IWindowManager _windowManager;
        private readonly ISimulationManager _simulationManager;

        private BindableCollection<Statistic> _protocolStatistics;

        public BindableCollection<Statistic> ProtocolStatistics
        {
            get => _protocolStatistics;

            set => SetAndNotify(ref _protocolStatistics, value);
        }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        /// <value>
        /// The back command.
        /// </value>
        public Action BackCommand => Back;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolAnalysisViewModel"/> class.
        /// </summary>
        /// <param name="navigationController">INavigationController navigationController.</param>
        /// <param name="windowManager">Interface that Stylet provides to display windows.</param>
        /// <param name="simulationManager">The simulationManager.</param>
        /// <param name="alice">An instance of an implementation of <see cref="IAlice"/>.</param>
        /// <param name="eve">An instance of an implementation of <see cref="IEve"/>.</param>
        /// <param name="bob">An instance of an implementation of <see cref="IBob"/>.</param>
        public ProtocolAnalysisViewModel(INavigationController navigationController, IWindowManager windowManager, ISimulationManager simulationManager, IAlice alice, IEve eve, IBob bob)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
            _simulationManager = simulationManager ?? throw new ArgumentNullException(nameof(simulationManager));

            int keySize = alice.GetFinalKey().KeySize;

            _protocolStatistics = new BindableCollection<Statistic>(
                new List<Statistic>()
                {
                    new Statistic()
                    {
                        Name = "Key Länge",
                        Description = "Länge des Final-Keys",
                        Value = keySize.ToString(),
                    },
                    new Statistic()
                    {
                        Name = "Paddding Faktor",
                        Description = "Faktor des Paddings",
                        Value = (alice.PlainText.ToBitArray().Count / (float)keySize).ToString(),
                    },
                    new Statistic()
                    {
                        Name = "Anzahl Photonen gesendet",
                        Description = "Anzahl gesendeter Photonen",
                        Value = alice.MyNotebook.Count(entry => entry.WasPhotonSent).ToString(),
                    },
                    new Statistic()
                    {
                        Name = "Verhältniss PreKey / FinalKey",
                        Description = "Verhältnis wie viele PreKey-Bits wurden für den Vergleich verwendet, wie viele weiter für den FinalKey verwendet",
                        Value = (alice.MyNotebook.Count(entry => entry.WasPreKeySelectionSentOrReceived) + " : " + (float)keySize).ToString(),
                    },
                    new Statistic()
                    {
                        Name = "Alice erkennt Eve",
                        Description = "Eve von Alice erkannt",
                        Value = alice.DetectedEve.ToString(),
                    },
                    new Statistic()
                    {
                        Name = "Bob erkennt Eve",
                        Description = "Eve von Bob erkannt",
                        Value = bob.DetectedEve.ToString(),
                    },
                });

            if (eve != null)
            {
                _protocolStatistics.AddRange(new List<Statistic>()
                {
                    new Statistic()
                    {
                        Name = "Anzahl geleakter Polarisationen von Alice",
                        Description = "Anzahl geleakter Polarisationen von Alice",
                        Value = eve.AliceLeakedPolarisationsCount.ToString(),
                    }, new Statistic()
                    {
                        Name = "Anzahl geleakter Polarisationen von Bob",
                        Description = "Anzahl geleakter Polarisationen von Bob",
                        Value = eve.BobLeakedPolarisationsCount.ToString(),
                    }, new Statistic()
                    {
                        Name = "Anzahl erkannter geleakter Polarisationen Eve",
                        Description = "Anzahl von Eve übernommener geleakter Polarisationen",
                        Value = eve.ExploitedLeakedPolarisationsCount.ToString(),
                    }, new Statistic()
                    {
                        Name = "Anzahl alle Teilnehmer gleiche Polarisation",
                        Description = "Anzahl gleicher Polarisationen bei Alice, Bob und Eve",
                        Value = eve.MyNotebook.Count(entry => entry.RelevanceType == MeasuredDataKeyRelevanceType.AliceBobEveMatch).ToString(),
                    },
                });
            }
            else
            {
                _protocolStatistics.Add(
                    new Statistic()
                    {
                        Name = "Anzahl ProtocolPartner (Alice + Bob) gleiche Polarisationen",
                        Description = "Anzahl von allen Rollen gleich gemessenener Photonen",
                        Value = alice.MyNotebook.Count(entry => entry.IsPolarisationMatching).ToString(),
                    });
            }
        }

        /// <summary>
        /// Navigate back to the requested side.
        /// </summary>
        public void Back()
        {
            _navigationController.NavigateToSimulationOverviewView();
        }

        /// <summary>
        /// Methode that gets called when the HelpButton is pressed.
        /// </summary>
        public void NavigateToMainMenuCommand()
        {
            MessageBoxViewModel.ButtonLabels[MessageBoxResult.Yes] = "Ja";
            MessageBoxViewModel.ButtonLabels[MessageBoxResult.No] = "Nein";
            MessageBoxResult result = _windowManager.ShowMessageBox(
                "Wollen Sie die aktuelle Simulation beenden? Danach kann eine neue Simulation gestartet werden.",
                "Sind Sie sicher?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _simulationManager.StopSimulation();

                _navigationController.NavigateToMain();
            }
        }
    }
}