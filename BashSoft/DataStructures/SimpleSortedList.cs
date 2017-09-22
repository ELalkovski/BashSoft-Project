namespace BashSoft.DataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using BashSoft.Contracts;

    public class SimpleSortedList<T> : ISimpleOrderedBag<T> where T : IComparable<T>
    {
        private const int DefaultSize = 16;

        private T[] innerCollection;
        private int size;
        private IComparer<T> comparison;
        private int capacity;

        public SimpleSortedList(IComparer<T> comparer, int capacity)
        {
            this.comparison = comparer;
            this.InitializeInnerCollection(capacity);
        }

        public SimpleSortedList(int capacity)
            : this(Comparer<T>.Create((x, y) => x.CompareTo(y)), capacity)
        {
        }

        public SimpleSortedList(IComparer<T> comparer)
            : this(comparer, DefaultSize)
        {
        }

        public SimpleSortedList()
            : this(Comparer<T>.Create((x, y) => x.CompareTo(y)), DefaultSize)
        {
        }

        public int Size
        {
            get { return this.size; }
        }

        public int Capacity
        {
            get { return this.innerCollection.Length; }
        }

        public void InitializeInnerCollection(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("Capacity cannot  be negative!");
            }

            this.innerCollection = new T[capacity];
        }
        
        public void Add(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("You can't add empty element");
            }

            if (this.innerCollection.Length == this.size)
            {
                this.Resize();
            }

            this.innerCollection[this.size] = element;
            this.size++;

            this.SortCollectionWithSelectionSort();
        }

        public void AddAll(ICollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Input collection cannot be null");
            }

            if (this.size + collection.Count >= this.innerCollection.Length)
            {
                this.MultiResize(collection);
            }

            foreach (var element in collection)
            {
                this.innerCollection[this.size] = element;
                this.size++;
            }

            this.SortCollectionWithSelectionSort();
        }

        public bool Remove(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("Input element cannot be null");
            }

            bool hasBeenRemoved = false;
            int indexOfRemovedElement = 0;

            for (int i = 0; i < this.size; i++)
            {
                if (this.innerCollection[i].Equals(element))
                {
                    indexOfRemovedElement = i;
                    this.innerCollection[i] = default(T);
                    hasBeenRemoved = true;
                    break;
                }
            }

            if (hasBeenRemoved)
            {
                for (int i = indexOfRemovedElement; i < this.size - 1; i++)
                {
                    this.innerCollection[i] = this.innerCollection[i + 1];
                }

                this.innerCollection[this.size - 1] = default(T);
                this.size--;
            }

            return hasBeenRemoved;
        }

        public string JoinWith(string joiner)
        {
            if (joiner == null)
            {
                throw new ArgumentNullException("Joiner cannot be null");
            }

            var sb = new StringBuilder();

            foreach (var element in this)
            {
                sb.Append(element);
                sb.Append(joiner);
            }

            sb.Remove(sb.Length - joiner.Length, joiner.Length);
            return sb.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.size; i++)
            {
                yield return this.innerCollection[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void Resize()
        {
            T[] newCollection = new T[this.Size * 2];
            Array.Copy(this.innerCollection, newCollection, this.Size);
            this.innerCollection = newCollection;
        }

        private void MultiResize(ICollection<T> collection)
        {
            int newSize = this.innerCollection.Length * 2;

            while (this.Size + collection.Count >= newSize)
            {
                newSize *= 2;
            }

            T[] newCollection = new T[newSize];
            Array.Copy(this.innerCollection, newCollection, this.size);
            this.innerCollection = newCollection;
        }

        private void SortCollectionWithSelectionSort()
        {
            var currMinIndex = 0;
            T temp;

            for (int i = 0; i < this.size; i++)
            {
                currMinIndex = i;

                for (int j = i + 1; j < this.size; j++)
                {
                    if (this.comparison.Compare(this.innerCollection[j], this.innerCollection[currMinIndex]) == -1)
                    {
                        currMinIndex = j;
                    }
                }

                temp = this.innerCollection[currMinIndex];
                this.innerCollection[currMinIndex] = this.innerCollection[i];
                this.innerCollection[i] = temp;
            }
        }
    }
}