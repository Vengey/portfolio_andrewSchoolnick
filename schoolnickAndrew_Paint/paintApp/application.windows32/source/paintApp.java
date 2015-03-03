import processing.core.*; 
import processing.data.*; 
import processing.event.*; 
import processing.opengl.*; 

import java.util.HashMap; 
import java.util.ArrayList; 
import java.io.File; 
import java.io.BufferedReader; 
import java.io.PrintWriter; 
import java.io.InputStream; 
import java.io.OutputStream; 
import java.io.IOException; 

public class paintApp extends PApplet {

/*
Andrew Schoolnick
Interactive Media Development
Homework 1 Painting Application
*/


//List of globally accessible colors//
int backgroundCol;
int foregroundCol;
int backgroundColBrighter;
int highlightedCol;
int currentColor;


//Menu Object//
Menu menuDisplay;


//Reference pointers to arraylists in menu//
ArrayList<Button> buttons;
ArrayList<HScrollbar> scrollBars;


//Current Tool being used//
int currentTool = 0;


//Custom Font//
PFont font01 = createFont("UbuntuCondensed-Regular", 14, true);



//ArrayList for obtaining text input for text tool//
ArrayList textInput;
boolean inputtingString = false;
float xLocOfTextInput = 0;
float yLocOfTextInput = 0;


//Color Wheel object//
ColorWheel[][] colorWheel;


//Used in the try and catch statement as an incrementer in case user messes up twice in saving//
int imageNumberSaved = 0;

//Absolute URL of saved file and the image to be loaded//
String locationOfLoadedImage;
PImage loadedImage;







public void setup()
{
  //This program can be resized to any resolution and still work//
  size(1024,768);
  frameRate(30);
  background(0xffe9e9e9);
  noStroke();
  
  //Assign the global colors//
  backgroundCol = color(0xff2d5078);
  foregroundCol = color(0xffe9e9e9);
  backgroundColBrighter = color(0xff4273a9);
  highlightedCol = color(0xff961227);
  currentColor = color(0);
  
  //Build the menu and pass in a parameter for the spacing of the horizontal and vertical scales
  menuDisplay = new Menu(5);
  
  menuDisplay.display();                  //Display the Menu
  menuDisplay.buildButtons();             //Build the menu buttons
  
  buttons = menuDisplay.ListOfButtons();  //Point the buttons arrayList to the arrayList within Menu
  buttons.get(0).activate();              //Set paintbrush as currently activated tool
  
  scrollBars = menuDisplay.ListOfScrollBars();  //Point the scrollBars arrayList to the arrayList in menu
  
  textInput = new ArrayList();   //Create an arrayList for text input
  
  colorWheel = menuDisplay.ColWheel();    //Points to the color wheel array in menu
  
  
  
}



public void draw()
{
   //Display the Menu and Buttons//
   menuDisplay.display();
   menuDisplay.displayButtons();
   
   
   //Allow for text to be input if using text tool//
   if(inputtingString)
   {
      awaitTextInput(); 
   }
   
   //Build the box that displays the current color
   displayCurrentColor();
   

  //Check where the mouse currently is for UI elements
  checkForMouse();
}

public void mouseDragged()
{
  
  //If mouse is dragged do stuff
  dragMouseDrawing();
  

}

public void mousePressed()
{
    
    //If you click a button in menu
    for(int x = 0; x < buttons.size(); x++)
    {
      float xPosition = buttons.get(x).xPos;
      float yPosition = buttons.get(x).yPos;
      float sizeOfButton = buttons.get(x).size;
      
      //Check which button was clicked
       if(mouseX >= xPosition - (sizeOfButton/2) && mouseX <= xPosition + (sizeOfButton/2) && mouseY >= yPosition - (sizeOfButton/2) && mouseY <= yPosition + (sizeOfButton/2))
       {
           println("Clicked");
           buttons.get(x).activate();    //Activate the button that was clicked
           FSMmenuOptions(x);            //Assign the current tool
       }
       //If the button was not clicked keep it inactive
       else if(mouseX >= 0 && mouseX <= sizeOfButton * buttons.size() * 1.75f && mouseY >= 0 && mouseY <= sizeOfButton + 20)
       {
          buttons.get(x).deactivate();
       }
       else
       {
         
       }
    }
   //If you click within the canvas
   if(mouseX >= 35 && mouseX <= width - 35 && mouseY >= 95 && mouseY <= height-35)
   {
    noStroke();
    
    //Create Rectangle Tool
    if(currentTool == 2)
     {
        colorMode(HSB, 180, 60, 60);
        fill(currentColor);
        rectMode(CENTER);
        rect(mouseX, mouseY, scrollBars.get(2).percentageOfBarDraggedTo, scrollBars.get(2).percentageOfBarDraggedTo);
        rectMode(CORNER);
        colorMode(RGB, 255, 255, 255);
     } 
    //Create Ellipse Tool
     else if(currentTool == 3)
     {
       colorMode(HSB, 180, 60, 60);
       fill(currentColor);
       ellipseMode(CENTER);
       ellipse(mouseX, mouseY, scrollBars.get(3).percentageOfBarDraggedTo, scrollBars.get(3).percentageOfBarDraggedTo);
       colorMode(RGB, 255, 255, 255);
     }
     
     //Create Text Tool
     else if(currentTool == 4)
     {
         inputtingString = true;
         xLocOfTextInput = mouseX;
         yLocOfTextInput = mouseY;
         textInput = new ArrayList();
     }
   }
   
   //If you click on the color wheel and it is currently showing
   else if(mouseX >= width - 215 && mouseX <= width - 35 && mouseY >= 0 && mouseY <= 60)
   {
     if(currentTool == 5)
     {
        for(int x = 0; x < 180; x++)
        {
          for(int y = 0; y < 60; y++)
          {
           colorWheel[x][y].checkIfMousePressed(); 
          }
        }
     } 
   }
    
   //SAVE//
   else if(mouseX >= 35 && mouseX <= 285 && mouseY >= height - 35 && mouseY <= height)
   {
       selectOutput("Select Where to Save to", "saveSelected");  //Save Prompt
       
       //PImage subImage = get(35,95, width - 75, height - 130);      ////////////////SET AREA ON SCREEN TO TAKE PICTURE//////////////////
       //subImage.save("images/image" + imageNumberSaved + ".png");
       //print("SAVED");
       //imageNumberSaved++;
   }
   //LOAD//
   else if(mouseX >= 300 && mouseX <= 550 && mouseY >= height - 35 && mouseY <= height)
   {
      selectInput("Select a File to Open", "fileSelected");      //Load Prompt
   }
   
   //CLEAR//
   else if(mouseX >= width-285 && mouseX <= width - 35 && mouseY >= height - 35 && mouseY <= height)
   {
      background(foregroundCol);
      
      //Reset the active tool to the paint brush
      for(Button b : buttons)
      {
         b.deactivate(); 
      }
      buttons.get(0).activate();
      currentTool = 0;
      inputtingString = false;
      menuDisplay.currentMode = "paint";
   }
    
    
}

//LOADING//
public void fileSelected(File selection) 
{
  if (selection != null) 
  {
    println("File Chosen " + selection.getAbsolutePath());
    locationOfLoadedImage = selection.getAbsolutePath();
    loadedImage = loadImage(locationOfLoadedImage);
    imageMode(CORNERS);
    image(loadedImage, 35, 95, width - 35, height - 35);
    imageMode(CENTER);
  } 
  else 
  {
    println("No File Selected");
  }
}

//SAVING
public void saveSelected(File selection) 
{
  if (selection != null) 
  {
    String finalFileName;
    println("File Path " + selection.getAbsolutePath());
    String locationOfSavedImage = selection.getAbsolutePath();
    PImage subImage = get(35,95, width - 75, height - 130);  //Set area on screen to take picture
    finalFileName = locationOfSavedImage;
    String[] fileName;
    
    try
    {
    //Redundant, BUT it checks if the user accidentally put more than one '.'
    //and only saves first part of string
    if(locationOfSavedImage.contains(".png") || locationOfSavedImage.contains(".pn") || locationOfSavedImage.contains(".p"))
    {
       fileName =  split(locationOfSavedImage, ".");
       finalFileName = fileName[0] + ".png";
    }
    else if(locationOfSavedImage.contains(".jpg") || locationOfSavedImage.contains(".jpeg") || locationOfSavedImage.contains(".jpe") || locationOfSavedImage.contains(".jp") || locationOfSavedImage.contains(".j"))
    {
       fileName =  split(locationOfSavedImage, ".");
       //println(fileName);
       finalFileName = fileName[0] + ".jpg";
    }
    else if(locationOfSavedImage.contains(".gif") || locationOfSavedImage.contains(".gi") || locationOfSavedImage.contains(".g"))
    {
       fileName =  split(locationOfSavedImage, ".");
       finalFileName = fileName[0] + ".gif";
    }
    else if(locationOfSavedImage.contains(".tif") || locationOfSavedImage.contains(".tiff") || locationOfSavedImage.contains(".ti") || locationOfSavedImage.contains(".t"))
    {
       fileName =  split(locationOfSavedImage, ".");
       finalFileName = fileName[0] + ".tif";
    }
    else
    {
        fileName =  split(locationOfSavedImage, ".");
        finalFileName = fileName[0] + ".png";
    }
    }
    catch(Exception e)
    {
       finalFileName =  "image" + imageNumberSaved + ".png";
       imageNumberSaved++;
    }
    println(finalFileName);
    subImage.save(finalFileName);
    println("SAVED");
  } 
  else 
  {
    println("File Wasn't Saved");
  }
}


//Rectangle Display Box for Color
public void displayCurrentColor()
{
   colorMode(HSB, 180, 60, 60);
   fill(currentColor);
   rectMode(CENTER);
   rect(480, 30, 45, 45);
   colorMode(RGB, 255, 255, 255);
   rectMode(CORNER);
}


//FSM For changing current tool mode
public void FSMmenuOptions(int x)
{
    if(x == 0)
    {
        currentTool = 0;
        menuDisplay.currentMode = "paint";
    }
    else if(x == 1)
    {
        currentTool = 1;
        menuDisplay.currentMode = "erase";
    }
    else if(x == 2)
    {
        currentTool = 2;
        menuDisplay.currentMode = "square";
    }
    else if(x == 3)
    {
        currentTool = 3;
        menuDisplay.currentMode = "circle";
    }
    else if(x == 4)
    {
        currentTool = 4;
        menuDisplay.currentMode = "text";
    }
    else if(x == 5)
    {
        currentTool = 5;
        menuDisplay.currentMode = "color";
    }
    
}



//Check where mouse is located
public void checkForMouse()
{
   //On Canvas
   if(mouseX >= 35 && mouseX <= width - 35 && mouseY >= 95 && mouseY <= height-35)
   {
      
      cursor(CROSS);
      stroke(50,50,50,50);
      strokeWeight(1);
      /*
        line(mouseX, mouseY, mouseX, 95);
        line(mouseX, mouseY, mouseX, 708);
        line(mouseX, mouseY, 35, mouseY);
        line(mouseX, mouseY, 988, mouseY);
        */
   }
   //On Save//
   else if(mouseX >= 35 && mouseX <= 285 && mouseY >= height - 35 && mouseY <= height)
   {
      cursor(HAND); 
   }
   //On Load//
   else if(mouseX >= 300 && mouseX <= 550 && mouseY >= height - 35 && mouseY <= height)
   {
      cursor(HAND); 
   }
   //On Clear//
   else if(mouseX >= width-285 && mouseX <= width - 35 && mouseY >= height - 35 && mouseY <= height)
   {
      cursor(HAND); 
   }
   //All else//
   else
   {
      cursor(ARROW); 
   }
   
   //On a button
   for(Button b : buttons)
   {
      float xPosition = b.xPos;
      float yPosition = b.yPos;
      float sizeOfButton = b.size;
      if(mouseX >= xPosition - (sizeOfButton/2) && mouseX <= xPosition + (sizeOfButton/2) && mouseY >= yPosition - (sizeOfButton/2) && mouseY <= yPosition + (sizeOfButton/2))
       {
          cursor(HAND); 
          b.hovering();
       }
   }
   
   
}


//When mouse is dragged on canvas
public void dragMouseDrawing()
{
   
  if(mouseX >= 35 && mouseX <= width - 35 && mouseY >= 95 && mouseY <= height-35)
  {
     //Paint Brush and Eraser
    if(currentTool < 2)
    {
      if(currentTool == 0)
      {
        colorMode(HSB, 180, 60, 60);
        stroke(currentColor);
        strokeWeight(scrollBars.get(0).percentageOfBarDraggedTo);
        colorMode(RGB, 255, 255, 255);
      }
      else if(currentTool == 1)
      {
        stroke(foregroundCol);
        strokeWeight(scrollBars.get(1).percentageOfBarDraggedTo);
      }
      
      line(pmouseX, pmouseY, mouseX, mouseY); 
      noStroke();
    }
    //Rectangle and Ellipse
    else
    {
      noStroke();
     if(currentTool == 2)
     {
        colorMode(HSB, 180, 60, 60);
        fill(currentColor);
        rectMode(CENTER);
        rect(mouseX, mouseY, scrollBars.get(2).percentageOfBarDraggedTo, scrollBars.get(2).percentageOfBarDraggedTo);
        rectMode(CORNER);
        colorMode(RGB, 255, 255, 255);
     } 
     else if(currentTool == 3)
     {
       colorMode(HSB, 180, 60, 60);
       fill(currentColor);
       ellipseMode(CENTER);
       ellipse(mouseX, mouseY, scrollBars.get(3).percentageOfBarDraggedTo, scrollBars.get(3).percentageOfBarDraggedTo);
       colorMode(RGB, 255, 255, 255);
     } 
    }
  }
  //If you click on the color wheel and it is currently showing
   else if(mouseX >= width - 215 && mouseX <= width - 35 && mouseY >= 0 && mouseY <= 60)
   {
     if(currentTool == 5)
     {
        for(int x = 0; x < 180; x++)
        {
          for(int y = 0; y < 60; y++)
          {
           colorWheel[x][y].checkIfMousePressed(); 
          }
        }
     } 
   }
}

public void awaitTextInput()
{
   colorMode(HSB, 180, 60, 60);
   fill(currentColor);
   line(xLocOfTextInput, yLocOfTextInput - (scrollBars.get(4).percentageOfBarDraggedTo / 2), xLocOfTextInput, yLocOfTextInput + (scrollBars.get(4).percentageOfBarDraggedTo / 2));
   
}


//Add text to the text tool
public void keyReleased()
{
   if(currentTool == 4)
   {
     if(keyCode != ENTER)
     {
     String finalText = "";
     textInput.add(PApplet.parseChar(key));
     for(int x = 0; x < textInput.size(); x++)
     {
       finalText += textInput.get(x);
     }
     textSize(scrollBars.get(4).percentageOfBarDraggedTo);
     text(finalText, xLocOfTextInput, yLocOfTextInput);
     }
     else
     {
        inputtingString = false; 
        colorMode(RGB, 255, 255, 255);
     }
   }
}

class Button
{
 float xPos;
 float yPos;
 PImage icon;
 float size;
 boolean clicked;
 Button(float x, float y, PImage img)
 {
  xPos = x;
  yPos = y;
  icon = img;
  size = 45;
  clicked = false;
 } 
 
 public void display()
 {
    
    rectMode(CENTER);
    imageMode(CENTER);
    
    if(clicked)
    {
       tint(100, 0, 0);
       fill(0xff961227);

    }
    else
    {
       noTint(); 
       fill(foregroundCol);
    }
    rect(xPos, yPos, size, size);
    image(icon, xPos, yPos);
 }
 
 //Make it selected
 public void activate()
 {
     clicked = true;
     display();
 }
 //Deselect it
 public void deactivate()
 {
     clicked = false; 
     display();
 }
 
 //Add hover effect
 public void hovering()
  {
     fill(foregroundCol, 150);
     rectMode(CENTER);
     rect(xPos, yPos, size, size); 
  }
  
  
 
}


//Each part of the wheel is its own pixel with its own color value
class ColorWheel
{
  float xPos;
  float yPos;
  float hue;
  float sat;
  ColorWheel(float x, float y)
  {
     hue = x;
     sat = y;
     xPos = width - 215 + x;
     yPos = y;
  } 
  
  public void display()
  {
    colorMode(HSB, 180, 60, 60);
    stroke(hue, sat, 60);
    point(xPos, yPos);
    colorMode(RGB, 255, 255, 255); 
    noStroke();
  }
  
  
  //If mouse selects the color
  public void checkIfMousePressed()
  {
      if(mouseX == xPos && mouseY == yPos)
      {
         colorMode(HSB, 180, 60, 60);
         currentColor = color(hue, sat, 60); 
          colorMode(RGB, 255, 255, 255); 
      }
  }
}
//Scrollbars
class HScrollbar
{
  float xPos;
  float yPos;
  float widthOfBar;
  float heightOfBar;
  
  float xPosOfSlider;  //CurrentPosition of Slider
  
  float leftLim;    //Left of the scrollBar
  float rightLim;   //Right of the scrollBar
  
  int percentageOfBarDraggedTo;    //What value to return from the slider
  
  boolean enabled;      //Whether or not this tools scrollbar is active
  
  HScrollbar(float x, float y, float w, float h)
  {
     xPos = x;
     yPos = y;
     xPosOfSlider = xPos;
     widthOfBar = w;
     heightOfBar = h;
     enabled = false;
     
     leftLim = xPos;
     rightLim = xPos + widthOfBar;
  } 
  
  
  public void display()
  {
     fill(foregroundCol);
     rectMode(CORNER);
     rect(xPos, yPos, widthOfBar, heightOfBar);
     
     fill(backgroundColBrighter);
     rect(xPosOfSlider, yPos, widthOfBar / 20, heightOfBar);
     enabled = true;
  }
  
  //Move Slider if mouse is dragging on it and change the value to return
  public void changeXPos()
  {
    if(enabled)
    {
     if(mouseX >= xPos && mouseX <= xPos + widthOfBar && mouseY >= yPos && mouseY <= yPos + heightOfBar)
     {
       xPosOfSlider = mouseX - (widthOfBar / 40);
       if(xPosOfSlider < leftLim)
       {
          xPosOfSlider = leftLim; 
       }
       else if(xPosOfSlider + (widthOfBar / 20) > rightLim)
       {
          xPosOfSlider = rightLim - (widthOfBar / 20);
       }
     }
    }
    
    percentageOfBarDraggedTo = PApplet.parseInt(((xPosOfSlider + (widthOfBar / 40) - xPos) / (xPos + widthOfBar)) * 400);
    //println(percentageOfBarDraggedTo);
  }
  
}
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
 public void display()
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
 public void buildScale(float startValue, boolean horizontal)
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
 public void buildButtons()
 {
    for(int x = 0; x < 6; x++)
    {
     listOfButtons.add(new Button(70 * x + 60, 30, loadImage("icon0"+x+".png")));
     
    }
    //println(listOfButtons.size());
    
 }
 
 
 //Display the Buttons
 public void displayButtons()
 {
    for(int x = 0; x < 6; x++)
    {
       listOfButtons.get(x).display();
    }
    fill(0xff2d5078);
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
 public void paintBrushOptions()
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
 public void eraserOptions()
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
 public void squareOptions()
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
 public void circleOptions()
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
 public void textOptions()
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
 public void colorOptions()
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
 
 public void checkCurrentMode()
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
  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "paintApp" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
