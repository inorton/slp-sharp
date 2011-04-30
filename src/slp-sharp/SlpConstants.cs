using System;

namespace SlpSharp
{
  public enum SlpError : int {
   LAST_CALL                    = 1,
   /*!< Passed to callback functions when the API library has no more data 
    * for them and therefore no further calls will be made to the callback 
    * on the currently outstanding operation. The callback can use this to 
    * signal the main body of the client code that no more data will be 
    * forthcoming on the operation, so that the main body of the client code 
    * can break out of data collection loops. On the last call of a callback 
    * during both a synchronous and asynchronous call, the error code 
    * parameter has value LAST_CALL, and the other parameters are all 
    * NULL. If no results are returned by an API operation, then only one 
    * call is made, with the error parameter set to LAST_CALL.
    */

   OK                           = 0,
   /*!< Indicates that the no error occurred during the operation. */

   LANGUAGE_NOT_SUPPORTED       = -1,
   /*!< No DA or SA has service advertisement or attribute information in 
    * the language requested, but at least one DA or SA indicated, via the 
    * LANGUAGE_NOT_SUPPORTED error code, that it might have information for 
    * that service in another language.
    */

   PARSE_ERROR                  = -2,
   /*!< The SLP message was rejected by a remote SLP agent. The API returns 
    * this error only when no information was retrieved, and at least one SA 
    * or DA indicated a protocol error. The data supplied through the API 
    * may be malformed or a may have been damaged in transit.
    */

   INVALID_REGISTRATION         = -3,
   /*!< The API may return this error if an attempt to register a service 
    * was rejected by all DAs because of a malformed URL or attributes. SLP 
    * does not return the error if at least one DA accepted the registration.
    */

   SCOPE_NOT_SUPPORTED          = -4,
   /*!< The API returns this error if the SA has been configured with 
    * net.slp.useScopes value-list of scopes and the SA request did not 
    * specify one or more of these allowable scopes, and no others. It may 
    * be returned by a DA or SA if the scope included in a request is not 
    * supported by the DA or SA.
    */

   AUTHENTICATION_ABSENT        = -6,
   /*!< If the SLP framework supports authentication, this error arises 
    * when the UA or SA failed to send an authenticator for requests or 
    * registrations in a protected scope.
    */

   AUTHENTICATION_FAILED        = -7,
   /*!< If the SLP framework supports authentication, this error arises when 
    * a authentication on an SLP message failed.
    */

   INVALID_UPDATE               = -13,
   /*!< An update for a non-existing registration was issued, or the update 
    * includes a service type or scope different than that in the initial 
    * registration, etc.
    */

   REFRESH_REJECTED             = -15,
   /*!< The SA attempted to refresh a registration more frequently than
    * the minimum refresh interval. The SA should call the appropriate API 
    * function to obtain the minimum refresh interval to use.
    */

   NOT_IMPLEMENTED              = -17,
   /*!< If an unimplemented feature is used, this error is returned. */

   BUFFER_OVERFLOW              = -18,
   /*!< An outgoing request overflowed the maximum network MTU size. The 
    * request should be reduced in size or broken into pieces and tried 
    * again.
    */

   NETWORK_TIMED_OUT            = -19,
   /*!< When no reply can be obtained in the time specified by the configured 
    * timeout interval for a unicast request, this error is returned.
    */

   NETWORK_INIT_FAILED          = -20,
   /*!< If the network cannot initialize properly, this error is returned. */

   MEMORY_ALLOC_FAILED          = -21,
   /*!< If the API fails to allocate memory, the operation is aborted and 
    * returns this.
    */

   PARAMETER_BAD                = -22,
   /*!< If a parameter passed into an interface is bad, this error is 
    * returned.
    */

   NETWORK_ERROR                = -23,
   /*!< The failure of networking during normal operations causes this error 
    * to be returned.
    */

   INTERNAL_SYSTEM_ERROR        = -24,
   /*!< A basic failure of the API causes this error to be returned. This 
    * occurs when a system call or library fails. The operation could not 
    * recover.
    */

   HANDLE_IN_USE                = -25,
   /*!< In the C API, callback functions are not permitted to recursively 
    * call into the API on the same SLPHandle, either directly or indirectly.  
    * If an attempt is made to do so, this error is returned from the called 
    * API function.
    */

   TYPE_ERROR                   = -26
   /*!< If the API supports type checking of registrations against service 
    * type templates, this error can arise if the attributes in a 
    * registration do not match the service type template for the service.
    */

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

