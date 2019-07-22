using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public class RegistryException : Exception
    {
        public string RegistryKeyPath { get; }

        ///Initializes a new instance of the <see cref="System.Exception"/> class.
        public RegistryException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Exception"/> class with a specified error
        /// message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RegistryException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Exception"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified.</param>
        public RegistryException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information
        /// about the source or destination.</param>
        /// <exception cref="ArgumentNullException">
        /// The info parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="SerializationException">
        /// The class name is <see langword="null"/> or <see cref="Exception.HResult"/> is zero (0).
        /// </exception>
        [SecuritySafeCritical]
        protected RegistryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public RegistryException(string message, string registryKeyPath) : this(message, null, registryKeyPath) { }    

        public RegistryException(string message, Exception innerException, string registryKeyPath) : base(message, innerException) => RegistryKeyPath = registryKeyPath;
    }
}
