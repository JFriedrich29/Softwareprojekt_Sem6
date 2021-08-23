using System.IO;
using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the DocumentationDialogView.
    /// It displays Markdown Text and a help-image.
    /// </summary>
    public class DocumentationDialogViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        /// <summary>
        /// Gets the text to be displayed in the help.
        /// </summary>
        public string LernhilfeText { get; private set; }

        /// <summary>
        /// Gets the path to the image displayed in the help.
        /// </summary>
        public string LernhilfeImage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationDialogViewModel"/> class.
        /// </summary>
        /// <param name="windowManager">The <paramref name="windowManager"/> used to instantiate the window.</param>
        public DocumentationDialogViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        /// <summary>
        /// Displays the window with the help.
        /// </summary>
        /// <param name="textSourceName">The path to the source of the help-text.</param>
        /// <param name="imageSourceName">The path to the source of the help-image.</param>
        public void DisplayLernhilfe(string textSourceName, string imageSourceName)
        {
            LernhilfeText = File.ReadAllText($@"Ressources\Documentation\{textSourceName}");
            LernhilfeImage = $"/QuantumCryptoCram.Presentation;component/Ressources/Documentation/{imageSourceName}";
            _windowManager.ShowWindow(this);
        }
    }
}