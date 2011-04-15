using System;

namespace SlpSharp
{
  public class SlpException : Exception
  {
    public SlpError Error = SlpError.OK;

    public SlpException ( SlpError err ) : base ( String.Format("SLP_{0}", err.ToString() ) )
    {
      Error = err;
    }
  }
}

