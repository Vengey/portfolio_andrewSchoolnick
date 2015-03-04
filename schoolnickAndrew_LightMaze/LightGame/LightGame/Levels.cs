using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LightGame
{
    class Levels : Grid
    {
        //LEVEL SELECTION SCREEN//

        Rectangle levelRect;

        TextBlock levelNum;
        public static int SIZE = 100;
        public static int START = 30;
        public static int SPACING = 20;
        Grid combinedObjects = new Grid();


        //Is the level complete?
        private bool completed = false;
        public bool Completed
        {
            get
            {
                return completed;
            }
            set
            {
                completed = value;
            }
        }

        public int levelNumValue;

        public bool existsInFolder = false;

        public Levels(int x, int y, int value, Grid grid, bool customLevels)
        {

            
            //Create one tile in the level selection screen

            levelRect = new Rectangle();
            levelRect.Width = SIZE;
            levelRect.Height = SIZE;
            levelRect.RadiusX = levelRect.RadiusY = 5;

            if (completed)
            {
                levelRect.Fill = new SolidColorBrush(Colors.Red);
            }
            else
            {
                levelRect.Fill = new SolidColorBrush(Colors.White);
            }
            levelNumValue = value;

            levelNum = new TextBlock();
            string numValue;
            if (customLevels)
            {
                numValue = "EX. " + levelNumValue.ToString();
            }
            else
            {
                numValue = levelNumValue.ToString();
            }
            
            levelNum.Text = numValue;
            levelNum.Foreground = new SolidColorBrush(Colors.Black);
            

            if (customLevels)
            {
                levelNum.Padding = new Thickness(0, 10, 10, 5);
                levelNum.FontSize = 16;
            }
            else
            {
                levelNum.Padding = new Thickness(10, 5, 5, 5);
                levelNum.FontSize = 24;
            }


            if (File.Exists(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Levels\Level" + value + ".txt"))
            {
                levelRect.Fill = new SolidColorBrush(Colors.White);
                existsInFolder = true;
            }
            else
            {
                levelRect.Fill = new SolidColorBrush(Colors.DarkGray);
                existsInFolder = false;
            }
            

            levelNum.TextAlignment = TextAlignment.Center;


            combinedObjects.Children.Add(levelRect);
            combinedObjects.Children.Add(levelNum);
            grid.Children.Add(combinedObjects);
            
            levelRect.HorizontalAlignment = HorizontalAlignment.Left;
            levelRect.VerticalAlignment = VerticalAlignment.Top;
            levelNum.HorizontalAlignment = HorizontalAlignment.Left;
            levelNum.VerticalAlignment = VerticalAlignment.Top;

            Thickness theMargin = new Thickness(
                START + x * (SIZE + SPACING),
                START + 25 + y * (SIZE + SPACING), 0, 0);
            levelRect.Margin = theMargin;
            Thickness textMargin = new Thickness(
                START + 32 + x * (SIZE + SPACING),
                START + 50 + y * (SIZE + SPACING), 0, 0);
            levelNum.Margin = textMargin;

            
            
            
            
        }


        
        public void updateColors()
        {
            if (completed)
            {
                levelRect.Fill = new SolidColorBrush(Colors.Red);
            }
            else
            {
                levelRect.Fill = new SolidColorBrush(Colors.White);
            }
        }



        //Check to see which level is hovered over
        public void checkIfMouseOver()
        {
            combinedObjects.MouseEnter += Levels_MouseEnter;
            combinedObjects.MouseLeave += Levels_MouseLeave;

            
        }

        public int checkIfMouseUp()
        {
            combinedObjects.MouseEnter -= Levels_MouseEnter;
            combinedObjects.MouseLeave -= Levels_MouseLeave;
            if (combinedObjects.IsMouseOver)
            {
                string returnString;
                if (levelNum.Text.Contains("EX."))
                {
                    returnString = levelNum.Text.Substring(4);
                }
                else
                {
                    returnString = levelNum.Text;
                }
                return Int32.Parse(returnString);      //Return level selected by player
            }
            else
            {
                return 0;                               //If nothing is selected, return 0
            }
           
        }

        

        void Levels_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (existsInFolder)
            {
                levelRect.Fill = new SolidColorBrush(Colors.White);
            }
            else
            {
                levelRect.Fill = new SolidColorBrush(Colors.DarkGray);
            }
            combinedObjects.MouseEnter -= Levels_MouseEnter;
            combinedObjects.MouseLeave -= Levels_MouseLeave;
            
        }

        void Levels_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (existsInFolder)
            {
                levelRect.Fill = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                
            }
            
        }

        
    }
}
