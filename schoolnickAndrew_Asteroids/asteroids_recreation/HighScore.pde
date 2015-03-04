//Print Highscore
class HighScore
{
  BufferedReader reader;
  String line;
  HighScore()
  {
     reader = createReader("highscores.txt");
  } 
  
  void displayScores()
  {
     try 
     {
       if(line!= null)
       {
        line = reader.readLine();
       }
     } 
     catch (IOException e) 
     {
        e.printStackTrace();
        line = null;
     }
     if (line == null) 
     {
       // Stop reading because of an error or file is empty
       //noLoop();  
     } 
     else 
     {
       String[] pieces = split(line, ',');
       
       //point(x, y);
       println(pieces[0] + "\n" + pieces[1]);
    } 
  }
  
  
}
