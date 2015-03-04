using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LightGame
{
    class GameGrid : Grid
    {

        //BUILD GRID FOR GAMEBOARD




        public static double sizeOfRect = 50;
        private double startOfGrid = 30;
        public static double spacingOfRects = 5;


        private int distanceFromPlayer = 4;     //Checks for which spaces to be black or semi visible in light source

        public int DistanceFromPlayer
        {
            get
            {
                return distanceFromPlayer;
            }
            set
            {
                distanceFromPlayer = value;
            }
        }
        

        private int typeOfSpace = 0;

        private bool isPlayerOnSpace = false;
        public bool IsPlayerOnSpace
        {
            get
            {
                return isPlayerOnSpace;
            }
            set
            {
                isPlayerOnSpace = value;
            }
        }
     

        public int TypeOfSpace
        {
            get
            {
                return typeOfSpace;
            }
            set
            {
                typeOfSpace = value;
            }
        }

        private SolidColorBrush colorOfBlock;
        public SolidColorBrush ColorOfBlock
        {
            get
            {
                return colorOfBlock;
            }
            set
            {
                colorOfBlock = value;
            }
        }

        private int xPos;
        public int XPos
        {
            get
            {
                return xPos;
            }
            set
            {
                xPos = value;
            }
        }
        private int yPos;
        public int YPos
        {
            get
            {
                return yPos;
            }
            set
            {
                yPos = value;
            }
        }

        Rectangle spaceRect = new Rectangle();


        ScaleTransform myScale = new ScaleTransform(1, 1, sizeOfRect / 2, sizeOfRect / 2);


        Grid myGrid;


        TextBlock nameOfBlock;

        Thickness theMargin;

        private int priorityValue = 0;
        public int PriorityValue
        {
            get
            {
                return priorityValue;
            }
            set
            {
                priorityValue = value;
            }
        }

        

        public GameGrid(int x, int y, char spaceType, Grid grid, bool inBuilderMode)
        {
            myGrid = grid;
            xPos = x;
            yPos = y;

            //Obtain typeOfSpace i.e. Platform/Switch etc.
            string typeOfSpaceString = "" + spaceType;
            typeOfSpace = Int32.Parse(typeOfSpaceString);

            Console.WriteLine(typeOfSpace);

            spaceRect.Width = sizeOfRect;
            spaceRect.Height = sizeOfRect;

            if (typeOfSpace == 0)   //Pit
            {
                colorOfBlock = new SolidColorBrush(Colors.Black);
                isPlayerOnSpace = false;
                Console.WriteLine("Black");
            }
            else if (typeOfSpace == 1)  //Platform
            {
                colorOfBlock = new SolidColorBrush(Colors.White);
                isPlayerOnSpace = false;
                Console.WriteLine("White");
            }
            else if (typeOfSpace == 2)  //Exit Switch
            {   
                colorOfBlock = new SolidColorBrush(Colors.Yellow);
                isPlayerOnSpace = false;
                Console.WriteLine("Yellow");
            }
            else if (typeOfSpace == 3)  //Exit Locked
            {
                colorOfBlock = new SolidColorBrush(Colors.DarkRed);
                isPlayerOnSpace = false;
                Console.WriteLine("Yellow");
            }
            else if (typeOfSpace == 4)  //Exit Unlocked
            {
                colorOfBlock = new SolidColorBrush(Colors.Green);
                isPlayerOnSpace = false;
                Console.WriteLine("Yellow");
            }
            else if (typeOfSpace == 5)  //Player
            {
                colorOfBlock = new SolidColorBrush(Colors.Blue);
                isPlayerOnSpace = true;
            }

            else if (typeOfSpace == 6)  //Bridge NOT Lowered
            {
                colorOfBlock = new SolidColorBrush(Colors.Black);
                isPlayerOnSpace = false;
            }
            else if (typeOfSpace == 7)  //Bridge Lowered
            {
                colorOfBlock = new SolidColorBrush(Colors.Gray);
                isPlayerOnSpace = false;
                Console.WriteLine("Bridge");
            }
            else if (typeOfSpace == 8)  //Bridge Switch
            {
                colorOfBlock = new SolidColorBrush(Colors.Violet);
                isPlayerOnSpace = false;
                Console.WriteLine("Bridge");
            }
            
            spaceRect.Fill = colorOfBlock;
            

            


            spaceRect.HorizontalAlignment = HorizontalAlignment.Left;
            spaceRect.VerticalAlignment = VerticalAlignment.Top;

            theMargin = new Thickness(startOfGrid + x * (sizeOfRect + spacingOfRects), startOfGrid + y * (sizeOfRect + spacingOfRects), 0, 0);
            spaceRect.Margin = theMargin;

            spaceRect.Opacity = 1;
            if (inBuilderMode)
            {
                nameOfBlock = new TextBlock();
                nameOfBlock.Text = typeOfSpace + "";
                nameOfBlock.Padding = new Thickness(5, 5, 5, 5);
                nameOfBlock.TextAlignment = TextAlignment.Left;
                nameOfBlock.VerticalAlignment = VerticalAlignment.Top;
                nameOfBlock.Foreground = new SolidColorBrush(Colors.White);
                nameOfBlock.FontFamily = new System.Windows.Media.FontFamily("Moire");
                nameOfBlock.FontSize = 20;

                nameOfBlock.Margin = new Thickness(startOfGrid + 5 + xPos * (sizeOfRect + spacingOfRects), startOfGrid + yPos * (sizeOfRect + spacingOfRects), 0, 0);
                spaceRect.Opacity = .5;
                grid.Children.Add(nameOfBlock);
                updateColorBuilder();
            }
            
            
            grid.Children.Add(spaceRect);
            
        }

        public void updateColor()
        {
            
            if (isPlayerOnSpace)        //Check if player is on current space, if so assign player to that square
            {
                colorOfBlock = new SolidColorBrush(Colors.Blue);
                Console.WriteLine("Yes");
            }
          

            else                      //If not.... Update Colors so that the player isn't on said space
            {
                if (typeOfSpace == 0)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Black);
                    isPlayerOnSpace = false;
                    Console.WriteLine("Black");

                }
                else if (typeOfSpace == 1)
                {
                    colorOfBlock = new SolidColorBrush(Colors.White);
                    isPlayerOnSpace = false;
                    Console.WriteLine("White");
                }
                else if (typeOfSpace == 2)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Yellow);
                    isPlayerOnSpace = false;
                    Console.WriteLine("Yellow");
                }
                else if (typeOfSpace == 3)
                {
                    colorOfBlock = new SolidColorBrush(Colors.DarkRed);
                    isPlayerOnSpace = false;
                    Console.WriteLine("Red");
                }
                else if (typeOfSpace == 4)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Green);
                    isPlayerOnSpace = false;
                    
                    Console.WriteLine("Green");
                }
                else if (typeOfSpace == 5)
                {
                    colorOfBlock = new SolidColorBrush(Colors.White);
                    isPlayerOnSpace = false;
                    
                    Console.WriteLine("Green");
                }
                else if (typeOfSpace == 6)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Black);
                    isPlayerOnSpace = false;
                    Console.WriteLine("Bridge");
                }
                else if (typeOfSpace == 7)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Gray);
                    isPlayerOnSpace = false;
                    Console.WriteLine("Bridge");
                }
                else if (typeOfSpace == 8)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Violet);
                    isPlayerOnSpace = false;
                    Console.WriteLine("Bridge");
                }
                

            }
            spaceRect.Fill = colorOfBlock;
            
            
        }
        /*public void typeOfBlockText()
        {
            nameOfBlock = new TextBlock();
            nameOfBlock.Text = typeOfSpace + "";
            nameOfBlock.Padding = new Thickness(5, 5, 5, 5);
            nameOfBlock.TextAlignment = TextAlignment.Left;
            nameOfBlock.VerticalAlignment = VerticalAlignment.Top;
            nameOfBlock.Foreground = new SolidColorBrush(Colors.White);
            nameOfBlock.FontFamily = new System.Windows.Media.FontFamily("Moire");
            nameOfBlock.FontSize = 20;

            nameOfBlock.Margin = new Thickness(startOfGrid + 5 + xPos * (sizeOfRect + spacingOfRects), startOfGrid + yPos * (sizeOfRect + spacingOfRects), 0, 0);

            
            
            myGrid.Children.Add(nameOfBlock);
        }*/
        public void updateColorBuilder()
        {

            nameOfBlock.Text = typeOfSpace + "";
            nameOfBlock.FontSize = 20;
            nameOfBlock.Foreground = new SolidColorBrush(Colors.Black);
            
                if (typeOfSpace == 0)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Black);
                    nameOfBlock.Text = "Pit";
                    Console.WriteLine("Black");

                }
                else if (typeOfSpace == 1)
                {
                    colorOfBlock = new SolidColorBrush(Colors.LightGray);
                    nameOfBlock.Text = "Path";
                    nameOfBlock.Foreground = new SolidColorBrush(Colors.Black);
                    nameOfBlock.FontSize = 18;
                    Console.WriteLine("White");
                }
                else if (typeOfSpace == 2)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Yellow);
                    nameOfBlock.Foreground = new SolidColorBrush(Colors.Black);
                    nameOfBlock.Text = "Exit\nKey";
                    nameOfBlock.FontSize = 18;
                    Console.WriteLine("Yellow");
                }
                else if (typeOfSpace == 3)
                {
                    colorOfBlock = new SolidColorBrush(Colors.DarkRed);
                    nameOfBlock.Text = "Exit";
                    
                    Console.WriteLine("Red");
                }
                else if (typeOfSpace == 4)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Green);

                    nameOfBlock.Text = "Exit";
                    

                    Console.WriteLine("Green");
                }
                else if (typeOfSpace == 5)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Blue);
                    nameOfBlock.Text = "Player";
                    nameOfBlock.FontSize = 12;
                    nameOfBlock.Padding = new Thickness(5, 5, 5, 5);
                    Console.WriteLine("Green");
                }
                else if (typeOfSpace == 6)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Black);
                    nameOfBlock.Text = "OFF";
                    
                    Console.WriteLine("Bridge");
                }
                else if (typeOfSpace == 7)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Gray);
                    nameOfBlock.Foreground = new SolidColorBrush(Colors.Black);
                    nameOfBlock.Text = "ON";
                    Console.WriteLine("Bridge");
                }
                else if (typeOfSpace == 8)
                {
                    colorOfBlock = new SolidColorBrush(Colors.Violet);
                    nameOfBlock.Foreground = new SolidColorBrush(Colors.Black);
                    nameOfBlock.Text = "Switch";
                    nameOfBlock.FontSize = 12;
                    Console.WriteLine("Bridge");
                }
                else
                {
                    colorOfBlock = new SolidColorBrush(Colors.Black);
                    
                    
                }


            spaceRect.Fill = colorOfBlock;


        }


        public void AnimateDeath()      //Fall into pit animation
        {
            

            ScaleTransform trans = new ScaleTransform(1, 1, sizeOfRect / 2, sizeOfRect / 2);
            spaceRect.RenderTransform = trans;
            // if you use the same animation for X & Y you don't need anim1, anim2 
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(1000));
            trans.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            trans.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }


        //ADJUST OPACITIES OF NEARBY BLOCKS DEPENDING ON DISTANCE FROM PLAYER....Light Source cont.d
        public void adjustOpacityNear()
        {
            distanceFromPlayer = 1;
            spaceRect.Opacity = .5;
        }
        public void adjustOpacityFurther()
        {
            distanceFromPlayer = 2;
            spaceRect.Opacity = .25;
        }
        public void adjustOpacityFar()
        {
            distanceFromPlayer = 3;
            spaceRect.Opacity = 0;
        }
        public void adjustOpacityPlayer()
        {
            distanceFromPlayer = 0;
            spaceRect.Opacity = .75;
        }







        

        public void checkIfMouseUp(int priority)
        {
            if (spaceRect.IsMouseOver)
            {
                
                typeOfSpace++;
                priorityValue = priority;

                
                if (typeOfSpace > 8)
                {
                    typeOfSpace = 0;
                }
                updateColorBuilder();
            }

        }


        
        
    }
}
