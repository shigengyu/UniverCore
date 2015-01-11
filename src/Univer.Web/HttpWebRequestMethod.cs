using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Univer.Web
{
    [Flags]
    public enum HttpWebRequestMethod
    {
        Connect = 1,
        Get = 1 << 1,
        Head = 1 << 2,
        MkCol = 1 << 3,
        Post = 1 << 4,
        Put = 1 << 5
    }
}
