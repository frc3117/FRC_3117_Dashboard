using NetworkTablesNET.Events.Internal;
using UnityEngine.Events;

namespace NetworkTablesNET.Events
{
    namespace Internal
    {
        public class ValueEvent<T> : UnityEvent<T[]> { }
        public class ArrayEvent<T> : UnityEvent<T[]> { }
    }
    
    [System.Serializable] public class BoolEvent : ValueEvent<bool> { }
    [System.Serializable] public class IntEvent : ValueEvent<int> { }
    [System.Serializable] public class FloatEvent : ValueEvent<float> { }
    [System.Serializable] public class DoubleEvent : ValueEvent<double> { }
    [System.Serializable] public class StringEvent : ValueEvent<string> { }
    
    [System.Serializable] public class BoolArrayEvent : ArrayEvent<bool> { }
    [System.Serializable] public class IntArrayEvent : ArrayEvent<int> { }
    [System.Serializable] public class FloatArrayEvent : ArrayEvent<float> { }
    [System.Serializable] public class DoubleArrayEvent : ArrayEvent<double> { }
    [System.Serializable] public class StringArrayEvent : ArrayEvent<string> { }
    [System.Serializable] public class BytesArrayEvent : ArrayEvent<byte> { }
}