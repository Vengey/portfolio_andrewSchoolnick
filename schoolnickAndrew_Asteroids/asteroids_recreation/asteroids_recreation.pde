import java.io.BufferedWriter;
import java.io.FileWriter;

/*
  Andrew Schoolnick
  Interactive Media Development
  Asteroids Program
  2/26/15
  
  NOTES
    There have been some issues with collision detection on the first instance of the asteroid lists and newly instantiated asteroids after
    the first set have been initialized that I could not figure out for the life of me.
    
    I left the collision circles in the game with a blackish stroke to show Im not crazy and that collisions ARE happening
*/


//GameMode Enum
GameMode gameMode;


//Screens for various game modes
Menu menuScreen;
Options optionScreen;
Game gameScreen;
HighScore highScoreScreen;
GameOver gameOverScreen;




PFont font01;

color bgColor;
color fgColor;
float fgColorValue;



float playerLives = 3;




boolean turningLeft = false;
boolean turningRight = false;
boolean moving = false;


int score = 0;    //Player's score



PVector playerPositionGlobal;  //Global player position just in case because of some issues I had


float timeToAddAsteroids = 50;  //How long before new asteroids will be instantiated






void setup()
{
   size(1024, 768, P2D);
   background(25);
   frameRate(30);
   
   font01 = createFont("OpenSans-CondLight.ttf", 64, true);
   
   
   playerPositionGlobal = new PVector(0,0);
   
   
   
   noStroke();
   bgColor = color(25);
   fgColor = color(235);
   fgColorValue = 235;
   
   
   gameMode = gameMode.Menu;
   
   menuScreen = new Menu();
   optionScreen = new Options();
   gameScreen = new Game();
   highScoreScreen = new HighScore();
   gameOverScreen = new GameOver();
}
void draw()
{
    //Begin to choose game mode
    //println(gameMode);
    GameModeSelection();
}


//Based on the Enum
void GameModeSelection()
{
  if(gameMode == gameMode.Menu)
  {
      menuScreen.displayMenu();
  }
  
  else if(gameMode == gameMode.Options)
  {
      menuScreen.displayBackground();
      optionScreen.display();
  }
  
  else if(gameMode == gameMode.ExitGame)
  {
      menuScreen.displayBackground();
  }
  
  else if(gameMode == gameMode.Game)
  {
      menuScreen.displayBackground();
      gameScreen.showGame();
      
      
      
  }
  else if(gameMode == gameMode.HighScoreInput)
  {
      menuScreen.displayBackground();
      gameOverScreen.inputName();
      gameOverScreen.writeInTheScore();
  }
  else if(gameMode == gameMode.HighScoreDisplay)
  {
      menuScreen.displayBackground();
      highScoreScreen.displayScores();
      
  }
  else if(gameMode == gameMode.GameOver)
  {
      menuScreen.displayBackground();
      //gameOverScreen.writeInTheScore();
      gameOverScreen.display();
  }
  else
  {
    
  }
}


void keyPressed()
{
  //If playing game
  if(gameMode == gameMode.Game)
  {
    if(keyCode == RIGHT || key == 'd' || key == 'D')
    {
       turningRight = true;
    }
    else if(keyCode == LEFT || key == 'a' || key == 'A')
    {
       turningLeft = true;
    }
      
    if(keyCode == UP || key == 'w' || key == 'W')
    {
       moving = true;
    }
    
    
  }
}

void keyReleased()
{
  //if playing game
  if(gameMode == gameMode.Game)
  {
    if(keyCode == RIGHT || key == 'd' || key == 'D')
    {
       turningRight = false;
    }
    else if(keyCode == LEFT || key == 'a' || key == 'A')
    {
       turningLeft = false;
    }
    if(keyCode == UP || key == 'w' || key == 'W')
    {
       moving = false;
    } 
    if(key == ' ')
    {
       gameScreen.havePlayerShoot(); 
    }
    
  }
  
  //If game over
  else if(gameMode == gameMode.GameOver)
  {
    if(keyCode == ENTER)
    {
       gameMode = gameMode.HighScoreInput;
    } 
  }
  else if(gameMode == gameMode.HighScoreInput)
  {
    if(keyCode == ENTER)
    {
       gameMode = gameMode.HighScoreDisplay;
    } 
  }
}


void mousePressed()
{
    menuScreen.checkForMouseClick();
}





