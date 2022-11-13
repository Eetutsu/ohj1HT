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
            string[] vaihtoehdot = { "Aloita Peli", "Lopeta" };
            MultiSelectWindow alkuvalikko = new MultiSelectWindow("Lumiukko Sumo", vaihtoehdot);
            Add(alkuvalikko);

            alkuvalikko.AddItemHandler(0, AloitaPeli);
            alkuvalikko.AddItemHandler(1, Exit);

            ukko1 = PiirraLumiukko(false, 200.0, 200.0);
            ukko1.Image = LoadImage("Lumiukko2.PNG");
            ukko1.MakeStatic();

            ukko2 = PiirraLumiukko(true, 200.0, 200.0);
            ukko2.Image = LoadImage("Lumiukko1.PNG");
            ukko2.MakeStatic();



            //TODO:
            //Luo loputkin kentät
            //Tee kommentit

        }


        void AloitaPeli()
        {
            ClearAll();

            LuoUkot();

            PhysicsObject alaTaso = TasonPalikat(10000, 10, Shape.Rectangle, 0, Level.Bottom - 100);

            LuoNappaimet();

            Camera.ZoomToLevel();
            Level.Background.Color = Color.Red;
            Level.CreateTopBorder();

            AddCollisionHandler(ukko1, alaTaso, Lisays);
            AddCollisionHandler(ukko2, alaTaso, Lisays);

            AddCollisionHandler(ukko1, "lattia", Hyppy);
            AddCollisionHandler(ukko2, "lattia", Hyppy);

            laskuri1 = LuoPistelaskuri(Screen.Left + 100, "Pelaaja 1 pisteet: ");
            laskuri2 = LuoPistelaskuri(Screen.Right - 100, "Pelaaja 2 pisteet: ");

            LuoTaso();
        }


        void LuoUkot()
        {
            double ukkoX = 200.0;
            double ukkoY = 200.0;

            ukko1 = PiirraLumiukko(false, ukkoX, ukkoY);
            ukko1.Image = LoadImage("Lumiukko2.PNG");
            ukko1.Tag = "pelaaja1";

            ukko2 = PiirraLumiukko(true, ukkoX, ukkoY);
            ukko2.Image = LoadImage("Lumiukko1.PNG");
            ukko2.Tag = "pelaaja2";
        }

        void LuoNappaimet()
        {
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
        }

        public PhysicsObject PiirraLumiukko(bool kumpiPuoli ,double synnytysX, double synnytysY)
        {

            double p1X = 0;
            if (kumpiPuoli == true)
            {
                p1X = Level.Left + synnytysX;
            }
            else p1X = Level.Right - synnytysX;

            double ukkoKorkeus = 70.0;
            double ukkoLeveys = 210.0;

            PhysicsObject ukko = new PhysicsObject(ukkoKorkeus,ukkoLeveys , Shape.Rectangle);
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
            int maxPisteet = 3;
            if (ukko.Tag == "pelaaja1") laskuri1.Value += 1;
            else laskuri2.Value += 1;

            Siirtyma(ukko1);
            Siirtyma(ukko2);

            if (laskuri1.Value == maxPisteet || laskuri2.Value == maxPisteet) LopetaPeli(maxPisteet);

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
            int b = 2;
            if (b == 0)
            {
                 
                TasonPalikat(600, 100, Shape.Rectangle, 0, Level.Bottom, true);
                TasonPalikat(50, 500, Shape.Rectangle, Level.Left-10, 0);
                TasonPalikat(50, 500, Shape.Rectangle, Level.Right+10, 0);
            }

            if (b == 1)
            {
                for (double i = Level.Left; i<=0; i+=80)
                {
                    TasonPalikat(80,80, Shape.Rectangle, i, i,true);
                }

                for (double i = Level.Right; i >= 0; i -= 80)
                {
                    TasonPalikat(80, 80, Shape.Rectangle, i, -i, true);
                }

                TasonPalikat(10, 1000, Shape.Rectangle, Level.Left-500, 0);
                TasonPalikat(10, 1000, Shape.Rectangle, Level.Right+500, 0);

            }
            if (b == 2)
            {
                for (int i = 1; i<=6; i++)
                {
                    double palikanX = Level.Left;
                    double palikanY = Level.Bottom;
                    TasonPalikat(50, 50, Shape.Rectangle, palikanX, palikanY, true);
                    if (i%2 == 0)
                    {
                        TasonPalikat(50, 50, Shape.Rectangle, palikanX+=100, palikanY+=100, true);
                    }
                    else TasonPalikat(50, 50, Shape.Rectangle, palikanX += 100, palikanY -= 100, true);

                }
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


        IntMeter LuoPistelaskuri(double x, string pelaaja)
        {
            IntMeter pistelaskuri = new IntMeter(0);

            Label pistenaytto = new Label();
            pistenaytto.X = x;
            pistenaytto.Y = Screen.Top - 100;
            pistenaytto.Title = pelaaja;
            pistenaytto.TextColor = Color.Black;
            pistenaytto.Color = Color.White;

            pistenaytto.BindTo(pistelaskuri);
            Add(pistenaytto);


            return pistelaskuri;
        }

        void LopetaPeli(int maxPisteet)
        {
            ClearAll();

            
            string voittaja = "";

            if (laskuri1.Value == maxPisteet) voittaja = "Pelaaja 1 voitti!";
            else voittaja = "Pelaaja 2 voitti!";

            string[] vaihtoehdot = { "Aloita Peli Alusta", "Lopeta" };
            MultiSelectWindow loppuvalikko = new MultiSelectWindow(voittaja, vaihtoehdot);
            Add(loppuvalikko);

            loppuvalikko.AddItemHandler(0, AloitaPeli);
            loppuvalikko.AddItemHandler(1, Exit);


        }
    }
}