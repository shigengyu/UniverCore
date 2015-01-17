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
using System.Linq;
using System.Text;

namespace Univer.WorkflowLight
{
    public class WorkflowContext
    {
        private Dictionary<string, object> _dictionary;

        public WorkflowContext()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get
            {
                if (_dictionary.ContainsKey(key))
                    return _dictionary[key];
                else
                    return null;
            }
            set
            {
                _dictionary[key] = value;
            }
        }

        public T GetValue<T>(string key)
        {
            if (_dictionary.ContainsKey(key))
                return (T)_dictionary[key];
            else
                return default(T);
        }
    }
}
