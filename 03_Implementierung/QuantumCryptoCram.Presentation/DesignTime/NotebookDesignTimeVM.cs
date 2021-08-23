using System.Collections.Generic;

using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.ViewModels.Protocol;

namespace QuantumCryptoCram.Presentation.DesignTime
{
    public class NotebookDesignTimeVM
    {
        public List<AliceNotebookEntryViewModel> ProtocolNotebook { get; set; }

        public NotebookDesignTimeVM()
        {
            ProtocolNotebook = new List<AliceNotebookEntryViewModel>
            {
                new AliceNotebookEntryViewModel()
                {
                    InternalNotebookEntry = new AliceNotebookEntry()
                    {
                        MyData = DataBit.One,
                        MyPolarisation = Polarisation.Rectilinear,
                        PolarisationPartner = Polarisation.Diagonal,
                        WasPolarisationSent = true,
                    },
                },
            };
        }
    }
}