

class Bullet
{
  PVector position;
  PVector velocity;
  PVector direction;
  
  float size;
  
  float speed;
  
  color fillCol;
  
  PShape bullet;
  
  boolean onScreen;
  boolean enableCollisions;
  
  int travelTime = 0;
  
  
  
  Bullet(/*PVector loc*/)
  {
     position = new PVector(playerPositionGlobal.get().x, playerPositionGlobal.get().y);
     direction = new PVector(0, 0);
     velocity = new PVector(0,0);
     
     fillCol = fgColor;
     speed = 20;
     size = 5;
     
     bullet = createShape(ELLIPSE, -size/2, -size/2, size, size);
     //bullet.setfill(fillCol);
     
     

     onScreen = true;
     enableCollisions = false;
  } 
  
  void setDir(PVector loc, PVector dir)
  {
      position = loc.get();
      direction = dir.get();
  }
  
  //Once the bullet has reached a certain point reset it back to player so it can be reused
  void resetValues(PVector playerPos, PVector playerDir)
  {
     onScreen = true;
     travelTime = 0;
     speed = 20; 
     position = playerPos.get();
     direction = playerDir.get();
     velocity = new PVector(0,0);
     enableCollisions = false;
     

  }
  
  //Move and wrap bullet
  void move()
  {
     
     velocity = PVector.mult(direction, speed);
     position.add(velocity);
     
     if(position.x < 0)
     {
         position.x = width;
     }
     else if(position.x > width)
     {
         position.x = 0; 
     }
     if(position.y < 0)
     {
         position.y = height;
     }
     else if(position.y > height)
     {
         position.y = 0; 
     }
     travelTime++;
     
     if(travelTime > 20)
     {
         speed *= 0.97;
     }
     if(travelTime > 50)
     {
         onScreen = false; 
         
         
     }
     
     
  }
  
  void display()
  {
      
     if(onScreen)
     {
       //enableCollisions = true;
       shape(bullet, position.x, position.y);
     }
  }
}
