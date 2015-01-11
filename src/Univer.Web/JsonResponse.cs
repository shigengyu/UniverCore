using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Univer.Common;
using Univer.Common.IO;

namespace Univer.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonResponse : HttpResponseBase
    {
        private string Json { get; set; }

        public override long ContentLength
        {
            get
            {
                return this.Json == null ? 0 : this.Json.Length;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonResponse"/> class.
        /// </summary>
        protected JsonResponse()
            : base(HttpResponseType.Json)
        {
        }

        /// <summary>
        /// Writes the content to the response output stream.
        /// </summary>
        /// <param name="responseOutputStream">The response output stream.</param>
        public override void WriteTo(Stream responseOutputStream)
        {
            responseOutputStream.Write(this.Json);
        }

        public static JsonResponse With(object obj)
        {
            return new JsonResponse
            {
                Json = JsonConvert.SerializeObject(obj, Formatting.Indented)
            };
        }
    }
}
