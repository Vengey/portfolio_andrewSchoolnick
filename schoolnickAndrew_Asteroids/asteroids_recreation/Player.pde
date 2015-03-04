//Player Object

class Player
{
  PShape shipGroup;
  PShape playerShip;
  color fillCol;
  
  PVector position;
  PVector velocity;
  PVector direction;
  
  
  float speedLimit;
  float speed = 0;
  
  float acceleration;
  float deceleration;
  
  //ArrayList<Bullet> bullets;
  Bullet [] bullets;
  
  int bulletNum = -1;
  
  
  ArrayList<PShape>explodeParticle;
  ArrayList<PVector>explodeParticlePositions;
  
  boolean dead = false;
  
  float deathTimer = 60;
  
  PShape flames;
  
  
  public Bullet [] bulletList()
  {
     return bullets; 
  }
  
  public PShape PlayersShip()
  {
     return playerShip; 
  }
  
  Player(float x, float y, color c)
  {
     bullets = new Bullet[4];
     position = new PVector(x, y);
     
     velocity = new PVector(0, 0);
     direction = new PVector(0, 0);
     
     PVector.fromAngle(0, direction);
     
     //Lines that show ship is destroyed
     explodeParticle = new ArrayList<PShape>();
     explodeParticlePositions = new ArrayList<PVector>();
     
     //Build the lines to be generated or shown
     for(int i = 0; i < 5; i++)
     {
        PShape particle = createShape(LINE, random(-10,-5), random(-10, 10), random(5, 10), random(-10, 10));
        particle.setStroke(true);
        particle.setStroke(fgColor);  
        explodeParticle.add(particle);
        explodeParticlePositions.add(new PVector(random(-10, 10), random(-10, 10)));
     }
     
     speedLimit = 7;
     acceleration = 0.25;
     deceleration = 0.95;
     
     fillCol = c;
     
     //bullets = new ArrayList<Bullet>();
     
     
     buildShape();
     buildBullets();
  } 
  
  //Create the Player shape and the jet flames
  void buildShape()
  {
     playerShip = createShape();
     playerShip.beginShape();
       playerShip.vertex(0,-10);
       playerShip.vertex(8, 15);
       playerShip.vertex(0, 10);
       playerShip.vertex(-8, 15);
     playerShip.endShape(CLOSE);
     
     playerShip.setStroke(true);
     playerShip.setStroke(fillCol);
     playerShip.setFill(false);
     playerShip.rotate(radians(90));
     
     //bumper = createShape(ELLIPSE);
     //shape(playerShip, xPos, yPos);
     
     flames = createShape();
     flames.beginShape();
       flames.vertex(0,-5);
       flames.vertex(5, 0);
       flames.vertex(3, 8);
       flames.vertex(0, 4);
       flames.vertex(-1, 10);
       flames.vertex(-2, 3);
       flames.vertex(-4, 6);
       flames.vertex(-5, 0);
     flames.endShape(CLOSE);
     
     flames.setStroke(true);
     flames.setStroke(fillCol);
     flames.setFill(false);
     flames.rotate(radians(90));
  }
  
  
  //Initialize the bullets to be used
  void buildBullets()
  {
     for(int x = 0; x < bullets.length; x++)
    {
       bullets[x] = new Bullet();
    } 
  }
  
  
  //Movement
  void move()
  {
      if(turningRight)
      {
        direction.rotate(radians(10));
      }
      else if(turningLeft)
      {
        direction.rotate(radians(-10));
      }
      if(moving)
      {
        speed += acceleration;
        
        displayFlames();      //Display movement flames
       //ACCELERATE AT 0.05
      }
      else
      {
         speed *= deceleration; 
      }
      
      velocity = PVector.mult(direction, speed);
      
      velocity.limit(speedLimit);
      position.add(velocity);
      
      float offset = 10;
      if(position.x < -offset)
      {
         position.x = width + offset; 
      }
      else if(position.x > width + offset)
      {
         position.x = -offset; 
      }
      if(position.y < -offset)
      {
         position.y = height + offset; 
      }
      else if(position.y > height + offset)
      {
         position.y = -offset; 
      }
  }
  
  
  //ONLY DISPLAY THE FLAMES IF NOT DEAD AND MOVING
  void displayFlames()
  {
      pushMatrix();
      translate(position.x, position.y);
      rotate(direction.heading());
      if(dead)
      {
        
      }
      else
      {
        shape(flames, -12, 0);
      }
    popMatrix();
  }
  
  
  //Shoot a bullet
  void shoot()
  {
       
       bulletNum++;
       if(bulletNum > bullets.length - 1)
       {
          bulletNum = bullets.length - 1;  
       }
       bullets[bulletNum].resetValues(position, direction);
       bullets[bulletNum].enableCollisions = true;
       bullets[bulletNum].setDir(position.get(), direction.get());
       
       
       

     
     
     //println(bulletNum + "");
  }
  
  
  //Show the ship and the bullets
  void display()
  {
    
    if(bulletNum > -1)
    {
        for(int x = 0; x < bullets.length - 1; x++)
        {
           
           //bullets[x].setDir(position,direction);
              bullets[x].move();
           bullets[x].display();
           
        } 
        if(bullets[bullets.length - 2].onScreen == false)
        {
            println("Reset");
            for(int x = 0; x < bullets.length - 1; x++)
            {
                bullets[x].resetValues(position, direction);
                //println(bullets[x].onScreen + "");
            }
            bulletNum = -1;
        }
        
    }
    
    
    
    pushMatrix();
      translate(position.x, position.y);
      rotate(direction.heading());
      if(dead)
      {
        displayDeathAnimation();
      }
      else
      {
        shape(playerShip, 0, 0);
      }
    popMatrix();
    
    
  }
  
  
  //Display death animation with the explodeParticle, decrease lives and score
  
  void displayDeathAnimation()
  {
     for(int x = 0; x < explodeParticle.size() -1; x++)
     {
         shape(explodeParticle.get(x), explodeParticlePositions.get(x).x, explodeParticlePositions.get(x).y);
     }
     deathTimer--;
     if(deathTimer < 0)
     {
         dead = false;
         deathTimer = 60;  
         playerLives--;
         score -= 2000;
         if(score < 0)
         {
            score = 0; 
         }
         if(playerLives == 0)
         {
            gameMode = gameMode.GameOver; 
         }
     }
  }
}









