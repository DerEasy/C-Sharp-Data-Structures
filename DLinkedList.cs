using System.Text;

/// Represents a doubly linked list.
public class DLinkedList<T> {
    internal class Node {
        internal T val = default!;
        internal Node prev = default!; 
        internal Node next = default!;

        internal Node() {}
        internal Node(T value) {
            val = value;
        }
    }

    private readonly Node head;
    private readonly Node tail;
    private int length;

    public DLinkedList() {
        head = new Node();
        tail = new Node();
        head.next = tail;
        tail.prev = head;
    }

    private bool OutOfRange(int i) {
        return i < 0 || i > Index();
    }
    
    /// Checks if list is empty. O(1).
    /// <returns>True if list is empty.</returns>
    public bool IsEmpty() {
        return length is 0;
    }

    /// Length of this list. O(1).
    /// <returns>Integer.</returns>
    public int Length() {
        return length;
    }

    /// Highest addressable index in this list. O(1).
    /// <returns>-1 if list is empty.</returns>
    public int Index() {
        return length - 1;
    }

    /// Deletes all elements of this list. Length
    /// will be 0. O(1).
    public void Clear() {
        head.next = tail;
        tail.prev = head;
        length = 0;
    }

    private Node LastNode() {
        return tail.prev;
    }

    private Node FirstNode() {
        return head.next;
    }

    private bool IterateForwards(int i) {
        return i < length / 2;
    }

    /// Adds a new element to the end of this list. O(1).
    /// <param name="value">Value of new element.</param>
    public void Append(T value) {
        var node = new Node(value);
        var last = LastNode();
        last.next = node;
        node.prev = last;
        node.next = tail;
        tail.prev = node;
        ++length;
    }

    /// Adds a new element to the start of this list. O(1).
    /// <param name="value">Value of new element.</param>
    public void Prepend(T value) {
        var node = new Node(value);
        var first = FirstNode();
        first.prev = node;
        node.next = first;
        node.prev = head;
        head.next = node;
        ++length;
    }

    /// Peeks the first element in this list. O(1).
    /// <returns>The first element.</returns>
    public T First() {
        return FirstNode().val;
    }

    /// Peeks the last element in this list. O(1).
    /// <returns>The last element.</returns>
    public T Last() {
        return LastNode().val;
    }

    private Node NodeAt(int i) {
        if (OutOfRange(i)) 
            throw new IndexOutOfRangeException("DLinkedList index out of bounds.");

        Node node;
        if (IterateForwards(i)) {
            node = FirstNode();
            for (int k = 0; k < i; ++k)
                node = node.next;
        } else {
            node = LastNode();
            for (int k = Index(); k > i; --k)
                node = node.prev;
        }

        return node;
    }

    /// Peeks the ith element in this list. O(n).
    /// <returns>The element at index i.</returns>
    public T ElementAt(int i) {
        return NodeAt(i).val;
    }
    
    /// Inserts an element into this list so that the
    /// element has the given index i. All elements
    /// to the right of this new element increase their
    /// index by +1. O(n).
    /// <param name="i">Index the element should take.</param>
    /// <param name="value">Value of the element.</param>
    public void InsertAt(int i, T value) {
        var node = NodeAt(i);
        var ins = new Node(value) {
            next = node,
            prev = node.prev
        };

        node.prev = ins;
        ins.prev.next = ins;
        ++length;
    }

    private void InsertNode(Node node, T value) {
        var ins = new Node(value);
        ins.next = node.next;
        ins.next.prev = ins;
        ins.prev = node;
        node.next = ins;
        ++length;
    }

    /// Modifies the value at index i. O(n).
    /// <param name="i">Index of element.</param>
    /// <param name="value">New value of element.</param>
    public void Modify(int i, T value) {
        var node = NodeAt(i);
        node.val = value;
    }

    private void DeleteNode(Node node) {
        node.prev.next = node.next;
        node.next.prev = node.prev;
        --length;
    }

    /// Deletes the last node of this list. O(1).
    public void DeleteLast() {
        DeleteNode(LastNode());
    }

    /// Deletes the first node of this list. O(1).
    public void DeleteFirst() {
        DeleteNode(FirstNode());
    }

    /// Deletes the node at index i. O(n).
    public void DeleteAt(int i) {
        DeleteNode(NodeAt(i));
    }

    /// Deletes all elements in this list that have
    /// the given value. O(n).
    /// <returns>True if there was at least one
    /// matching value in the list.</returns>
    public bool DeleteAll(T value) {
        var node = FirstNode();
        bool success = false;
        for (int i = 0; i++ < length; node = node.next)
            if (node.val!.Equals(value)) {
                node = node.next;
                DeleteNode(node.prev);
                success = true;
            }

        return success;
    }

    /// Deletes the first element in this list that
    /// has the given value. O(n).
    /// <returns>True if there was an element
    /// with the given value.</returns>
    public bool DeleteFirstOccurrence(T value) {
        var node = FirstNode();
        for (int i = 0; i++ < length; node = node.next)
            if (node.val!.Equals(value)) 
                goto success;
        
        return false;
    success:
        DeleteNode(node);
        return true;
    }

    /// Deletes the last element in this list that
    /// has the given value. O(n).
    /// <returns>True if there was an element
    /// with the given value.</returns>
    public bool DeleteLastOccurrence(T value) {
        var node = LastNode();
        for (int i = Index(); i-- > ~0; node = node.prev)
            if (node.val!.Equals(value)) 
                goto success;
        
        return false;
    success:
        DeleteNode(node);
        return true;
    }

    /// Checks if there is an element in this list
    /// that has the given value. O(n).
    /// <param name="value">The value to be searched.</param>
    /// <returns>True if this list contains this value.</returns>
    public bool Contains(T value) {
        var node = FirstNode();
        for (int i = 0; i++ < length; node = node.next)
            if (node.val!.Equals(value)) return true;
        return false;
    }

    /// Same as Contains(T), but you may decide to start
    /// searching from the end of the list instead. O(n).
    /// <param name="value">The value to be searched.</param>
    /// <returns>True if this list contains this value.</returns>
    public bool Contains(T value, bool searchFromFirst) {
        if (searchFromFirst)
            return Contains(value);
        
        var node = LastNode();
        for (int i = Index(); i-- > ~0; node = node.prev)
            if (node.val!.Equals(value)) return true;
        return false;
    }

    /// Returns a string displaying all elements of this list
    /// in order or [] if this list is empty.
    /// <returns>String of all elements in this list.</returns>
    public override string ToString() {
        if (IsEmpty()) return "[]";

        var sb = new StringBuilder();
        var node = FirstNode();
        
        sb.Append('[');
        for (int i = 0; i++ < Index(); node = node.next)
            sb.Append($"{node.val}, ");
        sb.Append($"{node.val}]");

        return sb.ToString();
    }

    /// Creates an array of all elements in this list in order.
    /// The array is therefore a shallow copy of this list.
    /// An empty list will returns a 0-size array.
    /// <returns>The array containing all elements.</returns>
    public T[] ToArray() {
        T[] array = new T[length];
        var node = FirstNode();
        
        for (int i = 0; i < length; node = node.next)
            array[i++] = node.val;

        return array;
    }
    
    /// Returns a shallow copy of this list.
    /// <returns>A shallow copy of this list.</returns>
    public DLinkedList<T> Copy() {
        var list = new DLinkedList<T>();
        var node = FirstNode();
        
        for (int i = 0; i++ < length; node = node.next)
            list.Append(node.val);

        return list;
    }
    
    /// Represents an iterator over a DLinkedList. The iterator
    /// is akin to a read-write head. It features a state
    /// mechanism that allows it to save one position in the
    /// list, so it can return to that position in constant time.
    /// IMPORTANT NOTE:
    /// Iterators are unsafe. If you structurally modify the
    /// underlying list, any further operation carried out by
    /// the iterator is classified as undefined behaviour.
    /// The iterator does not check if the underlying list has
    /// been structurally modified or not. It has built-in
    /// functionality to structurally modify the underlying
    /// list in a safe manner to mitigate this. You may
    /// try manually calling Safe() to see if the list's length
    /// differs from the iterator's saved length, which would
    /// indicate a modification. You may also try fixing the
    /// iterator for reuse with FixIterator(int). Fiddling
    /// around with the state mechanism might help as well.
    /// <seealso cref="Safe()"/>
    /// <seealso cref="FixIterator(int)"/>
    public class Iterator {
        private int savedIndex = -1;
        private Node? savedNode;
        
        private int index;
        private int length;
        private Node curr;
        private readonly DLinkedList<T> super;

        internal Iterator(int index, Node node, DLinkedList<T> parent) {
            super = parent;
            curr = node;
            length = super.length;
            this.index = index;
        }

        /// Checks if this iterator contains a saved state.
        /// This method DOES NOT check if the state can
        /// actually be loaded or if the saved node may
        /// have already been deleted or else. O(1).
        /// <returns>True if there is a saved state.</returns>
        public bool HasState() {
            return savedNode is not null && savedIndex is not -1;
        }

        /// Saves the current state of the iterator. O(1).
        public void SaveState() {
            savedIndex = index;
            savedNode = curr;
        }

        /// Tries to load the saved state. O(1).
        /// Throws an exception if there is no saved state.
        public void LoadState() {
            if (savedNode is null || savedIndex is -1)
                throw new InvalidOperationException("No save state to load found.");
            index = savedIndex;
            curr = savedNode;
            length = super.length;
        }

        /// Switches the current state and the saved state. O(1).
        /// Throws an exception if there is no saved state.
        public void SwitchState() {
            if (savedNode is null || savedIndex is -1)
                throw new InvalidOperationException("No save state to load found.");
            (index, savedIndex) = (savedIndex, index);
            (curr, savedNode) = (savedNode, curr);
        }

        /// Returns the index of the saved state. O(1).
        /// <returns>Index of saved state.</returns>
        public int IndexOfState() {
            return savedIndex;
        }

        /// Invalidates the saved state, so it may not be
        /// used any further. O(1).
        public void InvalidateState() {
            savedIndex = -1;
            savedNode = null;
        }

        /// Checks if the iterator's length matches with the
        /// length of the underlying list. If they do not
        /// match, this must mean that the list has been
        /// structurally modified. O(1).
        /// <returns>True if iterator length equals list length.</returns>
        public bool Safe() {
            return length == super.length;
        }

        /// Sets the index that the iterator uses to perform
        /// its operations to the given value i and resets
        /// the iterator length to the length of the list. O(1).
        public void FixIterator(int i) {
            index = i;
            length = super.length;
        }

        /// Returns the highest addressable index of this iterator. O(1).
        /// <returns>Integer.</returns>
        public int Index() {
            return length - 1;
        }

        /// Returns the index the iterator is currently at. O(1).
        /// <returns>Integer.</returns>
        public int CurrentIndex() {
            return index;
        }

        /// Returns the currently pointed to element. O(1).
        /// <returns>The element.</returns>
        public T Current() {
            return curr.val;
        }

        /// Checks if the iterator has an element to its right. O(1).
        /// <returns>True if iterator is not at end of list.</returns>
        public bool HasNext() {
            return index + 1 < length;
        }

        /// Checks if the iterator has an element to its left. O(1).
        /// /// <returns>True if iterator is not at start of list.</returns>
        public bool HasPrev() {
            return index > 0;
        }

        /// Peeks the next element. O(1).
        public T Next() {
            return curr.next.val;
        }

        /// Peeks the previous element. O(1).
        public T Prev() {
            return curr.prev.val;
        }

        /// Moves the iterator one element to the right. O(1).
        /// <returns>True if operation was successful.
        /// False if we reached the end of the list.
        /// In this case, the iterator does not move.</returns>
        public bool MoveToNext() {
            if (!HasNext()) return false;
            curr = curr.next; 
            ++index;
            return true;
        }

        /// Moves the iterator one element to the left. O(1).
        /// <returns>True if operation was successful.
        /// False if we reached the start of the list.
        /// In this case, the iterator does not move.</returns>
        public bool MoveToPrev() {
            if (!HasPrev()) return false;
            curr = curr.prev;
            --index;
            return true;
        }

        /// Moves the iterator by a relative amount in the list. O(n).
        /// <param name="relativePosition">
        /// Positive integers move the iterator to the right.
        /// Negative integers move the iterator to the left.
        /// 0 does nothing.</param>
        /// <returns>True if moving the iterator does not cause
        /// it to go out of bounds. If this method returns false,
        /// it does nothing.</returns>
        public bool Move(int relativePosition) {
            if (relativePosition is 0) return true;
            if (index + relativePosition > Index() ||
                index + relativePosition < 0)
                return false;

            if (relativePosition > 0)
                for (int i = 0; i < relativePosition; ++i)
                    curr = curr.next;
            else 
                for (int i = 0; i > relativePosition; --i)
                    curr = curr.prev;
            
            index += relativePosition;
            return true;
        }

        /// Moves the iterator to a specific, absolute index. O(n).
        /// <param name="i">The index to go to.</param>
        /// <returns>False if index is out of bounds.
        /// In this case, the method does nothing.</returns>
        public bool MoveToIndex(int i) {
            if (i < 0 || i > Index())
                return false;
            
            if (index < i)
                while (index++ < i)
                    curr = curr.next;
            
            else if (index > i)
                while (index-- > i + 1)
                    curr = curr.prev;
            
            return true;
        }

        /// Safely deletes the current element from the list.
        /// This method preserves the index it currently has,
        /// which means that it sets the current element to
        /// the one next to the element that will be deleted.
        /// So if you delete the element at index 4, the index
        /// of the iterator will remain 4. This does not apply
        /// when the iterator deletes the last node, as there
        /// is no next element in that case, so the index will
        /// decrease by 1 and the iterator points to the new
        /// last element. O(1).
        /// <returns>True if the index stays the same after
        /// calling this method. False if the iterator's
        /// index was decremented, meaning that the iterator
        /// is pointing to the last element.</returns>
        public bool Delete() {
            if (index == Index()) {
                curr = curr.prev;
                super.DeleteLast();
                --index; --length;
                return false;
            }

            curr = curr.next;
            super.DeleteNode(curr.prev);
            --length;
            return true;
        }

        /// Inserts a new element into this list so that it
        /// is to the right of the current element. O(1).
        /// <param name="value">Values of the new element.</param>
        public void InsertAsNext(T value) {
            super.InsertNode(curr, value);
            ++length;
        }

        /// Inserts a new element into this list so that it
        /// is to the left of the current element. O(1).
        /// <param name="value">Values of the new element.</param>
        public void InsertAsPrev(T value) {
            super.InsertNode(curr.prev, value);
            ++index; ++length;
        }

        /// Sets the value of the current element to the given value. O(1).
        /// <param name="value">The new value.</param>
        public void Modify(T value) {
            curr.val = value;
        }
    }

    /// Returns an iterator over this list initialised to
    /// the given index. O(n).
    /// <returns>A new instance of an iterator.</returns>
    public Iterator GetIterator(int index) {
        if (OutOfRange(index))
            throw new IndexOutOfRangeException("DLinkedList index out of bounds.");

        Node node = IterateForwards(index) ? FirstNode() : LastNode();
        if (IterateForwards(index))
            for (int i = 0; i < index; ++i)
                node = node.next;
        else 
            for (int i = Index(); i > index; --i)
                node = node.prev;

        return new Iterator(index, node, this);
    }

    /// Returns an iterator over this list initialised to
    /// the first element. O(1).
    /// <returns>A new instance of an iterator.</returns>
    public Iterator GetIterator() {
        return new Iterator(0, FirstNode(), this);
    }
} 