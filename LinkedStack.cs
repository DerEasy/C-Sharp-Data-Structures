using System.Text;

namespace Data_Structures; 

public class LinkedStack<T> {
    private class Frame {
        internal T val;
        internal Frame next = default!;

        internal Frame(T value) {
            val = value;
        }
        internal Frame(T value, Frame onTop) {
            val = value;
            next = onTop;
        }
    }

    private Frame top = default!;
    private int length;

    public bool IsEmpty() {
        return length is 0;
    }
    
    public int Length() {
        return length;
    }

    public int Index() {
        return length - 1;
    }

    public void Clear() {
        top = default!;
        length = 0;
    }
    
    public bool Contains(T value) {
        var frame = top;
        for (int i = 0; i++ < length; frame = frame.next)
            if (frame.val!.Equals(value))
                return true;
        return false;
    }

    public void Push(T value) {
        top = new Frame(value, top);
        ++length;
    }

    public T Pop() {
        if (IsEmpty()) throw new InvalidOperationException("Stack empty.");
        T value = top.val;
        top = top.next;
        --length;
        return value;
    }

    public bool TryPop(out T output) {
        try {
            output = Pop();
            return true;
        } catch (InvalidOperationException) {
            output = default!;
            return false;
        }
    }

    public T Peek() {
        if (IsEmpty()) throw new InvalidOperationException("Stack empty.");
        return top.val;
    }

    public bool TryPeek(out T output) {
        try {
            output = Peek();
            return true;
        } catch (InvalidOperationException) {
            output = default!;
            return false;
        }
    }

    public T ElementAt(uint i) {
        if (i > Index())
            throw new IndexOutOfRangeException("LinkedStack index out of bounds.");

        var frame = top;
        for (int k = 0; k < i; ++k)
            frame = frame.next;

        return frame.val;
    }

    public override string ToString() {
        if (IsEmpty()) return "[]";
        
        var sb = new StringBuilder();
        var frame = top;

        sb.Append('[');
        for (int i = 0; i++ < length - 1; frame = frame.next)
            sb.Append($"{frame.val}, ");
        sb.Append($"{frame.val}]");

        return sb.ToString();
    }
    
    public T[] ToArray() {
        T[] array = new T[length];
        var frame = top;
        
        for (int i = 0; i < length; frame = frame.next)
            array[i++] = frame.val;

        return array;
    }

    private Frame Enqueue(T value, Frame? prev) {
        var insertable = new Frame(value);
        
        if (prev is null)
            top = insertable;
        else
            prev.next = insertable;
        
        ++length;
        return insertable;
    }

    public LinkedStack<T> Copy() {
        var stack = new LinkedStack<T>();
        var frame = top;
        var stackFrame = stack.top;
        
        for (int i = 0; i < length; frame = frame.next)
            stackFrame = stack.Enqueue(frame.val, i++ == 0 ? null : stackFrame);

        return stack;
    }
}