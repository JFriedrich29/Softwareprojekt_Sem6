using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Protocol;

namespace QuantumCryptoCram.Domain.Config
{
    public class CredentialsManager : PropertyChangedBase
    {
        private string _alicePassword;
        private string _evePassword;
        private string _bobPassword;

        /// <summary>
        /// Gets or sets Alices password.
        /// </summary>
        public string AlicePassword
        {
            get => _alicePassword;

            set
            {
                _alicePassword = value;
                OnPropertyChanged(nameof(IsAlicePasswordSet));
            }
        }

        /// <summary>
        /// Gets or sets Eves password.
        /// </summary>
        public string EvePassword
        {
            get => _evePassword;

            set
            {
                _evePassword = value;
                OnPropertyChanged(nameof(IsEvePasswordSet));
            }
        }

        /// <summary>
        /// Gets or sets Bobs password.
        /// </summary>
        public string BobPassword
        {
            get
            {
                return _bobPassword;
            }

            set
            {
                _bobPassword = value;
                OnPropertyChanged(nameof(IsBobPasswordSet));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialsManager"/> class.
        /// </summary>
        public CredentialsManager()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the password for Alice is set.
        /// </summary>
        /// <returns>True if Alice has a password.</returns>
        public bool IsAlicePasswordSet => !string.IsNullOrEmpty(AlicePassword);

        /// <summary>
        /// Gets a value indicating whether the password for Eve is set.
        /// </summary>
        /// <returns>True if Eve has a password.</returns>
        public bool IsEvePasswordSet => !string.IsNullOrEmpty(EvePassword);

        /// <summary>
        /// Gets a value indicating whether the password for Bob is set.
        /// </summary>
        /// <returns>True if Bob has a password.</returns>
        public bool IsBobPasswordSet => !string.IsNullOrEmpty(BobPassword);

        /// <summary>
        /// Checks if <paramref name="comparison"/> equals the password of the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="comparison">The string to compare to Bobs password.</param>
        /// <returns>True if the string equals the password.</returns>
        public bool EqualsRolePassword(ProtocolRoleType role, string comparison)
        {
            switch (role)
            {
                case ProtocolRoleType.Alice:
                    return AlicePassword.Equals(comparison);

                case ProtocolRoleType.Eve:
                    return EvePassword.Equals(comparison);

                case ProtocolRoleType.Bob:
                    return BobPassword.Equals(comparison);

                default:
                    return false;
            }
        }
    }
}