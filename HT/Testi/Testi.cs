using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// @author Omanimi
/// @version Päivämäärä
/// <summary>
/// 
/// </summary>
public class Testi
{
    /// <summary>
    /// 
    /// </summary>
    public static void Main()
    {
        for (int i = 0; i < 10; i++)
        {
            Random rnd = new Random();
            int b = rnd.Next(0,4);

            Console.WriteLine(b);
        }
        
    }

}
