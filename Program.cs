/*
 * CSCI 641 Project 1
 * @author  Prakash Mishra  (pm1739)
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace RIT_CS
{
    /// <summary>
    /// An exception used to indicate a problem with how
    /// a HashTable instance is being accessed
    /// </summary>
    public class NonExistentKey<Key> : Exception
    {
        /// <summary>
        /// The key that caused this exception to be raised
        /// </summary>
        public Key BadKey { get; private set; }

        /// <summary>
        /// Create a new instance and save the key that
        /// caused the problem.
        /// </summary>
        /// <param name="k">
        /// The key that was not found in the hash table
        /// </param>
        public NonExistentKey(Key k) :
            base("Non existent key in HashTable: " + k)
        {
            BadKey = k;
        }

    }

    /// <summary>
    /// An associative (key-value) data structure.
    /// A given key may not appear more than once in the table,
    /// but multiple keys may have the same value associated with them.
    /// Tables are assumed to be of limited size a re expected to automatically
    /// expand if too many entries are put in them.
    /// </summary>
    /// <param name="Key">the types of the table's keys (uses Equals())</param>
    /// <param name="Value">the types of the table's values</param>
    interface Table<Key, Value> : IEnumerable<Key>
    {
        /// <summary>
        /// Add a new entry in the hash table. If an entry with the
        /// given key already exists, it is replaced without error.
        /// put() always succeeds.
        /// (Details left to implementing classes.)
        /// </summary>
        /// <param name="k">the key for the new or existing entry</param>
        /// <param name="v">the (new) value for the key</param>
        void Put(Key k, Value v);

        /// <summary>
        /// Does an entry with the given key exist?
        /// </summary>
        /// <param name="k">the key being sought</param>
        /// <returns>true iff the key exists in the table</returns>
        bool Contains(Key k);

        /// <summary>
        /// Fetch the value associated with the given key.
        /// </summary>
        /// <param name="k">The key to be looked up in the table</param>
        /// <returns>the value associated with the given key</returns>
        /// <exception cref="NonExistentKey">if Contains(key) is false</exception>
        Value Get(Key k);
    }

    class TableFactory
    {
        /// <summary>
        /// Create a Table.
        /// (The student is to put a line of code in this method corresponding to
        /// the name of the Table implementor s/he has designed.)
        /// </summary>
        /// <param name="K">the key type</param>
        /// <param name="V">the value type</param>
        /// <param name="capacity">The initial maximum size of the table</param>
        /// <param name="loadThreshold">
        /// The fraction of the table's capacity that when
        /// reached will cause a rebuild of the table to a 50% larger size
        /// </param>
        /// <returns>A new instance of Table</returns>
        public static Table<K, V> Make<K, V>(int capacity = 100, double loadThreshold = 0.75)
        {
            return new LinkedHashTable<K, V>(capacity, loadThreshold);
        }
    }

    class TestTable
    {
        public void Test()
        {
            char testToRun = '0';
            do
            {
                Console.Write("Select Test to Run (1-7, 0 to exit): ");
                do
                {
                    testToRun = Console.ReadLine().ToCharArray()[0];
                } while (testToRun == '\n' || testToRun == '\r');
                switch (testToRun)
                {
                    case '1':
                        test1();
                        break;
                    case '2':
                        test2();
                        break;
                    case '3':
                        test3();
                        break;
                    case '4':
                        test4();
                        break;
                    case '5':
                        test5();
                        break;
                    case '6':
                        test6();
                        break;
                    case '7':
                        test7();
                        break;
                }
            } while (testToRun != '0');
        }


        private void test1()
        {
            Table<String, String> ht = TableFactory.Make<String, String>(4, 0.5);
            ht.Put("Joe", "Doe");
            ht.Put("Jane", "Brain");
            ht.Put("Chris", "Swiss");
            try
            {
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

                ht.Put("Wavy", "Gravy");
                ht.Put("Chris", "Bliss");
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

                Console.Write("Jane -> ");
                Console.WriteLine(ht.Get("Jane"));
                Console.Write("John -> ");
                Console.WriteLine(ht.Get("John"));
            }
            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }

        }

        private void test2()
        {
            Table<int, String> ht = TableFactory.Make<int, String>(4, 0.5);
            ht.Put(1, "Eat");
            ht.Put(2, "Play");
            ht.Put(3, "Sleep");
            Console.WriteLine("This test case is to check if the developed data structure supports generics");
            Console.WriteLine("and if diferent value is passed with same key, value should be overwrritten");
            try
            {
                foreach (int first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

                ht.Put(2, "Study");
                ht.Put(4, "Work");
                foreach (int first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

            }
            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }

        }
      
        private void test3()
        {
            Table<object, String> ht = TableFactory.Make<object, String>(1, 0.5);
            Console.WriteLine("This is to make sure that null value cannot be inserted as Key");
            Console.WriteLine("Trying to Insert null as key");
            ht.Put(null, "Null check");
            try
            {
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

            }
            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }

        }
        /// <summary>
        /// This test case has been shared with Ashwani Kumar(ak8647) and Devavrat Kalam(dk2792)
        /// </summary>
        private void test4()
        {
            Table<String, String> ht = TableFactory.Make<String, String>(6, 0.5);
            Console.WriteLine("This is to make sure that rehashing works properly");
            ht.Put("Betty", "boop");
            ht.Put("Buggs", "Bunny");
            ht.Put("Charlie", "Brown");
            ht.Put("Duffy", "Duck");
            ht.Put("Dennis", "The Mennace");
            ht.Put("Donald", "Duck");
            ht.Put("Garfield", "MickeyMouse");
            ht.Put("Olive", "Oyl");
            ht.Put("Popeye", "Tailorman");
            ht.Put("Powerpuff", "Girls");
            ht.Put("Road", "Runner");
            ht.Put("Scooby", "Doo");
            ht.Put("Snoopy", "Dog");
            ht.Put("Ninja", "Turtle");
            ht.Put("The", "Simpsons");
            ht.Put("Tom", "Jerry");
            ht.Put("Yogi", "Bear");

            try
            {
 
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

            }
            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }

        }

        /// <summary>
        /// This test case was shared with devavrat kalam(dk2792)
        /// </summary>
        private void test5()
        {
            Table<String, String> ht = TableFactory.Make<String, String>(-1, 0.5);
            Console.WriteLine("If the provided loadfactor is negative, ask user to enter positive value");
            ht.Put("Any", "Negative loadfactor");
            try
            {
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

            }
            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }

        }

        private void test6()
        {
            Table<String, String> ht = TableFactory.Make<String, String>(10, -2);
            Console.WriteLine("In case of Incorrect threshold, value it asks user to enter value between 0 and 1");
            ht.Put("Test", "Incorrect threshold value");
            try
            {
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

            }
            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }

        }
        /// <summary>
        /// The below test case was taken from Ashwani Kumar(ak8647)
        /// </summary>
        private void test7()
        {
            Console.WriteLine("This test case checkes performance of my hashtable. " +
                "It also feeds hashtable with size having negative values and loadThreshold ");
            DateTime start = DateTime.Now;
            Console.WriteLine("Testing hashTable with threshold of 0.633 and start capacity" +
                " of 10");
            Table<int, int> myHashTable1 = TableFactory.Make<int, int>(10, 0.633);
            Console.WriteLine("Inserting 10 million entries into hashtable");
            for (int i = 0; i < 10000000; i++)
            {
                myHashTable1.Put(i, i);
            }
            Console.WriteLine("Enumerating throught each entries in HashTable");
            foreach (int x in myHashTable1)
                myHashTable1.Get(x);
            DateTime end = DateTime.Now;
            Console.WriteLine("Time taken for execution: " + (end - start) + "\n");



            Console.WriteLine("Test case 3");
            start = DateTime.Now;

            Console.WriteLine("Testing hashTable with threshold of -10 and start capacity " +
                " of -0.43333");

            myHashTable1 = TableFactory.Make<int, int>(-10, -0.43333);
            Console.WriteLine("Inserting 10 million entries into hashtable");
            for (int i = 0; i < 10000000; i++)
            {
                myHashTable1.Put(i, i);
            }

            Console.WriteLine("Enumerating throught each entries in HashTable");
            foreach (int x in myHashTable1)
            {
                myHashTable1.Get(x);
            }
            end = DateTime.Now;
            Console.WriteLine("Time taken for execution: " + (end - start) + "\n");

            Console.WriteLine("TestCase successfully completed.\n");
        }

        

    }

    class NodeList<k, v>
    {
        private k key;
        private v data;
        public NodeList<k, v> next;

        //public NodeList<k,v>(v vaky)
        public v Data {
            get {
                return data;
            }
             set {
                data = value;
            }
        }
        public k Key
        {
            get
            {
                return key;
            }
             set
            {
                key = value;
            }
        }


    }

    
    public class LinkedHashTable<key, value> : Table<key, value>
    {

        NodeList<key,value> head = null;
        NodeList<key,value>[] buckets;
        public int bucketIndex = -1;
        private double loadThreshold = 0.0;
        public int capacity = 0;
        int count = 0;

        public int Size1()
        {
            return this.buckets.Length;
        }
        public LinkedHashTable(int bucketsLength, double loadThreshold)
        {
            int x = bucketsLength;
            double y = loadThreshold;
            while (x <= 0)
            {
                Console.WriteLine("Please enter a positive value to set as the capacity");
                string temp = Console.ReadLine();
                x = Convert.ToInt32(temp);
            }
            while (y <= 0 || y > 1)
            {
                Console.WriteLine("Please enter value between 0 to 1");
                string temp1 = Console.ReadLine();
                y = Convert.ToDouble(temp1);
            }

            this.loadThreshold = y;
            this.capacity = x;
            buckets = new NodeList<key,value>[x];
        }

        /// <summary>
        /// Checks if the key is present in the hashmap
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns>Boolean, True if present else returns false</returns>
        public bool Contains(key k)
        {
            if (k != null)
            {
                int hashCode = Math.Abs(k.GetHashCode() % capacity);
                head = buckets[hashCode];

                while (head != null)
                {
                    if (head.Key != null && head.Key.Equals(k))
                    {
                        return true;
                    }
                    if (head.Key == null && k == null)
                    {
                        return true;
                    }
                    head = head.next;
                }
                return false;
            }
            else
            {
                Console.WriteLine("Key value cannot be null");
                return false;
            }
            
        }

        /// <summary>
        /// Whenver the loadfactor value goes beyond threshold, increase the 
        /// size of the map by 50 % and re assign the key and value based on updated
        /// hash value
        /// </summary>
        public void rehash()
        {
            NodeList<key, value>[] old = buckets;
            capacity = capacity + capacity / 2;
 
            buckets = new NodeList<key, value>[capacity];

            for (int i = 0; i < old.Length; i++)
            {
                NodeList<key, value> newNode;

                if (old[i] != null)
                {
                    newNode = old[i];
                    while (newNode != null)
                    {
                        this.Put(newNode.Key, newNode.Data);
                        newNode = newNode.next;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the value based on the provided key or else returns none.
        /// </summary>
        /// <param name="k"></param>
        /// <returns>Value associated with the key in my Linked hashtable structure</returns>
        public value Get(key k) 
        {
            
            if (k != null)
            {
                try
                {
                    if (Contains(k))
                    {
                        int hashCode = Math.Abs(k.GetHashCode() % capacity);
                        head = buckets[hashCode];

                        while (head != null)
                        {
                            if (head.Key != null && head.Key.Equals(k))
                            {
                                return head.Data;
                            }
                            head = head.next;
                        }
                        return default(value);
                    }
                    else
                    {
                        throw new NonExistentKey<key>(k);
                    }
                }
                catch (NonExistentKey<String> nek)
                {
                    Console.WriteLine(nek.Message);
                    Console.WriteLine(nek.StackTrace);
                    return default(value);

                }
            }
            else
            {
                return default(value);
            }
            
        }
        /// <summary>
        /// Eumerates the developed data structure and returns the associated key
        /// </summary>
        /// <returns>returns the key where the current iterator is pointing it</returns>
        public IEnumerator<key> GetEnumerator()
        {
            NodeList<key, value> curr = null;
            for (var i = 0; i < buckets.Length; i++)
            {
                if (buckets[i] != null)
                {
                    curr = buckets[i];
                    while (curr != null)
                    {
                        
                        yield return curr.Key;
                        curr = curr.next;
                    }
                }
            }
        }
        /// <summary>
        ///   Adds a key, value pair to the Linkedhash table data structure
        /// </summary>
        /// <param name="k">Key of the Node</param>
        /// <param name="v">Key of the Node</param>
        public void Put(key k, value v)
        {
            if (k != null) {
                double loadFactor = (count) / (double)capacity;
                if (loadFactor > this.loadThreshold)
                {
                    count = 0;
                    rehash();
                }
                NodeList<key, value> prev = null;
                NodeList<key, value> node = new NodeList<key, value>();
                node.Key = k;
                node.Data = v;
                int hashCode = Math.Abs(k.GetHashCode() % capacity);
                if (buckets[hashCode] == null)
                {
                    buckets[hashCode] = node;
                    count += 1;
                }
                else
                {
                    head = buckets[hashCode];

                    while (head != null)
                    {
                        if (head.Key.Equals(node.Key))
                        {
                            head.Data = node.Data;
                            break;
                        }
                        prev = head;
                        head = head.next;
                    }
                    if (head == null) {
                        prev.next = node;
                        count += 1;
                        
                    }
                }
                
            }
            else
            {
                Console.WriteLine("Key cannot be null");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class MainClass
    {
        /// <summary>
        /// Main method to control the complete program flow.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var t = new TestTable();
            t.Test();
            Console.ReadLine();
        }
    }
}
