using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace hwj.CommonLibrary.Object.Email
{
    public class FileStream
    {
        public FileStream() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FileStream"/> class,default translate into GzipStream .
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="stream">The in stream.</param>
        public FileStream(string fileName, Stream stream)
            : this(fileName, stream, true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStream"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="stream">The in stream.</param>
        /// <param name="useGzip">if set to <c>true</c> [use gzip].</param>
        public FileStream(string fileName, Stream stream, bool useGzip)
        {
            this.FileName = fileName;
            this.Stream = stream;
            this.UseGzip = useGzip;
        }

        /// <summary>
        /// Get or Set the value to control either use Gzip to compress the stream or not
        /// </summary>
        public bool UseGzip { get; set; }
        public string FileName { get; set; }
        public Stream Stream { get; set; }

    }
}
