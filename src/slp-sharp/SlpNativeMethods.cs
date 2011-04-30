using System;
using System.Runtime.InteropServices;

using SlpHandle = System.IntPtr;

namespace SlpSharp.Native {

  internal static class SlpNativeMethods {

    [DllImport("slp", EntryPoint="SLPOpen" )]
    public static extern SlpError Open( string pcLang,
      SlpBoolean isAsync, ref SlpHandle phSlp );

    [DllImport("slp", EntryPoint="SLPClose" )]
    public static extern void Close( SlpHandle hSlp );

    [DllImport("slp", EntryPoint="SLPFindSrvs" )]
    public static extern SlpError FindSrvs( SlpHandle hSlp,
      string pcServiceType,
      string pcScopeType,
      string pcSearchFilter,
      SlpSharp.SrvURLCallback callback,
      IntPtr pvCookie );
      
    [DllImport("slp", EntryPoint="SLPFindAttrs" )]
    public static extern SlpError FindAttrs( SlpHandle hSlp,
      string pcURLOrServiceType, 
      string pcScopeList,
      string pcAttrIds, 
      SlpSharp.AttrCallback callback, 
      IntPtr pvCookie);
  }

}
