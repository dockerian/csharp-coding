using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Library
{
    public delegate bool BinaryTreeNodeVisitor<TData>(BinaryTreeNode<TData> node) where TData : IComparable;

    /// <summary>
    /// Binary tree is a tree data structure in which each node has at most two child nodes, 
    /// usually distinguished as "left" and "right". Nodes with children are parent nodes, 
    /// and child nodes may contain references to their parents. 
    /// Outside the tree, there is often a reference to the "root" node (the ancestor of all nodes), if it exists. 
    /// Any node in the data structure can be reached by starting at root node 
    /// and repeatedly following references to either the left or right child. 
    /// A tree which does not have any node other than root node is called a null tree. 
    /// In a binary tree, a degree of every node is maximum two. 
    /// A tree with n nodes has exactly n−1 branches or degree.
    /// Binary trees are used to implement binary search trees and binary heaps, 
    /// finding applications in efficient searching and sorting algorithms.
    /// </summary>
    /// <see cref="http://en.wikipedia.org/wiki/Binary_tree"/>
    /// <typeparam name="TData">data type of the tree node</typeparam>
    /// <typeparam name="TNode">node type</typeparam>
    public class BinaryTree<TNode, TData> //: BTree<TNode, TData> 
        where TNode : BinaryTreeNode<TData> 
        where TData : IComparable
    {
        #region Fields

        protected BinaryTreeNode<TData> _root;

        #endregion

        #region Constructors

        public BinaryTree(TNode root)
        {
            _root = root;
        }

        public BinaryTree(TData rootData)
        {
            _root = new BinaryTreeNode<TData>(rootData);
        }

        #endregion

        #region Properties

        public BinaryTreeNode<TData> Root
        {
            get { return _root; }
        }

        #endregion

        #region Functions

        #endregion

        #region Methods

        /// <summary>
        /// Starting from a binary tree node, traverse breadth-first and perform an action.
        ///         A(root)
        ///        /      \
        ///       B        C
        ///      /  \       \
        ///     D    E       F
        ///         / \     /
        ///        G   H   I
        /// </summary>
        /// <param name="node">The node to start a traversal.</param>
        /// <param name="doFuncVisit">The action to perform on the node.</param>
        public void BreadthFirst(TNode node, Action<TNode> doFuncVisit)
        {
            if (node == null || doFuncVisit == null) return;

            List<TNode> nodeList = new List<TNode>();
            Queue<TNode> nodeQueue = new Queue<TNode>();

            nodeQueue.Enqueue(node);

            while (nodeQueue.Count > 0)
            {
                node = nodeQueue.Dequeue();
                doFuncVisit(node);

                if (node.Left != null)
                {
                    nodeQueue.Enqueue(node.Left as TNode);
                }

                if (node.Right != null)
                {
                    nodeQueue.Enqueue(node.Right as TNode);
                }
            }
        }

        /// <summary>
        /// Starting from a binary tree node, traverse depth-first by in-order and perform an action.
        ///         F(root)
        ///        /      \
        ///       B        G
        ///      /  \       \
        ///     A    D       I
        ///         / \     /
        ///        C   E   H
        /// </summary>
        /// <param name="node">The node to start a traversal.</param>
        /// <param name="doFuncVisit">The action to perform on the node.</param>
        public void DepthFirstInOrder(TNode node, Action<TNode> doFuncVisit)
        {
            if (node == null || doFuncVisit == null) return;

            this.DepthFirstInOrder(node.Left as TNode, doFuncVisit);
            doFuncVisit(node);
            this.DepthFirstInOrder(node.Right as TNode, doFuncVisit);
        }

        /// <summary>
        /// Starting from a binary tree node, traverse depth-first by in-order and perform an action.
        /// </summary>
        /// <param name="node">The node to start a traversal.</param>
        /// <param name="doFuncVisit">The action to perform on the node.</param>
        public void DepthFirstInOrderIterative(TNode node, Action<TNode> doFuncVisit)
        {
            if (node == null || doFuncVisit == null) return;

            StackGeneric<TNode> stackNodes = new StackGeneric<TNode>();

            while(node != null || !stackNodes.IsEmpty)
            {
                if (node != null)
                {
                    stackNodes.Push(node);
                    node = node.Left as TNode;
                }
                else //// LIFO vist from the stack
                {
                    node = stackNodes.Pop();
                    doFuncVisit(node);
                    node = node.Right as TNode;
                }
            }
        }

        /// <summary>
        /// Starting from a binary tree node, traverse depth-first by in-order and add to a sequential list.
        /// </summary>
        /// <param name="node">The node to start a traversal.</param>
        /// <param name="sequentializedTraverseList">The sequential list.</param>
        public void DepthFirstInOrderIterative(TNode node, List<TNode> sequentializedTraverseList)
        {
            if (sequentializedTraverseList == null)
            {
                sequentializedTraverseList = new List<TNode>();
            }

            StackGeneric<TNode> stackNodes = new StackGeneric<TNode>();

            while(node != null || !stackNodes.IsEmpty)
            {
                if (node != null)
                {
                    stackNodes.Push(node);
                    node = node.Left as TNode;
                }
                else //// LIFO vist from the stack
                {
                    node = stackNodes.Pop();
                    sequentializedTraverseList.Add(node);
                    node = node.Right as TNode;
                }
            }
        }

        /// <summary>
        /// Starting from a binary tree node, traverse depth-first by post-order and add to a sequential list.
        ///         I(root)
        ///        /      \
        ///       E        H
        ///      /  \       \
        ///     A    D       G
        ///         / \     /
        ///        B   C   F
        /// </summary>
        /// <param name="node">The node to start a traversal.</param>
        /// <param name="doFuncVisit">The action to perform on the node.</param>
        public void DepthFirstPostOrder(TNode node, Action<TNode> doFuncVisit)
        {
            if (node == null || doFuncVisit == null) return;

            this.DepthFirstPostOrder(node.Left as TNode, doFuncVisit);
            this.DepthFirstPostOrder(node.Right as TNode, doFuncVisit);
            doFuncVisit(node);
        }

        /// <summary>
        /// Starting from a binary tree node, traverse depth-first by post-order and perform an action.
        /// </summary>
        /// <param name="node">The node to start a traversal.</param>
        /// <param name="doFuncVisit">The action to perform on the node.</param>
        public void DepthFirstPostOrderIterative(TNode node, Action<TNode> doFuncVisit)
        {
            if (node == null || doFuncVisit == null) return;

            TNode nodeLastVisted = null;
            StackGeneric<TNode> stackNodes = new StackGeneric<TNode>();

            while (node != null || !stackNodes.IsEmpty)
            {
                if (node != null)
                {
                    stackNodes.Push(node);
                    node = node.Left as TNode;
                }
                else //// traverse to right
                {
                    TNode peekNode = stackNodes.Peek();

                    if (peekNode.Right != null && peekNode.Right != nodeLastVisted)
                    {
                        node = peekNode.Right as TNode;
                    }
                    else
                    {
                        stackNodes.Pop();
                        doFuncVisit(peekNode);
                        nodeLastVisted = peekNode;
                    }
                }
            }
        }

        /// <summary>
        /// Starting from a binary tree node, traverse depth-first by pre-order and perform an action.
        ///         A(root)
        ///        /      \
        ///       B        G
        ///      /  \       \
        ///     C    D       H
        ///         / \     /
        ///        E   F   I
        /// </summary>
        /// <param name="node">The node to start a traversal.</param>
        /// <param name="doFuncVisit">The action to perform on the node.</param>
        public void DepthFirstPreOrder(TNode node, Action<TNode> doFuncVisit)
        {
            if (node == null || doFuncVisit == null) return;

            doFuncVisit(node);
            this.DepthFirstPreOrder(node.Left as TNode, doFuncVisit);
            this.DepthFirstPreOrder(node.Right as TNode, doFuncVisit);
        }

        /// <summary>
        /// Starting from a binary tree node, traverse depth-first by pre-order and perform an action.
        /// </summary>
        /// <param name="node">The node to start a traversal.</param>
        /// <param name="doFuncVisit">The action to perform on the node.</param>
        public void DepthFirstPreOrderIterative(TNode node, Action<TNode> doFuncVisit)
        {
            if (node == null || doFuncVisit == null) return;

            StackGeneric<TNode> stackNodes = new StackGeneric<TNode>();

            while (node != null || !stackNodes.IsEmpty)
            {
                doFuncVisit(node);

                if (node.Right != null)
                {
                    stackNodes.Push(node.Right as TNode);
                }

                if (node.Left != null)
                {
                    stackNodes.Push(node.Left as TNode);
                }

                node = stackNodes.Pop();
            }
        }

        #endregion

    }

    /// <summary>
    /// Binary Search Tree (BST) node
    /// </summary>
    /// <typeparam name="TData">data type of the node</typeparam>
    public class BinarySearchTreeNode<TData> : BinaryTreeNode<TData> where TData : IComparable
    {
        #region Fields

        #endregion

        #region Constructors

        public BinarySearchTreeNode(TData data) : base(data)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Functions

        #endregion

        #region Methods

        #endregion

    }

    public class BinaryTreeNode<TData> : IComparable where TData : IComparable
    {
        #region Fields

        protected TData _data;
        protected BinaryTreeNode<TData> _leftNode;
        protected BinaryTreeNode<TData> _rightNode;

        #endregion

        #region Constructors

        public BinaryTreeNode(TData data)
        {
            _data = data;
        }

        #endregion

        #region Properties

        public TData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public BinaryTreeNode<TData> Left
        {
            get { return _leftNode; }
            set
            {
                ReplaceNode(_leftNode, value);
            }
        }

        public BinaryTreeNode<TData> Right
        {
            get { return _rightNode; }
            set
            {
                ReplaceNode(_rightNode, value);
            }
        }

        #endregion

        #region Functions

        private void ReplaceNode(BinaryTreeNode<TData> node, BinaryTreeNode<TData> newNode)
        {
            var oldNode = node;
            node = newNode;

            if (oldNode != null)
            {
                node.Left = oldNode.Left;
                node.Right = oldNode.Right;
            }
        }

        #endregion

        #region Methods

        public int CompareTo(object compareToObject)
        {
            int result = -1;

            if (compareToObject is TData)
            {
                result = this._data.CompareTo(compareToObject);
            }
            else if (compareToObject is BinaryTreeNode<TData>)
            {
                var node = compareToObject as BinaryTreeNode<TData>;
                result = this._data.CompareTo(node.Data);
            }
            return result;
        }

        #endregion

    }


}
