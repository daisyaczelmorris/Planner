using System;
using SwinGameSDK;

namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
            //Initialize objects
            Planner myPlanner = new Planner();
           


            //Open the game window
            SwinGame.OpenGraphicsWindow("GameMain", 800, 600);
            SwinGame.ShowSwinGameSplashScreen();
           


            //Run the game loop
            while (false == SwinGame.WindowCloseRequested())
            {
                //Fetch the next batch of UI interaction
                SwinGame.ProcessEvents();
                //
               
                
                //Clear the screen and draw the framerate
                SwinGame.ClearScreen(Color.White);
                SwinGame.DrawFramerate(0,0);

                //Draw onto the screen
                
                myPlanner.Draw();
                myPlanner.SaveButton("C:\\Users\\Daisy\\Desktop\\test.txt");
                myPlanner.LoadButton("C:\\Users\\Daisy\\Desktop\\test.txt");
                SwinGame.RefreshScreen(60);
            }

        }

    }
}