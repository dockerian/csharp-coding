using Common.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace UnitTest
{
    ///<summary>
    ///This is a test class for LinkedListTest and is intended
    ///to contain all LinkedListTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LinkedListTest
    {
        #region :: Private Fields and Functions

        private TestContext testContextInstance;

        private bool CompareReversedList(BiDirectionalList<object> old_list, BiDirectionalList<object> new_list, bool compareValue = false)
        {
            bool result = true; 
            Node<object> node_old = old_list.Last;
            Node<object> node_new = new_list.Head;

            while (node_old != null)
            {
                if (compareValue)
                {
                    if (node_old.Value != node_new.Value)
                    {
                        result = false;
                        return false;
                    }
                }
                else if (node_old != node_new)
                {
                    result = false;
                    break;
                }
                node_new = node_new.Previous;
                node_old = node_old.Next;
            }

            return result;
        }

        #endregion

        #region :: Properties
        ///<summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        #region :: Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region :: Tests

        ///<summary>
        ///test case for LinkedList - adding/inserting nodes
        ///</summary>
        [TestMethod()]
        public void AddNodeTest()
        {
            
        }

        ///<summary>
        ///test case for LinkedList - deleting node by value
        ///</summary>
        [TestMethod()]
        public void DeleteNodeTest()
        {
            Node<string> node1 = new Node<string>("abc");
            BiDirectionalList<string> aList = new BiDirectionalList<string>(node1);

            Assert.AreEqual(aList.Current, node1);
            Assert.AreEqual(aList.Current, aList.Head);
            Assert.AreEqual(aList.Current, aList.Last);
            Assert.AreEqual(aList.Count, 1);

            Node<string> node2 = aList.DeleteNode(node1);
            Assert.AreEqual(node1, node2);

            node2 = aList.AddNode(node1);
            Assert.AreEqual(node1, node2);

            node2 = aList.DeleteNode();
            Assert.AreEqual(node1, node2);

            node2.Value = "second-node";
            aList.AddNode(node2.Value); // adding the value, not the node
            node2 = aList.Current;
            Assert.IsTrue(node1.Equals(node2));
            Assert.AreEqual(node1, node2);

            aList.InsertNode(node1);
            aList.Current.Value = "first-node";
            Assert.AreEqual(aList.Head, node1);
            Assert.AreEqual(aList.Last, node2);
            Assert.IsNull(aList.DeleteNode(""));

            Node<string> node3 = aList.AddNode("3rd-node", node2.Value);
            Assert.AreEqual(aList.Last, node3);
            Assert.AreEqual(aList.Count, 3);

            aList.SeekToHead();
            Assert.AreEqual(aList.Current, aList.Head);
            aList.SeekToLast();
            Assert.AreEqual(aList.Current, aList.Last);
            aList.SeekToNext();
            Assert.IsNotNull(aList.Current);
            aList.SeekToPrevious();
            Assert.AreEqual(aList.Current.Value, node2.Value);
            aList.SeekToPrevious();
            Assert.AreEqual(aList.Current, node1);
            aList.SeekToPrevious();
            aList.SeekToNext();
            Assert.AreEqual(aList.Current.Value, node2.Value);
            aList.SeekToNext();
            Assert.AreEqual(aList.Current, node3);

            node2 = aList.DeleteNode(node2.Value);
            Assert.AreEqual(aList.Current, node1);

            aList.AddNode(node2.Value, 1);
            Assert.AreEqual(aList.Current.Value, node2.Value);

            aList.DeleteNode();
            aList.InsertNode(node2.Value, 2);
            Assert.AreEqual(aList.Current.Value, node2.Value);

            aList.Clear();
            Assert.AreEqual(aList.IsEmpty, true);
            Assert.AreEqual(aList.Count, 0);
            Assert.IsNull(aList.DeleteNode());
            Assert.IsNull(aList.Current);
            Assert.IsNull(aList.Head);
            Assert.IsNull(aList.Last);

        }

        ///<summary>
        ///test case for LinkedList - finding node by position or value
        ///</summary>
        [TestMethod()]
        public void FindNodeTest()
        {
            string[] data = {"", "abcde", "3", "4", "5", "0"};

            BiDirectionalList<string> aList = new BiDirectionalList<string>(data);
            BiDirectionalList<string> bList = aList.Clone();
            SingleLinkedList<string> list1 = new SingleLinkedList<string>(data);
            SingleLinkedList<string> list2 = aList.Clone();

            Assert.AreEqual(list1.ToString(), list2.ToString());
            Assert.AreEqual(list1.FindNode("3").Value, "3");

            string[] array = aList.ToArray();
            string ssArray = aList.ToString();
            Assert.AreEqual(String.Join(", ", array), String.Join(", ", bList.ToString()));
            Assert.AreEqual(String.Join(", ", array), String.Join(", ", data));
            Assert.AreEqual(String.Join(", ", array), ssArray);

            Node<string> node1 = aList.FindNode("abcde");
            Assert.AreEqual(aList.FindNode(node1).Value, "abcde");
            Assert.AreEqual(aList.FindNode("3").Value, "3");
            Assert.AreEqual(aList.FindNode(3).Value, "4");
            Assert.AreEqual(aList.FindNode(2).Value, aList.FindNode(-4).Value);
            Assert.AreEqual(aList.FindNode(5).Value, aList.FindNode(-1).Value);
            Assert.AreNotEqual(aList.FindNode(0).Value, "0");
            Assert.IsNotNull(aList.FindNode(-6));
            Assert.IsNull(aList.FindNode(6));
            Assert.IsNull(aList.FindNodeBackward(6));
            Assert.IsNull(aList.FindNode(-7));

            Node<string> node2 = new Node<string>("abc");
            Assert.IsNull(aList.FindNode(node2));

            Node<string> node3 = aList.InsertNode(node2.Value, 2);
            Assert.AreEqual(node2, node3);
            Assert.IsNotNull(node3);
            Assert.IsNotNull(aList.AddNode(node2, aList.Last));
            Assert.AreEqual(node2, aList.Last);

            Node<string> node4 = aList.FindNodeBackward(4);
            Assert.AreEqual(node4.Value, "3");

            Node<string> node5 = aList.FindNodeBackward(5);
            Node<string> test5 = aList.FindNode(aList.Count - 5 - 1);
            Assert.AreEqual(node5.Value, test5.Value);

            aList.InsertNode(node1, node1);
            Assert.AreEqual(node1.Value, aList.Head.Next.Value);
        }

        ///<summary>
        ///test case for SingleLinkedList
        ///</summary>
        [TestMethod()]
        public void LinkedSingleListTest()
        {
            
        }

        ///<summary>
        ///test case 1 for Reverse
        ///</summary>
        [TestMethod()]
        public void ReverseTest1()
        {
            string abc = "abc";
            string xyz = "xyz";
            string test1 = "test1";
            string test2 = "test2";
            string test3 = "test3";
            string test4 = "test4";

            Node<String> newNode = null;
            BiDirectionalList<String> myList = new BiDirectionalList<String>();
            Assert.AreEqual(myList.Count, 0);

            // testing empty list
            Assert.IsNull(myList.Current);
            Assert.IsNull(myList.Head);
            Assert.IsNull(myList.Last);
            myList.Reverse();
            Assert.AreEqual(myList.Count, 0);

            // testing linked list with only one node
            newNode = myList.AddNode(test1);
            Assert.AreEqual(myList.Count, 1);
            Assert.AreEqual(myList.Head, newNode);
            Assert.AreEqual(myList.Last, myList.Current);
            Assert.AreEqual(myList.Head, myList.Current);

            myList.Reverse();
            Assert.AreEqual(myList.Count, 1);
            Assert.AreEqual(myList.Last, myList.Last);

            // reinitializing a new linked list
            myList = new BiDirectionalList<String>(test1);
            Assert.AreEqual(myList.Count, 1);

            newNode = myList.AddNode(test2);
            Assert.AreEqual(myList.Count, 2);
            Assert.AreEqual(myList.Last, newNode);
            Assert.AreNotEqual(myList.Head, newNode);
            newNode = myList.AddNode(test3);
            Assert.AreEqual(myList.Last, newNode);
            Assert.AreEqual(myList.Current.Value.ToString(), test3.ToString());
            Assert.AreNotEqual(myList.Head, newNode);
            newNode = myList.AddNode(abc);
            Assert.AreEqual(myList.Count, 4);
            Assert.AreEqual(myList.Last, newNode);
            Assert.AreNotEqual(myList.Head, newNode);
            newNode = myList.AddNode(xyz);
            Assert.AreEqual(myList.Count, 5);
            Assert.AreEqual(myList.Last, newNode);
            Assert.AreEqual(myList.Current, myList.Last);
            Assert.AreNotEqual(myList.Head, newNode);

            // searching in linked list
            Assert.AreEqual(myList.Head, myList.FindNode(0));
            Assert.AreEqual(myList.Head, myList.FindNode(test1));
            Assert.AreEqual(myList.Last, myList.FindNode(xyz));
            Assert.AreEqual(myList.Last, myList.FindNode(myList.Count - 1));
            Assert.AreNotEqual(myList.Last, myList.FindNode(myList.Head));

            myList.Last.Previous.Value = test4;
            Assert.AreEqual(myList.FindNode(3), myList.FindNode(test4));
            Assert.AreEqual(myList.FindNode(-2), myList.FindNode(test4));
            Assert.AreEqual(myList.FindNode(-1), myList.FindNode(xyz));
            Assert.AreEqual(myList.FindNode(-1), myList.Last);
            Assert.AreEqual(myList.FindNode(0), myList.Head);
            Assert.AreEqual(myList.Current, myList.Head);

            Assert.IsNull(myList.FindNode(abc));
            Assert.AreEqual(myList.Current, myList.Head);

            // saving original linked list
            Node<String> node_old = myList.Head;
            Node<String> node_nul = null;
            object[] original_list = myList.ToArray();

            Assert.IsNotNull(node_old);
            Assert.AreEqual(myList.Current.CompareTo(node_nul), 1);
            Assert.AreEqual(myList.Count, 5);

            object head = myList.Head.Value;
            object last = myList.Last.Value;

            myList.Reverse();

            Assert.AreEqual(myList.Current, myList.Last);
            Assert.AreEqual(myList.Head.Value.ToString(), last.ToString());
            Assert.AreEqual(myList.Last.Value.ToString(), head.ToString());

            Assert.AreEqual(myList.Last.Previous.Value.ToString(), test2);
            Assert.AreEqual(myList.Last.Previous.Previous.Value.ToString(), test3);
            Assert.AreEqual(myList.Head.Next.Next.Value.ToString(), test3);

            newNode = myList.Head;

            for(int n = original_list.Length - 1; n >= 0; n--)
            {
                Assert.AreEqual(original_list[n], newNode.Value);
                newNode = newNode.Next;
            }

            Node<String> node_new = myList.Last;

            Assert.IsNotNull(node_new);

            while (node_old != null)
            {
                Assert.AreEqual(node_old, node_new);
                node_new = node_new.Previous;
                node_old = node_old.Next;
            }

        }

        ///<summary>
        ///test case 2 for Reverse - comparing node value
        ///</summary>
        [TestMethod()]
        public void ReverseTest2()
        {
            string test1 = "test1";
            string test6 = "test6";

            BiDirectionalList<object> oldList = new BiDirectionalList<object>(
                    "", "abcde", new Node<object>('3'), 4, 5.0
                );

            oldList.AddNode(test6);

            BiDirectionalList<string> strList1 = new BiDirectionalList<string>("", "abcde", "3", "4", "5.0");
            BiDirectionalList<string> strList2 = strList1.Clone();

            BiDirectionalList<object> newList = oldList.Clone();
            string comp1 = oldList.ToString();
            string comp2 = newList.ToString();
            Assert.AreEqual(comp1, comp2);

            newList.Reverse();
            Assert.IsFalse(this.CompareReversedList(oldList, newList));
            Assert.IsTrue(this.CompareReversedList(oldList, newList, true));

            Assert.IsFalse(strList1.Equals(strList2));
            Assert.IsTrue(strList1.Equals(strList2, true));

            Node<object> node1 = newList.InsertNode(test1);
            Assert.AreEqual(newList.Head, node1);

            Node<object> node5 = newList.DeleteNode(-1);
            Assert.IsNull(newList.FindNode(node5));

            Node<object> node6 = newList.AddNode(test6);
            Assert.AreEqual(newList.Last, node6);
        }

        #endregion

    }
}
