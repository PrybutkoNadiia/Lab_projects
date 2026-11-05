using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTreeApp
{
    public class TreeNode<T>
    {
        public T Data { get; set; }
        public TreeNode<T>? Left { get; set; }
        public TreeNode<T>? Right { get; set; }

        public TreeNode(T data)
        {
            Data = data;
        }
    }

    public class BinaryTree<T> : IEnumerable<T> where T : class
    {
        public TreeNode<T>? Root { get; private set; }
        private readonly IComparer<T>? _customComparer;

        public BinaryTree(IComparer<T>? customComparer = null)
        {
            _customComparer = customComparer;
        }

        private int Compare(T x, T y)
        {
            return (_customComparer ?? Comparer<T>.Default).Compare(x, y);
        }

        public void Add(T data)
        {
            if (Root == null)
            {
                Root = new TreeNode<T>(data);
                return;
            }

            TreeNode<T> current = Root;
            while (true)
            {
                int cmp = Compare(data, current.Data);
                if (cmp < 0)
                {
                    if (current.Left == null)
                    {
                        current.Left = new TreeNode<T>(data);
                        break;
                    }
                    current = current.Left;
                }
                else
                {
                    if (current.Right == null)
                    {
                        current.Right = new TreeNode<T>(data);
                        break;
                    }
                    current = current.Right;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TreePreOrderEnumerator(Root);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class TreePreOrderEnumerator : IEnumerator<T>
        {
            private readonly TreeNode<T>? _root;
            private Stack<TreeNode<T>> _stack = new Stack<TreeNode<T>>();
            private TreeNode<T>? _current;

            public TreePreOrderEnumerator(TreeNode<T>? root)
            {
                _root = root;
                Reset();
            }

            public T Current => _current != null ? _current.Data : throw new InvalidOperationException();

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_stack.Count == 0)
                    return false;

                _current = _stack.Pop();

                if (_current.Right != null)
                    _stack.Push(_current.Right);
                if (_current.Left != null)
                    _stack.Push(_current.Left);

                return true;
            }

            public void Reset()
            {
                _stack = new Stack<TreeNode<T>>();
                if (_root != null)
                    _stack.Push(_root);
                _current = null;
            }

            public void Dispose() { }
        }
    }
}