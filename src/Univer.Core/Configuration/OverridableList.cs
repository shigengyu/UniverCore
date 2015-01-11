using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Univer.Core.Configuration
{
    public class OverridableList<T> : IList<T>
    {
        private List<T> _list;

        #region Constructors

        public OverridableList()
        {
            _list = new List<T>();
        }

        public OverridableList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }

        #endregion

        public virtual int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public virtual T this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public virtual void Add(T item)
        {
            _list.Add(item);
        }

        public virtual void Clear()
        {
            _list.Clear();
        }

        public virtual bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return _list.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(T item)
        {
            return _list.Remove(item);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
