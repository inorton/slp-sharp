using System;
using System.Text;
using System.Collections.Generic;
using SlpSharp.Native;

using SlpHandle = System.IntPtr;

namespace SlpSharp
{
  internal delegate void RegReport (SlpHandle hSLP,
    SlpError errCode,IntPtr pvCookie);

  public class SlpService : IDisposable
  {
    private SlpHandle hSlp = IntPtr.Zero;
    
    public SlpService (string slpLang)
    {
	  Attributes = new Dictionary<string, string>();
      SlpError err = SlpNativeMethods.Open (slpLang, SlpBoolean.False, ref hSlp);
      if (err != SlpError.OK)
        throw new SlpException ( err );
    }
    
    public Uri ServiceUri { get; set; }
    public string ServiceType { get; set; }
    public Dictionary<string,string> Attributes { get; private set; }

	public string AttributeString { 
	  get {
	    var sb = new StringBuilder();
	    foreach ( var aname in Attributes.Keys ){
	      if ( sb.Length > 0 ) sb.Append(",");
	      sb.AppendFormat("({0}={1})", aname, Attributes[aname] );
	    }
	    return sb.ToString();
	  } 
	}

    public SlpError Register ( UInt16 lifetime ) {
	  return SlpNativeMethods.Reg( hSlp,ServiceUri.ToString(), lifetime, ServiceType, AttributeString, SlpBoolean.True, 
	    ( SlpHandle h, SlpError err, IntPtr cookie ) => { }, IntPtr.Zero );
    }
    
    public SlpError Update ( UInt16 lifetime ) {
	  return SlpNativeMethods.Reg( hSlp,ServiceUri.ToString(), lifetime, ServiceType, AttributeString, SlpBoolean.False, 
	    ( SlpHandle h, SlpError err, IntPtr cookie ) => { }, IntPtr.Zero );
    }

    public void Dispose ()
    {
      if (hSlp != IntPtr.Zero) {
        var tmp = hSlp;
        hSlp = IntPtr.Zero;
        SlpNativeMethods.Close (tmp);
      }
    }
  }
}