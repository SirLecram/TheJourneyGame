using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TheJourneyGame
{
    /// <summary>
    /// Logika interakcji dla klasy SelectLevelWindow.xaml
    /// </summary>
    public partial class SelectLevelWindow : Window, ISendData
    {
        private Dictionary<int, Button> _buttonList = new Dictionary<int, Button>();
        private ISendData _mainWindow { get; }
        public SelectLevelWindow(int levelCompleted, ISendData mainWindow)
        {
            InitializeComponent();
            foreach(object btn in selectLevelGrid.Children)
            {
                if(btn is Button)
                    _buttonList.Add(int.Parse((btn as Button).Content.ToString()), btn as Button);
            }
            SetButtonEnability(levelCompleted +1);
            _mainWindow = mainWindow;
        }

        public void SendAndReciveLevel(int levelNumberToSend)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            _mainWindow.SendAndReciveLevel(int.Parse(button.Content.ToString()));
            (_mainWindow as MainWindow).Show();
            this.Close();
        }

        private void SetButtonEnability(int levelCompleted)
        {
            for(int i = 1; i<=_buttonList.Count; i++ )
            {
                if (i <= levelCompleted)
                    _buttonList[i].IsEnabled = true;
                else
                    _buttonList[i].IsEnabled = false;
            }
                
        }
    }
}
