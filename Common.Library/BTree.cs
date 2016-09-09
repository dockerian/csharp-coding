using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Library
{
    /// <summary>
    /// B-tree is a tree data structure that keeps data sorted 
    /// and allows searches, sequential access, insertions, and deletions in logarithmic time. 
    /// The B-tree is a generalization of a binary search tree in that 
    /// a node can have more than two children. 
    /// Unlike self-balancing binary search trees, the B-tree is optimized 
    /// for systems that read and write large blocks of data. 
    /// It is commonly used in databases and filesystems.
    /// </summary>
    /// <see cref="http://en.wikipedia.org/wiki/B-tree"/>
    /// <typeparam name="TData">data type of the node</typeparam>
    /// <typeparam name="TNode">node type</typeparam>
    public class BTree<TNode, TData>
        where TNode : TreeNode<TData>
        where TData : IComparable
    {
        #region Fields

        protected TreeNode<TData> _root;

        #endregion

        #region Constructors

        public BTree(TreeNode<TData> root)
        {
            _root = root;
        }

        public BTree(TData rootData)
        {
            _root = new TreeNode<TData>(rootData);
        }

        #endregion

        #region Properties

        public TreeNode<TData> Root
        {
            get { return _root; }
        }

        #endregion

        #region Functions

        #endregion

        #region Methods

        #endregion

    }

    /// <summary>
    /// Binary search tree (BST), sometimes also called an ordered or sorted binary tree, 
    /// is a node-based binary tree data structure which has the following properties:
    /// - The left subtree of a node contains only nodes with keys less than the node's key.
    /// - The right subtree of a node contains only nodes with keys greater than the node's key.
    /// - The left and right subtree must each also be a binary search tree.
    /// - There must be no duplicate nodes.
    /// 
    /// Generally, the information represented by each node is a record rather than a single data element. 
    /// However, for sequencing purposes, nodes are compared according to their keys 
    /// rather than any part of their associated records.
    ///
    /// The major advantage of binary search trees over other data structures is that 
    /// the related sorting algorithms and search algorithms such as in-order traversal can be very efficient.
    ///
    /// Binary search trees are a fundamental data structure used to construct more 
    /// abstract data structures such as sets, multisets, and associative arrays.
    /// </summary>
    /// <see cref="http://en.wikipedia.org/wiki/Binary_search_tree"/>
    /// <typeparam name="TData">data type of the node</typeparam>
    /// <typeparam name="TNode">node type</typeparam>
    public class BinarySearchTree<TNode, TData> //: BinaryTree<TNode, TData> 
        where TNode : BinarySearchTreeNode<TData>
        where TData : IComparable
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Functions

        #endregion

        #region Methods

        #endregion

    }

    public class TreeNode<TData> where TData : IComparable
    {
        #region Fields

        protected List<TreeNode<TData>> _childnodes;
        protected TData _data;
        protected TreeNode<TData> _parent;

        #endregion

        #region Constructors

        public TreeNode(TData data)
        {
            _data = data;
        }

        #endregion

        #region Properties

        public List<TreeNode<TData>> ChildNodes { get { return _childnodes; } }

        public TData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Functions

        #endregion

        #region Methods

        public void AddChild(TData data)
        {
            if (data != null)
            {
                var node = new TreeNode<TData>(data);
                ChildNodes.Add(node);
            }
        }

        public void AddChild(TreeNode<TData> node)
        {
            if (node == null) return;
            if (ChildNodes.Contains(node)) return;

            ChildNodes.Add(node);
        }

        public void RemoveChild(TreeNode<TData> node)
        {
            ChildNodes.Remove(node);
        }

        public void Sort()
        {
            ChildNodes.Sort((x, y) => x.Data.CompareTo(y.Data));
        }

        #endregion

    }

}
