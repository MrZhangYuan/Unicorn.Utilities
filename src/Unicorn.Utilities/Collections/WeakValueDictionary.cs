using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Unicorn.Utilities.Collections
{
    [DebuggerDisplay("#Entries={Count}")]
    public class WeakValueDictionary<K, V> where V : class
    {
        private int capacity = 10;
        private Dictionary<K, WeakReference> backingDictionary;

        public WeakValueDictionary()
          : this((IEqualityComparer<K>)null)
        {
        }

        public WeakValueDictionary(IEqualityComparer<K> keyComparer)
        {
            this.backingDictionary = new Dictionary<K, WeakReference>(keyComparer);
        }

        public int Count
        {
            get
            {
                return this.backingDictionary.Count;
            }
        }

        public V this[K key]
        {
            get
            {
                WeakReference backing1 = this.backingDictionary[key];
                if (backing1 == null)
                    return default(V);
                V target = backing1.Target as V;
                if ((object)target == null)
                {
                    this.Remove(key);
                    WeakReference backing2 = this.backingDictionary[key];
                }
                return target;
            }
            set
            {
                if (this.backingDictionary.Count == this.capacity)
                {
                    this.EnumerateAndScavenge((List<K>)null);
                    if (this.backingDictionary.Count == this.capacity)
                        this.capacity = this.backingDictionary.Count * 2;
                }
                this.backingDictionary[key] = (object)value == null ? (WeakReference)null : new WeakReference((object)value);
            }
        }

        public bool Contains(K key)
        {
            V v;
            return this.TryGetValue(key, out v);
        }

        public bool TryGetValue(K key, out V value)
        {
            WeakReference weakReference;
            if (this.backingDictionary.TryGetValue(key, out weakReference))
            {
                if (weakReference == null)
                {
                    value = default(V);
                    return true;
                }
                value = weakReference.Target as V;
                if ((object)value != null)
                    return true;
                this.Remove(key);
                return false;
            }
            value = default(V);
            return false;
        }

        public bool Remove(K key)
        {
            return this.backingDictionary.Remove(key);
        }

        public IEnumerable<V> Values
        {
            get
            {
                foreach (WeakReference weakReference in this.backingDictionary.Values)
                {
                    V target = weakReference.Target as V;
                    if (weakReference.IsAlive)
                        yield return target;
                }
            }
        }

        public IEnumerable<K> Keys
        {
            get
            {
                return (IEnumerable<K>)this.backingDictionary.Keys;
            }
        }

        public IEnumerable<K> GetAliveKeys()
        {
            List<K> aliveKeys = new List<K>();
            this.EnumerateAndScavenge(aliveKeys);
            return (IEnumerable<K>)aliveKeys;
        }

        public void Scavenge()
        {
            this.EnumerateAndScavenge((List<K>)null);
        }

        private int EnumerateAndScavenge(List<K> aliveKeys)
        {
            List<K> kList = (List<K>)null;
            foreach (KeyValuePair<K, WeakReference> backing in this.backingDictionary)
            {
                if (backing.Value == null)
                    aliveKeys?.Add(backing.Key);
                else if ((object)(backing.Value.Target as V) == null)
                {
                    kList = kList ?? new List<K>();
                    kList.Add(backing.Key);
                }
                else
                    aliveKeys?.Add(backing.Key);
            }
            if (kList == null)
                return 0;
            foreach (K key in kList)
                this.backingDictionary.Remove(key);
            return kList.Count;
        }

        public void Clear()
        {
            this.backingDictionary.Clear();
        }
    }

}
