﻿using System;
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
using Microsoft.Win32;

namespace TheJourneyGame
{
    /// <summary>
    /// 
    /// ##################### VERSION: BETA 1.0 #########################
    /// 
    /// Ostatnio dodane: PauseGame(); Instrukcje w GUI i Sterowanie; Dodane selectLevelWindow;
    /// Działanie przycisku wyjdź;
    /// 
    /// Do zrobienia niedługo:  
    /// 
    /// BLEDY: Poruszanie przed rozpoczeciem gry -> Wyjatek (WYELIMINOWANE, PRZETESTOWANE);
    /// Bledne wskazywanie aktualnie rozgrywanego lvl (WYELIMINOWANE - TESTY);
    /// enemy moving timespan nic nie zmienia!! ; 
    /// ZNOWU PROBLEM Z ZAKONCZENIEM LVL (WYELIMINOWANE, PRZETESTOWANE);
    /// Blad - czyszczenie image ekwipunku przy wczytywaniu gry (WYELIMINOWANE, PRZETESTOWANE);
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
    /// Klasa Enemy i klasy pochodne; Podstawy EXP i LEVEL;
    /// Podstawy exp i levelowania; GainExp(); Experience stages; LevelUp();
    /// dodano _basicAttacPower dla Player; Gdy enemy umiera dodaje sie exp; Enum EnemyType;
    /// SpawnEnemy(); Ghost i Ghoul; Ponowne rozgrywanie levelu; Śmierć!; Max player level;
    /// Statystyki Grid;
    /// Serializacja, zapis i odczyt stanu gry -> SaveFile() i LoadFile(); 
    /// Inicializacja wygladu ekwipunku po deserializacji 
    /// Poprawa wczytywania - rozpoczecie od poziomu zakonczonego; Wskazywanie wybranej aktualnie broni;
    /// Label wyswietlajacy aktualny poziom; IDeserializable;
    /// Poprawa błędu z podwójną inkrementacją _level po ukonczonym lvl;
    /// poprawa bledu z zakonczeniem lvl; poprawa bledu z wyjatkiem przy ataku na przeciwnikow,
    /// w przypadku gdy gracz zginal; Poprawki w spawnowaniu enemy i przedmiotow i levelTimer;
    /// Dodanie nowych poziomow (2-8) i bilansowanie gry; Ghost potrafi sie teleportować;
    /// Zamiana uzywanej broni numerami 1 - 3; Poprawiony blad z czyszczeniem ekwipunku po wczutaniu
    /// wcześnejszego zapisu gry; Poziomy Gry;  Wybór poziomu z ukonczonych; Select level window; 
    /// GroupBox; Instrukcje i przyciski;
    /// 
    /// 
    /// Do zrobienia:  
    /// dalsze usprawnianie metody Move(); Płynne poruszanie?; 
    /// GRAFIKA (levelup), GUI; Ulepszanie poruszania się enemy; Punkty rankingowe;
    /// Atak i TakeAHit - rozwój w Player i Enemy(Obrona); POZIOMY!; Skille Specjalne;
    /// Zbilansowanie rozgrywki!; ReportBox; ZAPIS I ODCZYT;
    /// </summary>


    public partial class MainWindow : Window, ISendData
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
            actualPos.DataContext = GameController;
            actualHp.DataContext = GameController;
            playerLevelLabel.DataContext = GameController;
            playerHitPointsLabel.DataContext = GameController;
            playerExperienceLabel.DataContext = GameController;
            playerCompletedLevelsLabel.DataContext = GameController;
            numberOfPlayingLevelLabel.DataContext = GameController;
            
        }
        public void InitializeGame()
        {
            GameController = new GameController(playArea, equipmentGrid);
            lastAttack = DateTime.Now;
            countDownDTimer.Tick += CountDownDTimer_Tick;
        }

        public void SaveGame()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Pliki zapisu (*.dat)|*.dat| Wszystkie pliki (*.*)|*.*";
            bool? result = saveFile.ShowDialog();
            if(result == true)
            {
                GameController.SaveGame(saveFile.FileName);
            }
            
        }

        public void LoadGame()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Pliki zapisu (*.dat)|*.dat| Wszystkie pliki (*.*)|*.*";
            bool? result = openFile.ShowDialog();
            if(result == true)
            {
                GameController.LoadGame(openFile.FileName);
            }           
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Key keyPressed = e.Key;
            if (!countDownDTimer.IsEnabled && !GameController.IsGamePaused)
            {
                //Key keyPressed = e.Key;
                switch (keyPressed)
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
                    case Key.D:
                        GameController.UsePotion(EquipmentType.BluePotion);
                        break;
                    case Key.F:
                        GameController.UsePotion(EquipmentType.RedPotion);
                        break;
                    case Key.Left:
                    case Key.Up:
                    case Key.Right:
                    case Key.Down:
                        GameController.Move((Direction)keyPressed);
                        break;
                    case Key.D1:
                        GameController.ChangeWeapon(EquipmentType.Sword);
                        break;
                    case Key.D2:
                        GameController.ChangeWeapon(EquipmentType.Bow);
                        break;
                    case Key.D3:
                        GameController.ChangeWeapon(EquipmentType.Mace);
                        break;
                }
            }
            if ((!countDownDTimer.IsEnabled && mainMenuStackPanel.Visibility != Visibility.Visible))
            {
                if (keyPressed == Key.P)
                    GameController.PauseGame(!GameController.IsGamePaused);
            }
            
                
        }

        private void StartNewGameButton_Click(object sender, RoutedEventArgs e)
        {
            mainMenuStackPanel.Visibility = Visibility.Hidden;
            countDownTimer.Visibility = Visibility.Visible;
            GameController.InitializeLevel();
            countDownDTimer.Start(); // Game is starting ;
            
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

        private void SaveGameButton_Click(object sender, RoutedEventArgs e)
        {
            
            SaveGame();
        }

        private void LoadGameButton_Click(object sender, RoutedEventArgs e)
        {
            LoadGame();
        }

        private void SelectLevelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectLevelWindow levelWindow = new SelectLevelWindow(GameController.PlayerCompletedLevels, this);
            levelWindow.Show();
            this.Hide();
        }

        public void SendAndReciveLevel(int levelNumberToSend)
        {
            GameController.SetLevelNumber(levelNumberToSend);

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm",
                MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
