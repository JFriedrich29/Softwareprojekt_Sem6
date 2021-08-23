namespace QuantumCryptoCram.Presentation.ViewModels
{
    // Used to instantiate new view models while making use of dependency injection
    public interface IDialogViewModelFactory
    {
        PasswordDialogViewModel CreatePasswordDialogViewModel();

        DocumentationDialogViewModel CreateDocumentationDialogViewModel();

        DocumentationTextDialogViewModel CreateDocumentationTextDialogViewModel();
    }
}