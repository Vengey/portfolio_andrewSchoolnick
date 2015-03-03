//Main Menu GUI

class Menu
{
  float sizeIncrementOfScale;      //Incrementation of the X,Y Scale on the sides
  
  
  ArrayList<Button> listOfButtons;    //List for all buttons on screen
  
  String currentMode;                //Current tool selected (Made it a string for comprehension)
  
  
  ArrayList<HScrollbar> listOfScrollBars;    //List for all scrollbars
  
  ColorWheel[][]colWheel;                    //2D Array to display color wheel
  
  
  
  Menu(float size)
  {
    sizeIncrementOfScale = size;
    
    listOfButtons = new ArrayList<Button>();
    currentMode = "paint";
    listOfScrollBars = new ArrayList<HScrollbar>();
    
    //Create the scrollbars
    for(int i = 0; i < 5; i++)
    {
      listOfScrollBars.add(new HScrollbar(width - 235, 27, 200, 15)); 
    }
    
    //Create the color wheel using HSB Mode
    colWheel = new ColorWheel[180][60];
      for(int sat = 0; sat < 60; sat++)
      {
        for(int hue = 0; hue < 180; hue++)
        {
        //stroke(hue,sat,100);
        //point(hue,sat);
        colWheel[hue][sat] = new ColorWheel(hue, sat);
        }
      
      }
  }
  
//Public accessors to return the lists of objects back to the main program for use
 public ArrayList<Button> ListOfButtons()
 {
   return listOfButtons;
 }
 public ArrayList<HScrollbar> ListOfScrollBars()
 {
   return listOfScrollBars;
 }
 public ColorWheel[][] ColWheel()
 {
   return colWheel; 
 }


//Display the menu
 void display()
 {
    noStroke();
    rectMode(CORNER);
    
    //Heading Of Menu
    fill(backgroundCol);
    rect(0, 0, width, 60);
    //Scale Background
    fill(backgroundColBrighter);
    rect(0, 60, width, 35);
    rect(0, 60, 35, height);
    rect(width - 35, 60, 35, height);
    rect(0, height - 35, width, 35);
    rectMode(CORNER);
    strokeWeight(1);
    stroke(foregroundCol);
    
    buildScale(37, true);    //Build X Axis
    buildScale(95, false);   //Build Y Axis
    noStroke();
    
    
 } 

 
 //Horizontal/Vertical scales
 void buildScale(float startValue, boolean horizontal)
 {
     int increment = 0;
     float loopStart = startValue;
     int lessThanIntervalForLoop;
     
     //Change loop for X Axis
     if(horizontal)
     {
        lessThanIntervalForLoop = width-35;
     }
     
     //Change loop for Y Axis
     else
     {
        lessThanIntervalForLoop = height-35;
     }
     
     for(float i = loopStart; i < lessThanIntervalForLoop; i += sizeIncrementOfScale)
     {
        //Every 5 notch marks, make the 5th one longer
        if(increment == 5)
        {
          if(horizontal)
          {
            line(i, 70, i, 90);
          }
          else
          {
            line(10, i, 30, i); 
          }
          increment = 0;
        }
        else
        {
          if(horizontal)
          {
            line(i, 75, i, 90);
          }
          else
          {
            line(15, i, 30, i); 
          }
        }
        
        increment++;
     }
     
 }
 
 
 //Build the buttons
 void buildButtons()
 {
    for(int x = 0; x < 6; x++)
    {
     listOfButtons.add(new Button(70 * x + 60, 30, loadImage("icon0"+x+".png")));
     
    }
    //println(listOfButtons.size());
    
 }
 
 
 //Display the Buttons
 void displayButtons()
 {
    for(int x = 0; x < 6; x++)
    {
       listOfButtons.get(x).display();
    }
    fill(#2d5078);
    rectMode(CORNER);
    rect(35, height - 35, 250, 40);
    rect(300, height - 35, 250, 40);
    rect(width - 285, height - 35, 250, 40);
    fill(foregroundCol);
    textFont(font01);
    textSize(24);
    textAlign(CENTER);
    text("SAVE", 160, height - 8);
    text("LOAD", 425, height - 8);
    text("CLEAR", width - 160, height - 8);
    checkCurrentMode();
 }
 
 
 //Paint Brush Slider
 void paintBrushOptions()
 {
    textAlign(LEFT);
    fill(foregroundCol);
    text("Size", width - 300, 40);
    listOfScrollBars.get(0).display();
    if(mousePressed)
    {
      listOfScrollBars.get(0).changeXPos();
    }
 }
 
 //Eraser Slider
 void eraserOptions()
 {
    textAlign(LEFT);
    fill(foregroundCol);
    text("Size", width - 300, 40);
    listOfScrollBars.get(1).display();
    if(mousePressed)
    {
      listOfScrollBars.get(1).changeXPos();
    }
 }
 
 //Rectangle Slider
 void squareOptions()
 {
    textAlign(LEFT);
    fill(foregroundCol);
    text("Size", width - 300, 40);
    listOfScrollBars.get(2).display();
    if(mousePressed)
    {
      listOfScrollBars.get(2).changeXPos();
    }
 }
 
 //Ellipse Slider
 void circleOptions()
 {
    textAlign(LEFT);
    fill(foregroundCol);
    text("Size", width - 300, 40);
    listOfScrollBars.get(3).display();
    if(mousePressed)
    {
      listOfScrollBars.get(3).changeXPos();
    }
 }
 
 //Text Slider
 void textOptions()
 {
    textAlign(LEFT);
    fill(foregroundCol);
    text("Size", width - 300, 40);
    listOfScrollBars.get(4).display();
    if(mousePressed)
    {
      listOfScrollBars.get(4).changeXPos();
    }
 }
 
 
 //Color Wheel Object display
 void colorOptions()
 {
    textAlign(LEFT);
    fill(foregroundCol);
    text("Choose a Color", width - 400, 40);
    for(int hue = 0; hue < 180; hue++)
    {
      for(int sat = 0; sat < 60; sat++)
      {
        //stroke(hue,sat,100);
        //point(hue,sat);
        colWheel[hue][sat].display();
      }
      
    }
 }
 
 
 
 //Check for w hich tool is currently being used then perform the actions
 
 void checkCurrentMode()
 {
    //clear background of section of screen for these options
    fill(backgroundCol);
    rect(width-500, 0, width, 60);
    
    if(currentMode == "paint")
    {
        paintBrushOptions();
    }
    else if(currentMode == "erase")
    {
        eraserOptions();
    }
    else if(currentMode == "square")
    {
      squareOptions();
    }
    else if(currentMode == "circle")
    {
      circleOptions();
    }
    else if(currentMode == "text")
    {
      textOptions();
    }
    else if(currentMode == "color")
    {
      colorOptions();
    }
 }
}
