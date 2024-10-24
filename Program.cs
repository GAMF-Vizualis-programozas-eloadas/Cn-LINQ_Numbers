using System;
using System.Collections.Generic;
using System.Linq;

namespace CnLINQ_Numbers
{
  /// <summary>
  /// Három érdekes évszám: 2010, 2011, 2012.
  /// Ezek olyan egymást követő négyjegyű pozitív egész számok, amelyekben 
  /// ugyanaz a három számjegy található és pontosan az egyikből van 
  /// mindegyikben pontosan kettő. Keress további ilyen tulajdonságú 
  /// négyjegyű pozitív egész szám hármasokat úgy, hogy a számjegyek 
  /// összege legtöbb 21 lehet!
  /// Az alkalmazás célja az, hogy demonstráljon különböző LINQ lekérdezési
  /// lehetőségeket.
  /// </summary>
  class SzamokProgram
  {
    /// <summary>
    /// Generikus lista a számnégyesek tárolására.
    /// </summary>
    static List<int[]> Számok = new List<int[]>();

    /// <summary>
    /// Feltölti a listát az összes szóba jöhető számnégyessel.
    /// </summary>
    static void Feltölt()
    {
      for (int i = 1; i <= 9; i++)
        for (int j = 0; j <= 9; j++)
          for (int k = 0; k <= 9; k++)
            for (int l = 0; l <= 9; l++)
            {
              int[] Sor = new int[4];
              Sor[0] = i;
              Sor[1] = j;
              Sor[2] = k;
              Sor[3] = l;
              Számok.Add(Sor);
            }
    }

    /// <summary>
    /// Fő metódus.
    /// </summary>
    static void Main(string[] args)
    {
      // Lista feltöltése az összes szóbajöhető számnégyessel.
      Feltölt();
      // Első művelet: A számjegyek összege <= 21
      IEnumerable<int[]> E1 = from x in Számok
                              where x[0] + x[1] + x[2] + 
                                    x[3] <= 21
                              select x;
      Console.WriteLine("A számok összege <=21: {0} számsorra teljesül", E1.Count());


      // Második művelet: Egy számjegy pontosan kétszer szerepel (három fajta számjegy van).
      IEnumerable<int[]> E2 = from x in E1
                              where
                                x.Distinct().Count() == 3
                              select x;
      Console.WriteLine("Egy számjegy pontosan kétszer szerepel: {0} számsorra teljesül",
        E2.Count());

      // Harmadik művelet: Alakítsunk ki csoportokat úgy, hogy egy csoportban azonos 
      //                   számjegyek szerepeljenek.
      // Névtelen metódus, aminek az a feladata, hogy visszaadjon egy értéket, ami alapján 
      // majd végrehajtható a rendezés.
      Func<int, int> Elem = delegate (int i) { return i; };
      // A csoportosításhoz kell egy kulcs, ami azonos minden csoporttag esetén. 
      // Esetünkben ez az aktuális számnégyesben szereplő három egyjegyű szám lesz
      // növekvő sorrendbe rendezve. 
      // A kulcs lehet egyedi érték vagy több mezőből összeálló implicit osztály.
      // Az egyes mezők névvel kell rendelkezzenek. A végeredmény implicit típusú 
      // objektum. Ez egy gyűjtemény, aminek minden eleme egy csoportot tartalmaz.
      // Minden elem egy gyűjtemény, ami a csoportotsíttásnál létrehozott implicit
      // típusú objektumokat tartalmazza.
      var E3 = from x in E2
               group x by
               new
               {
                 a = ((int[])x.Distinct().OrderBy(Elem).ToArray())[0],
                 b = ((int[])x.Distinct().OrderBy(Elem).ToArray())[1],
                 c = ((int[])x.Distinct().OrderBy(Elem).ToArray())[2]
               }
                     into y
               select y;
      Console.WriteLine("Ugyanazon számjegyeket tartalmazó csoportok száma: {0}",
        E3.Count());

      // Minden egyes csoportot külön megvizsgálunk
      foreach (var x in E3)
      {
        // A négyelemű számtömböket a továbbiakban négyjegyű számként kezeljük.
        // Ezek tárolására létrehozunk egy tömböt.
        int[] Tömb = new int[x.Count()];
        // Végighaladunk az aktuális csoporthoz tartozó számnégyeseken.
        int i = 0;
        foreach (var t in x)
        {
          // A számtömböket négyjegyű számokká alakítjuk.
          Tömb[i] = t[0] * 1000 + t[1] * 100 + t[2] * 10 + t[3];
          // A harmadik számtól kezdődően megvizsgáljuk, hogy három egymást követő 
          // számról van-e szó.
          if (i > 1)
            if (Tömb[i] - Tömb[i - 1] == 1 && Tömb[i - 1] - Tömb[i - 2] == 1)
              // Ha igen, kiírjuk a konzolra az érintett három számot.
              Console.WriteLine(Tömb[i - 2] + ", " + Tömb[i - 1] + ", " + Tömb[i]);
          i++;
        }
      }
      Console.ReadLine();
    }
  }

}
