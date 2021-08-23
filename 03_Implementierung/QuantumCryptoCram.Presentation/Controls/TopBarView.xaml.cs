using System;
using System.Windows;
using System.Windows.Controls;

using MaterialDesignThemes.Wpf;

namespace QuantumCryptoCram.Presentation.Controls
{
    /// <summary>
    /// Interaktionslogik für TopBarView.xaml.
    /// </summary>
    ///
    public partial class TopBarView : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(TopBarView));

        public static readonly DependencyProperty HasBackButtonProperty =
            DependencyProperty.Register(nameof(HasBackButton), typeof(bool), typeof(TopBarView));

        public static readonly DependencyProperty HasHelpButtonProperty =
            DependencyProperty.Register(nameof(HasHelpButton), typeof(bool), typeof(TopBarView), new PropertyMetadata(true));

        public static readonly DependencyProperty BackButtonCommandProperty =
            DependencyProperty.Register(nameof(BackButtonCommand), typeof(Action), typeof(TopBarView));

        public static readonly DependencyProperty HelpButtonCommandProperty =
            DependencyProperty.Register(nameof(HelpButtonCommand), typeof(Action), typeof(TopBarView));

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }

            set { SetValue(TitleProperty, value); }
        }

        public bool HasBackButton
        {
            get { return (bool)GetValue(HasBackButtonProperty); }

            set { SetValue(HasBackButtonProperty, value); }
        }

        public bool HasHelpButton
        {
            get { return (bool)GetValue(HasHelpButtonProperty); }

            set { SetValue(HasHelpButtonProperty, value); }
        }

        public Action BackButtonCommand
        {
            get { return (Action)GetValue(BackButtonCommandProperty); }

            set { SetValue(BackButtonCommandProperty, value); }
        }

        public Action HelpButtonCommand
        {
            get { return (Action)GetValue(HelpButtonCommandProperty); }

            set { SetValue(HelpButtonCommandProperty, value); }
        }

        public TopBarView()
        {
            InitializeComponent();
            HasBackButton = true;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BackButtonCommand?.Invoke();
        }

        private void ThemeMode_Click(object sender, RoutedEventArgs e)
        {
            if (!ThemeModeButton.IsChecked.HasValue) return;

            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = ThemeModeButton.IsChecked.Value ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        private void HelpMode_Click(object sender, RoutedEventArgs e)
        {
            HelpButtonCommand?.Invoke();
        }
    }
}