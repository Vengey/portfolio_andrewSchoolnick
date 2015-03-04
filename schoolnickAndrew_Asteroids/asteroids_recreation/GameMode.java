//ENUMERATOR FOR GAME MODES In pure Java Code
public enum GameMode
{
   Menu(0),
   Options(1),
   ExitGame(2),
   Game(3),
   HighScoreInput(4),
   HighScoreDisplay(5),
   GameOver(6);
   
   
   private final int value;
   
   private GameMode(int val)
   {
      this.value = val;
   }
   
   public int getVal()
   {
      return value; 
   }
};
