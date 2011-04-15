using System;
using System.Collections.Generic;
using SlpSharp.Native;

using SlpHandle = System.IntPtr;

namespace SlpSharp
{
  internal delegate SlpBoolean SrvTypeCallback( SlpHandle hSlp,
    string pcSrvTypes, SlpError errCode, IntPtr pvCookie );

  internal delegate SlpBoolean AttrCallback( SlpHandle hSlp,
    string pcAttrList, SlpError errCode, IntPtr pvCookie );

  internal delegate SlpBoolean SrvURLCallback( SlpHandle hSlp,
    string pcSrvUrl, UInt16 sLifetime, SlpError errCode, IntPtr pvCookie );


  public class SlpClient : IDisposable
  {
    private SlpHandle hSlp = IntPtr.Zero;

    public SlpClient( string slpLang )
    {
      SlpError err = SlpNative.Open( slpLang, SlpBoolean.False, ref hSlp );
      if ( err != SlpError.OK )
        throw new SlpException( err );
    }

    public void Dispose ()
    {
      if ( hSlp != IntPtr.Zero ){
        var tmp = hSlp; hSlp = IntPtr.Zero;
        SlpNative.Close( tmp );
      }
    }

    /// <summary>
    /// Calls SlpFindSrvs (synchronously)
    /// </summary>
    /// <param name="serviceType">
    /// A <see cref="System.String"/> service type.
    /// </param>
    /// <param name="scopes">
    /// A <see cref="IEnumerable<System.String>"/> list of service scopes.
    /// </param>
    /// <returns>
    /// A <see cref="Dictionary<System.String, Uint16>"/> of found services and thier lifetimes.
    /// </returns>
    public Dictionary<string,UInt16> Find( string serviceType, string[] scopes )
    {
      var ret = new Dictionary<string,UInt16>();
      String scopelist = null;

      if ( serviceType == null ) throw new ArgumentNullException("serviceType");
      if ( serviceType.Equals( string.Empty ) ) throw new SlpException( SlpError.TYPE_ERROR );

      if (scopes != null)
        scopelist = String.Join (",", scopes);

      var err = SlpNative.FindSrvs( hSlp, serviceType, scopelist, String.Empty,
        delegate ( SlpHandle h, string url, UInt16 lifetime, SlpError errcode, IntPtr cookie ) {
          if ( errcode == SlpError.OK ){
            if ( url != null ){
              var str = url;
              ret.Add(str,lifetime);
            }
          }
          if ( errcode == SlpError.LAST_CALL ) return SlpBoolean.False;
          return SlpBoolean.True;
        }, IntPtr.Zero );
      if ( err != SlpError.OK ) throw new SlpException( err );

      return ret;
    }
  }

}

