using QuantumCryptoCram.Application.Protocol;

namespace QuantumCryptoCram.Application
{
    /// <summary>
    /// Class that holds the main classes alive for communication over the lifetime of the application.
    /// </summary>
    public interface ISimulationManager
    {
        /// <summary>
        /// Gets the reference of the application class for Alice.
        /// </summary>
        IAlice Alice { get; }

        /// <summary>
        /// Gets the reference of the application class for Bob.
        /// </summary>
        IBob Bob { get; }

        /// <summary>
        /// Gets the reference of the application class for Eve.
        /// </summary>
        IEve Eve { get; }

        /// <summary>
        /// Gets a value indicating whether Alice is done with the key exchange.
        /// </summary>
        bool IsAliceDone { get; }

        /// <summary>
        /// Gets a value indicating whether Bob is done with the key exchange.
        /// </summary>
        bool IsBobDone { get; }

        /// <summary>
        /// Gets a value indicating whether Eve is done.
        /// </summary>
        bool IsEveDone { get; }

        /// <summary>
        /// Sets up the communication topology for all protocol participants so that the simulation can be started.
        /// </summary>
        void StartSimulation();

        /// <summary>
        /// Cleans up managed resources like event handlers to prevent memory leaks.
        /// Resets references for the next simulation.
        /// </summary>
        void StopSimulation();
    }
}