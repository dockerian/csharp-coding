/*
 ************************************************************
 * Source: LinkedList.cs
 * System: Microsoft Windows with .NET Framework
 * Author: Jason Zhu <jason_zhuyx@hotmail.com>
 * Update: 2011-01-21 Initial version
 *         
 ************************************************************
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace Common.Library
{
    /// <summary>
    /// A dual-direction linked list.
    /// </summary>
    public class BiDirectionalList<TNodeValue> : SingleLinkedList<TNodeValue>
    {
        private new Node<TNodeValue> _current_node;
        private new Node<TNodeValue> _head_node;
        private new Node<TNodeValue> _last_node;

        #region class BiDirectionalList - constructors

        public BiDirectionalList()
        {
        }
        public BiDirectionalList(TNodeValue[] array)
        {
            if (array == null || array.Length == 0) return;

            int count = 0;

            foreach(TNodeValue nodeValue in array)
            {
                if (count == 0)
                {
                    this.Initialize(nodeValue);
                }
                else // adding to list
                {
                    this.AddNode(nodeValue);
                }
                count++;
            }
        }
        public BiDirectionalList(params object[] nodes)
        {
            if (nodes == null || nodes.Length == 0) return;

            int count = 0;

            foreach(object item in nodes)
            {
                TNodeValue nodeValue;

                if (item is TNodeValue)
                {
                    nodeValue = (TNodeValue) item;
                }
                else if (item is Node<TNodeValue>)
                {
                    nodeValue = (item as Node<TNodeValue>).Value;
                }
                else // invalid item
                {
                    continue;
                }
                if (count == 0)
                {
                    this.Initialize(nodeValue);
                }
                else // adding to list
                {
                    this.AddNode(nodeValue);
                }
                count++;
            }
        }
        public BiDirectionalList(Node<TNodeValue> node)
        {
            this.Initialize(node);
        }
        public BiDirectionalList(TNodeValue nodeValue)
        {
            this.Initialize(nodeValue);
        }

        #endregion

        #region class BiDirectionalList - functions

        private Node<TNodeValue> DeleteCurrentNode(Node<TNodeValue> current)
        {
            if (this._count == 0 || current == null) return null;

            Node<TNodeValue> previous = current.Previous;
            Node<TNodeValue> next = current.Next;

            this._current_node = previous;

            if (previous != null)
            {
                previous.Next = next;
            }
            else // current node is the head node
            {
                this._current_node = next;
                this._head_node = next;
            }
            if (next != null)
            {
                next.Previous = previous;
            }
            else // current node is the last node
            {
                this._last_node = previous;
            }

            this._count--;

            return current;
        }

        private Node<TNodeValue> Initialize(Node<TNodeValue> node)
        {
            if (node != null)
            {
                this._current_node = node;
                this._current_node.Previous = null;
                this._current_node.Next = null;
                this._head_node = this._current_node;
                this._last_node = this._current_node;
                this._count++;
            }
            return node;
        }
        private Node<TNodeValue> Initialize(TNodeValue nodeValue)
        {
            Node<TNodeValue> node = new Node<TNodeValue>(nodeValue, null, null);

            return this.Initialize(node);
        }

        private Node<TNodeValue> InsertAfterNode(Node<TNodeValue> newNode, Node<TNodeValue> theNode)
        {
            if (newNode == null) return null;
            if (theNode == null)
            {
                if (_count == 0)
                {
                    return Initialize(newNode);
                }
                return InsertAfterNode(newNode, _last_node);
            }

            Node<TNodeValue> next = theNode.Next;

            theNode.Next = newNode;
            newNode.Previous = theNode;
            newNode.Next = next;

            if (next != null)
            {
                // fix previous link of the original next node
                next.Previous = theNode.Next;
            }
            else // theNode is at the end of the list
            {
                // fix the last node
                this._last_node = theNode.Next;
            }

            this._count++;

            // remember to change current node
            this._current_node = theNode.Next;

            return theNode.Next;
        }
        private Node<TNodeValue> InsertAfterNode(TNodeValue newValue, Node<TNodeValue> theNode)
        {
            if (theNode == null)
            {
                if (_count == 0)
                {
                    return Initialize(newValue);
                }
                return InsertAfterNode(newValue, _last_node);
            }

            Node<TNodeValue> next = theNode.Next;
            theNode.Next = new Node<TNodeValue>(newValue, theNode, next);

            if (next != null)
            {
                // fix previous link of the original next node
                next.Previous = theNode.Next;
            }
            else // theNode is at the end of the list
            {
                // fix the last node
                this._last_node = theNode.Next;
            }

            this._count++;

            // remember to change current node
            this._current_node = theNode.Next;

            return theNode.Next;
        }

        private Node<TNodeValue> InsertBeforeNode(Node<TNodeValue> newNode, Node<TNodeValue> current = null)
        {
            if (current == null)
            {
                if (_count == 0)
                {
                    return Initialize(newNode);
                }
                return InsertBeforeNode(newNode, _head_node);
            }

            Node<TNodeValue> previous = current.Previous;

            newNode.Previous = previous;
            newNode.Next = current;

            if (previous != null)
            {
                current.Previous = newNode;
                // fix next link of the original previous node
                previous.Next = current.Previous;
            }
            else // current node is at the beginning of the list
            {
                current.Previous = newNode;
                // fix the head node
                _head_node = current.Previous;
            }

            // remember to change current node
            this._current_node = current.Previous;

            this._count++;

            return this._current_node;
        }
        private Node<TNodeValue> InsertBeforeNode(TNodeValue newValue, Node<TNodeValue> current = null)
        {
            if (current == null)
            {
                if (_count == 0)
                {
                    return Initialize(newValue);
                }
                return InsertBeforeNode(newValue, _head_node);
            }

            Node<TNodeValue> previous = current.Previous;

            if (previous != null)
            {
                current.Previous = new Node<TNodeValue>(newValue, previous, current);
                // fix next link of the original previous node
                previous.Next = current.Previous;
            }
            else // current node is at the beginning of the list
            {
                current.Previous = new Node<TNodeValue>(newValue, null, current);
                // fix the head node
                _head_node = current.Previous;
            }

            // remember to change current node
            this._current_node = current.Previous;

            this._count++;

            return this._current_node;
        }

        #endregion

        #region class BiDirectionalList - properties

        /*//
        public int Count
        {
            get { return _count; }
        }
        //*/

        public new Node<TNodeValue> Current
        {
            get { return _current_node; }
        }

        public new Node<TNodeValue> Head
        {
            get { return _head_node; }
        }

        /*//
        public bool IsEmpty
        {
            get { return _count == 0; }
        }
        //*/

        public new Node<TNodeValue> Last
        {
            get { return _last_node; }
        }

        #endregion

        #region class BiDirectionalList - methods

        #region class BiDirectionalList - methods :: AddNode
        /// <summary>
        /// AddNode(newValue, position) adds a new node after specified node by position in the list.
        /// </summary>
        /// <param name="newValue">A new node value to be added in the list</param>
        /// <param name="position">position of a node in the list</param>
        /// <returns>Added node in the list</returns>
        public new Node<TNodeValue> AddNode(TNodeValue newValue, int position)
        {
            Node<TNodeValue> current = FindNode(position);

            if (current != null)
            {
                return InsertAfterNode(newValue, current);
            }
            return null;
        }

        /// <summary>
        /// AddNode(newValue, nodeValue) adds a new node after a node with specified node value in the list.
        /// </summary>
        /// <param name="newValue">A new node value to be added in the list</param>
        /// <param name="nodeValue">specified node value in the list</param>
        /// <returns>Added node in the list</returns>
        public new Node<TNodeValue> AddNode(TNodeValue newValue, TNodeValue nodeValue)
        {
            Node<TNodeValue> theNode = FindNode(nodeValue);

            if (theNode != null)
            {
                return InsertAfterNode(newValue, theNode);
            }
            return null;
        }

        /// <summary>
        /// AddNode(newValue, node = null) inserts a new node after a specified node in the list.
        /// </summary>
        /// <param name="newValue">A new node value to be added in the list</param>
        /// <param name="node">specified node in the list</param>
        /// <returns>Added node in the list</returns>
        public Node<TNodeValue> AddNode(TNodeValue newValue, Node<TNodeValue> node = null)
        {
            Node<TNodeValue> current = node == null ? _last_node : FindNode(node);

            return InsertAfterNode(newValue, current);
        }

        public Node<TNodeValue> AddNode(Node<TNodeValue> newNode, Node<TNodeValue> node = null)
        {
            Node<TNodeValue> current = node == null ? _last_node : FindNode(node);

            return InsertAfterNode(newNode, current);
        }

        #endregion

        /// <summary>
        /// Clear the linked list.
        /// </summary>
        public new void Clear()
        {
            Node<TNodeValue> node = this._head_node;

            while(node != null)
            {
                this._current_node = node;
                this._current_node.Previous = null;
                this._current_node.Next = null;

                node = node.Next;
            }
            this._current_node = this._head_node = this._last_node = null;
            this._count = 0;
        }

        /// <summary>
        /// Clone to a new BiDirectionalList.
        /// </summary>
        /// <returns>Cloned linked list</returns>
        public new BiDirectionalList<TNodeValue> Clone()
        {
            BiDirectionalList<TNodeValue> newlist = new BiDirectionalList<TNodeValue>();

            Node<TNodeValue> node = this.Head;
            while(node != null)
            {
                newlist.AddNode(node.Value);
                node = node.Next;
            }

            return newlist;
        }

        public new bool Equals(BiDirectionalList<TNodeValue> that, bool compareValue = false)
        {
            bool result = true;
            Node<TNodeValue> node_this = this.Head;
            Node<TNodeValue> node_that = that.Head;

            while(node_this != null && node_that != null)
            {
                if (compareValue)
                {
                    if (node_this.Equals(node_that) == false)
                    {
                        result = false;
                        break;
                    }
                }
                else if (node_this != node_that)
                {
                    result = false;
                    break;
                }
                node_that = node_that.Next;
                node_this = node_this.Next;
            }

            result = result && (node_that == null && node_this == null);

            return result;
        }

        #region class BiDirectionalList - methods :: DeleteNode
        /// <summary>
        /// DeleteNode(position) deletes node by specified position in the list.
        /// </summary>
        public new Node<TNodeValue> DeleteNode(int position)
        {
            Node<TNodeValue> theNode = FindNode(position);

            if (theNode != null)
            {
                return DeleteCurrentNode(theNode);
            }
            return null;
        }

        /// <summary>
        /// DeleteNode(node = null) deletes node (or current node) in the list.
        /// </summary>
        public Node<TNodeValue> DeleteNode(Node<TNodeValue> node = null)
        {
            Node<TNodeValue> current = node == null ? _current_node : FindNode(node);

            return DeleteCurrentNode(current);
        }

        /// <summary>
        /// DeleteNode(nodeValue) deletes first node with specified node value in the list.
        /// </summary>
        public new Node<TNodeValue> DeleteNode(TNodeValue nodeValue)
        {
            Node<TNodeValue> node = FindNode(nodeValue);

            if (node != null)
            {
                return DeleteCurrentNode(node);
            }
            return null;
        }

        #endregion

        #region class BiDirectionalList - methods :: FindNode

        /// <summary>
        /// FindNode(sequence) finds a node by (zero-based) sequence in the list
        /// </summary>
        /// <param name="sequence">Finding node sequence in the list</param>
        /// <returns>Found node in the list</returns>
        public new Node<TNodeValue> FindNode(int sequence)
        {
            if (sequence >= _count || sequence < -_count)
            {
                return null;
            }
            if (sequence < 0)
            {
                sequence += _count;
            }

            bool startFromHead = sequence < _count/2;
            int currentIndex = startFromHead ? 0 : _count - 1;
            Node<TNodeValue> node = startFromHead ? this.Head : this.Last;

            while(currentIndex != sequence && node != null)
            {
                if (startFromHead)
                {
                    node = node.Next; currentIndex++;
                }
                else //search backward
                {
                    node = node.Previous; currentIndex--;
                }
            }
            return _current_node = node; // should not be null
        }

        /// <summary>
        /// FindNode(node) finds a node in the list
        /// </summary>
        /// <param name="node">Finding node in the list</param>
        /// <returns>Found node in the list</returns>
        public Node<TNodeValue> FindNode(Node<TNodeValue> node)
        {
            Node<TNodeValue> current = this.Head;

            while(current != null)
            {
                if (current == node)
                {
                    return _current_node = node;
                }
                current = current.Next;
            }
            return null;
        }

        /// <summary>
        /// FindNode(nodeValue) finds the first node by node value in the list
        /// </summary>
        /// <param name="nodeValue">Finding node value in the list</param>
        /// <returns>Found node in the list</returns>
        public new Node<TNodeValue> FindNode(TNodeValue nodeValue)
        {
            Node<TNodeValue> node = this.Head;

            while(node != null && node.CompareTo(nodeValue) != 0)
            {
                node = node.Next;
            }
            return node;
        }

        /// <summary>
        /// FindNode(sequence) finds a node by backward (zero-based) sequence in the list
        /// </summary>
        /// <param name="position">Finding node sequence (zero-based, backward) in the list</param>
        /// <returns>Found node in the list</returns>
        public new Node<TNodeValue> FindNodeBackward(int position)
        {
            Node<TNodeValue> node1 = this.Head;

            for (int i = 0; i < position; i++)
            {
                if (node1.Next != null)
                {
                    node1 = node1.Next;
                }
                else
                {
                    return null;
                }
            }

            Node<TNodeValue> node2 = this.Head;

            while(node1.Next != null)
            {
                node1 = node1.Next;
                node2 = node2.Next;
            }
            return node2;
        }

        #endregion

        #region class BiDirectionalList - methods :: InsertNode
        /// <summary>
        /// InsertNode(node) inserts a new node in front of specified node by position in the list.
        /// </summary>
        /// <param name="newValue">A node value to be inserted as a new node in the list</param>
        /// <param name="position">position of a node in the list</param>
        /// <returns>Inserted node in the list</returns>
        public new Node<TNodeValue> InsertNode(TNodeValue newValue, int position)
        {
            if (position == _count)
            {
                return InsertAfterNode(newValue, _last_node);
            }

            Node<TNodeValue> current = FindNode(position);

            if (current != null)
            {
                return InsertBeforeNode(newValue, current);
            }
            return null;
        }

        /// <summary>
        /// InsertNode(newValue, nodeValue) inserts a new node in front of a node with specified node value in the list.
        /// </summary>
        /// <param name="newValue">A node value to be inserted as a new node in the list</param>
        /// <param name="nodeValue">specified node value in the list</param>
        /// <returns>Inserted node in the list</returns>
        public new Node<TNodeValue> InsertNode(TNodeValue newValue, TNodeValue nodeValue)
        {
            Node<TNodeValue> current = FindNode(nodeValue);

            if (current != null)
            {
                return InsertBeforeNode(newValue, current);
            }
            return null;
        }

        /// <summary>
        /// InsertNode(newValue, node = null) inserts a new node in front of a specified node in the list.
        /// </summary>
        /// <param name="newValue">A node value to be inserted as a new node in the list</param>
        /// <param name="node">specified node in the list, or assuming the head node</param>
        /// <returns>Inserted node in the list</returns>
        public Node<TNodeValue> InsertNode(TNodeValue newValue, Node<TNodeValue> node = null)
        {
            Node<TNodeValue> current = node == null ? _head_node : FindNode(node);

            return InsertBeforeNode(newValue, current);
        }

        public Node<TNodeValue> InsertNode(Node<TNodeValue> newNode, Node<TNodeValue> node = null)
        {
            Node<TNodeValue> current = node == null ? _head_node : FindNode(node);

            return InsertBeforeNode(newNode, current);
        }

        #endregion

        /// <summary>
        /// Reverse the list.
        /// </summary>
        public new void Reverse()
        {
            Node<TNodeValue> node = this.Head;

            this._head_node = this.Last;
            this._last_node = node;

            while(node != null)
            {
                Node<TNodeValue> previous = node.Previous;
                Node<TNodeValue> next = node.Next;

                node.Next = previous;
                node.Previous = next;

                node = next;
            }
        }

        #region class BiDirectionalList - methods :: Seek

        public new Node<TNodeValue> SeekToHead()
        {
            return this._current_node = _head_node;
        }

        public new Node<TNodeValue> SeekToLast()
        {
            return this._current_node = _last_node;
        }

        /// <summary>
        /// Set current node to the next one in the list.
        /// </summary>
        public new Node<TNodeValue> SeekToNext()
        {
            if (this._current_node != null && this._current_node.Next != null)
            {
                return this._current_node = this._current_node.Next;
            }
            return null;
        }

        /// <summary>
        /// Set current node to the previous one in the list.
        /// </summary>
        public Node<TNodeValue> SeekToPrevious()
        {
            if (this._current_node != null && this._current_node.Previous != null)
            {
                return this._current_node = this._current_node.Previous;
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Clone contents (values) to a new array.
        /// </summary>
        /// <returns>NodeValueType array</returns>
        public new TNodeValue[] ToArray()
        {
            int i = 0;
            TNodeValue[] array = new TNodeValue[this._count];

            Node<TNodeValue> node = this.Head;

            while(node != null)
            {
                array[i++] = node.Value;
                node = node.Next;
            }

            return array;
        }

        public override string ToString()
        {
            TNodeValue[] array = this.ToArray();

            string result = string.Empty;

            for (int i = 0; i < array.Length; i++)
            {
                if (i == 0)
                {
                    result = array[i].ToString();
                }
                else //appending to result
                {
                    result += ", " + array[i].ToString();
                }
            }

            return result;
        }

        # endregion

    }// class BiDirectionalList


    /// <summary>
    /// A node for (dual-direction) linked list
    /// </summary>
    public class Node<TNodeValue> : SingleNode<TNodeValue>
    {
        protected Node<TNodeValue> previousNode;
        protected new Node<TNodeValue> nextNode;

        #region class Node - constructors

        public Node(Node<TNodeValue> node) : base(node)
        {
            this.nodeValue = node.Value;
            this.previousNode = node.Previous;
            this.nextNode = node.Next;
        }

        public Node(TNodeValue nodeValue) : base(nodeValue)
        {
            this.nodeValue = nodeValue;
        }
        public Node(TNodeValue nodeValue, Node<TNodeValue> previous, Node<TNodeValue> next) : base(nodeValue, next)
        {
            this.nodeValue = nodeValue;
            this.previousNode = previous;
            this.nextNode = next;
        }

        #endregion

        #region class Node - properties

        public new Node<TNodeValue> Next
        {
            get { return nextNode; }
            set { nextNode = value; }
        }

        public Node<TNodeValue> Previous
        {
            get { return previousNode; }
            set { previousNode = value; }
        }

        #endregion

        #region class Node - methods

        public int CompareTo(Node<TNodeValue> otherNode)
        {
            if (this.Value == null) return -1;
            if (otherNode == null || otherNode.Value == null) return 1;

            return this.CompareTo(otherNode.Value);
        }

        public override bool Equals(object otherNode)
        {
            if (otherNode is Node<TNodeValue>)
            {
                return this.CompareTo((Node<TNodeValue>)otherNode) == 0;
            }
            return this.GetHashCode().CompareTo(otherNode.GetHashCode()) == 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        #endregion

    }// class Node


    /// <summary>
    /// A single-direction linked list.
    /// </summary>
    public class SingleLinkedList<TNodeValue>
    {
        protected int _count = 0;
        protected SingleNode<TNodeValue> _current_node;
        protected SingleNode<TNodeValue> _head_node;
        protected SingleNode<TNodeValue> _last_node;

        #region class LinkedList - constructors

        public SingleLinkedList()
        {
        }
        public SingleLinkedList(SingleLinkedList<TNodeValue> aList)
        {
            if (aList != null)
            {
                SingleNode<TNodeValue> node = aList.Head;

                while(node != null)
                {
                    AddNode(node.Value);
                    node = node.Next;
                }
            }
        }
        public SingleLinkedList(TNodeValue[] array)
        {
            if (array == null || array.Length == 0) return;

            int count = 0;

            foreach(TNodeValue nodeValue in array)
            {
                if (count == 0)
                {
                    this.Initialize(nodeValue);
                }
                else // adding to list
                {
                    this.AddNode(nodeValue);
                }
                count++;
            }
        }
        public SingleLinkedList(params object[] nodes)
        {
            if (nodes == null || nodes.Length == 0) return;

            int count = 0;

            foreach(object item in nodes)
            {
                TNodeValue nodeValue;

                if (item is TNodeValue)
                {
                    nodeValue = (TNodeValue) item;
                }
                else if (item is SingleNode<TNodeValue>)
                {
                    nodeValue = (item as SingleNode<TNodeValue>).Value;
                }
                else // invalid item
                {
                    continue;
                }
                if (count == 0)
                {
                    this.Initialize(nodeValue);
                }
                else // adding to list
                {
                    this.AddNode(nodeValue);
                }
                count++;
            }
        }
        public SingleLinkedList(SingleNode<TNodeValue> node)
        {
            this.Initialize(node);
        }
        public SingleLinkedList(TNodeValue nodeValue)
        {
            this.Initialize(nodeValue);
        }

        #endregion

        #region class LinkedList - functions

        private SingleNode<TNodeValue> DeleteTheNode(SingleNode<TNodeValue> theNode)
        {
            if (this._count == 0 || theNode == null) return null;

            SingleNode<TNodeValue> previous = this.Head;
            SingleNode<TNodeValue> next = theNode.Next;

            while(previous != null && previous.Next != theNode)
            {
                previous = previous.Next;
            }

            this._current_node = previous;

            if (previous != null)
            {
                previous.Next = next;

                if (next == null) // theNode is the last node
                {
                    this._last_node = previous;
                }

                this._count--;
            }
            else // theNode is not in the list
            {
            }
            return theNode;
        }

        private SingleNode<TNodeValue> Initialize(SingleNode<TNodeValue> node)
        {
            if (node != null)
            {
                this._current_node = node;
                this._current_node.Next = null;
                this._head_node = this._current_node;
                this._last_node = this._current_node;
                this._count++;
            }
            return node;
        }
        private SingleNode<TNodeValue> Initialize(TNodeValue nodeValue)
        {
            SingleNode<TNodeValue> node = new SingleNode<TNodeValue>(nodeValue, null);

            return this.Initialize(node);
        }

        private SingleNode<TNodeValue> InsertAfterNode(SingleNode<TNodeValue> newNode, SingleNode<TNodeValue> theNode = null)
        {
            if (newNode == null) return null;
            if (theNode == null)
            {
                if (_count == 0)
                {
                    return Initialize(newNode);
                }
                else // adding after the last node
                {
                    return InsertAfterNode(newNode, _last_node);
                }
            }

            SingleNode<TNodeValue> next = theNode.Next;

            theNode.Next = newNode;
            newNode.Next = next;

            if (next == null) // theNode is at the end of the list
            {
                this._last_node = newNode;
            }

            this._count++;

            return newNode;
        }
        private SingleNode<TNodeValue> InsertAfterNode(TNodeValue newValue, SingleNode<TNodeValue> theNode = null)
        {
            if (theNode == null)
            {
                if (_count == 0)
                {
                    return Initialize(newValue);
                }
                else // adding after the last node
                {
                    return InsertAfterNode(newValue, _last_node);
                }
            }

            SingleNode<TNodeValue> next = theNode.Next;
            theNode.Next = new SingleNode<TNodeValue>(newValue, next);

            if (next == null) // theNode is at the end of the list
            {
                this._last_node = theNode.Next;
            }

            this._count++;

            return theNode.Next;
        }

        private SingleNode<TNodeValue> InsertBeforeNode(SingleNode<TNodeValue> newNode, SingleNode<TNodeValue> theNode = null)
        {
            if (newNode == null) return null;

            if (theNode == null)
            {
                if (_count == 0)
                {
                    return Initialize(newNode);
                }
                return InsertBeforeNode(newNode, _head_node);
            }

            SingleNode<TNodeValue> previous = this.Head;

            while(theNode != this.Head && previous != null && previous.Next != theNode)
            {
                previous = previous.Next;
            }

            if (previous != null)
            {
                if (theNode == this.Head)
                {
                    this._head_node = newNode;
                }
                newNode.Next = theNode;

                this._count++;
            }
            else // theNode is not in the list
            {
                return null;
            }

            return newNode;
        }
        private SingleNode<TNodeValue> InsertBeforeNode(TNodeValue newValue, SingleNode<TNodeValue> theNode = null)
        {
            if (theNode == null)
            {
                if (_count == 0)
                {
                    return Initialize(newValue);
                }
                return InsertBeforeNode(newValue, _head_node);
            }

            SingleNode<TNodeValue> previous = this.Head;
            SingleNode<TNodeValue> newNode = new SingleNode<TNodeValue>(newValue);

            while(theNode != this.Head && previous != null && previous.Next != theNode)
            {
                previous = previous.Next;
            }

            if (previous != null)
            {
                if (theNode == this.Head)
                {
                    this._head_node = newNode;
                }
                newNode.Next = theNode;

                this._count++;
            }
            else // theNode is not in the list
            {
                return null;
            }

            return newNode;
        }

        #endregion

        #region class LinkedList - properties

        public int Count
        {
            get { return _count; }
        }

        public SingleNode<TNodeValue> Current
        {
            get { return _current_node; }
        }

        public SingleNode<TNodeValue> Head
        {
            get { return _head_node; }
        }

        public bool IsEmpty
        {
            get { return _count == 0; }
        }

        public SingleNode<TNodeValue> Last
        {
            get { return _last_node; }
        }

        #endregion

        #region class LinkedList - methods

        #region class LinkedList - methods :: AddNode
        /// <summary>
        /// AddNode(newValue, position) adds a new node after specified node by position in the list.
        /// </summary>
        /// <param name="newValue">A new node value to be added in the list</param>
        /// <param name="position">position of a node in the list</param>
        /// <returns>Added node in the list</returns>
        public SingleNode<TNodeValue> AddNode(TNodeValue newValue, int position)
        {
            SingleNode<TNodeValue> current = FindNode(position);

            if (current != null)
            {
                return InsertAfterNode(newValue, current);
            }
            return null;
        }

        /// <summary>
        /// AddNode(newValue, nodeValue) adds a new node after a node with specified node value in the list.
        /// </summary>
        /// <param name="newValue">A new node value to be added in the list</param>
        /// <param name="nodeValue">specified node value in the list</param>
        /// <returns>Added node in the list</returns>
        public SingleNode<TNodeValue> AddNode(TNodeValue newValue, TNodeValue nodeValue)
        {
            SingleNode<TNodeValue> current = FindNode(nodeValue);

            if (current != null)
            {
                return InsertAfterNode(newValue, current);
            }
            return null;
        }

        /// <summary>
        /// AddNode(newValue, node = null) inserts a new node after a specified node in the list.
        /// </summary>
        /// <param name="newValue">A new node value to be added in the list</param>
        /// <param name="node">specified node in the list</param>
        /// <returns>Added node in the list</returns>
        public SingleNode<TNodeValue> AddNode(TNodeValue newValue, SingleNode<TNodeValue> node = null)
        {
            SingleNode<TNodeValue> current = node == null ? _last_node : FindNode(node);

            return InsertAfterNode(newValue, current);
        }

        public SingleNode<TNodeValue> AddNode(SingleNode<TNodeValue> newNode, SingleNode<TNodeValue> node = null)
        {
            SingleNode<TNodeValue> current = node == null ? _last_node : FindNode(node);

            return InsertAfterNode(newNode, current);
        }

        #endregion

        /// <summary>
        /// Clear the linked list.
        /// </summary>
        public void Clear()
        {
            SingleNode<TNodeValue> node = this._head_node;

            while(node != null)
            {
                this._current_node = node;
                this._current_node.Next = null;

                node = node.Next;
            }
            this._current_node = this._head_node = this._last_node = null;
            this._count = 0;
        }

        /// <summary>
        /// Clone to a new LinkedList.
        /// </summary>
        /// <returns>Cloned linked list</returns>
        public SingleLinkedList<TNodeValue> Clone()
        {
            SingleLinkedList<TNodeValue> newlist = new BiDirectionalList<TNodeValue>();

            SingleNode<TNodeValue> node = this.Head;
            while(node != null)
            {
                newlist.AddNode(node.Value);
                node = node.Next;
            }

            return newlist;
        }

        public bool Equals(BiDirectionalList<TNodeValue> that, bool compareValue = false)
        {
            bool result = true;
            SingleNode<TNodeValue> node_this = this.Head;
            SingleNode<TNodeValue> node_that = that.Head;

            while(node_this != null && node_that != null)
            {
                if (compareValue)
                {
                    if (node_this.Equals(node_that) == false)
                    {
                        result = false;
                        break;
                    }
                }
                else if (node_this != node_that)
                {
                    result = false;
                    break;
                }
                node_that = node_that.Next;
                node_this = node_this.Next;
            }

            result = result && (node_that == null && node_this == null);

            return result;
        }

        #region class LinkedList - methods :: DeleteNode

        /// <summary>
        /// Delete duplicates from unsorted list (with or without buffer)
        /// </summary>
        public void DeleteDuplicate()
        {
            
        }

        /// <summary>
        /// DeleteNode(position) deletes node by specified position in the list.
        /// </summary>
        public SingleNode<TNodeValue> DeleteNode(int position)
        {
            SingleNode<TNodeValue> current = FindNode(position);

            if (current != null)
            {
                return DeleteTheNode(current);
            }
            return null;
        }

        /// <summary>
        /// DeleteNode(node = null) deletes node (or current node) in the list.
        /// </summary>
        public SingleNode<TNodeValue> DeleteNode(SingleNode<TNodeValue> node = null)
        {
            SingleNode<TNodeValue> current = node == null ? _current_node : FindNode(node);

            return DeleteTheNode(current);
        }

        /// <summary>
        /// DeleteNode(nodeValue) deletes first node with specified node value in the list.
        /// </summary>
        public SingleNode<TNodeValue> DeleteNode(TNodeValue nodeValue)
        {
            SingleNode<TNodeValue> current = FindNode(nodeValue);

            if (current != null)
            {
                return DeleteTheNode(current);
            }
            return null;
        }

        #endregion

        #region class LinkedList - methods :: Find node

        /// <summary>
        /// FindLoop() returns the start node if there is a loop; otherwise returns null.
        /// </summary>
        /// <returns>Start of a loop</returns>
        /// <example>
        /// For any linked list, a loop (with a node's next pointing backward to a previous node) may look like this
        ///
        ///     0 (head) ->...->...-> x ->...-> y -> ... -> z -> [x]
        ///
        /// Here, x is the start of the loop, z is the node whose next pointing to x
        /// and if using two pointers from the head, one moves one node a time while
        /// another one moves two nodes a time (speed 2x), eventually they will meet
        /// at node y. Notice that y could be same as x.
        /// 
        /// Let m = y - x, and k = z - y + 1, n is how many rounds for node 2 in the loop before the meeting
        /// 
        ///     2 * (x + m) == x + m + n * (k + m)
        /// 
        /// so that
        /// 
        ///     x + m = n * (k + m)
        ///
        /// </example>
        public SingleNode<TNodeValue> FindLoop(bool fixingLoop = false)
        {
            SingleNode<TNodeValue> p1 = Head;
            SingleNode<TNodeValue> p2 = Head;
            SingleNode<TNodeValue> pz = Head;

            while(p2 != null)
            {
                p1 = p1.Next;
                p2 = p2.Next != null ? p2.Next.Next : null;

                if (p1 == p2) break; // a loop is detected
            }

            SingleNode<TNodeValue> start = null;

            if (p2 != null)
            {
                p1 = Head;

                while(p1 != p2)
                {
                    pz = p2;
                    p1 = p1.Next;
                    p2 = p2.Next;
                }
                start = p1;
            }

            if (fixingLoop && pz != null)
            {
                pz.Next = null;
            }
            return start;
        }

        /// <summary>
        /// FindNode(sequence) finds a node by (zero-based) sequence in the list
        /// </summary>
        /// <param name="sequence">Finding node sequence in the list</param>
        /// <returns>Found node in the list</returns>
        public SingleNode<TNodeValue> FindNode(int sequence)
        {
            if (sequence >= _count || sequence < -_count)
            {
                return null;
            }
            if (sequence < 0)
            {
                sequence += _count;
            }

            int currentIndex = 0;
            SingleNode<TNodeValue> node = this.Head;

            while(currentIndex != sequence && node != null)
            {
                node = node.Next; currentIndex++;
            }

            return node;
        }

        /// <summary>
        /// FindNode(node) finds a node in the list
        /// </summary>
        /// <param name="node">Finding node in the list</param>
        /// <returns>Found node in the list</returns>
        public SingleNode<TNodeValue> FindNode(SingleNode<TNodeValue> node)
        {
            SingleNode<TNodeValue> found = this.Head;

            while(found != null && found != node)
            {
                found = found.Next;
            }
            return found;
        }

        /// <summary>
        /// FindNode(nodeValue) finds the first node by node value in the list
        /// </summary>
        /// <param name="nodeValue">Finding node value in the list</param>
        /// <returns>Found node in the list</returns>
        public SingleNode<TNodeValue> FindNode(TNodeValue nodeValue)
        {
            SingleNode<TNodeValue> node = this.Head;

            while(node != null && node.CompareTo(nodeValue) != 0)
            {
                node = node.Next;
            }
            return node;
        }

        /// <summary>
        /// FindNode(sequence) finds a node by backward (zero-based) sequence in the list
        /// </summary>
        /// <param name="position">Finding node sequence (zero-based, backward) in the list</param>
        /// <returns>Found node in the list</returns>
        public SingleNode<TNodeValue> FindNodeBackward(int position)
        {
            SingleNode<TNodeValue> node1 = this.Head;

            for(int i = 0; i < position; i++)
            {
                if (node1.Next != null)
                {
                    node1 = node1.Next;
                }
                else
                {
                    return null;
                }
            }

            SingleNode<TNodeValue> node2 = this.Head;

            while(node1.Next != null)
            {
                node1 = node1.Next;
                node2 = node2.Next;
            }
            return node2;
        }

        #endregion

        #region class LinkedList - methods :: InsertNode
        /// <summary>
        /// InsertNode(node) inserts a new node in front of specified node by position in the list.
        /// </summary>
        /// <param name="newValue">A node value to be inserted as a new node in the list</param>
        /// <param name="position">position of a node in the list</param>
        /// <returns>Inserted node in the list</returns>
        public SingleNode<TNodeValue> InsertNode(TNodeValue newValue, int position)
        {
            if (position == _count)
            {
                return InsertAfterNode(newValue, _last_node);
            }

            SingleNode<TNodeValue> current = FindNode(position);

            if (current != null)
            {
                return InsertBeforeNode(newValue, current);
            }
            return null;
        }

        /// <summary>
        /// InsertNode(newValue, nodeValue) inserts a new node in front of a node with specified node value in the list.
        /// </summary>
        /// <param name="newValue">A node value to be inserted as a new node in the list</param>
        /// <param name="nodeValue">specified node value in the list</param>
        /// <returns>Inserted node in the list</returns>
        public SingleNode<TNodeValue> InsertNode(TNodeValue newValue, TNodeValue nodeValue)
        {
            SingleNode<TNodeValue> current = FindNode(nodeValue);

            if (current != null)
            {
                return InsertBeforeNode(newValue, current);
            }
            return null;
        }

        /// <summary>
        /// InsertNode(newValue, node = null) inserts a new node in front of a specified node in the list.
        /// </summary>
        /// <param name="newValue">A node value to be inserted as a new node in the list</param>
        /// <param name="node">specified node in the list, or assuming the head SingleNode</param>
        /// <returns>Inserted node in the list</returns>
        public SingleNode<TNodeValue> InsertNode(TNodeValue newValue, SingleNode<TNodeValue> node = null)
        {
            SingleNode<TNodeValue> current = node == null ? _head_node : FindNode(node);

            return InsertBeforeNode(newValue, current);
        }

        public SingleNode<TNodeValue> InsertNode(SingleNode<TNodeValue> newNode, SingleNode<TNodeValue> node = null)
        {
            SingleNode<TNodeValue> current = node == null ? _head_node : FindNode(node);

            return InsertBeforeNode(newNode, current);
        }

        #endregion

        /// <summary>
        /// Reverse the list.
        /// </summary>
        public void Reverse()
        {
            SingleNode<TNodeValue> node = this.Head;
            SingleNode<TNodeValue> next = node.Next;

            while(node.Next != null)
            {
                next.Next = node;
                node = next;
            }

            this._last_node = this._head_node;
            this._last_node.Next = null;
            this._head_node = node;
        }

        #region class LinkedList - methods :: Seek

        public SingleNode<TNodeValue> SeekToHead()
        {
            return this._current_node = _head_node;
        }

        public SingleNode<TNodeValue> SeekToLast()
        {
            return this._current_node = _last_node;
        }

        /// <summary>
        /// Set current node to the next one in the list.
        /// </summary>
        public SingleNode<TNodeValue> SeekToNext()
        {
            if (this._current_node != null && this._current_node.Next != null)
            {
                return this._current_node = this._current_node.Next;
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Clone contents (values) to a new array.
        /// </summary>
        /// <returns>NodeValueType array</returns>
        public TNodeValue[] ToArray()
        {
            int i = 0;
            TNodeValue[] array = new TNodeValue[this._count];

            SingleNode<TNodeValue> node = this.Head;

            while(node != null)
            {
                array[i++] = node.Value;
                node = node.Next;
            }

            return array;
        }

        public override string ToString()
        {
            TNodeValue[] array = this.ToArray();

            string result = string.Empty;

            for(int i = 0; i < array.Length; i++)
            {
                if (i == 0)
                {
                    result = array[i].ToString();
                }
                else //appending to result
                {
                    result += ", " + array[i].ToString();
                }
            }

            return result;
        }

        # endregion

    }// class LinkedList


    /// <summary>
    /// A simple node for linked list.
    /// </summary>
    public class SingleNode<TNodeValue> : IComparable<SingleNode<TNodeValue>>
    {
        protected SingleNode<TNodeValue> nextNode;
        protected TNodeValue nodeValue;

        #region class SingleNode - constructors

        public SingleNode(SingleNode<TNodeValue> node)
        {
            this.nodeValue = node.Value;
            this.nextNode = node.Next;
        }
        public SingleNode(TNodeValue nodeValue)
        {
            this.nodeValue = nodeValue;
        }
        public SingleNode(TNodeValue nodeValue, SingleNode<TNodeValue> next)
        {
            this.nodeValue = nodeValue;
            this.nextNode = next;
        }

        #endregion

        #region class SingleNode - properties

        public SingleNode<TNodeValue> Next
        {
            get { return nextNode; } set { nextNode = value; }
        }

        public TNodeValue Value
        {
            get { return nodeValue; } set { nodeValue = value; }
        }

        #endregion

        #region class SingleNode - methods

        public int CompareTo(SingleNode<TNodeValue> otherNode)
        {
            if (this.Value == null) return -1;
            if (otherNode == null || otherNode.Value == null) return 1;

            return this.CompareTo(otherNode.Value);
        }

        public int CompareTo(TNodeValue otherValue)
        {
            if (this.Value == null) return -1;
            if (otherValue == null) return 1;

            if (this.Value is IComparable)
            {
                IComparable thisValue = (IComparable)this.Value;
                IComparable nodeValue = (IComparable)otherValue;

                return thisValue.CompareTo(nodeValue);
            }
            else // comparing hash code as value
            {
                int hash1 = this.Value.GetHashCode();
                int hash2 = otherValue.GetHashCode();

                return hash1.CompareTo(hash2);
            }
        }

        public override bool Equals(object otherNode)
        {
            if (otherNode is TNodeValue)
            {
                return this.CompareTo((SingleNode<TNodeValue>)otherNode) == 0;
            }
            return this.GetHashCode().CompareTo(otherNode.GetHashCode()) == 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        #endregion

    }

}// namespace Common.Library
