using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyFileManager
{
    [Serializable]
    public class MyDirectoryNotFoundException : DirectoryNotFoundException
    {
        public MyDirectoryNotFoundException() { }
        public MyDirectoryNotFoundException(string message) : base(message) { }
        public MyDirectoryNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected MyDirectoryNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class MyDriveNotFoundException : DriveNotFoundException
    {
        public MyDriveNotFoundException() { }
        public MyDriveNotFoundException(string message) : base(message) { }
        public MyDriveNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected MyDriveNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class MyFileNotFoundException : FileNotFoundException
    {
        public MyFileNotFoundException() { }
        public MyFileNotFoundException(string message) : base(message) { }
        public MyFileNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected MyFileNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class MyUnauthorizedAccessException : UnauthorizedAccessException
    {
        public MyUnauthorizedAccessException() { }
        public MyUnauthorizedAccessException(string message) : base(message) { }
        public MyUnauthorizedAccessException(string message, Exception inner) : base(message, inner) { }
        protected MyUnauthorizedAccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
