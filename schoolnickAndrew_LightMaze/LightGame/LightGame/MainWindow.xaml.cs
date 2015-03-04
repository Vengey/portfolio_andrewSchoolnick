using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int gameState = 0;
            //0 = Main Menu
            //1 = LevelSelect
            //2 = inGame
            //3 = Help
            //4 = Level Editor
            //5 = Dead
            //6 = Win

        private bool selectedLevel = false; //Check if a level has been selected



        TextBlock timerText;
        

        public int currentLevelIndexX = 0;      //Check location of level on levelGrid
        public int currentLevelIndexY = 0;

        
        public int currentXPos = 0;     //Player's Position
        public int currentYPos = 0;

        Timer myDrawTimer;
        public int assignedTimeViaOptions = 30;
        public int timeAllotted;

        public int exitXPos = 0;        //Exit's Position
        public int exitYPos = 0;

        Levels[,] levelSelection = new Levels[5, 5];    //Level Selection Grid

        GameGrid[,] gridForGame = new GameGrid[10, 10]; //Game Level Grid

        Dictionary<string, List<int>> listOfBridgeCoordinates = new Dictionary<string, List<int>>();    //Obtain coordinates for each position for bridge for later

        public static int CURRENTLEVEL = 0;
            //0 = Nothing
            //1-...  = LevelNumber


        Button backButton;


        Random rand = new Random();

        public bool customLevelPage = false;


        public List<string> comboBoxBuilderContents = new List<string> { };


        public ComboBox levelName;
        public int[,] typesOfSpacesInBuilder = new int[10, 10];

        ComboBox timerCombo;
        List<int> timerComboBoxContent = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60 };

        public bool inBuilder;

        public int priorityCountCurrent = 0;

        

        MediaPlayer sp;

        //Stream mainAudio = File.OpenRead(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\mainMusic_CalmTechno.wav");


       /* Stream audio1 = File.OpenRead(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\forestMazeTechno.wav");
        Stream audio2 = File.OpenRead(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\airManTechno.wav");
        Stream audio3 = File.OpenRead(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\bloodyTearsTechno.wav");
        */
        Button saveButton;


        
        
        //FOR GENERAL U.I. ENHANCEMENTS DURING GAMEPLAY//
        TextBlock exitUnlocked;
        TextBlock bridgeActivated;

        int timerToTurnOffExitUnlockedText = 3;
        int timerToTurnOffBridgeText = 3;

        bool bridgeOn = false;
        bool exitActivated = false;
        /////////////////////////////////////////////////

        Random randomSong = new Random();


        Uri pathSound;

        Uri mainAudio = new Uri(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\mainMusic_CalmTechno.wav");
        Uri audio1 = new Uri(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\forestMazeTechno.wav");
        Uri audio2 = new Uri(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\airManTechno.wav");
        Uri audio3 = new Uri(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\bloodyTearsTechno.wav");


        TextBlock currentSongPlaying;
       

        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = System.Windows.ResizeMode.NoResize;       //I Resize window, not the player


            

            listOfBridgeCoordinates["XCoords"] = new List<int> { };     //Initialize Dictionary Lists
            listOfBridgeCoordinates["YCoords"] = new List<int> { };
            timeAllotted = assignedTimeViaOptions;

            myDrawTimer = new Timer(40000 / 60);
            myDrawTimer.Elapsed += myDrawTimer_Elapsed;
            




            

            pathSound = mainAudio;
            sp.Open(pathSound);
            //sp.Stream = mainAudio;
            sp.Play();
            
            

            


            for (int x = 26; x < 51; x++)
            {
                comboBoxBuilderContents.Add("EX. " + x);
            }
        }

        private void RunOnceOnStartup(object sender, EventArgs e)           //INITIALIZE SOME EVENT HANDLERS SO PROGRAM DOESNT CRASH OR RUN SLOW WITH TOO MANY EVENT HANDLERS
        {
            
            Builder.MouseLeftButtonUp += Builder_MouseLeftButtonUp;

            LevelSelectGrid.MouseMove += LevelSelectGrid_MouseMove;
            LevelSelectGrid.MouseLeftButtonUp += LevelSelectGrid_MouseLeftButtonUp;
            sp = new MediaPlayer();
            sp.MediaEnded += sp_MediaEnded;
            sp.Changed += sp_Changed;
            //sp.Volume = 0.5;
            this.Left = 0;
            this.Top = 0;
            
        }

        void sp_Changed(object sender, EventArgs e)
        {
            sp.Position = new TimeSpan(0);
        }

        void sp_MediaEnded(object sender, EventArgs e)
        {
            sp.Position = new TimeSpan(0);
        }
        

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            gameState = 1;      //Level Selection
            initializeGame();   //Initialize Level Selection
        }

        private void initializeGame()
        {
            CURRENTLEVEL = 0;   //No level selected
            selectedLevel = false;  //No level selected

            //Change Screens
            MainScreen.Visibility = Visibility.Hidden;
            LevelSelectGrid.Visibility = Visibility.Visible;
            Game.Visibility = Visibility.Hidden;
            Options.Visibility = Visibility.Hidden;
            Builder.Visibility = Visibility.Hidden;
            LevelSelectGrid.Background = new SolidColorBrush(Colors.Black);
            
            this.Height = 700;
            this.Width = 650;
            
            //Build Level Select Screen
            levelSelect();
        }

        private void levelSelect()
        {
            if (gameState == 1) //Only do it if on level selection state
            {
                //Back Button
                backButton = new Button();
                backButton.Content = "Back";
                backButton.Width = 100;
                backButton.Height = 25;
                backButton.Foreground = new SolidColorBrush(Colors.Black);
                backButton.Background = new SolidColorBrush(Colors.White);
                backButton.Margin = new Thickness(-500, -600, 0, 0);

                //NextPage Button
                Button nextPageButton = new Button();
                if (customLevelPage == false)
                {
                    nextPageButton.Content = "Custom Levels...";
                }
                else
                {
                    nextPageButton.Content = "Main Levels...";
                }
                nextPageButton.Width = 100;
                nextPageButton.Height = 25;
                nextPageButton.Foreground = new SolidColorBrush(Colors.Black);
                nextPageButton.Background = new SolidColorBrush(Colors.White);
                nextPageButton.Margin = new Thickness(500, -600, 0, 0);

                backButton.Click += backButton_Click;
                nextPageButton.Click += nextPageButton_Click;

                //Level Select Title
                TextBlock levelSelectText = new TextBlock();
                levelSelectText.Text = "Level Select:";
                levelSelectText.TextAlignment = TextAlignment.Center;

                levelSelectText.Margin = new Thickness(0, 10, 0, 0);
                levelSelectText.FontSize = 24;
                levelSelectText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");

                levelSelectText.Foreground = new SolidColorBrush(Colors.White);
                LevelSelectGrid.Children.Add(levelSelectText);

                LevelSelectGrid.Children.Add(backButton);
                LevelSelectGrid.Children.Add(nextPageButton);


                //Mouse Events
                

                int currentLevel;
                if (customLevelPage == false)
                {
                    currentLevel = 1;
                }
                else
                {
                    currentLevel = 26;
                }

                
                
                //Build Grid
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {

                        levelSelection[x, y] = new Levels(x, y, currentLevel, LevelSelectGrid, customLevelPage);
                        currentLevel++;
                    }
                }
            }
        }

        void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            customLevelPage = !customLevelPage;
            levelSelect();
        }

        //Back Button to return to main menu
        void backButton_Click(object sender, RoutedEventArgs e)
        {
            gameState = 0;  //Main Menu State
            MainScreen.Visibility = Visibility.Visible;
            LevelSelectGrid.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Hidden;
            Options.Visibility = Visibility.Hidden;
            Builder.Visibility = Visibility.Hidden;
            

            //Clear the Builder
            if (inBuilder)
            {
                foreach (GameGrid g in gridForGame)
                {
                    g.TypeOfSpace = 0;
                    g.updateColorBuilder();

                }
            }
            
            Builder.Children.Clear();
            inBuilder = false;
            this.Height = 350;
            this.Width = 526;

            
        }



        void LevelSelectGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            for(int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    int levelSelected = levelSelection[x,y].checkIfMouseUp();       //Check which level was selected
                    if (levelSelected != 0 && selectedLevel == false)               //As long as a level was selected and are hovering over that level
                    {
                        selectedLevel = true;
                        currentLevelIndexX = x;     //Location of the level on grid for color purposes later
                        currentLevelIndexY = y;
                        CURRENTLEVEL = levelSelected;
                        

                    }
                }
            }

            //If a level is selected start the level
            if (selectedLevel && CURRENTLEVEL != 0)
            {
                startLevel();
                
            }
            Console.WriteLine(CURRENTLEVEL);
        }

        void LevelSelectGrid_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Levels l in levelSelection)
            {
                l.checkIfMouseOver();
            }
        }



        private void startLevel()
        {
            

            //Begin the level
            gameState = 2;
            Game.Background = new SolidColorBrush(Colors.Black);

            /*sp.Stop();
            sp.SoundLocation = System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\forestMazeTechno.wav";
            //sp = new System.Media.SoundPlayer(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\forestMazeTechno.wav");
            sp.PlayLooping();
            */

            loadInLevel();
            
            
            
            
            //myDrawTimer.AutoReset = true;
            
        }

        private void myDrawTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {

                    updateValues();
                    
                }));
            }
            catch
            {

            }
        }

        private void updateValues()
        {
            timeAllotted--;
            updateTimer();
            Console.WriteLine("Elapsed");

            timerToTurnOffExitUnlockedText--;
            timerToTurnOffBridgeText--;

            ////////Timer To Get Rid Of Go To Exit Text/////////
            if (timerToTurnOffExitUnlockedText <= 0)
            {
                timerToTurnOffExitUnlockedText = 0;
                Game.Children.Remove(exitUnlocked);

            }
            ////////Timer To Get Rid Of Bridge Active Text/////////
            if (timerToTurnOffBridgeText <= 0)
            {
                timerToTurnOffBridgeText = 0;
                Game.Children.Remove(bridgeActivated);

            }
            ////If time runs out/////
            if (timeAllotted == 0)
            {
                DisplayWinOrLose(false);
            }
        }


       
            
        

        private void loadInLevel()
        {
           
            //LOAD IN THE LEVEL VIA TEXT DOCUMENT
            if (gameState == 2 && File.Exists(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Levels\Level" + CURRENTLEVEL + ".txt"))
            {
                
                //sp.SoundLocation = System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\forestMazeTechno.wav";
                //Audio//

                currentSongPlaying = new TextBlock();
                
                currentSongPlaying.TextAlignment = TextAlignment.Right;
                currentSongPlaying.VerticalAlignment = VerticalAlignment.Top;
                currentSongPlaying.Foreground = new SolidColorBrush(Colors.White);
                currentSongPlaying.FontFamily = new System.Windows.Media.FontFamily("Moire");
                currentSongPlaying.FontSize = 16;
                currentSongPlaying.Margin = new Thickness(0, 10, 10, 0);
                //THIS IS MAKING THE PROGRAM CRASH//
                int nextSong = randomSong.Next(1, 4);
                if (nextSong == 1)
                {
                    sp.Open(audio1);
                    currentSongPlaying.Text = "Forest Maze Remix by LotSS";
                }
                else if (nextSong == 2)
                {
                    sp.Open(audio2);
                    currentSongPlaying.Text = "Airman Remix by VGremixproduction";
                }
                else
                {
                    sp.Open(audio3);
                    currentSongPlaying.Text = "Bloody Tears Remix by Hazeri";
                }
                //sp.Stream = audio1;
                //sp.Stream.Position = 0;
                //sp.Load();
                
                sp.Play();

                
                ////////////////////////////////////


                MainScreen.Visibility = Visibility.Hidden;
                LevelSelectGrid.Visibility = Visibility.Hidden;
                Game.Visibility = Visibility.Visible;
                Options.Visibility = Visibility.Hidden;
                Builder.Visibility = Visibility.Hidden;
                
                string currentLine = null;
                try
                {
                    using (StreamReader sr = new StreamReader(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Levels\Level" + CURRENTLEVEL + ".txt"))
                    {
                        int y = 0;
                        Console.WriteLine((System.IO.Directory.GetCurrentDirectory().ToString() + @"\Levels\Level" + CURRENTLEVEL + ".txt"));
                        assignedTimeViaOptions = Int32.Parse(sr.ReadLine());
                        timeAllotted = assignedTimeViaOptions;
                        while ((currentLine = sr.ReadLine()) != null)
                        {

                            //Read each line as the row for the grid
                            //Each character in the line is the xPosition of the grid space
                            for (int x = 0; x < 10; x++)
                            {
                                if (currentLine != "")
                                {

                                    gridForGame[x, y] = new GameGrid(x, y, currentLine[x], Game, inBuilder);

                                }

                            }
                            //Increment the y value as we are on next row of read lines
                            y++;

                        }
                    }
                }
                catch
                {
                    
                }



                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if (gridForGame[x, y].TypeOfSpace == 5)     //Player
                        {
                            currentXPos = x;
                            currentYPos = y;
                        }
                        else if (gridForGame[x, y].TypeOfSpace == 3)    //Exit
                        {
                            exitXPos = x;
                            exitYPos = y;
                        }
                        else if (gridForGame[x, y].TypeOfSpace == 6)    //Deactivated Bridge Spaces
                        {
                            listOfBridgeCoordinates["XCoords"].Add(x);
                            listOfBridgeCoordinates["YCoords"].Add(y);
                        }

                    }
                }
                createDarknessAroundThePlayer();    //Create the light source around the player
                createTimer();
                Game.Children.Add(currentSongPlaying);
                //sp.Stop();
                
                myDrawTimer.Start();

                


                
                //sp = new System.Media.SoundPlayer(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\forestMazeTechno.wav");
                
                
                
                

            }
            else
            {
                selectedLevel = false;
            }
        }

        

        private void createTimer()          //Display Timer
        {
            timerText = new TextBlock();
            timerText.Text = "Time Left: ";
            
            timerText.TextAlignment = TextAlignment.Left;
            timerText.VerticalAlignment = VerticalAlignment.Top;
            timerText.Foreground = new SolidColorBrush(Colors.White);
            timerText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
            timerText.FontSize = 24;
            
            Game.Children.Add(timerText);
            
            
        }
        private void updateTimer()      //Update the timer to display time as it counts down
        {
            timerText.Text = "Time Left: " + timeAllotted;
        }

       

        //Clear previous spot player was on to its original color
        private void deletePreviousSpot(int x, int y)
        {
            gridForGame[currentXPos + x, currentYPos + y].IsPlayerOnSpace = false;
            gridForGame[currentXPos + x, currentYPos + y].updateColor();
        }
        
        
        

        















        //Help Menu
        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            gameState = 3;
            goToOptions();
        }
        private void goToOptions()
        {
            MainScreen.Visibility = Visibility.Hidden;
            LevelSelectGrid.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Hidden;
            Options.Visibility = Visibility.Visible;
            Builder.Visibility = Visibility.Hidden;

            Options.Background = new SolidColorBrush(Colors.Black);
        }

        private void BuilderButton_Click(object sender, RoutedEventArgs e)
        {
            gameState = 4;
            goToBuilder();
        }
        private void goToBuilder()
        {
            MainScreen.Visibility = Visibility.Hidden;
            LevelSelectGrid.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Hidden;
            Options.Visibility = Visibility.Hidden;
            Builder.Visibility = Visibility.Visible;
            this.Height = 700;
            this.Width = 625;

            inBuilder = true;
            

            buildTheBuilderScreen();
        }
        private void buildTheBuilderScreen()
        {
            
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    gridForGame[x, y] = new GameGrid(x, y, '0', Builder, inBuilder);
                   
                }
            }

            TextBlock builderGuide = new TextBlock();
            builderGuide.Text = "Click a square to change its type and build your level!";
            builderGuide.TextAlignment = TextAlignment.Center;
            builderGuide.VerticalAlignment = VerticalAlignment.Center;
            builderGuide.Foreground = new SolidColorBrush(Colors.Black);
            builderGuide.FontFamily = new System.Windows.Media.FontFamily("Moire ExtraBold");
            builderGuide.FontSize = 20;
            builderGuide.Margin = new Thickness(0, 550, 0, 0);


            TextBlock levelToBeSavedTo = new TextBlock();
            levelToBeSavedTo.Text = "Level Number:";
            levelToBeSavedTo.TextAlignment = TextAlignment.Center;
            levelToBeSavedTo.VerticalAlignment = VerticalAlignment.Center;
            levelToBeSavedTo.Foreground = new SolidColorBrush(Colors.Black);
            levelToBeSavedTo.FontFamily = new System.Windows.Media.FontFamily("Moire ExtraBold");
            levelToBeSavedTo.FontSize = 16;
            levelToBeSavedTo.Margin = new Thickness(-125, 625, 0, 0);


            Button backButton = new Button();
            backButton.Content = "Back";
            backButton.Width = 100;
            backButton.Height = 25;
            backButton.Foreground = new SolidColorBrush(Colors.White);
            backButton.Background = new SolidColorBrush(Colors.Black);
            backButton.BorderBrush = null;
            backButton.Margin = new Thickness(-500, 625, 0, 0);

            saveButton = new Button();
            saveButton.Content = "Save";
            saveButton.Width = 100;
            saveButton.Height = 25;
            saveButton.Foreground = new SolidColorBrush(Colors.White);
            saveButton.Background = new SolidColorBrush(Colors.Black);
            saveButton.BorderBrush = null;
            saveButton.Margin = new Thickness(500, 625, 0, 0);
            saveButton.IsEnabled = false;

            Button loadButton = new Button();
            loadButton.Content = "Load";
            loadButton.Width = 100;
            loadButton.Height = 25;
            loadButton.Foreground = new SolidColorBrush(Colors.White);
            loadButton.Background = new SolidColorBrush(Colors.Black);
            loadButton.BorderBrush = null;
            loadButton.Margin = new Thickness(300, 625, 0, 0);

            levelName = new ComboBox();
            levelName.ItemsSource = comboBoxBuilderContents;
            levelName.Width = 75;
            levelName.Height = 25;
            levelName.Foreground = new SolidColorBrush(Colors.Black);
            levelName.Background = new SolidColorBrush(Colors.White);

            levelName.BorderBrush = new SolidColorBrush(Colors.Black);
            levelName.Margin = new Thickness(100, 625, 0, 0);
            levelName.SelectedIndex = 0;


            TextBlock timerComboText = new TextBlock();
            timerComboText.Text = "Timer Duration:";
            timerComboText.TextAlignment = TextAlignment.Center;
            timerComboText.VerticalAlignment = VerticalAlignment.Center;
            timerComboText.Foreground = new SolidColorBrush(Colors.Black);
            timerComboText.FontFamily = new System.Windows.Media.FontFamily("Moire ExtraBold");
            timerComboText.FontSize = 16;
            timerComboText.Margin = new Thickness(-350, -633, 0, 0);

            timerCombo = new ComboBox();
            timerCombo.ItemsSource = timerComboBoxContent;
            timerCombo.Width = 75;
            timerCombo.Height = 25;
            timerCombo.Foreground = new SolidColorBrush(Colors.Black);
            timerCombo.Background = new SolidColorBrush(Colors.White);

            timerCombo.BorderBrush = new SolidColorBrush(Colors.Black);
            timerCombo.Margin = new Thickness(-100, -633, 0, 0);
            timerCombo.SelectedIndex = 5;


            Builder.Children.Add(builderGuide);
            
            
            Builder.Children.Add(levelToBeSavedTo);
            Builder.Children.Add(backButton);
            Builder.Children.Add(saveButton);
            Builder.Children.Add(loadButton);
            Builder.Children.Add(levelName);
            Builder.Children.Add(timerComboText);
            Builder.Children.Add(timerCombo);
            

            backButton.Click += backButton_Click;
            saveButton.Click += saveButton_Click;
            loadButton.Click += loadButton_Click;

            

            
            

        }

        void loadButton_Click(object sender, RoutedEventArgs e)     //Load in Level into builder
        {
            try
            {
                if (File.Exists(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Levels\Level" + (levelName.SelectedIndex + 26) + ".txt"))
                {
                    string currentLine = null;
                    saveButton.IsEnabled = true;
                    using (StreamReader sr = new StreamReader(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Levels\Level" + (levelName.SelectedIndex + 26) + ".txt"))
                    {
                        int y = 0;
                        //Console.WriteLine((System.IO.Directory.GetCurrentDirectory().ToString() + @"\Levels\Level" + (levelName.SelectedIndex + 26) + ".txt"));
                        assignedTimeViaOptions = Int32.Parse(sr.ReadLine());
                        timeAllotted = assignedTimeViaOptions;
                        timerCombo.SelectedItem = timeAllotted;
                        while ((currentLine = sr.ReadLine()) != null)
                        {

                            //Read each line as the row for the grid
                            //Each character in the line is the xPosition of the grid space
                            for (int x = 0; x < 10; x++)
                            {
                                if (currentLine != "")
                                {

                                    gridForGame[x, y].TypeOfSpace = currentLine[x] - 48;   //(x, y, currentLine[x], Game, inBuilder)

                                    gridForGame[x, y].updateColorBuilder();
                                }

                            }
                            //Increment the y value as we are on next row of read lines
                            y++;
                            Console.WriteLine();

                        }
                    }
                }
            }
            catch
            {

            }
        }

        void Builder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int numberOfPlayers = 0;
            int numberOfExits = 0;
            int numberOfExitSwitches = 0;
            int priorityCountPrevious = priorityCountCurrent;
            priorityCountCurrent++;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {

                    gridForGame[x, y].checkIfMouseUp(priorityCountCurrent);
                    
                    
                    if (gridForGame[x, y].TypeOfSpace == 2)
                    {
                        numberOfExitSwitches++;
                    }
                    else if (gridForGame[x, y].TypeOfSpace == 3 || gridForGame[x, y].TypeOfSpace == 4)
                    {
                        numberOfExits++;
                        
                    }
                    else if (gridForGame[x, y].TypeOfSpace == 5)
                    {
                        numberOfPlayers++;
                    }
                    


                    
                    


                    
                    if (numberOfExitSwitches > 1 && gridForGame[x, y].TypeOfSpace == 2 && gridForGame[x,y].PriorityValue > priorityCountPrevious)
                    {
                        gridForGame[x, y].TypeOfSpace = 3;
                        numberOfExitSwitches--;
                    }
                    if (numberOfExits > 1 && gridForGame[x, y].TypeOfSpace == 3 && gridForGame[x, y].PriorityValue > priorityCountPrevious)
                    {
                        gridForGame[x, y].TypeOfSpace = 5;
                        numberOfExits--;
                    }
                    else if (numberOfExits > 1 && gridForGame[x, y].TypeOfSpace == 4 && gridForGame[x, y].PriorityValue > priorityCountPrevious)
                    {
                        gridForGame[x, y].TypeOfSpace = 5;
                        numberOfExits--;
                    }
                    if (numberOfPlayers > 1 && gridForGame[x, y].TypeOfSpace == 5 && gridForGame[x, y].PriorityValue > priorityCountPrevious)
                    {
                        gridForGame[x, y].TypeOfSpace = 6;
                        numberOfPlayers--;
                    }

                    Console.WriteLine(numberOfExitSwitches + "\n" + numberOfExits + "\n" + numberOfPlayers);

                    gridForGame[x, y].updateColorBuilder();
                    typesOfSpacesInBuilder[x, y] = gridForGame[x, y].TypeOfSpace;
                    
                }

            }
            if (numberOfPlayers > 0)
            {
                saveButton.IsEnabled = true;
            }
            else
            {
                saveButton.IsEnabled = false;
            }
        }

        

        void saveButton_Click(object sender, RoutedEventArgs e)
        {
            int currentLevelSelected = levelName.SelectedIndex + 26;
            
            using (StreamWriter sw = new StreamWriter(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Levels\Level" + currentLevelSelected + ".txt"))
            {
                sw.WriteLine(timerCombo.SelectedItem);
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        sw.Write(gridForGame[x, y].TypeOfSpace + "");
                    }
                    sw.WriteLine();
                } 
            }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////


        private void KeyReleased(object sender, KeyEventArgs e)
        {
            //IF PLAYING THE GAME//
            if (gameState == 2)
            {
                Console.WriteLine("Yes");
                if (e.Key == Key.A || e.Key == Key.Left)          //Move Left
                {
                    if (currentXPos > 0)
                    {
                        currentXPos--;
                        deletePreviousSpot(1, 0);
                    }
                }
                else if (e.Key == Key.D || e.Key == Key.Right)    //Move Right
                {
                    if (currentXPos < 9)
                    {
                        currentXPos++;
                        deletePreviousSpot(-1, 0);
                    }
                }
                else if (e.Key == Key.W || e.Key == Key.Up)       //Move Up
                {
                    if (currentYPos > 0)
                    {
                        currentYPos--;
                        deletePreviousSpot(0, 1);
                    }
                }
                else if (e.Key == Key.S || e.Key == Key.Down)     //Move Down
                {
                    if (currentYPos < 9)
                    {
                        currentYPos++;
                        deletePreviousSpot(0, -1);
                    }
                }




                gridForGame[currentXPos, currentYPos].IsPlayerOnSpace = true;   //Tell the grid space the player is on that it is on it to maintain colors, but use this for collision detection later

                if (gridForGame[currentXPos, currentYPos].TypeOfSpace == 2 && exitActivated == false)     //If player lands on the exit switch
                {
                    gridForGame[exitXPos, exitYPos].TypeOfSpace = 4;
                    gridForGame[exitXPos, exitYPos].updateColor();

                    exitUnlocked = new TextBlock();
                    exitUnlocked.Text = "Exit Unlocked! Go to Exit!";
                    exitUnlocked.TextAlignment = TextAlignment.Center;
                    exitUnlocked.VerticalAlignment = VerticalAlignment.Center;
                    exitUnlocked.Foreground = new SolidColorBrush(Colors.Green);
                    exitUnlocked.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
                    exitUnlocked.FontSize = 36;
                    exitActivated = true;
                    timerToTurnOffExitUnlockedText = 3;
                    Game.Children.Add(exitUnlocked);

                }
                else if (gridForGame[currentXPos, currentYPos].TypeOfSpace == 0)    //If player falls in a pit
                {
                    levelSelection[currentLevelIndexX, currentLevelIndexY].Completed = false;
                    gameState = 1;
                    DisplayWinOrLose(false);
                    
                }
                

                else if (gridForGame[currentXPos, currentYPos].TypeOfSpace == 4)    //If player lands on the Activated Exit
                {
                    levelSelection[currentLevelIndexX, currentLevelIndexY].Completed = true;
                    gameState = 1;
                    DisplayWinOrLose(true);
                    
                }
                else if (gridForGame[currentXPos, currentYPos].TypeOfSpace == 6)    //If player lands on a deactivated bridge space
                {
                    levelSelection[currentLevelIndexX, currentLevelIndexY].Completed = false;
                    gameState = 1;
                    DisplayWinOrLose(false);
                }
                else if (gridForGame[currentXPos, currentYPos].TypeOfSpace == 8 && bridgeOn == false)    //If player lands on a bridge switch
                {
                    List<int> xValues = listOfBridgeCoordinates["XCoords"];     //Obtain the XCoordinates of Bridge Spaces
                    List<int> yValues = listOfBridgeCoordinates["YCoords"];     //Obtain the YCoordinates of Bridge Spaces

                    for (int x = 0; x < xValues.Count; x++)                     //For all bridge spaces, activate them
                    {
                        gridForGame[xValues[x], yValues[x]].TypeOfSpace = 7;    
                        gridForGame[xValues[x], yValues[x]].updateColor();
                    }


                    bridgeActivated = new TextBlock();
                    bridgeActivated.Text = "Bridge Triggered Somewhere";
                    bridgeActivated.TextAlignment = TextAlignment.Center;
                    bridgeActivated.VerticalAlignment = VerticalAlignment.Center;
                    bridgeActivated.Foreground = new SolidColorBrush(Colors.Red);
                    bridgeActivated.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
                    bridgeActivated.FontSize = 36;
                    bridgeOn = true;
                    timerToTurnOffBridgeText = 3;
                    Game.Children.Add(bridgeActivated);
                }
                gridForGame[currentXPos, currentYPos].updateColor();            //Update Colors of each space


                createDarknessAroundThePlayer();


                
            
            }

            else if (gameState == 5 || gameState == 6)      //If player wins or loses
            {
                if (e.Key == Key.Enter)
                {
                    gameState = 1;
                    timeAllotted = assignedTimeViaOptions;
                    myDrawTimer.Close();
                    //myDrawTimer.Elapsed += myDrawTimer_Elapsed;
                    
                    //myDrawTimer.Dispose();
                    
                    initializeGame();       //Go back to level select
                    for (int y = 0; y < 10; y++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            gridForGame[x, y] = null;
                        }
                    }
                    Game.Children.Clear();  //Clear screen so next level can be played

                }
            }
        }


        //Create light source around player
        //Checks for locations surrounding player, and as long as it is within the bounds of the array it will adjust the opacity of the surrounding spaces
        //Gives the illusion of light
        private void createDarknessAroundThePlayer()
        {
            try
            {
                gridForGame[currentXPos, currentYPos].adjustOpacityPlayer();
                if (currentXPos > 0)
                {
                    gridForGame[currentXPos - 1, currentYPos].adjustOpacityNear();
                    if (currentXPos > 1)
                    {
                        gridForGame[currentXPos - 2, currentYPos].adjustOpacityFar();
                    }
                }
                if (currentXPos < 9)
                {
                    gridForGame[currentXPos + 1, currentYPos].adjustOpacityNear();
                    if (currentXPos < 8)
                    {
                        gridForGame[currentXPos + 2, currentYPos].adjustOpacityFar();
                    }
                }
                if (currentYPos > 0)
                {
                    gridForGame[currentXPos, currentYPos - 1].adjustOpacityNear();
                    if (currentYPos > 1)
                    {
                        gridForGame[currentXPos, currentYPos - 2].adjustOpacityFar();
                    }
                }
                if (currentYPos < 9)
                {
                    gridForGame[currentXPos, currentYPos + 1].adjustOpacityNear();
                    if (currentYPos < 8)
                    {
                        gridForGame[currentXPos, currentYPos + 2].adjustOpacityFar();
                    }
                }


                if (currentXPos > 0 && currentYPos > 0)
                {
                    gridForGame[currentXPos - 1, currentYPos - 1].adjustOpacityFurther();
                    if (currentXPos > 1 && currentYPos > 1)
                    {
                        gridForGame[currentXPos - 2, currentYPos - 2].adjustOpacityFar();
                        gridForGame[currentXPos - 2, currentYPos - 1].adjustOpacityFar();
                        gridForGame[currentXPos - 1, currentYPos - 2].adjustOpacityFar();

                    }


                }
                if (currentXPos > 0 && currentYPos < 9)
                {
                    gridForGame[currentXPos - 1, currentYPos + 1].adjustOpacityFurther();
                    if (currentXPos > 1 && currentYPos < 8)
                    {
                        gridForGame[currentXPos - 2, currentYPos + 2].adjustOpacityFar();
                        gridForGame[currentXPos - 2, currentYPos + 1].adjustOpacityFar();
                        gridForGame[currentXPos - 1, currentYPos + 2].adjustOpacityFar();
                    }

                }
                if (currentXPos < 9 && currentYPos > 0)
                {
                    gridForGame[currentXPos + 1, currentYPos - 1].adjustOpacityFurther();
                    if (currentXPos < 8 && currentYPos > 1)
                    {
                        gridForGame[currentXPos + 2, currentYPos - 2].adjustOpacityFar();
                        gridForGame[currentXPos + 2, currentYPos - 1].adjustOpacityFar();
                        gridForGame[currentXPos + 1, currentYPos - 2].adjustOpacityFar();
                    }

                }
                if (currentXPos < 9 && currentYPos < 9)
                {
                    gridForGame[currentXPos + 1, currentYPos + 1].adjustOpacityFurther();
                    if (currentXPos < 8 && currentYPos < 8)
                    {
                        gridForGame[currentXPos + 2, currentYPos + 2].adjustOpacityFar();
                        gridForGame[currentXPos + 2, currentYPos + 1].adjustOpacityFar();
                        gridForGame[currentXPos + 1, currentYPos + 2].adjustOpacityFar();
                    }

                }



                foreach (GameGrid g in gridForGame)     //Anything too far from the player assign a 0 opacity i.e. black
                {
                    if (g.DistanceFromPlayer == 4)
                    {
                        g.adjustOpacityFar();
                    }
                }
            }
            catch
            {

            }
        }



        private void DisplayWinOrLose(bool win)
        {
            sp.Stop();
            sp.Open(mainAudio);
            
            sp.Play();


            exitActivated = false;
            bridgeOn = false;
            //sp.SoundLocation = System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\mainMusic_CalmTechno.wav";
            //sp = new System.Media.SoundPlayer(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Audio\mainMusic_CalmTechno.wav");
            
            //Display win screen
            if (win)
            {
                TextBlock gameWinText = new TextBlock();
                gameWinText.Text = "You Won!\nPress Enter to Continue";
                gameWinText.TextAlignment = TextAlignment.Center;
                gameWinText.VerticalAlignment = VerticalAlignment.Center;
                gameWinText.Foreground = new SolidColorBrush(Colors.Green);
                gameWinText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
                gameWinText.FontSize = 48;
                Game.Children.Add(gameWinText);
                gameState = 6;
            }

            //Display Lose Screen
            else if(win == false && timeAllotted > 0)
            {
                TextBlock gameOverText = new TextBlock();
                gameOverText.Text = "You Died\nPress Enter to Continue";
                gameOverText.TextAlignment = TextAlignment.Center;
                gameOverText.VerticalAlignment = VerticalAlignment.Center;
                gameOverText.Foreground = new SolidColorBrush(Colors.Red);
                gameOverText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
                gameOverText.FontSize = 48;
                Game.Children.Add(gameOverText);
                gameState = 5;
                gridForGame[currentXPos, currentYPos].AnimateDeath();
                
                
            }
            else if (win == false && timeAllotted == 0)
            {
                TextBlock gameOverText = new TextBlock();
                gameOverText.Text = "Time's Up!\nPress Enter to Continue";
                gameOverText.TextAlignment = TextAlignment.Center;
                gameOverText.VerticalAlignment = VerticalAlignment.Center;
                gameOverText.Foreground = new SolidColorBrush(Colors.Red);
                gameOverText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
                gameOverText.FontSize = 48;
                Game.Children.Add(gameOverText);
                gameState = 5;
                gridForGame[currentXPos, currentYPos].AnimateDeath();


            }
            
            myDrawTimer.Stop();
            
            
        }

        //If help button is clicked
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            gameState = 3;
            this.Width = 526;
            this.Height = 350;
            
            goToHelpScreen();
        }

        //Build Help Screen
        private void goToHelpScreen()
        {
            MainScreen.Visibility = Visibility.Hidden;
            LevelSelectGrid.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Hidden;
            Options.Visibility = Visibility.Visible;
            Builder.Visibility = Visibility.Hidden;

            Options.Background = new SolidColorBrush(Colors.Black);



            TextBlock helpTitle = new TextBlock();
            helpTitle.Text = "Help:";
            helpTitle.TextAlignment = TextAlignment.Center;

            helpTitle.Margin = new Thickness(0, 10, 0, 0);
            helpTitle.FontSize = 24;
            helpTitle.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");

            helpTitle.Foreground = new SolidColorBrush(Colors.White);
            Options.Children.Add(helpTitle);




            Rectangle r;

            r = new Rectangle();
            r.Width = 50;
            r.Height = 50;
            r.Fill = new SolidColorBrush(Colors.LightGray);
            r.Margin = new Thickness(-400, -100, 0, 0);

            TextBlock rectangleText = new TextBlock();
            rectangleText.Text = "= Platform";
            rectangleText.TextAlignment = TextAlignment.Left;
            rectangleText.Margin = new Thickness(100, 100, 0, 0);
            rectangleText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
            rectangleText.Foreground = new SolidColorBrush(Colors.White);
            Options.Children.Add(r);
            Options.Children.Add(rectangleText);


            
            r = new Rectangle();
            r.Width = 50;
            r.Height = 50;
            r.Fill = new SolidColorBrush(Colors.Yellow);
            r.Margin = new Thickness(-400, 50, 0, 0);

            rectangleText = new TextBlock();
            rectangleText.Text = "= Exit Unlocker";
            rectangleText.TextAlignment = TextAlignment.Left;
            rectangleText.Margin = new Thickness(100, 175, 0, 0);
            rectangleText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
            rectangleText.Foreground = new SolidColorBrush(Colors.White);
            Options.Children.Add(r);
            Options.Children.Add(rectangleText);

            r = new Rectangle();
            r.Width = 50;
            r.Height = 50;
            r.Fill = new SolidColorBrush(Colors.Red);
            r.Margin = new Thickness(-400, 200, 0, 0);

            rectangleText = new TextBlock();
            rectangleText.Text = "= Locked Exit";
            rectangleText.TextAlignment = TextAlignment.Left;
            rectangleText.Margin = new Thickness(100, 250, 0, 0);
            rectangleText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
            rectangleText.Foreground = new SolidColorBrush(Colors.White);
            Options.Children.Add(r);
            Options.Children.Add(rectangleText);


            r = new Rectangle();
            r.Width = 50;
            r.Height = 50;
            r.Fill = new SolidColorBrush(Colors.Green);
            r.Margin = new Thickness(0, -100, 0, 0);

            rectangleText = new TextBlock();
            rectangleText.Text = "= Unlocked Exit";
            rectangleText.TextAlignment = TextAlignment.Left;
            rectangleText.Margin = new Thickness(300, 100, 0, 0);
            rectangleText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
            rectangleText.Foreground = new SolidColorBrush(Colors.White);
            Options.Children.Add(r);
            Options.Children.Add(rectangleText);



            r = new Rectangle();
            r.Width = 50;
            r.Height = 50;
            r.Fill = new SolidColorBrush(Colors.Violet);
            r.Margin = new Thickness(0, 50, 0, 0);

            rectangleText = new TextBlock();
            rectangleText.Text = "= Bridge Activator";
            rectangleText.TextAlignment = TextAlignment.Left;
            rectangleText.Margin = new Thickness(300, 175, 0, 0);
            rectangleText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
            rectangleText.Foreground = new SolidColorBrush(Colors.White);
            Options.Children.Add(r);
            Options.Children.Add(rectangleText);


            rectangleText = new TextBlock();
            rectangleText.Text = "\t\t↑ W\n\n\t← A\t↓ S\t→ D";
            rectangleText.TextAlignment = TextAlignment.Left;
            rectangleText.FontSize = 18;
            rectangleText.FontWeight = FontWeights.Bold;
            rectangleText.Margin = new Thickness(200, 225, 0, 0);
            rectangleText.FontFamily = new System.Windows.Media.FontFamily("Quartz MS");
            rectangleText.Foreground = new SolidColorBrush(Colors.White);
            Options.Children.Add(rectangleText);



            Button backButton = new Button();
            backButton.Content = "Back";
            backButton.Width = 75;
            backButton.Height = 25;
            backButton.Foreground = new SolidColorBrush(Colors.Black);
            backButton.Background = new SolidColorBrush(Colors.White);
            backButton.Margin = new Thickness(-400, -250, 0, 0);
            Options.Children.Add(backButton);
            backButton.Click+=backButton_Click;
        }

        private void DisableSoundPlayer(object sender, System.ComponentModel.CancelEventArgs e)
        {
            sp.Stop();
            sp.Close();
        }

        
    }
}
