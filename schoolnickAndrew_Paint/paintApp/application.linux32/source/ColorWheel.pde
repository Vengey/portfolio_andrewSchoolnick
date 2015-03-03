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
  
  void display()
  {
    colorMode(HSB, 180, 60, 60);
    stroke(hue, sat, 60);
    point(xPos, yPos);
    colorMode(RGB, 255, 255, 255); 
    noStroke();
  }
  
  
  //If mouse selects the color
  void checkIfMousePressed()
  {
      if(mouseX == xPos && mouseY == yPos)
      {
         colorMode(HSB, 180, 60, 60);
         currentColor = color(hue, sat, 60); 
          colorMode(RGB, 255, 255, 255); 
      }
  }
}
