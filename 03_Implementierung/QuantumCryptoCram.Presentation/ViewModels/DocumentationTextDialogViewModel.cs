using System.IO;
using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the DocumentationTextDialogView.
    /// It only displays Markdown text and no images.
    /// </summary>
    public class DocumentationTextDialogViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        /// <summary>
        /// Gets the text to be displayed in the help.
        /// </summary>
        public string LernhilfeText { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationTextDialogViewModel"/> class.
        /// </summary>
        /// <param name="windowManager">The <paramref name="windowManager"/> used to instantiate the window.</param>
        public DocumentationTextDialogViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        /// <summary>
        /// Displays the window with the help.
        /// </summary>
        /// <param name="textSourceName">The path to the source of the help-text.</param>
        /// <param name="imageSourceName">The path to the source of the help-image.</param>
        public void DisplayTextLernhilfe(string textSourceName)
        {
            LernhilfeText = File.ReadAllText($@"Ressources\Documentation\{textSourceName}");
            _windowManager.ShowWindow(this);
        }
    }
}
