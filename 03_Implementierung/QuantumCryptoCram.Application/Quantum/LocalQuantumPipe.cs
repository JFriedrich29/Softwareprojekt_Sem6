using System;
using System.Collections.Generic;

using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Quantum
{
    /// <summary>
    /// A quantum pipe for sending photons between roles on the same machine.
    /// </summary>
    public class LocalQuantumPipe : IQuantumPipe
    {
        /// <inheritdoc/>
        public event EventHandler PhotonReceived;

        /// <summary>
        /// Photons that were not fetched yet.
        /// </summary>
        private readonly Queue<IPhoton> _pendingPhotons = new Queue<IPhoton>();

        /// <inheritdoc/>
        public int PendingPhotonsCount
        {
            get
            {
                return _pendingPhotons.Count;
            }
        }

        /// <inheritdoc/>
        public void SendPhoton(IPhoton photon)
        {
            _pendingPhotons.Enqueue(photon);
            PhotonReceived?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public IPhoton DequeuePhoton()
        {
            if (_pendingPhotons.Count > 0)
            {
                return _pendingPhotons.Dequeue();
            }
            else
            {
                return null;
            }
        }
    }
}