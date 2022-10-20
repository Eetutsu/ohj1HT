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


            PhysicsStructure ukko1 = PiirraLumiukko(false, 300.0, 100.0, Color.Blue);
            PhysicsStructure ukko2 = PiirraLumiukko(true, 300.0, 100.0,Color.Red);

            PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Up, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(0, 100));
            Keyboard.Listen(Key.Right, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(100, 0));
            Keyboard.Listen(Key.Down, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(0, -100));
            Keyboard.Listen(Key.Left, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(-100, 0));

            Keyboard.Listen(Key.W, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(0, 100));
            Keyboard.Listen(Key.D, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(100, 0));
            Keyboard.Listen(Key.S, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(0, -100));
            Keyboard.Listen(Key.A, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(-100, 0));

            Camera.ZoomToLevel();
            Level.Background.Color = Color.Black;

            Level.CreateBorders();
            

            LuoTaso();
        }


        public PhysicsStructure PiirraLumiukko(bool c ,double a, double b, Color d)
        {

            double p1X = 0;
            if (c == true)
            {
                p1X = Level.Left + a;
            }
            else p1X = Level.Right - a;

            PhysicsObject p1 = new PhysicsObject(100.0, 100.0, Shape.Circle);
            p1.Y = Level.Bottom + b;
            p1.X = p1X;
            Add(p1);

            PhysicsObject p2 = new PhysicsObject(50.0, 50.0, Shape.Circle);
            p2.Y = p1.Y  + 50+25;
            p2.X = p1.X;
            Add(p2);

            PhysicsObject p3 = new PhysicsObject(30.0, 30.0, Shape.Circle);
            p3.Y = p2.Y + 30+10;
            p3.X = p1.X;
            Add(p3);

            

            PhysicsStructure ukko = new PhysicsStructure(p1, p2, p3);
            ukko.Restitution = 0.4;
            Add(ukko);

            ukko.Color = d;

            Gravity = new Vector(0, -400);

            return ukko;

        }
        void LuoTaso()
        {
            //int b = Arvo();
            int b = 0;
            if (b == 0)
            {
                PhysicsObject taso1 = new PhysicsObject(500.0, 100.0, Shape.Rectangle);
                taso1.Y = Level.Bottom;
                taso1.X = 0;
                taso1.MakeStatic();
                Add(taso1);
                PhysicsObject taso2 = new PhysicsObject(100.0, 500.0, Shape.Rectangle);
                taso2.Y = 0;
                taso2.X = Level.Left;
                taso2.IgnoresGravity = true;
                taso2.MakeStatic();
                Add(taso2);
                PhysicsObject taso3 = new PhysicsObject(100.0, 500.0, Shape.Rectangle);
                taso3.Y = 0;
                taso3.X = Level.Right;
                taso3.IgnoresGravity = true;
                taso3.MakeStatic();
                Add(taso3);

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
        private void LyoUkkoa(PhysicsStructure ukko, Vector suunta)
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
    }
}