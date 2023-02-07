﻿using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Mono.Core
{
    /// <summary>
    /// Utility class.
    /// </summary>
    public static class Utilities {
        /// <summary>
        /// Return the sizeof a struct from a CLR. Equivalent to sizeof operator but works on generics too.
        /// </summary>
        /// <typeparam name="T">a struct to evaluate</typeparam>
        /// <returns>sizeof this struct</returns>
        //public static int SizeOf<T>() where T : struct {
        //    int size = Marshal.SizeOf(typeof(T));
        //    return size;           
        //    //return Interop.SizeOf<T>();
        //}
        public static int SizeOf<T>() {
            var tk = typeof(T);
            if (genericSizes.ContainsKey(tk)) {
                return genericSizes[tk];
            }
            if (tk.IsValueType) { 
                return Marshal.SizeOf(tk);
            }
            return SizeOf(typeof(T));
        }
        public delegate T Func<T>();
        public static int SizeOf(this Type type) {
            var dynamicMethod = new DynamicMethod("SizeOf", typeof(int), Type.EmptyTypes);
            var generator = dynamicMethod.GetILGenerator();

            generator.Emit(OpCodes.Sizeof, type);
            generator.Emit(OpCodes.Ret);

            var function = (Func<int>)dynamicMethod.CreateDelegate(typeof(Func<int>));
            return function();
        }
        /// <summary>
        /// A lookup of type sizes. Used instead of Marshal.SizeOf() which has additional
        /// overhead, but also is compatible with generic functions for simplified code.
        /// </summary>
        private static Dictionary<Type, int> genericSizes = new Dictionary<Type, int>()
        {
            { typeof(bool),     sizeof(bool) },
            { typeof(float),    sizeof(float) },
            { typeof(double),   sizeof(double) },
            { typeof(sbyte),    sizeof(sbyte) },
            { typeof(byte),     sizeof(byte) },
            { typeof(short),    sizeof(short) },
            { typeof(ushort),   sizeof(ushort) },
            { typeof(int),      sizeof(int) },
            { typeof(uint),     sizeof(uint) },
            { typeof(ulong),    sizeof(ulong) },
            { typeof(long),     sizeof(long) },
        };
    }
}
namespace Mono.Core.Serialization { 

}
//#define Mono_PLATFORM_WINDOWS_DESKTOP
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Security;
//using System.Text;
//using System.Threading;
//#if NET45
//using TaskEx = System.Threading.Tasks.Task;
//#endif

//namespace Mono.Core
//{
//    /// <summary>
//    /// Utility class.
//    /// </summary>
//    public static class Utilities
//    {
//#if Mono_PLATFORM_WINDOWS_PHONE || Mono_PLATFORM_WINDOWS_STORE || Mono_PLATFORM_WINDOWS_10
//        public unsafe static void CopyMemory(IntPtr dest, IntPtr src, int sizeInBytesToCopy)
//        {
//            Interop.memcpy((void*)dest, (void*)src, sizeInBytesToCopy);
//        }
//#else
//#if Mono_PLATFORM_WINDOWS_DESKTOP
//        private const string MemcpyDll = "msvcrt.dll";
//#elif Mono_PLATFORM_ANDROID
//        private const string MemcpyDll = "libc.so";
//#elif Mono_PLATFORM_IOS
//        private const string MemcpyDll = ObjCRuntime.Constants.SystemLibrary;
//#else
//#   error Unsupported platform
//#endif
//        /// <summary>
//        /// Native memcpy.
//        /// </summary>
//        /// <param name="dest">The destination memory location</param>
//        /// <param name="src">The source memory location.</param>
//        /// <param name="sizeInBytesToCopy">The count.</param>
//        /// <returns></returns>
//        [DllImport(MemcpyDll, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
//        [SuppressUnmanagedCodeSecurity]
//        private static extern IntPtr CopyMemory(IntPtr dest, IntPtr src, ulong sizeInBytesToCopy);

//        public static void CopyMemory(IntPtr dest, IntPtr src, int sizeInBytesToCopy) {
//            CopyMemory(dest, src, (ulong)sizeInBytesToCopy);
//        }
//#endif

//        /// <summary>
//        /// Compares two block of memory.
//        /// </summary>
//        /// <param name="from">The pointer to compare from.</param>
//        /// <param name="against">The pointer to compare against.</param>
//        /// <param name="sizeToCompare">The size in bytes to compare.</param>
//        /// <returns>True if the buffers are equivalent, false otherwise.</returns>
//        public unsafe static bool CompareMemory(IntPtr from, IntPtr against, int sizeToCompare) {
//            var pSrc = (byte*)from;
//            var pDst = (byte*)against;

//            // Compare 8 bytes.
//            int numberOf = sizeToCompare >> 3;
//            while (numberOf > 0) {
//                if (*(long*)pSrc != *(long*)pDst)
//                    return false;
//                pSrc += 8;
//                pDst += 8;
//                numberOf--;
//            }

//            // Compare remaining bytes.
//            numberOf = sizeToCompare & 7;
//            while (numberOf > 0) {
//                if (*pSrc != *pDst)
//                    return false;
//                pSrc++;
//                pDst++;
//                numberOf--;
//            }

//            return true;
//        }

//        /// <summary>
//        /// Clears the memory.
//        /// </summary>
//        /// <param name="dest">The dest.</param>
//        /// <param name="value">The value.</param>
//        /// <param name="sizeInBytesToClear">The size in bytes to clear.</param>
//        public static void ClearMemory(IntPtr dest, byte value, int sizeInBytesToClear) {
//            unsafe {
//                Interop.memset((void*)dest, value, sizeInBytesToClear);
//            }
//        }

//        /// <summary>
//        /// Return the sizeof a struct from a CLR. Equivalent to sizeof operator but works on generics too.
//        /// </summary>
//        /// <typeparam name="T">a struct to evaluate</typeparam>
//        /// <returns>sizeof this struct</returns>
//        public static int SizeOf<T>() where T : struct {
//            return Interop.SizeOf<T>();
//        }

//        /// <summary>
//        /// Return the sizeof an array of struct. Equivalent to sizeof operator but works on generics too.
//        /// </summary>
//        /// <typeparam name="T">a struct</typeparam>
//        /// <param name="array">The array of struct to evaluate.</param>
//        /// <returns>sizeof in bytes of this array of struct</returns>
//        public static int SizeOf<T>(T[] array) where T : struct {
//            return array == null ? 0 : array.Length * Interop.SizeOf<T>();
//        }

//        /// <summary>
//        /// Pins the specified source and call an action with the pinned pointer.
//        /// </summary>
//        /// <typeparam name="T">The type of the structure to pin</typeparam>
//        /// <param name="source">The source.</param>
//        /// <param name="pinAction">The pin action to perform on the pinned pointer.</param>
//        public static void Pin<T>(ref T source, Action<IntPtr> pinAction) where T : struct {
//            unsafe {
//                pinAction((IntPtr)Interop.Fixed(ref source));
//            }
//        }

//        /// <summary>
//        /// Pins the specified source and call an action with the pinned pointer.
//        /// </summary>
//        /// <typeparam name="T">The type of the structure to pin</typeparam>
//        /// <param name="source">The source array.</param>
//        /// <param name="pinAction">The pin action to perform on the pinned pointer.</param>
//        public static void Pin<T>(T[] source, Action<IntPtr> pinAction) where T : struct {
//            unsafe {
//                pinAction(source == null ? IntPtr.Zero : (IntPtr)Interop.Fixed(source));
//            }
//        }

//        /// <summary>
//        /// Covnerts a structured array to an equivalent byte array.
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="source">The source.</param>
//        /// <returns></returns>
//        public static byte[] ToByteArray<T>(T[] source) where T : struct {
//            if (source == null) return null;

//            var buffer = new byte[SizeOf<T>() * source.Length];

//            if (source.Length == 0)
//                return buffer;

//            unsafe {
//                fixed (void* pBuffer = buffer)
//                    Interop.Write<T>(pBuffer, source, 0, source.Length);
//            }
//            return buffer;
//        }

//        /// <summary>
//        /// Reads the specified T data from a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to read</typeparam>
//        /// <param name="source">Memory location to read from.</param>
//        /// <returns>The data read from the memory location</returns>
//        public static T Read<T>(IntPtr source) where T : struct {
//            unsafe {
//                return Interop.ReadInline<T>((void*)source);
//            }
//        }

//        /// <summary>
//        /// Reads the specified T data from a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to read</typeparam>
//        /// <param name="source">Memory location to read from.</param>
//        /// <param name="data">The data write to.</param>
//        /// <returns>source pointer + sizeof(T)</returns>
//        public static void Read<T>(IntPtr source, ref T data) where T : struct {
//            unsafe {
//                Interop.CopyInline<T>(ref data, (void*)source);
//            }
//        }

//        /// <summary>
//        /// Reads the specified T data from a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to read</typeparam>
//        /// <param name="source">Memory location to read from.</param>
//        /// <param name="data">The data write to.</param>
//        /// <returns>source pointer + sizeof(T)</returns>
//        public static void ReadOut<T>(IntPtr source, out T data) where T : struct {
//            unsafe {
//                Interop.CopyInlineOut<T>(out data, (void*)source);
//            }
//        }

//        /// <summary>
//        /// Reads the specified T data from a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to read</typeparam>
//        /// <param name="source">Memory location to read from.</param>
//        /// <param name="data">The data write to.</param>
//        /// <returns>source pointer + sizeof(T)</returns>
//        public static IntPtr ReadAndPosition<T>(IntPtr source, ref T data) where T : struct {
//            unsafe {
//                return (IntPtr)Interop.Read<T>((void*)source, ref data);
//            }
//        }

//        /// <summary>
//        /// Reads the specified array T[] data from a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to read</typeparam>
//        /// <param name="source">Memory location to read from.</param>
//        /// <param name="data">The data write to.</param>
//        /// <param name="offset">The offset in the array to write to.</param>
//        /// <param name="count">The number of T element to read from the memory location</param>
//        /// <returns>source pointer + sizeof(T) * count</returns>
//        public static IntPtr Read<T>(IntPtr source, T[] data, int offset, int count) where T : struct {
//            unsafe {
//                return (IntPtr)Interop.Read<T>((void*)source, data, offset, count);
//            }
//        }

//        /// <summary>
//        /// Writes the specified T data to a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to write</typeparam>
//        /// <param name="destination">Memory location to write to.</param>
//        /// <param name="data">The data to write.</param>
//        /// <returns>destination pointer + sizeof(T)</returns>
//        public static void Write<T>(IntPtr destination, ref T data) where T : struct {
//            unsafe {
//                Interop.CopyInline((void*)destination, ref data);
//            }
//        }

//        /// <summary>
//        /// Writes the specified T data to a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to write</typeparam>
//        /// <param name="destination">Memory location to write to.</param>
//        /// <param name="data">The data to write.</param>
//        /// <returns>destination pointer + sizeof(T)</returns>
//        public static IntPtr WriteAndPosition<T>(IntPtr destination, ref T data) where T : struct {
//            unsafe {
//                return (IntPtr)Interop.Write<T>((void*)destination, ref data);
//            }
//        }

//        /// <summary>
//        /// Writes the specified array T[] data to a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to write</typeparam>
//        /// <param name="destination">Memory location to write to.</param>
//        /// <param name="data">The array of T data to write.</param>
//        /// <param name="offset">The offset in the array to read from.</param>
//        /// <param name="count">The number of T element to write to the memory location</param>
//        /// <returns>destination pointer + sizeof(T) * count</returns>
//        public static void Write<T>(byte[] destination, T[] data, int offset, int count) where T : struct {
//            unsafe {
//                fixed (void* pDest = destination) {
//                    Write((IntPtr)pDest, data, offset, count);
//                }
//            }
//        }

//        /// <summary>
//        /// Writes the specified array T[] data to a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to write</typeparam>
//        /// <param name="destination">Memory location to write to.</param>
//        /// <param name="data">The array of T data to write.</param>
//        /// <param name="offset">The offset in the array to read from.</param>
//        /// <param name="count">The number of T element to write to the memory location</param>
//        /// <returns>destination pointer + sizeof(T) * count</returns>
//        public static IntPtr Write<T>(IntPtr destination, T[] data, int offset, int count) where T : struct {
//            unsafe {
//                return (IntPtr)Interop.Write<T>((void*)destination, data, offset, count);
//            }
//        }

//        /// <summary>
//        /// Allocate an aligned memory buffer.
//        /// </summary>
//        /// <param name="sizeInBytes">Size of the buffer to allocate.</param>
//        /// <param name="align">Alignment, 16 bytes by default.</param>
//        /// <returns>A pointer to a buffer aligned.</returns>
//        /// <remarks>
//        /// To free this buffer, call <see cref="FreeMemory"/>
//        /// </remarks>
//        public unsafe static IntPtr AllocateMemory(int sizeInBytes, int align = 16) {
//            int mask = align - 1;
//            var memPtr = Marshal.AllocHGlobal(sizeInBytes + mask + IntPtr.Size);
//            var ptr = (long)((byte*)memPtr + sizeof(void*) + mask) & ~mask;
//            ((IntPtr*)ptr)[-1] = memPtr;
//            return new IntPtr(ptr);
//        }

//        /// <summary>
//        /// Allocate an aligned memory buffer and clear it with a specified value (0 by defaault).
//        /// </summary>
//        /// <param name="sizeInBytes">Size of the buffer to allocate.</param>
//        /// <param name="clearValue">Default value used to clear the buffer.</param>
//        /// <param name="align">Alignment, 16 bytes by default.</param>
//        /// <returns>A pointer to a buffer aligned.</returns>
//        /// <remarks>
//        /// To free this buffer, call <see cref="FreeMemory"/>
//        /// </remarks>
//        public static IntPtr AllocateClearedMemory(int sizeInBytes, byte clearValue = 0, int align = 16) {
//            var ptr = AllocateMemory(sizeInBytes, align);
//            ClearMemory(ptr, clearValue, sizeInBytes);
//            return ptr;
//        }

//        /// <summary>
//        /// Determines whether the specified memory pointer is aligned in memory.
//        /// </summary>
//        /// <param name="memoryPtr">The memory pointer.</param>
//        /// <param name="align">The align.</param>
//        /// <returns><c>true</c> if the specified memory pointer is aligned in memory; otherwise, <c>false</c>.</returns>
//        public static bool IsMemoryAligned(IntPtr memoryPtr, int align = 16) {
//            return ((memoryPtr.ToInt64() & (align - 1)) == 0);
//        }

//        /// <summary>
//        /// Allocate an aligned memory buffer.
//        /// </summary>
//        /// <returns>A pointer to a buffer aligned.</returns>
//        /// <remarks>
//        /// The buffer must have been allocated with <see cref="AllocateMemory"/>
//        /// </remarks>
//        public unsafe static void FreeMemory(IntPtr alignedBuffer) {
//            Marshal.FreeHGlobal(((IntPtr*)alignedBuffer)[-1]);
//        }

//        /// <summary>
//        /// If non-null, disposes the specified object and set it to null, otherwise do nothing.
//        /// </summary>
//        /// <param name="disposable">The disposable.</param>
//        public static void Dispose<T>(ref T disposable) where T : class, IDisposable {
//            if (disposable != null) {
//                disposable.Dispose();
//                disposable = null;
//            }
//        }

//        /// <summary>
//        /// String helper join method to display an array of object as a single string.
//        /// </summary>
//        /// <param name="separator">The separator.</param>
//        /// <param name="array">The array.</param>
//        /// <returns>a string with array elements serparated by the seperator</returns>
//        public static string Join<T>(string separator, T[] array) {
//            var text = new StringBuilder();
//            if (array != null) {
//                for (int i = 0; i < array.Length; i++) {
//                    if (i > 0) text.Append(separator);
//                    text.Append(array[i]);
//                }
//            }
//            return text.ToString();
//        }

//        /// <summary>
//        /// String helper join method to display an enumrable of object as a single string.
//        /// </summary>
//        /// <param name="separator">The separator.</param>
//        /// <param name="elements">The enumerable.</param>
//        /// <returns>a string with array elements serparated by the seperator</returns>
//        public static string Join(string separator, IEnumerable elements) {
//            var elementList = new List<string>();
//            foreach (var element in elements)
//                elementList.Add(element.ToString());

//            var text = new StringBuilder();
//            for (int i = 0; i < elementList.Count; i++) {
//                var element = elementList[i];
//                if (i > 0) text.Append(separator);
//                text.Append(element);
//            }
//            return text.ToString();
//        }

//        /// <summary>
//        /// String helper join method to display an enumrable of object as a single string.
//        /// </summary>
//        /// <param name="separator">The separator.</param>
//        /// <param name="elements">The enumerable.</param>
//        /// <returns>a string with array elements serparated by the seperator</returns>
//        public static string Join(string separator, IEnumerator elements) {
//            var elementList = new List<string>();
//            while (elements.MoveNext())
//                elementList.Add(elements.Current.ToString());

//            var text = new StringBuilder();
//            for (int i = 0; i < elementList.Count; i++) {
//                var element = elementList[i];
//                if (i > 0) text.Append(separator);
//                text.Append(element);
//            }
//            return text.ToString();
//        }

//        /// <summary>
//        ///   Read stream to a byte[] buffer
//        /// </summary>
//        /// <param name = "stream">input stream</param>
//        /// <returns>a byte[] buffer</returns>
//        public static byte[] ReadStream(Stream stream) {
//            int readLength = 0;
//            return ReadStream(stream, ref readLength);
//        }

//        /// <summary>
//        ///   Read stream to a byte[] buffer
//        /// </summary>
//        /// <param name = "stream">input stream</param>
//        /// <param name = "readLength">length to read</param>
//        /// <returns>a byte[] buffer</returns>
//        public static byte[] ReadStream(Stream stream, ref int readLength) {
//            System.Diagnostics.Debug.Assert(stream != null);
//            System.Diagnostics.Debug.Assert(stream.CanRead);
//            int num = readLength;
//            System.Diagnostics.Debug.Assert(num <= (stream.Length - stream.Position));
//            if (num == 0)
//                readLength = (int)(stream.Length - stream.Position);
//            num = readLength;

//            System.Diagnostics.Debug.Assert(num >= 0);
//            if (num == 0)
//                return new byte[0];

//            byte[] buffer = new byte[num];
//            int bytesRead = 0;
//            if (num > 0) {
//                do {
//                    bytesRead += stream.Read(buffer, bytesRead, readLength - bytesRead);
//                } while (bytesRead < readLength);
//            }
//            return buffer;
//        }

//        /// <summary>
//        /// Computes a hashcode for a dictionary.
//        /// </summary>
//        /// <returns>Hashcode for the list.</returns>
//        public static int GetHashCode(IDictionary dict) {
//            if (dict == null)
//                return 0;

//            int hashCode = 0;
//            foreach (DictionaryEntry keyValue in dict) {
//                hashCode = (hashCode * 397) ^ keyValue.Key.GetHashCode();
//                hashCode = (hashCode * 397) ^ (keyValue.Value != null ? keyValue.Value.GetHashCode() : 0);
//            }
//            return hashCode;
//        }

//        /// <summary>
//        /// Computes a hashcode for an enumeration
//        /// </summary>
//        /// <param name="it">An enumerator.</param>
//        /// <returns>Hashcode for the list.</returns>
//        public static int GetHashCode(IEnumerable it) {
//            if (it == null)
//                return 0;

//            int hashCode = 0;
//            foreach (var current in it) {
//                hashCode = (hashCode * 397) ^ ((current == null) ? 0 : current.GetHashCode());
//            }
//            return hashCode;
//        }

//        /// <summary>
//        /// Computes a hashcode for an enumeration
//        /// </summary>
//        /// <param name="it">An enumerator.</param>
//        /// <returns>Hashcode for the list.</returns>
//        public static int GetHashCode(IEnumerator it) {
//            if (it == null)
//                return 0;

//            int hashCode = 0;
//            while (it.MoveNext()) {
//                var current = it.Current;
//                hashCode = (hashCode * 397) ^ ((current == null) ? 0 : current.GetHashCode());
//            }
//            return hashCode;
//        }

//        /// <summary>
//        /// Compares two collection, element by elements.
//        /// </summary>
//        /// <param name="left">A "from" enumerator.</param>
//        /// <param name="right">A "to" enumerator.</param>
//        /// <returns>True if lists are identical. False otherwise.</returns>
//        public static bool Compare(IEnumerable left, IEnumerable right) {
//            if (ReferenceEquals(left, right))
//                return true;
//            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
//                return false;

//            return Compare(left.GetEnumerator(), right.GetEnumerator());
//        }

//        /// <summary>
//        /// Compares two collection, element by elements.
//        /// </summary>
//        /// <param name="leftIt">A "from" enumerator.</param>
//        /// <param name="rightIt">A "to" enumerator.</param>
//        /// <returns>True if lists are identical. False otherwise.</returns>
//        public static bool Compare(IEnumerator leftIt, IEnumerator rightIt) {
//            if (ReferenceEquals(leftIt, rightIt))
//                return true;
//            if (ReferenceEquals(leftIt, null) || ReferenceEquals(rightIt, null))
//                return false;

//            bool hasLeftNext;
//            bool hasRightNext;
//            while (true) {
//                hasLeftNext = leftIt.MoveNext();
//                hasRightNext = rightIt.MoveNext();
//                if (!hasLeftNext || !hasRightNext)
//                    break;

//                if (!Equals(leftIt.Current, rightIt.Current))
//                    return false;
//            }

//            // If there is any left element
//            if (hasLeftNext != hasRightNext)
//                return false;

//            return true;
//        }

//        /// <summary>
//        /// Compares two collection, element by elements.
//        /// </summary>
//        /// <param name="first">The collection to compare from.</param>
//        /// <param name="second">The colllection to compare to.</param>
//        /// <returns>True if lists are identical (but no necessarely of the same time). False otherwise.</returns>
//        public static bool Compare<TKey, TValue>(IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second) {
//            if (ReferenceEquals(first, second)) return true;
//            if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) return false;
//            if (first.Count != second.Count) return false;

//            var comparer = EqualityComparer<TValue>.Default;

//            foreach (var keyValue in first) {
//                TValue secondValue;
//                if (!second.TryGetValue(keyValue.Key, out secondValue)) return false;
//                if (!comparer.Equals(keyValue.Value, secondValue)) return false;
//            }

//            // Check that all keys in second are in first
//            return second.Keys.All(first.ContainsKey);
//        }

//        public static bool Compare<T>(T[] left, T[] right) {
//            if (ReferenceEquals(left, right))
//                return true;
//            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
//                return false;

//            if (left.Length != right.Length)
//                return false;

//            var comparer = EqualityComparer<T>.Default;
//            for (int i = 0; i < left.Length; ++i) {
//                if (!comparer.Equals(left[i], right[i]))
//                    return false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// Compares two collection, element by elements.
//        /// </summary>
//        /// <param name="left">The collection to compare from.</param>
//        /// <param name="right">The colllection to compare to.</param>
//        /// <returns>True if lists are identical (but no necessarely of the same time). False otherwise.</returns>
//        public static bool Compare<T>(ICollection<T> left, ICollection<T> right) {
//            if (ReferenceEquals(left, right))
//                return true;
//            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
//                return false;

//            if (left.Count != right.Count)
//                return false;

//            int count = 0;
//            var leftIt = left.GetEnumerator();
//            var rightIt = right.GetEnumerator();
//            var comparer = EqualityComparer<T>.Default;
//            while (leftIt.MoveNext() && rightIt.MoveNext()) {
//                if (!comparer.Equals(leftIt.Current, rightIt.Current))
//                    return false;
//                count++;
//            }

//            // Just double check to make sure that the iterator actually returns
//            // the exact number of elements
//            if (count != left.Count)
//                return false;

//            return true;
//        }

//        /// <summary>
//        /// Swaps the value between two references.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to swap.</typeparam>
//        /// <param name="left">The left value.</param>
//        /// <param name="right">The right value.</param>
//        public static void Swap<T>(ref T left, ref T right) {
//            var temp = left;
//            left = right;
//            right = temp;
//        }

//        /// <summary>
//        /// Suspends the current thread of a <see cref="sleepTimeInMillis"/>.
//        /// </summary>
//        /// <param name="sleepTime">The duration to sleep.</param>
//        public static void Sleep(TimeSpan sleepTime) {
//#if SILICONSTUDIO_PLATFORM_WINDOWS_DESKTOP || SILICONSTUDIO_PLATFORM_IOS || SILICONSTUDIO_PLATFORM_ANDROID
//            Thread.Sleep(sleepTime);
//#else
//            TaskEx.Delay(sleepTime).Wait();
//#endif
//        }

//        /// <summary>
//        /// Suspends the current thread of a <see cref="sleepTimeInMillis"/>.
//        /// </summary>
//        /// <param name="sleepTimeInMillis">The duration to sleep in milliseconds.</param>
//        public static void Sleep(int sleepTimeInMillis) {
//            Sleep(TimeSpan.FromMilliseconds(sleepTimeInMillis));
//        }

//        /// <summary>
//        /// Writes the specified T data to a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to write</typeparam>
//        /// <param name="destination">Memory location to write to.</param>
//        /// <param name="data">The data to write.</param>
//        /// <returns>destination pointer + sizeof(T)</returns>
//        internal static void UnsafeWrite<T>(IntPtr destination, ref T data) {
//            unsafe {
//                Interop.CopyInline((void*)destination, ref data);
//            }
//        }

//        /// <summary>
//        /// Reads the specified T data from a memory location.
//        /// </summary>
//        /// <typeparam name="T">Type of a data to read</typeparam>
//        /// <param name="source">Memory location to read from.</param>
//        /// <param name="data">The data write to.</param>
//        /// <returns>source pointer + sizeof(T)</returns>
//        internal static void UnsafeReadOut<T>(IntPtr source, out T data) {
//            unsafe {
//                Interop.CopyInlineOut<T>(out data, (void*)source);
//            }
//        }

//        /// <summary>
//        /// Return the sizeof a struct from a CLR. Equivalent to sizeof operator but works on generics too.
//        /// </summary>
//        /// <typeparam name="T">a struct to evaluate</typeparam>
//        /// <returns>sizeof this struct</returns>
//        internal static int UnsafeSizeOf<T>() {
//            return Interop.SizeOf<T>();
//        }
//    }
//}