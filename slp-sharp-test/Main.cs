using System;
using SlpSharp;


namespace slpsharptest
{
  class MainClass
  {
    public static void Main (string[] args)
    {
      Console.WriteLine ("find all service types");
      using (var slp = new SlpClient( String.Empty ) ){
        slp.FindTypes( String.Empty, null, delegate( string found ){
          Console.WriteLine(" {0}", found );
        } );
      }
      
      
      Console.WriteLine ("advertise a service");
      using (var slp = new SlpService (String.Empty)) {
        Console.WriteLine ("SLP Opened");
        slp.ServiceType = "testservice.tcp";
        slp.ServiceUri = new Uri ("testservice.tcp://10.9.8.7:1324/foo/bar");
        slp.Attributes.Add ("test", "1234");
        slp.Attributes.Add ("foo", "bar");
        //slp.Attributes.Add("invalidattrib","!<>()==");
        var err = slp.Register (8192);
        Console.WriteLine ("Register returned {0}", err);
      }
      Console.WriteLine ("SLP Closed");
      
      Console.WriteLine ("Find our own service");
      using (var slp = new SlpClient (String.Empty)) {
        Console.WriteLine ("SLP Opened");
        slp.Find ("testservice.tcp", new string[0], delegate(string url, UInt16 lifetime) {
          Console.WriteLine ("found {0}, lifetime = {1}", url, lifetime);
          
          // now find the attribs for this service
          using (var sslp = new SlpClient(String.Empty)) {
            Console.WriteLine("searching for all attribs");
            sslp.Attributes(url, null, null, delegate(string found) { Console.WriteLine(" -> {0}", found); return true; });
          }
          using (var sslp = new SlpClient (String.Empty)) {
            Console.WriteLine ("searching for test attrib");
            sslp.Attributes(url, null, new string[] { "test" }, delegate(string found) { Console.WriteLine(" -> {0}", found); return true; });
          }
          
        });
      }
      Console.WriteLine ("SLP Closed");


      Console.WriteLine("Test Sync Mode");
      using (var slp = new SlpClient(String.Empty)) {
          Console.WriteLine("SLP Opened");
          Console.WriteLine("Start Call Sync Find");
          var found = slp.Find("testservice.tcp", new string[0]);
          Console.WriteLine("Returned Call Sync Find");
          foreach (var f in found.Keys)
          {
              Console.WriteLine("Found {0}",f);
          }
      }
      Console.WriteLine("SLP Closed");

      System.Threading.Thread.Sleep(5000);
    }
  }
}

