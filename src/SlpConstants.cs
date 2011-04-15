using System;

namespace SlpSharp
{
  public enum SlpError : int
  {
    LAST_CALL = 1,
    OK = 0,
    LANGUAGE_NOT_SUPPORTED = -1,
    PARSE_ERROR = -2,
    INVALID_REGISTRATION = -3,
    SCOPE_NOT_SUPPORTED = -4,
    AUTHENTICATION_ABSENT = -6,
    AUTHENTICATION_FAILED = -7,
    INVALID_UPDATE = -13,
    REFRESH_REJECTED = -15,
    NOT_IMPLEMENTED = -17,
    BUFFER_OVERFLOW = -18,
    NETWORK_TIMED_OUT = -19,
    NETWORK_INIT_FAILED = -20,
    MEMORY_ALLOC_FAILED = -21,
    PARAMETER_BAD = -22,
    NETWORK_ERROR = -23,
    INTERNAL_SYSTEM_ERROR = -24,
    HANDLE_IN_USE = -25,
    TYPE_ERROR = -26,
    RETRY_UNICAST = -27
  }

  public enum SlpBoolean : int
  {
    False = 0,
    True  = 1,
  }

  public static class SlpConstants
  {
    public const UInt16 DefaultLifetime = 10800;
    public const UInt16 MaximumLifetime = 65535;
  }
}

