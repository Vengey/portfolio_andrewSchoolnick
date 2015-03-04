//Game Over screen
class GameOver
{
   //PrintWriter output;
   String outputFile = "highscores.txt";
   boolean writtenIn = false;
   String initials;
   GameOver()
   {
       
   }
   void display()
   {
       fill(fgColor);
       textAlign(CENTER);
       text("Game Over", width/2, height/2);
       text("Your Score: " + score, width/2, height/2 + 100);
       textAlign(LEFT);
       noFill();
   }
   
   //Write the score in
   void writeInTheScore()
   {
      println("Write");
      if(writtenIn)
      {
        
      }
      else
      {
        writeInScore(outputFile, "\n" + initials + "," + score); 
        writtenIn = true;
      }
   }
   

//Use the File IO to append the score and initials to the text document named highscores.txt
void writeInScore(String nameOfFile, String text)
{
  File fileToWriteTo = new File(dataPath(nameOfFile));
  if(!fileToWriteTo.exists())
  {
    createFile(fileToWriteTo);
  }
  try 
  {
    PrintWriter output = new PrintWriter(new BufferedWriter(new FileWriter(fileToWriteTo, true)));
    output.println(text);
    output.close();
  }
  catch (IOException e)
  {
      e.printStackTrace();
  }
}

/**
 * Creates a new file including all subfolders
 */
void createFile(File fileToWriteTo)
{
  File parentDir = fileToWriteTo.getParentFile();
  try
  {
    parentDir.mkdirs(); 
    fileToWriteTo.createNewFile();
  }
  catch(Exception e)
  {
    e.printStackTrace();
  }
} 


void inputName()
{
      initials = "AMS";
}
}
