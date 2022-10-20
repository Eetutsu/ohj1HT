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
        int Min = 0;
        int Max = 4;
        int[] a = new int[4];
        int b = 0;

        Random randNum = new Random();
        for (int i = 0; i < a.Length; i++)
        {
            b = a[i];
            b = randNum.Next(Min, Max);
            Console.WriteLine(b);
        }
    }

}
