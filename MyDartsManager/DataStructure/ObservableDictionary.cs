using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.DataStructure
{
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public ObservableDictionary() : base() { }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection) { }

        public TValue this[TKey key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public IEnumerable<TKey> Keys => this.Select(kvp => kvp.Key);
        public IEnumerable<TValue> Values => this.Select(kvp => kvp.Value);

        public bool ContainsKey(TKey key)
        {
            return this.Any(kvp => kvp.Key.Equals(key));
        }

        public bool Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                Add(new KeyValuePair<TKey, TValue>(key, value));
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var kvp = this.FirstOrDefault(entry => entry.Key.Equals(key));
            if (!EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(kvp, default(KeyValuePair<TKey, TValue>)))
            {
                value = kvp.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public bool Remove(TKey key)
        {
            var kvp = this.FirstOrDefault(entry => entry.Key.Equals(key));
            if (!EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(kvp, default(KeyValuePair<TKey, TValue>)))
            {
                return Remove(kvp);
            }
            return false;
        }

        private TValue GetValue(TKey key)
        {
            if (TryGetValue(key, out TValue value))
            {
                return value;
            }
            throw new KeyNotFoundException("The key was not found in the dictionary.");
        }

        private void SetValue(TKey key, TValue value)
        {
            var kvp = this.FirstOrDefault(entry => entry.Key.Equals(key));
            if (!EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(kvp, default(KeyValuePair<TKey, TValue>)))
            {
                int index = IndexOf(kvp);
                SetItem(index, new KeyValuePair<TKey, TValue>(key, value));
            }
            else
            {
                throw new KeyNotFoundException("The key was not found in the dictionary.");
            }
        }
    }
}
