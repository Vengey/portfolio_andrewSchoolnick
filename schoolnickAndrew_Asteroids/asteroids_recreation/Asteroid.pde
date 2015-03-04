class Asteroid
{
  //Initialize vectors//
  PVector position;
  PVector direction;
  PVector velocity;
  PVector playerPosition;
  
  
  float speed;
  
  
  int whichAsteroidToStart;    //Determines whether to start as 3x size, 2x size, or 1x size
  
  
  float time = 0;      //For the perlin noise generation
  float randomRotation;  //Add some variation in rotation to give more randomness
  float radius = 30;    //The radius to draw at
  int edgeNumber;
  float drawingXPos = 0;  //The X position to draw the next vertex
  float drawingYPos = 0;  //Y position
  
  
  //Different possible asteroid size/shape types
  ArrayList<PShape> asteroidLevels;
  
  
  boolean directionInitialized = false;    //Is the asteroid being instantiated offscreen or broken apart
  
  boolean firstTimeOnScreen;  //Determines if asteroid has entered screen yet
  
  int numberOfTimesToWrap = 3;  //How many times to wrap around the screen
  
  float size;
  
  
  
  boolean initializeDeath = false;
  
  
  ArrayList<DestroyParticle> destroyParticles;    //Destroy particle system
  
  float destroyTime = 30;    //Timer for destroy particle system display
  
  
  //DEBUGGING STROKE COLORS FOR ELLIPSES AROUND OBJECTS
  color collisionStrokeColor;
  color collisionStrokeColorCollided;
  color collisionStrokeColorNone;
  
  
  
  Asteroid(PVector pos, PVector dir, int typeOfAsteroid, boolean initial, float s)
  {
     //Determine where offscreen to start then tell whether it is on screen yet
     int sideToStartOnX = int(random(0,2));
     int sideToStartOnY = int(random(0,2));
     firstTimeOnScreen = initial;
     
     
     collisionStrokeColorCollided = fgColor;
     collisionStrokeColorNone = 50;
     collisionStrokeColor = collisionStrokeColorNone;
     
     
     float xPos;
     float yPos;
     
    //Build asteroid OFF SCREEN
    if(firstTimeOnScreen == false)
    {
       whichAsteroidToStart = typeOfAsteroid;
       if(sideToStartOnX == 0)
       {
         xPos = -100;
       }
       else
       {
          xPos = width + 100; 
       }
       if(sideToStartOnY == 0)
       {
          yPos = -100; 
       }
       else
       {
        yPos = height + 100; 
       }
    }
    
    //Build asteroid ON SCREEN where it splits into two
    else
    {
        
        
        
        
        //SOMETHING WRONG WITH THIS
        PVector getPos = pos.get();
        xPos = getPos.x;
        yPos = getPos.y;
        
        
        direction = dir.get(); 
        direction.add(new PVector(random(0, 300), random(-300, 300)));
        direction.normalize();
    }
     
     
     asteroidLevels = new ArrayList<PShape>();
     
     
     
     speed = int(random(3, 7));
     
     
     edgeNumber = int(random(6, 9));
     //Randomize the initial time to give randomness on the perlin noise curve
     time = random(0, 15);
     
     //Add some random rotation
     randomRotation = random(-30,30);
     
     
     //Build the different asteroid shape types
     for(int x = 1; x < 4; x++)
     {
       asteroidLevels.add(randomlyGenerateAsteroid(25 * x));    //Offset the size
     }
     
     
     
     
     
     
     
     
     size = s;
     
     position = new PVector(xPos, yPos);
     
     
     //Initialize destroy particles for use
     destroyParticles = new ArrayList<DestroyParticle>();
     for(int x = 0; x < 20; x++)
     {
         destroyParticles.add(new DestroyParticle(position.x, position.y, random(1,2)));
     }
     //println(xPos + " " + yPos);
  } 
  
  
  //Move asteroid
  void move(PVector playerStartPos)
  {
     //Set the direction ONCE to follow the player upon instantiation, then go in its own direction
     //Keeps player from sitting in one spot
     if(directionInitialized == false)
     {
       playerPosition = playerStartPos.get();
       directionInitialized = true;
       direction = PVector.sub(playerPosition, position); 
       direction.add(new PVector(random(0, 300), random(-300, 300)));
       direction.normalize();
     }
     
     //Once it enters the screen for the first time, now enable the wrapping
     if(position.x > 0 && position.x < width && position.y > 0 && position.y < height)
     {
         firstTimeOnScreen = true;
     }
     
     //Now that it has entered the screen for the first time now do the wrapping stuff
     else
     {
       if(firstTimeOnScreen == true)
       {
          if(position.x < -100 || position.x > width + 100 || position.y < -100 || position.y > height + 100)
          {
              numberOfTimesToWrap--;
              
          }
          if(position.x < -100)
          {
             position.x = width + 100;
          }
          else if(position.x > width + 100)
          {
             position.x = -100;
          }
          if(position.y < -100)
          {
             position.y = height+100; 
          }
          else if(position.y > height + 100)
          {
             position.y = -100; 
          }
           
          
          //println(numberOfTimesToWrap + "");
       }
     }
     
     velocity = PVector.mult(direction, speed);
     position.add(velocity);
  }
  
  //Circular Collision Detection
  //Returns a boolean to where function was called so that the other class using it can perform some stuff IF a Collision does happen
  boolean checkForCollision(PVector posOther, float widthOther, float heightOther, boolean checkingBullets)
  {
      float distanceX;
      float distanceY;
      if(checkingBullets)
      {
          distanceX = posOther.x  - position.x;
          distanceY = posOther.y  - position.y;
      }
      else
      {
          distanceX = (posOther.x - (widthOther / 3)) - position.x;
          distanceY = (posOther.y - (heightOther / 3)) - position.y;
      }
      float distance = sqrt((distanceX * distanceX) + (distanceY * distanceY));
      
      if(((widthOther + this.size) / 2) > distance)
       {
          println("Collision");
          collisionStrokeColor = collisionStrokeColorCollided;
          return true;
       }
       else
       {
          collisionStrokeColor = collisionStrokeColorNone;
          return false;
       }
  }
  
  //RANDOM ASTEROID SHAPE GENERATION
  //Borrowed from my previous golf program
  PShape randomlyGenerateAsteroid(float OffsetSize)
  {
     PShape asteroid;
     //Begin the shape
     asteroid = createShape();
     asteroid.beginShape();
     for(int x = 0; x < edgeNumber; x++)
     {
       radius = map(radius, 0, 1, 0, width);      //Remap the radius
       radius = noise(time) + random(0.08, 0.1);  //Randomize the radius on each vertex to draw for organic feeling
       
       //Randomize the scale to give some more variation in the perlin generated noise to offset the consistency in the perlin curve
       int randomXScale = int(random(OffsetSize - 1, OffsetSize + 1));
       int randomYScale = int(random(OffsetSize - 1, OffsetSize + 1));
       
       //Create the next vertex from the origin outwards
       //Angle based on edge complexity
       //Change positions based on the perlin random radius and a little variation using the random(X/Y)scale variables
       //Increment the time on the perlin noise
       drawingXPos = cos(radians((360 / edgeNumber) * x)) * radius * randomXScale; 
       drawingYPos = sin(radians((360 / edgeNumber) * x)) * radius * randomYScale;
       asteroid.vertex(drawingXPos, drawingYPos);
       time += 0.05;
     }
     //After all vertices are built, close the shape
     asteroid.endShape(CLOSE);
     
     asteroid.setFill(false);
     asteroid.setStroke(true);
     
     if(whichAsteroidToStart == 2)
     {
       asteroid.setStroke(color(235));
     }
     else if(whichAsteroidToStart == 1)
     {
       asteroid.setStroke(color(235, 235, 50));
     }
     else if(whichAsteroidToStart == 0)
     {
       asteroid.setStroke(color(235, 100, 100));
     }
     
     
     return asteroid;
  }
  
  
  
  
  
  
  
  
  void display()
  {
    //So long as the asteroid isn't about to be destroyed
    if(initializeDeath == false)
    {
    pushMatrix();
      translate(position.x, position.y);
      shape(asteroidLevels.get(whichAsteroidToStart), 0, 0);
    popMatrix();
      //println(whichAsteroidToStart + "");
      noFill();
      stroke(collisionStrokeColor);
      //ellipse(position.x, position.y, size, size);
    }
    
    //Destroy the asteroid
    else
    {
      if(destroyTime > 0)
      {
        for(int x = 0; x < 20; x++)
        {
           destroyParticles.get(x).move();
           destroyParticles.get(x).display();
        } 
        destroyTime--;
      }
    }
  }
  
  void destroy()
  {
     for(int x = 0; x < 20; x++)
     {
         destroyParticles.get(x).modifyStartLocation(position.get());
     } 
     initializeDeath = true;
  }
  
  
  
  
}



//PARTICLE SYSTEM FOR DESTROYING SMALL ASTEROIDS
//Moves in random circular direction
class DestroyParticle
{
  float xLoc;
  float yLoc;
  float xDeviation;
  float yDeviation;
  float speed;
  
  DestroyParticle(float x, float y, float s)
  {
    xLoc = x;
    yLoc = y;
    speed = s;
    xDeviation = random(-3,3);
    yDeviation = random(-3,3);
    if(xDeviation == 0)
    {
      xDeviation = random(-3,3);
    }
    if(yDeviation == 0)
    {
      yDeviation = random(-3,3);
    }
  } 
  
  
  void move()
  {
    xLoc += xDeviation * speed;
    yLoc += yDeviation * speed;
  }
  
  void display()
  {
     fill(255);
     ellipse(xLoc, yLoc, 4,4); 
  }
  
  void modifyStartLocation(PVector start)
  {
      xLoc = start.x;
      yLoc = start.y;
  }
}

