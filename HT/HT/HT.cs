using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Transactions;

namespace HT
{
    public class HT : PhysicsGame
    {
        IntMeter laskuri1;
        IntMeter laskuri2;
        PhysicsObject ukko1;
        PhysicsObject ukko2;

        bool pelaaja1VoiHypata = true;
        bool pelaaja2VoiHypata = true;

        public override void Begin()
        {
            // Kirjoita ohjelmakoodisi tähän
             ukko1 = PiirraLumiukko(false, 300.0, 100.0);
             ukko1.Image = LoadImage("Lumiukko2.PNG");
            ukko1.Tag = "pelaaja1";

             ukko2 = PiirraLumiukko(true, 300.0, 100.0);
             ukko2.Image = LoadImage("Lumiukko1.PNG");
            ukko2.Tag = "pelaaja2";



            PhysicsObject alaTaso = TasonPalikat(10000, 10, Shape.Rectangle, 0, Level.Bottom - 100);

            PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");

            double x = 30;
            double y = 600;

            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Up, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(0, y));
            Keyboard.Listen(Key.Right, ButtonState.Down, LyoUkkoa, "Liikuta lumiukkoa oikealle", ukko1, new Vector(x, 0));
            Keyboard.Listen(Key.Down, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa alaspäin", ukko1, new Vector(0, -y));
            Keyboard.Listen(Key.Left, ButtonState.Down, LyoUkkoa, "Liikuta lumiukkoa vasemmalle", ukko1, new Vector(-x, 0));



            Keyboard.Listen(Key.W, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(0, y));
            Keyboard.Listen(Key.D, ButtonState.Down, LyoUkkoa, "Liikuta lumiukkoa oikealle", ukko2, new Vector(x, 0));
            Keyboard.Listen(Key.S, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa alaspäin", ukko2, new Vector(0, -y));
            Keyboard.Listen(Key.A, ButtonState.Down, LyoUkkoa, "Liikuta lumiukkoa vasemmalle", ukko2, new Vector(-x, 0));


            Camera.ZoomToLevel();
            Level.Background.Color = Color.Red;

            Level.CreateTopBorder();

            AddCollisionHandler(ukko1, alaTaso, Lisays);
            AddCollisionHandler(ukko2, alaTaso, Lisays);

            AddCollisionHandler(ukko1, "lattia", Hyppy);
            AddCollisionHandler(ukko2, "lattia", Hyppy);

            laskuri1 = LuoPistelaskuri(Screen.Left+100);
            laskuri2 = LuoPistelaskuri(Screen.Right - 100);
            LuoTaso();


            //TODO:
            //Aloita peli alusta, kun tulee jommallekummalle 3 pistettä täyteen
            //Luo loputkin kentät
            //Tee alkumenu
            //Tee kommentit

        }


        public PhysicsObject PiirraLumiukko(bool kumpiPuoli ,double synnytysX, double synnytysY)
        {

            double p1X = 0;
            if (kumpiPuoli == true)
            {
                p1X = Level.Left + synnytysX;
            }
            else p1X = Level.Right - synnytysX;

            PhysicsObject ukko = new PhysicsObject(70,210 , Shape.Rectangle);
            ukko.Y = Level.Bottom + synnytysY;
            ukko.X = p1X;
           


            ukko.CanRotate = false;

         
            ukko.Restitution = 1;

            

            Add(ukko);

            ukko.LinearDamping = 0.95;

            Gravity = new Vector(0, -400);

            return ukko;

        }

       
        void Hyppy(PhysicsObject ukko, PhysicsObject lattia)
        {
            if (ukko.Tag == "pelaaja1") pelaaja1VoiHypata = true;
            else pelaaja2VoiHypata = true;
        }


        void Lisays(PhysicsObject ukko, PhysicsObject taso)
        {
            if (ukko.Tag == "pelaaja1") laskuri1.Value += 1;
            else laskuri2.Value += 1;

            Siirtyma(ukko1);
            Siirtyma(ukko2);

        }


        void Siirtyma(PhysicsObject ukko)
        {
            ukko1.IgnoresCollisionResponse = true;
            ukko2.IgnoresCollisionResponse = true;

            Vector spawn2 = new Vector(Level.Left + 300, Level.Bottom + 300);
            Vector spawn1 = new Vector(Level.Right - 300, Level.Bottom + 300);
            ukko1.MoveTo(spawn1, 10000);
            ukko2.MoveTo(spawn2, 10000);

            ukko1.IgnoresCollisionResponse = false;
            ukko2.IgnoresCollisionResponse = false;
        }


        PhysicsObject TasonPalikat(double x, double y , Shape muoto, double X, double Y, bool onLattia = false)
        {
           
            PhysicsObject taso = new PhysicsObject(x, y, muoto);
            taso.X = X;
            taso.Y = Y;
            taso.MakeStatic();
            taso.Restitution = 0.1;
            Add(taso);

            if (onLattia) taso.Tag = "lattia";

            return taso;
        }


        void LuoTaso()
        {
            //int b = Arvo();
            int b = 0;
            if (b == 0)
            {
                 
                TasonPalikat(500, 100, Shape.Rectangle, 0, Level.Bottom, true);
                TasonPalikat(100, 500, Shape.Rectangle, Level.Left, 0);
                TasonPalikat(100, 500, Shape.Rectangle, Level.Right, 0);
            }

            if (b == 1)
            {
                for (double i = Level.Right; i<=Level.Left; i+=20)
                {
                    TasonPalikat(20,100, Shape.Rectangle, i+40, Level.Bottom);
                }
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
            if (suunta.Y > 0)
            {
                if (ukko.Tag == "pelaaja1")
                {
                    if (!pelaaja1VoiHypata) return;
                    else pelaaja1VoiHypata = false;
                }

                if (ukko.Tag == "pelaaja2")
                {
                    if (!pelaaja2VoiHypata) return;
                    else pelaaja2VoiHypata = false;
                }
            }

            ukko.Hit(ukko.Mass * suunta);
        }


        public static int Arvo()
        {
            Random rnd = new Random();
            int b = rnd.Next(0,4);
            return b;
        }


        IntMeter LuoPistelaskuri(double x)
        {
            IntMeter pistelaskuri = new IntMeter(0);

            Label pistenaytto = new Label();
            pistenaytto.X = x;
            pistenaytto.Y = Screen.Top - 100;
            pistenaytto.TextColor = Color.Black;
            pistenaytto.Color = Color.White;

            pistenaytto.BindTo(pistelaskuri);
            Add(pistenaytto);


            return pistelaskuri;
        }

        void AloitaAlusta()
        {

        }
    }
}