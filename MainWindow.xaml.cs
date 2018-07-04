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
    /// Ostatnio dodane: Zarys klas Sword, Mace, Bow; Zarys Enemy i pierwszej z klas
    /// pochodnych -> Bat; podstaowe poruszanie sie enemy;
    /// dodano interfejs IFightable; Implementacja IFightable wstepnie w Player, w mniejszym 
    /// stopniu w enemy; Dodanie kierunku w którym zwrócony jest gracz; Dodanie w klasie Sword 
    /// sprawdzania czy przeciwnik znajduje się w odpowiednim kierunku wzgledem gracza; 
    /// Dodanie sprawdzenia czy przeciwnik jest w zasiegu ataku;
    /// 
    /// Do zrobienia niedługo: Zarys pochodnych weapon, Enemy -> rodzaje, poruszanie; Atak;
    /// Nearby();
    /// 
    /// Zrobione: Szkielet klasy GameController, (abstr) Position, Player; stworzone pole gry, 
    /// podstawowe dzialanie metody Move() (Equipment, zarys Player); Bindowanie pozycji gracza;
    /// Szkielet klas abstrakcyjnych Equipment, Weapon;
    /// 
    /// Do zrobienia: Klasa (Equipment i pochodne (Potiony), (Weapon i pochodne (bronie))); 
    /// Klasa Enemy i klasy pochodne; 
    /// od Enemy (potwory), dalsze usprawnianie metody Move(); metoda Nearby(); Płynne poruszanie?;
    /// Tworzenie kolejnych metod; interakcja miedzy obiektami; Bindowanie hp do paska ->
    /// -> Stack panel zamiast image; GRAFIKA, GUI; Ulepszanie poruszania się enemy;
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
            if (keyPressed == Key.A)
                GameController.AttackEnemy();
            else
                GameController.Move((Direction)keyPressed);
        }
    }
}
