using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Wasmtime
{
    public class HelperFunctions
    {
        public static Type[] GetWithoutLast(Type[] args)
        {
            var withoutLast = new Type[args.Length - 1];
            for (int i = 0; i < args.Length - 1; i++)
            {
                withoutLast[i] = args[i];
            }
            return withoutLast;
        }

        public static Type[] GetWithoutFirst(Type[] args)
        {
            var withoutLast = new Type[args.Length - 1];
            for (int i = 0; i < args.Length - 1; i++)
            {
                withoutLast[i] = args[i+1];
            }
            return withoutLast;
        }

        public static unsafe string PtrToStringUTF8(IntPtr ptr, int byteLen)
        {
            if (ptr == IntPtr.Zero)
                throw new ArgumentNullException(nameof(ptr));
            if (byteLen < 0)
                throw new ArgumentOutOfRangeException(nameof(byteLen));

            
            var buffer = new byte[byteLen];
            Marshal.Copy(ptr, buffer, 0, byteLen);
            return Encoding.UTF8.GetString(buffer);
        }
        
    }
}
