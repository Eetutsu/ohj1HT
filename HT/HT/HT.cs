using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Transactions;

namespace HT
{
    /// <summary>
    /// 
    /// </summary>
    public class HT : PhysicsGame
    {
        IntMeter laskuri1;
        IntMeter laskuri2;
        PhysicsObject ukko1;
        PhysicsObject ukko2;

        PhysicsObject alaTaso;

        bool pelaaja1VoiHypata = true;
        bool pelaaja2VoiHypata = true;

        /// <summary>
        /// Luodaan alkuvalikko
        /// Alkuvalikossa voidaan aloittaa peli, jolloin arvotaan kenttö, jossa pelataan ja luodaan ukot
        /// Tai lopettaa peli eli sulkea se
        /// </summary>
        /// 

        public override void Begin()
        {

            Level.Background.Color = Color.White;

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
            //Tee ukon siirto kunnolla
            //Luo loputkin kentät
            //Tee kommentit

        }

        /// <summary>
        /// Aloita peli aliohjelma ensin pyyhkii kaiken, mitä pääohjelmassa tehtiin, jonka jälkeen se luo pelattavat hahmot,
        /// luo tason, luo näppäimet, luo alatason, josta pisteet lasketaan, sekä törmäys tapahtumat.
        /// </summary>
        /// 

        void AloitaPeli()
        {
            ClearAll();

            LuoUkot();

           alaTaso = TasonPalikat(10000, 10, Shape.Rectangle, 0, Level.Bottom - 100);

            LuoNappaimet();

            Camera.ZoomToLevel();
            Level.Background.Color = Color.White;
            Level.CreateTopBorder();

            AddCollisionHandler(ukko1, alaTaso, Lisays);
            AddCollisionHandler(ukko2, alaTaso, Lisays);

            AddCollisionHandler(ukko1, "lattia", Hyppy);
            AddCollisionHandler(ukko2, "lattia", Hyppy);

            laskuri1 = LuoPistelaskuri(Screen.Left + 100, "Pelaaja 1 pisteet: ");
            laskuri2 = LuoPistelaskuri(Screen.Right - 100, "Pelaaja 2 pisteet: ");

            LuoTaso();
        }


        /// <summary>
        /// LuoUkot aliohjelma luo ukot tiettyyn X ja Y pisteisiin. Annetaan omat tagit kummallekkin ukolle sekä annetaan
        /// niille oma ulkoasu
        /// </summary>


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


        /// <summary>
        /// LuoNappaimet antaa tapahtumat näppäimille, joilla ukkoja liikutetaan
        /// </summary>
        


        void LuoNappaimet()
        {
            PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");


            double x = 30;
            double y = 600;

            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Up, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko1, new Vector(0, y));
            Keyboard.Listen(Key.Right, ButtonState.Down, LyoUkkoa, "Liikuta lumiukkoa oikealle", ukko1, new Vector(x, 0));
            Keyboard.Listen(Key.Right, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa oikealle", ukko1, new Vector(x, 0));
            Keyboard.Listen(Key.Down, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa alaspäin", ukko1, new Vector(0, -y));
            Keyboard.Listen(Key.Left, ButtonState.Down, LyoUkkoa, "Liikuta lumiukkoa vasemmalle", ukko1, new Vector(-x, 0));
            Keyboard.Listen(Key.Left, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa vasemmalle", ukko1, new Vector(-x, 0));



            Keyboard.Listen(Key.W, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa ylöspäin", ukko2, new Vector(0, y));
            Keyboard.Listen(Key.D, ButtonState.Down, LyoUkkoa, "Liikuta lumiukkoa oikealle", ukko2, new Vector(x, 0));
            Keyboard.Listen(Key.D, ButtonState.Pressed, LyoUkkoa, "Liikuta lumiukkoa oikealle", ukko2, new Vector(3*x, 0));
            Keyboard.Listen(Key.S, ButtonState.Pressed, LyoUkkoa, "Lyö lumiukkoa alaspäin", ukko2, new Vector(0, -y));
            Keyboard.Listen(Key.A, ButtonState.Down, LyoUkkoa, "Liikuta lumiukkoa vasemmalle", ukko2, new Vector(-x, 0));
            Keyboard.Listen(Key.A, ButtonState.Pressed, LyoUkkoa, "Liikuta lumiukkoa vasemmalle", ukko2, new Vector(3*-x, 0));
        }



        /// <summary>
        /// PiirräLumiukko piirtää lumiukot
        /// </summary>
        /// <param name="kumpiPuoli">jos false, piirretään ukko oikealle puolelle, jos true, piirtyy se vasemmalle</param>
        /// <param name="synnytysX">Kuinka paljon ukko on irti seinästä</param>
        /// <param name="synnytysY">Kuinka paljon ukko on irti lattiasta</param>
        /// <returns>Piirretyn lumiukon</returns>
         


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

         
            ukko.Restitution = 1.5;

            

            Add(ukko);

            ukko.LinearDamping = 0.95;

            Gravity = new Vector(0, -800);

            return ukko;

        }

       /// <summary>
       /// Tarkistaa, osuuko ukko lattiaan, jos osuu ukko voi hypätä
       /// </summary>
       /// <param name="ukko">Ukko jota tarkkaillaan</param>
       /// <param name="lattia">Kentän lattia</param>
       

        void Hyppy(PhysicsObject ukko, PhysicsObject lattia)
        {
            if (ukko.Tag == "pelaaja1") pelaaja1VoiHypata = true;
            else pelaaja2VoiHypata = true;
        }

        /// <summary>
        /// Lisätään pisteitä, kun ukko osuu alatasoon
        /// </summary>
        /// <param name="ukko">Ukko, joka osuu lattiaan</param>
        /// <param name="taso">Alataso</param>
        /// 

        void Lisays(PhysicsObject ukko, PhysicsObject taso)
        {
            int maxPisteet = 3;
            if (ukko.Tag == "pelaaja1") laskuri1.Value += 1;
            else laskuri2.Value += 1;

            Siirtyma(ukko, taso);
            


            if (laskuri1.Value == maxPisteet || laskuri2.Value == maxPisteet) LopetaPeli(maxPisteet);

        }

        void Siirtyma(PhysicsObject ukko, PhysicsObject taso)
        {
  
                ukko.Position = new Vector(0, 150);
        }

        /// <summary>
        /// Luodaan tasossa käytettäviä palikoita
        /// </summary>
        /// <param name="x">palikan korekus</param>
        /// <param name="y">palikan leveys</param>
        /// <param name="muoto">palikan muoto</param>
        /// <param name="X">palikan X koordinaatti</param>
        /// <param name="Y">Y koordinaatti</param>
        /// <param name="onLattia">jos true, on lattia, jos false niin seinä</param>
        /// <returns>Palikan</returns>

        PhysicsObject TasonPalikat(double x, double y , Shape muoto, double X, double Y, bool onLattia = false)
        {
           
            PhysicsObject taso = new PhysicsObject(x, y, muoto);
            taso.X = X;
            taso.Y = Y;
            taso.MakeStatic();
            taso.Restitution = -0.5;
            taso.Image = LoadImage("KenttaPalikka.png");
            Add(taso);

            if (onLattia) taso.Tag = "lattia";

            return taso;
        }


        /// <summary>
        /// Luodaan Taso käyttäen hyväksi TasonPalikat luomia palikoita
        /// Haetaan Arvo aliohjelmasta jokin satunnainen arvo, joka määrää, mikä kenttä luodaan
        /// </summary>


        void LuoTaso()
        {
            int mikaTaso = Arvo();
            if (mikaTaso == 0)
            {
                 
                TasonPalikat(600, 100, Shape.Rectangle, 0, Level.Bottom, true);
                TasonPalikat(50, 500, Shape.Rectangle, Level.Left-10, 0);
                TasonPalikat(50, 500, Shape.Rectangle, Level.Right+10, 0);
            }

            if (mikaTaso == 1)
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
            if (mikaTaso == 2)
            {
                for (int i = 1; i<=600; i+=100)
                {
                    double palikanX = Level.Left;
                    double palikanY = Level.Bottom;
                    TasonPalikat(50, 50, Shape.Rectangle, palikanX+=i*2, palikanY+=100, true);
                 
                }
                TasonPalikat(50, 50, Shape.Rectangle, 0, 65, true);
            }



        }


        /// <summary>
        /// LyöUkkoa aliojelma kertoo, että ukkoa halutaan lyödä tietyllä voimalla tiettyyn suuntaan
        /// </summary>
        /// <param name="ukko">Mikä ukko on kyseessä</param>
        /// <param name="suunta">Jos ukon suunta on ylöspäin, ei ukko voi hypätä</param>


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

        /// <summary>
        /// Arvotaan jokin arvo 0,2 välillä
        /// </summary>
        /// <returns>Arovttu luku</returns>
        

        public static int Arvo()
        {
            Random rnd = new Random();
            int b = rnd.Next(0,3);
            return b;
        }

        /// <summary>
        /// Luodaan 2 pistelaskuria kummallekkin pelaajalle. Pistelaskuri laskee pisteitä pelaajalle, kun toinen pelaaja tippuu
        /// </summary>
        /// <param name="x">Laskurin x koordinaatti</param>
        /// <param name="pelaaja">kumpi pelaaja on kyseessä</param>
        /// <returns>pistelaskurin</returns>


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

        /// <summary>
        /// Lopeta peli aliohjelma luo loppuvalikon, kun toinen pelaajista saa 3 pistettä
        /// Loppuvalikosta voi joko aloittaa pelin uudelleen, jolloin peli taas alkaa alusta. Voi myös lopettaa pelin, eli 
        /// sulkea pelin
        /// </summary>
        /// <param name="maxPisteet">Kumpi pelaajista sai 3 pistettä. Kerrotaan voittaja</param>

        void LopetaPeli(int maxPisteet)
        {
            ClearAll();

            
            string voittaja = "";

            if (laskuri1.Value == maxPisteet)
            {
                voittaja = "Pelaaja 1 voitti!";
                ukko1 = PiirraLumiukko(false, 500, 200);
                ukko1.Image = LoadImage("Lumiukko1.PNG");
                ukko1.MakeStatic();
            }
            else 
            {
                voittaja = "Pelaaja 2 voitti!"; 
                ukko2 = PiirraLumiukko(false, 500,200);
                ukko2.Image = LoadImage("Lumiukko2.PNG");
                ukko2.MakeStatic();
            }

            string[] vaihtoehdot = { "Aloita Peli Alusta", "Lopeta" };
            MultiSelectWindow loppuvalikko = new MultiSelectWindow(voittaja, vaihtoehdot);
            Add(loppuvalikko);

            loppuvalikko.AddItemHandler(0, AloitaPeli);
            loppuvalikko.AddItemHandler(1, Exit);


        }
    }
}