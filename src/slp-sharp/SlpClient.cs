using System;
using System.Collections.Generic;
using System.Threading;
using SlpSharp.Native;

using SlpHandle = System.IntPtr;

namespace SlpSharp
{
  internal delegate SlpBoolean SrvTypeCallback (SlpHandle hSlp, 
    string pcSrvTypes,SlpError errCode,IntPtr pvCookie);

  internal delegate SlpBoolean AttrCallback (SlpHandle hSlp,
    string pcAttrList, SlpError errCode, IntPtr pvCookie);

  internal delegate SlpBoolean SrvURLCallback (SlpHandle hSlp,
    string pcSrvUrl, UInt16 sLifetime, SlpError errCode, IntPtr pvCookie);

  public delegate void ServerTypeFoundCallback( string type );

  [CLSCompliant(false)]
  public delegate void ServerFoundCallback (string url, UInt16 lifetime);
  
  public delegate bool AttribFoundCallback (string attributeList );  

  [CLSCompliant(false)]
  public class SlpClient : IDisposable
  {
    private SlpHandle hSlp = IntPtr.Zero;
    private AutoResetEvent wait = null;
    public bool IsAsync { get; set; }
    
    public SlpClient (string slpLang)
    {
      // try async mode first
      SlpError err = SlpNativeMethods.Open (slpLang, SlpBoolean.True, ref hSlp);
      IsAsync = true;
      if (err == SlpError.NOT_IMPLEMENTED)
      {
          err = SlpNativeMethods.Open(slpLang, SlpBoolean.False, ref hSlp);
          IsAsync = false;
      }

      if (err != SlpError.OK)
        throw new SlpException ( err );
    }

    public void Dispose ()
    {
      if (hSlp != IntPtr.Zero) {
        var tmp = hSlp;
        hSlp = IntPtr.Zero;
        SlpNativeMethods.Close (tmp);
      }
    }

    public List<string> FindTypes( string namingAuthority, string[] scopes )
    {
      var ret = new List<string>();
      FindTypes( namingAuthority, scopes,
        delegate ( string stype ){
          ret.Add( stype );
        });
      return ret;
    }

    public void FindTypes( string namingAuthority, string[] scopes, ServerTypeFoundCallback cb )
    {
      String scopelist = null;
      if (scopes != null)
        scopelist = String.Join (",", scopes);

      if (wait != null) throw new SlpException(SlpError.HANDLE_IN_USE);
          
      wait = new AutoResetEvent(false);

      var collatedTypes = new HashSet<string>();

      var err = SlpNativeMethods.FindSrvTypes( hSlp, namingAuthority, scopelist,
        delegate ( SlpHandle h, string serviceType, SlpError errcode, IntPtr cookie ) {
          if ( errcode == SlpError.OK ){
            foreach ( var st in serviceType.Split(',') ){
              if ( !collatedTypes.Contains( st ) ){
                collatedTypes.Add( st );
                if ( cb != null ) cb( st );
              }
            }
          }
          if (errcode == SlpError.LAST_CALL) {
              wait.Set();
              return SlpBoolean.False; 
          }
          return SlpBoolean.True;
        }, IntPtr.Zero );
      if ( err != SlpError.OK )
        throw new SlpException( err );

      wait.WaitOne();
      wait = null;
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
    public Dictionary<string,UInt16> Find (string serviceType, string[] scopes)
    {
      var ret = new Dictionary<string,UInt16> ();
      Find (serviceType, scopes, 
        delegate ( string url, UInt16 lifetime ) {
          ret [url] = lifetime;
        });

      return ret;
    }

    /// <summary>
    /// Calls SlpFindSrvs and executes cb for every result found.
    /// </summary>
    /// <param name="serviceType">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="scopes">
    /// A <see cref="System.String[]"/>
    /// </param>
    /// <param name="cb">
    /// A <see cref="ServerFoundCallback"/>
    /// </param>
    public void Find (string serviceType, string[] scopes, ServerFoundCallback cb)
    {
      var scopelist = String.Empty;
      var collatedServices = new HashSet<string>();
      if (serviceType == null)
        throw new ArgumentNullException ("serviceType");
      if (serviceType.Equals (string.Empty))
        throw new SlpException ( SlpError.TYPE_ERROR );

      if (scopes != null)
        scopelist = String.Join (",", scopes);

      if (wait != null) throw new SlpException(SlpError.HANDLE_IN_USE);
      wait = new AutoResetEvent(false);

      var err = SlpNativeMethods.FindSrvs (hSlp, serviceType, scopelist, String.Empty, 
        delegate ( SlpHandle h, string url, UInt16 lifetime, SlpError errcode, IntPtr cookie ) {
          if ( errcode == SlpError.OK ){
            if ( !collatedServices.Contains(url) ){
              collatedServices.Add(url);
              if (cb != null) cb(url, lifetime);
            }
          }
          if (errcode == SlpError.LAST_CALL)
          {
              wait.Set();
              return SlpBoolean.False;
          }
          return SlpBoolean.True;
        }, IntPtr.Zero);
      if (err != SlpError.OK)
        throw new SlpException ( err );

      wait.WaitOne();
      wait = null;

    }
        
    public void Attributes( string serviceTypeOrUrl, string[] scopes, string[] wantAttributes, AttribFoundCallback cb)
    {
	  String scopelist = null;
	  String wantlist  = null;
	  if ( serviceTypeOrUrl == null )
	    throw new ArgumentNullException("serviceTypeOrUrl");
      if ( serviceTypeOrUrl.Equals( string.Empty ) )
        throw new SlpException ( SlpError.TYPE_ERROR );
        
      if (scopes != null)
        scopelist = String.Join (",", scopes);
      if (wantAttributes != null)
        wantlist = String.Join(",", wantAttributes);
        
      var err = SlpNativeMethods.FindAttrs( hSlp, serviceTypeOrUrl, scopelist, wantlist, 
        delegate (SlpHandle h, string al, SlpError errCode, IntPtr pvCookie) {
          var ret = SlpBoolean.False;
          if ( errCode == SlpError.OK ) {
              if ((cb != null) && cb(al)) ret = SlpBoolean.True; 
          }
          return ret;
        }, IntPtr.Zero );  
      if (err != SlpError.OK)
        throw new SlpException ( err );   
    }
  }

}
