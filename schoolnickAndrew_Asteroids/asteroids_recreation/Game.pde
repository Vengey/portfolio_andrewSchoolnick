//THE MAIN GAME CLASS

class Game
{
  Player player;
  HUD hud;
  
  //ArrayList<Bullet>bulletReference;
  Bullet[] bulletReference;      //References to bullets attached to player class
  
  
  PShape playerShipShapeReference;    //Reference to the shape used by player class to use for lives which will be passed into HUD
  
  
  int numberBullets;    //Number of bullets on the screen
  
  //Asteroid testStroid;
  //Asteroid lists
  ArrayList<Asteroid> asteroidListLarge;
  ArrayList<Asteroid> asteroidListMedium;
  ArrayList<Asteroid> asteroidListSmall;
  
  
  PVector playerPos;
  
  int maxAsteroidsOnScreen;
  
  
  //How often to add asteroids
  float timeToAddAnotherAsteroid = timeToAddAsteroids;
  
  
  Game()
  {
     player = new Player(width/2, height/2, fgColor);
     playerShipShapeReference = player.PlayersShip();
     
     
     
     hud = new HUD(playerShipShapeReference);
     
     numberBullets = player.bulletNum;
     bulletReference = player.bulletList();
     
     //testStroid = new Asteroid();
     maxAsteroidsOnScreen = 5;

     asteroidListLarge = new ArrayList<Asteroid>();
     asteroidListMedium = new ArrayList<Asteroid>();
     asteroidListSmall = new ArrayList<Asteroid>();
     
     
     for(int x = 1; x < maxAsteroidsOnScreen; x++)
     {
        addAsteroids();
        
     }
     
  } 
  
  //Add Asteroids randomly on the screen
  void addAsteroids()
  {
      //70% chance for large rocks
      //20% chance for medium rocks
      //10% chance for small rocks
      
      float probability = random(0,1);
      int typeOfAsteroid;
      float sizeAsteroid;
         if(probability < 0.7)
         {
           typeOfAsteroid = 2;
           sizeAsteroid = 100;
           asteroidListLarge.add(new Asteroid(new PVector(0,0), new PVector(0,0), typeOfAsteroid, false, sizeAsteroid)); 
         }
         else if(probability < 0.9)
         {
           typeOfAsteroid = 1;
           sizeAsteroid = 75;
           asteroidListMedium.add(new Asteroid(new PVector(0,0), new PVector(0,0), typeOfAsteroid, false, sizeAsteroid)); 
         }
         else if(probability < 1.0)
         {
           typeOfAsteroid = 0;
           sizeAsteroid = 50;
           asteroidListSmall.add(new Asteroid(new PVector(0,0), new PVector(0,0), typeOfAsteroid, false, sizeAsteroid)); 
         }
         else
         {
           typeOfAsteroid = 0; 
         }
  }
  
  
  //Display the game
  void showGame()
  {
     playerPositionGlobal = player.position.get();
     if(player.deathTimer < 0)
     {
        player = new Player(width/2, height/2, fgColor); 
     }
     if(player.dead == false)
     {
       player.move();
     }
     player.display(); 
     playerPos = player.position.get();
     //testStroid.move(playerPos);
     //testStroid.display();
     
     
     //Bullet collisions
     checkForCollisionsBullets(asteroidListLarge, asteroidListMedium, 50);
     checkForCollisionsBullets(asteroidListMedium, asteroidListSmall, 25);
     checkForCollisionsBullets(asteroidListSmall, asteroidListSmall, 0);
     
     //Player collisions
     checkForCollisionsPlayer(asteroidListLarge);
     checkForCollisionsPlayer(asteroidListMedium);
     checkForCollisionsPlayer(asteroidListSmall);
     
     displayAsteroids(asteroidListLarge);
     displayAsteroids(asteroidListMedium);
     displayAsteroids(asteroidListSmall);
     /*
     for(int x = 0; x < asteroidList.size() - 1; x++)
     {
         asteroidList.get(x).move(playerPos);
         asteroidList.get(x).display();
     }
     */
     
     
     hud.display();
     timeToAddAnotherAsteroid--;
     if(timeToAddAnotherAsteroid < 0)
     {
        timeToAddAnotherAsteroid = timeToAddAsteroids;
        addAsteroids();
     }
     //println(timeToAddAnotherAsteroid + "");
  }
  
  
  //Display the asteroids
  void displayAsteroids(ArrayList<Asteroid> asteroidList)
  {
     for(int x = 0; x < asteroidList.size(); x++)
     {
        if(asteroidList.get(x).initializeDeath == false)
        {
          asteroidList.get(x).move(playerPos);
        }
        asteroidList.get(x).display();
        if(asteroidList.get(x).numberOfTimesToWrap == 0)
        {
            asteroidList.remove(x);
        }
     }
  }
  
  
  //Check if player collides with asteroid
  void checkForCollisionsPlayer(ArrayList<Asteroid> asteroidList)
  {
     boolean collision = false;
     for(Asteroid a : asteroidList)
     {
         collision = a.checkForCollision(playerPositionGlobal.get(), 20, 20, false);
         //ellipse(playerPositionGlobal.get().x, playerPositionGlobal.get().y, 20, 20);
         if(collision)
         {
            println("Kill Player"); 
            player.dead = true;
         }  
     }
     
  }
  
  
  //Check if bullet collides with asteroid
  void checkForCollisionsBullets(ArrayList<Asteroid> asteroidList, ArrayList<Asteroid> nextArrayListToAddTo, float newSize)
  {
     boolean collision;
     boolean destroyed;
     for(int i = 0; i < bulletReference.length - 1; i++)
     {
       for(int x = 0; x < asteroidList.size() - 1; x++)
       {
          //println("Check");
          collision = asteroidList.get(x).checkForCollision(bulletReference[i].position, bulletReference[i].size, bulletReference[i].size, true);
          if(collision && bulletReference[i].enableCollisions)
          {
              
              if(asteroidList == asteroidListLarge)
              {
                score += 1000 + random(0, 100);
              }
              else if(asteroidList == asteroidListMedium)
              {
                score += 1500 + random(0, 100);
              }
              else if(asteroidList == asteroidListSmall)
              {
                score += 2000 + random(0, 100);
              }
              //SPLIT IT
              if(asteroidList.get(x).whichAsteroidToStart > 0)
              {
                
                asteroidList.get(x).whichAsteroidToStart--;
                int newAsteroidType = asteroidList.get(x).whichAsteroidToStart;
                
                
                PVector newPos = asteroidList.get(x).position.get();
                
                //Only split if the next list level isn't the smallest level
                if(asteroidList != nextArrayListToAddTo)
                {
                  //SPLIT ASTEROID//
                  for(int y = 0; y < 2; y++)
                  {
                    float newXDir = bulletReference[i].direction.get().x + random(0,100);
                    float newYDir = bulletReference[i].direction.get().y + random(-200,200);
                    PVector newAsteroidDirection = new PVector(newXDir, newYDir);
                    
                    nextArrayListToAddTo.add(new Asteroid(newPos, newAsteroidDirection, newAsteroidType, true, newSize));
                  }
                }
                bulletReference[i].enableCollisions = false;
                
                asteroidList.remove(x);
              }
              //DESTROY IT IF CANT BE SPLIT
              else
              {
                asteroidList.get(x).destroy();
                
              }
              
              
          }
          else
          {
              
          }
       } 
     }
     
  }
  
  
  void havePlayerShoot()
  {
     player.shoot(); 
  }
}
