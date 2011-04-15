using System;
using SlpSharp;


namespace slpsharptest
{
  class MainClass
  {
    public static void Main (string[] args)
    {
      using ( var slp = new SlpClient( String.Empty ) ){
        Console.WriteLine("SLP Opened");

        var found = slp.Find( "inbtest:inb.http", new string[0] );
        foreach ( var f in found ){
          Console.WriteLine("Found `{0}'", f );
        }

      }
      Console.WriteLine("SLP Closed");

    }
  }
}

