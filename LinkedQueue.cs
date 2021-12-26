using System.Text;

namespace Data_Structures;

public class LinkedQueue<T> {
    private class Frame {
        internal readonly T val = default!;
        internal Frame next = default!;

        internal Frame() {}
        internal Frame(T value) {
            val = value;
        }
    }
    
    private readonly Frame read;
    private readonly Frame write;
    private int length;

    public LinkedQueue() {
        read = new Frame();
        write = new Frame();
        read.next = write;
        write.next = read;
    }

    private Frame Newest() {
        return write.next;
    }

    private Frame Oldest() {
        return read.next;
    }

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
        read.next = write;
        write.next = read;
        length = 0;
    }

    public bool Contains(T value) {
        if (Newest().Equals(value)) return true;
        
        var frame = Oldest();
        for (int i = 0; i++ < Index(); frame = frame.next)
            if (frame.val!.Equals(value) )
                return true;
        return false;
    }

    public void Enqueue(T value) {
        var insertable = new Frame(value) { next = write };
        Newest().next = insertable;
        write.next = insertable;
        ++length;
    }

    public T Dequeue() {
        if (IsEmpty()) throw new InvalidOperationException("Queue empty.");
        T value = Oldest().val;
        read.next = Oldest().next;
        --length;
        return value;
    }

    public bool TryDequeue(out T output) {
        try {
            output = Dequeue();
            return true;
        } catch (InvalidOperationException){
            output = default!;
            return false;
        }
    }

    public T Peek() {
        if (IsEmpty()) throw new InvalidOperationException("Queue empty.");
        return Oldest().val;
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

        var frame = Oldest();
        for (int k = 0; k < i; ++k)
            frame = frame.next;

        return frame.val;
    }

    public override string ToString() {
        if (IsEmpty()) return "[]";
        
        var sb = new StringBuilder();
        var frame = Oldest();
        
        sb.Append('[');
        for (int i = 0; i++ < length - 1; frame = frame.next)
            sb.Append($"{frame.val}, ");
        sb.Append($"{frame.val}]");

        return sb.ToString();
    }

    public T[] ToArray() {
        T[] array = new T[length];
        var frame = Oldest();
        
        for (int i = 0; i < length; frame = frame.next)
            array[i++] = frame.val;

        return array;
    }

    public LinkedQueue<T> Copy() {
        var queue = new LinkedQueue<T>();
        var frame = Oldest();
        for (int i = 0; i++ < length; frame = frame.next)
            queue.Enqueue(frame.val);

        return queue;
    }
}