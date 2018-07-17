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
using System.Windows.Threading;

namespace TheJourneyGame
{
    /// <summary>
    /// Ostatnio dodane:  Podstawy exp i levelowania; GainExp(); Experience stages; LevelUp();
    /// dodano _basicAttacPower dla Player; Gdy enemy umiera dodaje sie exp; Enum EnemyType;
    /// SpawnEnemy(); Ghost i Ghoul; Ponowne rozgrywanie levelu; Śmierć!; Max player level;
    /// Statystyki Grid;
    /// 
    /// 
    /// Do zrobienia niedługo:  Poziomy Gry; Wybieranie z dostepnych leveli
    /// Obsluga przycisków Menu -> SAve i Load; 
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
    /// Zamiana EnemyApperiance na EnemyStackPanel; Dodano obrazenia wzgledem broni; Enum eqType;
    /// poruszanie (usprawnienie - zeby szly w kierunku gracza); 
    /// UStawienie mozliwosci ataku co pol sekundy; Dodano wstepnie potiony;
    /// Dodano grafiki potionow; Dzialanie potionow ; Tworzenie przedmiotow w grze - dwie przeciazone metody;
    /// Dodano tolerancje odleglosci gracz : podnoszony przedmiot; Podstawy renderowania poziomów;
    /// Timer ze zdarzeniami z danych poziomow; Klasa (Potiony); 
    /// 
    /// 
    /// Do zrobienia:  Klasa Enemy i klasy pochodne;
    /// od Enemy (potwory), dalsze usprawnianie metody Move(); Płynne poruszanie?; 
    /// GRAFIKA (levelup), GUI; Ulepszanie poruszania się enemy; EXP I LVL?; Punkty rankingowe;
    /// Atak i TakeAHit - rozwój w Player i Enemy(Obrona); POZIOMY!; Skille Specjalne;
    /// Zbilansowanie rozgrywki!; ReportBox;
    /// </summary>


    public partial class MainWindow : Window
    {
        GameController GameController;
        DateTime lastAttack;
        DispatcherTimer countDownDTimer = new DispatcherTimer()
        {
            Interval = new TimeSpan(TimeSpan.FromSeconds(1).Ticks)
        };

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
            playerLevelLabel.DataContext = GameController;
            playerHitPointsLabel.DataContext = GameController;
            playerExperienceLabel.DataContext = GameController;
            playerCompletedLevelsLabel.DataContext = GameController;
            
        }
        public void InitializeGame()
        {
            GameController = new GameController(playArea, equipmentGrid);
            lastAttack = DateTime.Now;
            countDownDTimer.Tick += CountDownDTimer_Tick;
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

        private void StartNewGameButton_Click(object sender, RoutedEventArgs e)
        {
            mainMenuStackPanel.Visibility = Visibility.Hidden;
            countDownTimer.Visibility = Visibility.Visible;
            
            countDownDTimer.Start(); // Game is starting ;
            GameController.InitializeLevel();
        }

        private void CountDownDTimer_Tick(object sender, EventArgs e)
        {
            int seconds = int.Parse(countDownTimer.Content.ToString());
            seconds--;
            countDownTimer.Content = seconds.ToString();
            if (seconds <= 0)
            {
                countDownDTimer.Stop();
                GameController.StartGame();
                countDownTimer.Visibility = Visibility.Hidden;
                countDownTimer.Content = "3";
            }
        }
       
        public void GetMainMenu(bool wasGamePlayedBefore)
        {
            GameController.ResetGame();
            if(wasGamePlayedBefore)
                startNewGameButton.Content = "Kontynuuj przygodę!";
            mainMenuStackPanel.Visibility = Visibility.Visible;

        }
    }
}
