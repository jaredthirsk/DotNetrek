//#define TRACE_ENDIAN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Net;

namespace LionFire.Netrek
{
    public static class MarshalSerialization
    {
        public static T Deserialize<T>(byte[] buffer, int bufferSize)
        //where T : struct
        {
            T obj;

            int size = Marshal.SizeOf(typeof(T));

            if (bufferSize < size)
            {
                throw new ArgumentException("Deserialize failure: provided byte array is too small for this object type: " + typeof(T) + " is " + size + " bytes, but array is " + bufferSize + " bytes.");
            }

            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(buffer, 0, ptr, size);

            obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            obj = ConvertToHostOrder(obj);
            return obj;
        }

        public static T ConvertToHostOrder<T>(T obj)
        {
            foreach (FieldInfo mi in typeof(T).GetFields())
            {
                if (mi.FieldType == typeof(int))
                {
                    int oldValue = (int)mi.GetValue(obj);
                    int newValue = IPAddress.NetworkToHostOrder(oldValue);
                    mi.SetValue(obj, newValue);
                    //mi.SetValue(obj, IPAddress.NetworkToHostOrder((int)mi.GetValue(obj)));
                }
                else if (mi.FieldType == typeof(uint))
                {
                    unchecked
                    {
                        int oldValue = (int)(uint)mi.GetValue(obj);
                        uint newValue = (uint)IPAddress.NetworkToHostOrder(oldValue);
                        mi.SetValue(obj, newValue);
                    }
                    //mi.SetValue(obj, (uint)IPAddress.NetworkToHostOrder((int)mi.GetValue(obj)));
                }
                else if (mi.FieldType == typeof(short))
                {
                    short oldValue = (short)mi.GetValue(obj);
                    short newValue = IPAddress.NetworkToHostOrder(oldValue);
                    mi.SetValue(obj, newValue);
                    //mi.SetValue(obj, IPAddress.NetworkToHostOrder((short)mi.GetValue(obj)));
                }
                else if (mi.FieldType == typeof(ushort))
                {
                    unchecked
                    {
                        short oldValue = (short)(ushort)mi.GetValue(obj);
                        ushort newValue = (ushort)IPAddress.NetworkToHostOrder(oldValue);
                        mi.SetValue(obj, newValue);
                    }
                    //mi.SetValue(obj, (uint)IPAddress.NetworkToHostOrder((int)mi.GetValue(obj)));
                }
                //else if (mi.FieldType == typeof(long)) // 64-bits not used in netrek
                //{
                //    mi.SetValue(obj, IPAddress.NetworkToHostOrder((Int32)mi.GetValue(obj)));
                //}
            }
            return obj;
        }
        public static T ConvertToNetworkOrder<T>(T obj)
        {
            foreach (FieldInfo mi in typeof(T).GetFields())
            {
                if (mi.FieldType == typeof(int))
                {
                    int oldValue = (int)mi.GetValue(obj);
                    int newValue = IPAddress.HostToNetworkOrder(oldValue);
#if TRACE_ENDIAN
                    if (oldValue != newValue)
                    {
                        l.Trace(oldValue.ToString() + " => (net) " + newValue);
                    }
#endif
                    mi.SetValue(obj, newValue);
                    //mi.SetValue(obj, IPAddress.NetworkToHostOrder((int)mi.GetValue(obj)));
                }
                else if (mi.FieldType == typeof(short))
                {
                    short oldValue = (short)mi.GetValue(obj);
                    short newValue = IPAddress.HostToNetworkOrder(oldValue);
#if TRACE_ENDIAN
                    if (oldValue != newValue)
                    {
                        l.Trace(oldValue.ToString() + " => (net) " + newValue);
                    }
#endif
                    mi.SetValue(obj, newValue);
                    //mi.SetValue(obj, IPAddress.NetworkToHostOrder((short)mi.GetValue(obj)));
                }
                //else if (mi.FieldType == typeof(long))
                //{
                //    mi.SetValue(obj, IPAddress.NetworkToHostOrder((Int32)mi.GetValue(obj)));
                //}

            }
            return obj;
        }

        public static byte[] MessageToBytes<T>(T obj, int sequenceNumber = -1)
        {
            obj = ConvertToNetworkOrder(obj);

            int size = System.Runtime.InteropServices.Marshal.SizeOf(obj);
            if (sequenceNumber != -1)
            {
                size += sizeof(int);
            }

            byte[] bytes = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, bytes, 0, size);
            if (sequenceNumber != -1)
            {
                Marshal.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(sequenceNumber)), 0, ptr + size - sizeof(int), sizeof(int));
            }
            Marshal.FreeHGlobal(ptr);

            return bytes;
        }

        private static ILogger l = Log.Get();
    }
}
