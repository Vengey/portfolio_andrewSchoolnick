//Heads Up Display

class HUD
{
   ArrayList<PShape> lives;
   PShape liveShape;
   HUD(PShape livesShape)
   {
       liveShape = livesShape;
       lives = new ArrayList<PShape>();
       
       for(int x = 0; x < playerLives; x++)
       {
          lives.add(liveShape);
       }
   }  
   
   void display()
   {
      for(int x = 0; x < playerLives; x++)
      {
        pushMatrix();
         translate((x * 30) + 50, 50);
         rotate(radians(-90));
         shape(lives.get(x), 0,0); 
        popMatrix();
      }
      fill(fgColor);
      textAlign(RIGHT);
      text("Score: " + score, width - 50, 50);
      noFill();
   }
}

