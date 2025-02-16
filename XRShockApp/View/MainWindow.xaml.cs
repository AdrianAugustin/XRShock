using System.ComponentModel;
using System.Windows;
using XRShock.ViewModels;


namespace XRShock.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ArduinoAPI arduinoAPI;
        //OSCAPI oscapi;
        ViewModel viewModel;
       
        public MainWindow()
        {
            MainWindow window;

            InitializeComponent();

            window = (MainWindow)Window.GetWindow(App.Current.MainWindow);
            viewModel = new ViewModel();
            MainProgram mainProgram = new MainProgram(viewModel);
            //mainProgram.VModel=viewModel;
            viewModel.mainProgram = mainProgram;
            window.DataContext = viewModel;
            
         
          

        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            viewModel.OnWindowClose(null);
        }


    }
}
