/*
Andrew Schoolnick
Interactive Media Development
Homework 1 Painting Application
*/


//List of globally accessible colors//
color backgroundCol;
color foregroundCol;
color backgroundColBrighter;
color highlightedCol;
color currentColor;


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







void setup()
{
  //This program can be resized to any resolution and still work//
  size(1024,768);
  frameRate(30);
  background(#e9e9e9);
  noStroke();
  
  //Assign the global colors//
  backgroundCol = color(#2d5078);
  foregroundCol = color(#e9e9e9);
  backgroundColBrighter = color(#4273a9);
  highlightedCol = color(#961227);
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



void draw()
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

void mouseDragged()
{
  
  //If mouse is dragged do stuff
  dragMouseDrawing();
  

}

void mousePressed()
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
       else if(mouseX >= 0 && mouseX <= sizeOfButton * buttons.size() * 1.75 && mouseY >= 0 && mouseY <= sizeOfButton + 20)
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
void fileSelected(File selection) 
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
void saveSelected(File selection) 
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
void displayCurrentColor()
{
   colorMode(HSB, 180, 60, 60);
   fill(currentColor);
   rectMode(CENTER);
   rect(480, 30, 45, 45);
   colorMode(RGB, 255, 255, 255);
   rectMode(CORNER);
}


//FSM For changing current tool mode
void FSMmenuOptions(int x)
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
void checkForMouse()
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
void dragMouseDrawing()
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

void awaitTextInput()
{
   colorMode(HSB, 180, 60, 60);
   fill(currentColor);
   line(xLocOfTextInput, yLocOfTextInput - (scrollBars.get(4).percentageOfBarDraggedTo / 2), xLocOfTextInput, yLocOfTextInput + (scrollBars.get(4).percentageOfBarDraggedTo / 2));
   
}


//Add text to the text tool
void keyReleased()
{
   if(currentTool == 4)
   {
     if(keyCode != ENTER)
     {
     String finalText = "";
     textInput.add(char(key));
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

