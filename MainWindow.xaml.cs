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
    /// Ostatnio dodane: Grafika pomieszczenie, ekwipunek (wstepnie), Bat, Ulepszenie metody Bat.Move() i
    /// Bat.TakeAHit(); Dodanie animacji uderzenia do Player i Bat; Dodanie Timera wyznaczajacego
    /// czestotliwosc ataku potworow; Dalsza implementacja IFightable; Poprawa czytelności, kilka komentarzy;
    /// PODNOSZENIE PRZEDMIOTOW - Moze tworzyc image w klasie GameController do której dostarcze
    /// Grid.Name = equipment? 
    /// 
    /// Do zrobienia niedługo: Zarys pochodnych weapon i dzialanie, Enemy -> rodzaje, poruszanie (usprawnienie); 
    ///  Bindowanie HP, StackPanel ??; Podnoszenie przedmiotów;
    /// Ekwipunek, wybór
    /// 
    /// 
    /// Zrobione: Szkielet klasy GameController, (abstr) Position, Player; stworzone pole gry, 
    /// podstawowe dzialanie metody Move() (Equipment, zarys Player); Bindowanie pozycji gracza;
    /// Szkielet klas abstrakcyjnych Equipment, Weapon; Zarys klas Sword, Mace, Bow; Zarys Enemy i 
    /// pierwszej z klas pochodnych -> Bat; podstaowe poruszanie sie enemy;
    /// dodano interfejs IFightable; Implementacja IFightable wstepnie w Player, w mniejszym 
    /// stopniu w enemy; Dodanie kierunku w którym zwrócony jest gracz; Dodanie w klasie Sword 
    /// sprawdzania czy przeciwnik znajduje się w odpowiednim kierunku wzgledem gracza; 
    /// Dodanie sprawdzenia czy przeciwnik jest w zasiegu ataku; Nearby();
    /// 
    /// Do zrobienia: Klasa (Equipment i pochodne (Potiony), (Weapon i pochodne (bronie))); 
    /// Klasa Enemy i klasy pochodne; 
    /// od Enemy (potwory), dalsze usprawnianie metody Move(); metoda Nearby(); Płynne poruszanie?;
    /// Tworzenie kolejnych metod; interakcja miedzy obiektami; Bindowanie hp do paska ->
    /// -> Stack panel zamiast image; GRAFIKA, GUI; Ulepszanie poruszania się enemy;
    /// Atak i TakeAHit - rozwój w Player i Enemy(Obrona, uzycie broni, przeliczanie obrazen);
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
            dg.ItemsSource = GameController.EnemiesList;
            
        }
        public void InitializeGame()
        {
            GameController = new GameController(playArea);
            weapon1.Source = new BitmapImage(new Uri(@"\image\sword100x100.png", UriKind.Relative));
            weapon1.Visibility = Visibility.Hidden;
            weapon1.IsEnabled = false;
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
