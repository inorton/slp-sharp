using System;
using System.Text;
using System.Runtime.InteropServices;

using SlpHandle = System.IntPtr;

namespace SlpSharp.Native
{

  static internal class SlpNativeMethods
  {

    [DllImport("slp", EntryPoint = "SLPOpen")]
    public static extern SlpError Open (string pcLang, SlpBoolean isAsync, ref SlpHandle phSlp);

    [DllImport("slp", EntryPoint = "SLPClose")]
    public static extern void Close (SlpHandle hSlp);

    [DllImport("slp", EntryPoint = "SLPReg")]
    public static extern SlpError Reg (SlpHandle hSlp, string pcSrvURL, UInt16 usLifetime, string pcSrvType, string pcAttrs, SlpBoolean fresh, SlpSharp.RegReport callback, IntPtr pvCookie);

    [DllImport("slp", EntryPoint = "SLPDereg")]
    public static extern SlpError DeReg (SlpHandle hSLP, string pcSrvURL, RegReport callback, IntPtr pvCookie);

    [DllImport("slp", EntryPoint = "SLPFindSrvs")]
    public static extern SlpError FindSrvs (SlpHandle hSlp, string pcServiceType, string pcScopeType, string pcSearchFilter, SlpSharp.SrvURLCallback callback, IntPtr pvCookie);
  
    [DllImport("slp", EntryPoint = "SLPFindSrvTypes")]
    public static extern SlpError FindSrvTypes (SlpHandle hSlp, string pcNamingAuthority, string pcScopeList, SlpSharp.SrvTypeCallback callback, IntPtr pvCookie);

    
    [DllImport("slp", EntryPoint = "SLPFindAttrs")]
    public static extern SlpError FindAttrs (SlpHandle hSlp, string pcURLOrServiceType, string pcScopeList, string pcAttrIds, SlpSharp.AttrCallback callback, IntPtr pvCookie);
    
  }
  
}
