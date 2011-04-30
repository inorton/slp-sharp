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

				slp.Find( "testservice", new string[0],
          delegate ( string url, UInt16 lifetime ){
            Console.WriteLine("found {0}, lifetime = {1}", url, lifetime );
          } );

      }
      Console.WriteLine("SLP Closed");

    }
  }
}

