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
  
  
  void display()
  {
     fill(foregroundCol);
     rectMode(CORNER);
     rect(xPos, yPos, widthOfBar, heightOfBar);
     
     fill(backgroundColBrighter);
     rect(xPosOfSlider, yPos, widthOfBar / 20, heightOfBar);
     enabled = true;
  }
  
  //Move Slider if mouse is dragging on it and change the value to return
  void changeXPos()
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
    
    percentageOfBarDraggedTo = int(((xPosOfSlider + (widthOfBar / 40) - xPos) / (xPos + widthOfBar)) * 400);
    //println(percentageOfBarDraggedTo);
  }
  
}
