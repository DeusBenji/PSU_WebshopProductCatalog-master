using System;

namespace Webshop.Search.Api.Utilities
{
    /// <summary>
    /// Wrapper for at standardisere API-responser.
    /// </summary>
    public class Envelope
    {
        /// <summary>
        /// Skaber en succesfuld respons med data.
        /// </summary>
        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result, null);
        }

        /// <summary>
        /// Skaber en succesfuld respons uden data.
        /// </summary>
        public static Envelope Ok()
        {
            return new Envelope();
        }

        /// <summary>
        /// Skaber en fejlrespons med en fejlmeddelelse.
        /// </summary>
        public static Envelope Error(string errorMessage)
        {
            return new Envelope(errorMessage);
        }

        protected Envelope(string errorMessage = null) // Fra private til protected
        {
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
        }

        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }
    }

    /// <summary>
    /// Generisk wrapper til at inkludere data i API-responser.
    /// </summary>
    public class Envelope<T> : Envelope
    {
        public T Result { get; }

        public Envelope(T result, string errorMessage) : base(errorMessage) // Fra internal til public
        {
            Result = result;
        }
    }
}
