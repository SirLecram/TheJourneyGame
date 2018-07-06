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
    /// Ostatnio dodane: Ulepszanie podnoszenia przedmiotow; Slownnik z mozliwym ekwipunkiem;
    /// Mozliwe wybieranie przedmiotu aktualnie uzywanego; Rozwój pozostalych klas pochodnych od weapon;
    /// Bow, Mace - dodano sprawdzanie kierunku i odleglosci; Dodano tooltips opisujace bronie; 
    /// Tooltips z opisami rowniez w ekwipunku GUI; Bindowanie HP Enemy do ProgressBar;
    /// Zamiana EnemyApperiance na EnemyStackPanel; Dodano obrazenia wzgledem broni; Enum eqType
    /// 
    /// Do zrobienia niedługo: Enemy -> rodzaje, poruszanie (usprawnienie - zeby szly w kierunku gracza); 
    /// Bindowanie HP, StackPanel ??; Equipment (inne niż weapon); Usprawnienie dodawania ToolTips
    /// do obrazkow w ekwipunku (prawdopoodbny problem z potionami); Dodanie tooltips do Enemy
    /// 
    /// Zrobione: Szkielet klasy GameController, (abstr) Position, Player; stworzone pole gry, 
    /// podstawowe dzialanie metody Move() (Equipment, zarys Player); Bindowanie pozycji gracza;
    /// Szkielet klas abstrakcyjnych Equipment, Weapon; Zarys klas Sword, Mace, Bow; Zarys Enemy i 
    /// pierwszej z klas pochodnych -> Bat; podstaowe poruszanie sie enemy;
    /// dodano interfejs IFightable; Implementacja IFightable wstepnie w Player, w mniejszym 
    /// stopniu w enemy; Dodanie kierunku w którym zwrócony jest gracz; Dodanie w klasie Sword 
    /// sprawdzania czy przeciwnik znajduje się w odpowiednim kierunku wzgledem gracza; 
    /// Dodanie sprawdzenia czy przeciwnik jest w zasiegu ataku; Nearby();
    /// Grafika pomieszczenie, ekwipunek (wstepnie), Bat, Ulepszenie metody Bat.Move() i
    /// Bat.TakeAHit(); Dodanie animacji uderzenia do Player i Bat; Dodanie Timera wyznaczajacego
    /// czestotliwosc ataku potworow; Dalsza implementacja IFightable;
    /// 
    /// Do zrobienia: Klasa (Equipment i pochodne (Potiony), (Weapon i pochodne (bronie))); 
    /// Klasa Enemy i klasy pochodne; 
    /// od Enemy (potwory), dalsze usprawnianie metody Move(); metoda Nearby(); Płynne poruszanie?;
    /// Tworzenie kolejnych metod; interakcja miedzy obiektami; Bindowanie hp do paska ->
    /// -> Stack panel zamiast image; GRAFIKA, GUI; Ulepszanie poruszania się enemy;
    /// Atak i TakeAHit - rozwój w Player i Enemy(Obrona, uzycie broni, przeliczanie obrazen);
    /// Zbilansowanie rozgrywki!
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
            GameController = new GameController(playArea, equipmentGrid);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Key keyPressed = e.Key;
            if (keyPressed == Key.A)
            {
                GameController.AttackEnemy();
            }
            else
                GameController.Move((Direction)keyPressed);
        }
    }
}
