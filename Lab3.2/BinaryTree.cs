using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab3_2
{
    // where T : class — структури заборонені
    public class BinaryTree<T> : IEnumerable<T> where T : class
    {
        private class Node
        {
            public T Value;
            public Node Left;
            public Node Right;
            public Node(T value) { Value = value; }
        }

        private Node _root;
        private readonly IComparer<T> _comparer;

        public int Count { get; private set; }

        public BinaryTree() : this(null) { }

        public BinaryTree(IComparer<T> comparer)
        {
            // Якщо компаратор не переданий — використовується IComparable<T> елемента
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public void Add(T value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            _root = Insert(_root, value);
            Count++;
        }

        private Node Insert(Node node, T value)
        {
            if (node == null) return new Node(value);
            if (_comparer.Compare(value, node.Value) < 0)
                node.Left = Insert(node.Left, value);
            else
                node.Right = Insert(node.Right, value);
            return node;
        }

        public bool Contains(T value)
        {
            Node cur = _root;
            while (cur != null)
            {
                int c = _comparer.Compare(value, cur.Value);
                if (c == 0) return true;
                cur = c < 0 ? cur.Left : cur.Right;
            }
            return false;
        }

        public bool Remove(T value)
        {
            bool removed = false;
            _root = Remove(_root, value, ref removed);
            if (removed) Count--;
            return removed;
        }

        private Node Remove(Node node, T value, ref bool removed)
        {
            if (node == null) return null;
            int c = _comparer.Compare(value, node.Value);
            if (c < 0) node.Left = Remove(node.Left, value, ref removed);
            else if (c > 0) node.Right = Remove(node.Right, value, ref removed);
            else
            {
                removed = true;
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;
                Node min = node.Right;
                while (min.Left != null) min = min.Left;
                node.Value = min.Value;
                bool dummy = false;
                node.Right = Remove(node.Right, min.Value, ref dummy);
            }
            return node;
        }

        // Власний ітератор
        public IEnumerator<T> GetEnumerator() => new PreorderEnumerator(_root);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Прямий обхід (preorder): корінь -> ліве -> праве, через стек
        private class PreorderEnumerator : IEnumerator<T>
        {
            private readonly Node _start;
            private readonly Stack<Node> _stack = new Stack<Node>();
            private T _current;

            public PreorderEnumerator(Node root)
            {
                _start = root;
                Reset();
            }

            public T Current => _current;
            object IEnumerator.Current => _current;

            public bool MoveNext()
            {
                if (_stack.Count == 0) return false;
                Node node = _stack.Pop();
                _current = node.Value;
                if (node.Right != null) _stack.Push(node.Right); // праве першим у стек,
                if (node.Left != null) _stack.Push(node.Left);   // щоб ліве вийшло раніше
                return true;
            }

            public void Reset()
            {
                _stack.Clear();
                _current = null;
                if (_start != null) _stack.Push(_start);
            }

            public void Dispose() { }
        }
    }
}