using Unity.Netcode;
using Unity.Collections;

namespace Network
{
    public struct FixedStringArray : INetworkSerializable
    {
        public FixedString128Bytes[] Values;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            int length = Values == null ? 0 : Values.Length;
            serializer.SerializeValue(ref length);

            if (serializer.IsReader)
                Values = new FixedString128Bytes[length];

            for (int i = 0; i < length; i++)
                serializer.SerializeValue(ref Values[i]);
        }
    }
}
