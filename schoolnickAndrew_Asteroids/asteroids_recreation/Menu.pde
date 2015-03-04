//Main Menu layout

class Menu
{
  ArrayList<MenuButtons> buttons;
  MenuBackground menuBG;
  
  
  Menu()
  {
       buttons = new ArrayList<MenuButtons>();
       
       menuBG = new MenuBackground();
       
       buildButtons();
       
  }
  
  void displayMenu()
  {
       background(bgColor);
       
       menuBG.displayBG2();
       displayButtons(); 
  }
  
  void displayBackground()
  {
      background(bgColor);
      menuBG.displayBG1();
  } 
 
  
  
  void displayButtons()
  {
    for(MenuButtons b : buttons)
    {
       if(mouseX >= b.xPos && mouseX <= b.xPos + b.widthOfTextBox && mouseY >= b.yPos - (b.heightOfTextBox / 1.75) && mouseY <= b.yPos)
       {
          b.hovering(true); 
       }
       else
       {
          b.hovering(false); 
       }
       b.displayButton(); 
    }
  }
  
  void checkForMouseClick()
  {
    for(MenuButtons b : buttons)
    {
        if(b.hovering)
        {
          if(b.nameOfButton == "Start Game")
          {
             gameMode = gameMode.Game; 
          }
          else if(b.nameOfButton == "Options")
          {
             gameMode = gameMode.Options; 
          }
          else if(b.nameOfButton == "Exit Game")
          {
             gameMode = gameMode.ExitGame; 
          }
        }
        else
        {
            
        }
    }
  }
  
  
  
  
  void buildButtons()
  {
     for(int y = 0; y < 4; y++)
       {
           String nameButton;
           if(y == 0)
           {
             nameButton = "Processing Asteroids";
             buttons.add(new MenuButtons(50, height/2 - 50, nameButton, 64));
           }
           else
           {
             if(y == 1)
             {
               nameButton = "Start Game";
             }
             else if(y == 2)
             {
               nameButton = "Options";
             }
             else if(y == 3)
             {
               nameButton = "Exit Game";
             }
             else
             {
               nameButton = "Null";
             }
           
           buttons.add(new MenuButtons(75, (y * 75) + (height/2), nameButton, 48));
           }
       } 
  }
  
}








//BUTTONS ON THE MENU
class MenuButtons
{
  float xPos;
  float yPos;
  
  float widthOfTextBox;
  float heightOfTextBox;
  
  String nameOfButton;
  
  color fillCol;
  
  float fontSize;
  
  PShape underLine;
  
  boolean hovering;
  
  MenuButtons(float x, float y, String name, float sizeText)
  {
      xPos = x;
      yPos = y;
      
      fillCol = fgColor;
      nameOfButton = name;
      
      fontSize = sizeText;
      widthOfTextBox = textWidth(nameOfButton) * 2.5;
      heightOfTextBox = 50;
      
      underLine = createShape(RECT, 5, 0, widthOfTextBox, 2);
      underLine.setFill(fgColor);
      underLine.setStroke(false);
  }
  
  void displayButton()
  {
     textFont(font01);
     textSize(fontSize);
     fill(fillCol);
     
     text(nameOfButton + "", xPos, yPos);
     
     if(nameOfButton != "Processing Asteroids")
     {
       shape(underLine, xPos, yPos + 5);
     }
  }
  
  void hovering(boolean hover)
  {
    hovering = hover;
    //ONLY HOVER ON BUTTONS NOT TITLE
    if(nameOfButton != "Processing Asteroids")
     {
      if(hover)
      {
          underLine.setFill(fgColor);
      }
      else
      {
          underLine.setFill(color(bgColor));
      }
     }
  }
  
}







//Reusable Background displaying the stars
class MenuBackground
{
  ArrayList<Stars> starList;
  MenuBackground()
  {
      starList = new ArrayList<Stars>();
      for(int x = 0; x < 50; x++)
      {
         starList.add(new Stars()); 
      }
  }
  
  //Have two separate BGs setup in case I feel like changing the menu background to be different from the game's in the future
  void displayBG1()
  {
    for(Stars s : starList)
    {
       s.move();
       s.display();
    } 
  }
  
  
  void displayBG2()
  {
    for(Stars s : starList)
    {
       s.move();
       s.display();
    }
  }
}







class Stars
{
  PVector position;
  PVector velocity;
  float size;
  PShape star;
  
  float alpha;
  Stars()
  {
     position = new PVector(random(0,width), random(0,height));
     velocity = new PVector(0,random(1,6));
     size = random(0.5, 3);
     
     alpha = random(150, 255);
     
     star = createShape(ELLIPSE, 0, 0, size, size);
     
  } 
  
  void move()
  {
      position.add(velocity);
      alpha += random(-5, 5);
      if(alpha < 150)
      {
         alpha = 150; 
      }
      else if(alpha > 255)
      {
         alpha = 255; 
      }
      star.setFill(color(fgColorValue, alpha));
      
      if(position.y > height)
      {
         position.y = 0; 
         position.x = random(0, width);
      }
  }
  void display()
  {
      shape(star, position.x, position.y);
  }
}
