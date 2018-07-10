using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Ostatnio dodane: poruszanie (usprawnienie - zeby szly w kierunku gracza); 
    /// Bindowanie HP; UStawienie mozliwosci ataku co pol sekundy; Dodano wstepnie potiony;
    /// Dodano grafiki potionow; Dzialanie potionow ; Tworzenie przedmiotow w grze - dwie przeciazone metody;
    /// Dodano tolerancje odleglosci gracz : podnoszony przedmiot; Podstawy renderowania poziomów;
    /// Timer ze zdarzeniami z danych poziomow; Poprawiona czytelnosc kodu;
    /// 
    /// Do zrobienia niedługo: Enemy -> rodzaje, SpawnEnemy(); Poziomy Gry; Reset();
    /// Obsluga przycisków Menu
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
    /// Ulepszanie podnoszenia przedmiotow; Slownnik z mozliwym ekwipunkiem;
    /// Mozliwe wybieranie przedmiotu aktualnie uzywanego; Rozwój pozostalych klas pochodnych od weapon;
    /// Bow, Mace - dodano sprawdzanie kierunku i odleglosci; Dodano tooltips opisujace bronie; 
    /// Tooltips z opisami rowniez w ekwipunku GUI; Bindowanie HP Enemy do ProgressBar;
    /// Zamiana EnemyApperiance na EnemyStackPanel; Dodano obrazenia wzgledem broni; Enum eqType
    /// 
    /// Do zrobienia: Klasa (Potiony); Klasa Enemy i klasy pochodne;
    /// od Enemy (potwory), dalsze usprawnianie metody Move(); Płynne poruszanie?; 
    /// GRAFIKA, GUI; Ulepszanie poruszania się enemy; EXP I LVL?; Punkty rankingowe;
    /// Atak i TakeAHit - rozwój w Player i Enemy(Obrona); POZIOMY!; Skille Specjalne;
    /// Zbilansowanie rozgrywki!
    /// </summary>


    public partial class MainWindow : Window
    {
        GameController GameController;
        DateTime lastAttack;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            InitializeBinding();
        }

        public void InitializeBinding()
        {
           // playArea.Children.Add(new Rectangle());
            actualPos.DataContext = GameController;
            actualHp.DataContext = GameController;
            
        }
        public void InitializeGame()
        {
            GameController = new GameController(playArea, equipmentGrid);
            lastAttack = DateTime.Now;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            Key keyPressed = e.Key;
            switch(keyPressed)
            {
                case Key.A:
                    TimeSpan timeSpan = DateTime.Now - lastAttack;

                    if (timeSpan > TimeSpan.FromSeconds(0.5))
                    {
                        lastAttack = DateTime.Now;
                        GameController.AttackEnemy();
                    }
                    else
                        Debug.Print("Za wczesnie aby uzyc broni");
                    break;
                case Key.E:
                    GameController.UsePotion(EquipmentType.BluePotion);
                    break;
                case Key.R:
                    GameController.UsePotion(EquipmentType.RedPotion);
                    break;
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                    GameController.Move((Direction)keyPressed);
                    break;
            }
                
        }
    }
}
