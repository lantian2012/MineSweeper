using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AIMineComponent
{
    public class SimpleList : IList
    {
        private object[] _contents;
        private int _count;
        private int _intNum;

        public SimpleList()  : this(16 * 16, 2)      {    }
        public SimpleList(int a, int MaxNum)
        {
            _contents = new object[a];
            _count = 0;
            _intNum = MaxNum;
        }

        // IList Members
        public int Add(object value)
        {
            if (_count < _contents.Length)
            {
                _contents[_count] = value;
                _count++;

                return (_count - 1);
            }
            else
            {
                return -1;
            }
        }

        public void Clear()
        {
            _count = 0;
        }

        public bool Contains(object value)
        {
            bool inList = false;
            for (int i = 0; i < Count; i++)
            {
                inList = true;
                for (int j = 0; j < _intNum; j++)
                {
                    if (((int[])_contents[i])[j] != ((int[])value)[j])
                    {
                        break;
                        inList = false;
                    }
                    if (inList) return inList;

                }
            }
            return inList;
        }

        public int IndexOf(object value)
        {
            int itemIndex = -1;
            bool isEqual;
            for (int i = 0; i < Count; i++)
            {
                isEqual = true;
                for (int j = 0; j < _intNum; j++)
                {
                    if (((int[])_contents[i])[j] != ((int[])value)[j])
                    {
                        break;
                        isEqual = false;
                    }
                    if (isEqual) return itemIndex = i;

                }
            }
            return itemIndex;
        }

        public void Insert(int index, object value)
        {
            if ((_count + 1 <= _contents.Length) && (index < Count) && (index >= 0))
            {
                _count++;

                for (int i = Count - 1; i > index; i--)
                {
                    _contents[i] = _contents[i - 1];
                }
                _contents[index] = value;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Remove(object value)
        {
            RemoveAt(IndexOf(value));
        }

        public void RemoveAt(int index)
        {
            if ((index >= 0) && (index < Count))
            {
                for (int i = index; i < Count - 1; i++)
                {
                    _contents[i] = _contents[i + 1];
                }
                _count--;
            }
        }

        public object this[int index]
        {
            get
            {
                return _contents[index];
            }
            set
            {
                _contents[index] = value;
            }
        }

        // ICollection Members

        public void CopyTo(Array array, int index)
        {
            int j = index;
            for (int i = 0; i < Count; i++)
            {
                array.SetValue(_contents[i], j);
                j++;
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        // Return the current instance since the underlying store is not
        // publicly available.
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        // IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            // Refer to the IEnumerator documentation for an example of
            // implementing an enumerator.
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
