namespace LRUCache
{
    public class LRUCache<TValue>
    {
        private int maxItems;
        private readonly Dictionary<int, LinkedListNode<KeyValuePair<int, TValue>>> cache;
        private readonly LinkedList<KeyValuePair<int, TValue>> itemList;
        private Action<KeyValuePair<int, TValue>>? onItemEvicted;

        public LRUCache(int maxItems, Action<KeyValuePair<int, TValue>>? onItemEvicted = null) {
            this.maxItems = maxItems;
            cache = new Dictionary<int, LinkedListNode<KeyValuePair<int, TValue>>>();
            itemList = new LinkedList<KeyValuePair<int, TValue>>();
            this.onItemEvicted = onItemEvicted;
        }

        public TValue Get(int key)
        {
            lock (cache)
            {
                lock (itemList)
                {
                    if (cache.TryGetValue(key, out LinkedListNode<KeyValuePair<int, TValue>>? node))
                    {
                        itemList.Remove(node);
                        itemList.AddFirst(node);
                        return node.Value.Value;
                    }
                    else
                    {
                        throw new KeyNotFoundException("Not found");
                    }
                }
            }
        }

        public void Add(int key, TValue value)
        {
            lock (cache)
            {
                lock (itemList)
                {
                    // remove if it exists already
                    if (cache.TryGetValue(key, out LinkedListNode<KeyValuePair<int, TValue>>? node))
                    {
                        cache.Remove(key);
                        itemList.Remove(node);
                    }

                    // add new item
                    var listItem = new LinkedListNode<KeyValuePair<int, TValue>>(new KeyValuePair<int, TValue>(key, value));
                    itemList.AddFirst(listItem);
                    cache[key] = listItem;

                    // remove LRU item if over capacity
                    if (cache.Count > maxItems)
                    {
                        var lastItem = itemList.Last;
                        itemList.RemoveLast();
                        cache.Remove(lastItem.Value.Key);
                        if (onItemEvicted != null)
                        {
                            onItemEvicted(lastItem.Value);
                        }
                    }

                }
            }
        }
    }
}
