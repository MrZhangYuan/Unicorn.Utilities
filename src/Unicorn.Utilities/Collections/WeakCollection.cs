using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicorn.Utilities.Collections
{
    public class WeakCollection<T> : IEnumerable<T>, IEnumerable where T : class
    {
        private List<WeakReference> innerList = null;
        public WeakCollection()
                : this(4)
        {

        }

        public WeakCollection(int capty)
        {
            this.innerList = new List<WeakReference>(capty);
        }

        public void Foreach(Action<T> action)
        {
            this.innerList.ForEach(_p =>
            {
                if (_p.IsAlive)
                {
                    T item = _p.Target as T;
                    if (item != null)
                    {
                        action(item);
                    }
                }
            });
        }

        public void Add(T item)
        {
            this.innerList.Add(new WeakReference((object)item));
        }

        public void Clear()
        {
            this.innerList.Clear();
        }

        public bool Contains(T item)
        {
            for (int index = 0; index < this.innerList.Count; ++index)
            {
                if (this.innerList[index].Target == item)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Remove(T item)
        {
            for (int index = 0; index < this.innerList.Count; ++index)
            {
                if (this.innerList[index].Target == item)
                {
                    this.innerList.RemoveAt(index);
                    return true;
                }
            }
            return false;
        }

        public IList<T> ToList()
        {
            List<T> objList = new List<T>(this.innerList.Count);
            foreach (WeakReference inner in this.innerList)
            {
                T target = inner.Target as T;
                if ((object)target != null)
                    objList.Add(target);
            }
            if (objList.Count != this.innerList.Count)
                this.Prune(objList.Count);
            return (IList<T>)objList;
        }

        public int GetAliveItemsCount()
        {
            return this.innerList.Where<WeakReference>((Func<WeakReference, bool>)(weakRef => weakRef.IsAlive)).Count<WeakReference>();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ToList().GetEnumerator();
        }

        private void Prune(int anticipatedSize)
        {
            List<WeakReference> innerList = this.innerList;
            this.innerList = new List<WeakReference>(anticipatedSize);
            foreach (WeakReference weakReference in innerList)
            {
                if (weakReference.IsAlive)
                    this.innerList.Add(weakReference);
            }
        }
    }

}
