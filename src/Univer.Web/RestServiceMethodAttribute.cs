using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Univer.Common;

namespace Univer.Web
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RestServiceMethodAttribute : Attribute
    {
        public HttpWebRequestMethod[] Methods
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the prefixes.
        /// </summary>
        /// <value>
        /// The prefixes.
        /// </value>
        public string[] Prefixes { get; set; }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public string Prefix
        {
            get
            {
                if (this.Prefixes != null)
                    return string.Join("/", Prefixes);
                else
                    return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceMethodAttribute"/> class.
        /// </summary>
        public RestServiceMethodAttribute()
        {
            this.Methods = new[] { HttpWebRequestMethod.Get };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceMethodAttribute"/> class.
        /// </summary>
        /// <param name="prefixes">The prefixes.</param>
        public RestServiceMethodAttribute(params string[] prefixes)
            : this()
        {
            this.Prefixes = prefixes;
        }

        public RestServiceMethodAttribute(HttpWebRequestMethod method, params string[] prefixes)
            : this(prefixes)
        {
            this.Methods = new[] { method };
        }

        public RestServiceMethodAttribute(HttpWebRequestMethod[] methods, params string[] prefixes)
            : this(prefixes)
        {
            this.Methods = methods;
        }
    }
}
