/*
 * Copyright (c) 2013-2015 Univer Shi
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;

namespace Univer.Core.Caching
{
    public abstract class ListCacheContainer<T> : AutoRefreshCacheContainer
    {
        private readonly List<T> _list = new List<T>();

        public ReadOnlyCollection<T> Items
        {
            get { return new ReadOnlyCollection<T>(_list); }
        }

        public void Add(T value)
        {
            _list.Add(value);
        }

        public bool Remove(T value)
        {
            return _list.Remove(value);
        }

        public override void Clear()
        {
            _list.Clear();
        }
    }

    [TestFixture]
    public class ListCacheContainerTest
    {
        [CacheRefreshPolicy(500)]
        private class ListCache : ListCacheContainer<int>
        {
            public int Version
            {
                get;
                private set;
            }

            protected override void DoRefresh()
            {
                ++Version;
            }
        }

        [Test]
        public void Test()
        {
            using (var cache = new ListCache())
            {
                Thread.Sleep(3000);

                Assert.AreEqual(cache.Version, 6);
            }
        }
    }
}
