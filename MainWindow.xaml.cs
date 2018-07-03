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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheJourneyGame
{
    /// <summary>
    /// Zrobione Ostatnio: Szkielet klasy GameController, (abstr) Position, Player; stworzone pole gry, 
    /// podstawowe dzialanie metody Move() (Equipment, zarys Player); Bindowanie pozycji gracza;
    /// Szkielet klas abstrakcyjnych Equipment, Weapon;
    /// 
    /// Do zrobienia: Klasa (Equipment i pochodne (Potiony), (Weapon i pochodne (bronie))); 
    /// Klasa Enemy i klasy pochodne; 
    /// od Enemy (potwory), dalsze usprawnianie metody Move(); metoda Nearby(); Płynne poruszanie?;
    /// Tworzenie kolejnych metod, interakcja miedzy obiektami, GRAFIKA, GUI
    /// </summary>


    public partial class MainWindow : Window
    {
        GameController GameController;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            InitializeBinding();

            
        }

        public void InitializeBinding()
        {
            playArea.Children.Add(new Rectangle());
            actualPos.DataContext = GameController;
        }
        public void InitializeGame()
        {
            GameController = new GameController(playArea);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Key keyPressed = e.Key;
            GameController.Move((Direction)keyPressed);
        }
    }
}
