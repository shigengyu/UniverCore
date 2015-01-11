/*
 * Copyright (c) 2013 Univer Shi
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Univer.Common;

namespace Univer.Core.Caching
{
    /// <summary>
    /// 
    /// </summary>
    [CacheRefreshPolicy]
    public abstract class AutoRefreshCacheContainer : ICacheContainer, IDisposable
    {
        #region Private Fields

        private readonly Timer _timer;
        private readonly int _refreshInterval;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRefreshCacheContainer"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected AutoRefreshCacheContainer()
        {
            var cacheRefreshPolicyAttribute = this.GetType().GetCustomAttribute<CacheRefreshPolicyAttribute>();
            _refreshInterval = cacheRefreshPolicyAttribute.MillisecondsInterval;

            if (_refreshInterval != Timeout.Infinite)
            {
                _timer = new Timer(
                    obj => DoRefresh(),
                    null,
                    _refreshInterval,
                    _refreshInterval);
            }

            this.Initialize();
        }

        #endregion

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected virtual void Initialize()
        {
            this.DoRefresh();
        }

        /// <summary>
        /// Refreshes the cache.
        /// </summary>
        public void Refresh()
        {
            DoRefresh();

            _timer.Change(_refreshInterval, _refreshInterval);
        }

        /// <summary>
        /// Performs a refresh for all cached items.
        /// </summary>
        protected virtual void DoRefresh()
        {
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public abstract void Clear();

        #region IDisposable Members

        private bool _isDisposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="AutoRefreshCacheContainer"/> is reclaimed by garbage collection.
        /// </summary>
        ~AutoRefreshCacheContainer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (_timer != null)
                    _timer.Dispose();

                GC.SuppressFinalize(this);
            }

            _isDisposed = true;
        }

        #endregion
    }
}
