using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace HT
{
    public class HT : PhysicsGame
    {
        public override void Begin()
        {
            // Kirjoita ohjelmakoodisi tähän


            PhysicsObject ukko1 = PiirraLumiukko(false, 300.0, 100.0);
            ukko1.Image = LoadImage("Lumiukko2.PNG");
            PhysicsObject ukko2 = PiirraLumiukko(true, 300.0, 100.0);
            ukko2.Image = LoadImage("Lumiukko1.PNG");

            PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Up, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(0, 300));
            Keyboard.Listen(Key.Right, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(100, 0));
            Keyboard.Listen(Key.Down, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(0, -1000));
            Keyboard.Listen(Key.Left, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(-100, 0));
          


            Keyboard.Listen(Key.W, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(0, 300));
            Keyboard.Listen(Key.D, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(100, 0));
            Keyboard.Listen(Key.S, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(0, -1000));
            Keyboard.Listen(Key.A, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(-100, 0));

            Camera.ZoomToLevel();
            Level.Background.Color = Color.Red;

            Level.CreateBorders();
            

            LuoTaso();
        }


        public PhysicsObject PiirraLumiukko(bool c ,double a, double b)
        {

            double p1X = 0;
            if (c == true)
            {
                p1X = Level.Left + a;
            }
            else p1X = Level.Right - a;

            PhysicsObject ukko = new PhysicsObject(100,300 , Shape.Circle);
            ukko.Y = Level.Bottom + b;
            ukko.X = p1X;
            Add(ukko);


            ukko.CanRotate = false;

         
            ukko.Restitution = 0.4;

            

            Add(ukko);

            

            Gravity = new Vector(0, -400);

            return ukko;

        }


        void TasonPalikat(double x, double y , Shape muoto, double X, double Y)
        {
            PhysicsObject taso = new PhysicsObject(x, y, muoto);
            taso.X = X;
            taso.Y = Y;
            taso.MakeStatic();
            Add(taso);
        }


        void LuoTaso()
        {
            //int b = Arvo();
            int b = 0;
            if (b == 0)
            {
                TasonPalikat(500, 100, Shape.Rectangle, 0, Level.Bottom);
                TasonPalikat(100, 500, Shape.Rectangle, Level.Left, 0);
                TasonPalikat(100, 500, Shape.Rectangle, Level.Right, 0);
            }

            if (b == 1)
            {
                PhysicsObject taso1 = new PhysicsObject(500.0, 100.0, Shape.Rectangle);
                taso1.Y = Level.Bottom;
                taso1.X = 100;
                Add(taso1);
            }
            if (b == 2)
            {
                PhysicsObject taso1 = new PhysicsObject(500.0, 100.0, Shape.Rectangle);
                taso1.Y = Level.Bottom;
                taso1.X = 200;
                Add(taso1);
            }
            if (b == 3)
            {
                PhysicsObject taso1 = new PhysicsObject(200.0, 100.0, Shape.Rectangle);
                taso1.Y = Level.Bottom;
                taso1.X = 0;
                Add(taso1);
            }


        }

        
        



        private void LyoUkkoa(PhysicsObject ukko, Vector suunta)
        {

            ukko.Hit(ukko.Mass * suunta);
        }


        public static int Arvo()
        {
            int Min = 0;
            int Max = 4;
            int[] a = new int[4];
            int b = 0;

            Random randNum = new Random();
            for (int i = 0; i < 1; i++)
            {
                b = a[i];
                b = randNum.Next(Min, Max);

            }
            return b;
        }

        void LuoPistelaskuri()
        {
            IntMeter pistelaskuri = new IntMeter(0);

            Label pistenaytto = new Label();
            pistenaytto.X = Screen.Left + 100;
            pistenaytto.Y = Screen.Top - 100;
            pistenaytto.TextColor = Color.Black;
            pistenaytto.Color = Color.White;

            pistenaytto.BindTo(pistelaskuri);
            Add(pistenaytto);
        }
    }
}