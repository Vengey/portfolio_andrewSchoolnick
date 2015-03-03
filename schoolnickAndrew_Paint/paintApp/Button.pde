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
 
 void display()
 {
    
    rectMode(CENTER);
    imageMode(CENTER);
    
    if(clicked)
    {
       tint(100, 0, 0);
       fill(#961227);

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
 void activate()
 {
     clicked = true;
     display();
 }
 //Deselect it
 void deactivate()
 {
     clicked = false; 
     display();
 }
 
 //Add hover effect
 void hovering()
  {
     fill(foregroundCol, 150);
     rectMode(CENTER);
     rect(xPos, yPos, size, size); 
  }
  
  
 
}


