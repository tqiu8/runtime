// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices.JavaScript;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace System.Runtime.InteropServices.JavaScript.Tests
{
    public static class HelperMarshal
    {
        internal const string INTEROP_CLASS = "[System.Private.Runtime.InteropServices.JavaScript.Tests]System.Runtime.InteropServices.JavaScript.Tests.HelperMarshal:";
        internal static int _i32Value;
        private static void InvokeI32(int a, int b)
        {
            _i32Value = a + b;
        }

        internal static float _f32Value;
        private static void InvokeFloat(float f)
        {
            _f32Value = f;
        }

        internal static double _f64Value;
        private static void InvokeDouble(double d)
        {
            _f64Value = d;
        }

        internal static long _i64Value;
        private static void InvokeLong(long l)
        {
            _i64Value = l;
        }

        internal static byte[] _byteBuffer;
        private static void MarshalArrayBuffer(ArrayBuffer buffer)
        {
            using (var bytes = new Uint8Array(buffer))
                _byteBuffer = bytes.ToArray();
        }

        private static void MarshalByteBuffer(Uint8Array buffer)
        {
            _byteBuffer = buffer.ToArray();
        }

        internal static int[] _intBuffer;
        private static void MarshalArrayBufferToInt32Array(ArrayBuffer buffer)
        {
            using (var ints = new Int32Array(buffer))
                _intBuffer = ints.ToArray();
        }

        internal static string _stringResource;
        private static void InvokeString(string s)
        {
            _stringResource = s;
        }

        internal static string _marshalledString;
        private static string InvokeMarshalString()
        {
            _marshalledString = "Hic Sunt Dracones";
            return _marshalledString;
        }

        internal static object _object1;
        private static object InvokeObj1(object obj)
        {
            _object1 = obj;
            return obj;
        }

        internal static object _object2;
        private static object InvokeObj2(object obj)
        {
            _object2 = obj;
            return obj;
        }

        internal static object _marshalledObject;
        private static object InvokeMarshalObj()
        {
            _marshalledObject = new object();
            return _marshalledObject;
        }

        internal static int _valOne, _valTwo;
        private static void ManipulateObject(JSObject obj)
        {
            _valOne = (int)obj.Invoke("inc");
            _valTwo = (int)obj.Invoke("add", 20);
        }

        internal static object[] _jsObjects;
        private static void MinipulateObjTypes(JSObject obj)
        {
            _jsObjects = new object[4];
            _jsObjects[0] = obj.Invoke("return_int");
            _jsObjects[1] = obj.Invoke("return_double");
            _jsObjects[2] = obj.Invoke("return_string");
            _jsObjects[3] = obj.Invoke("return_bool");
        }

        internal static int _jsAddFunctionResult;
        private static void UseFunction(JSObject obj)
        {
            _jsAddFunctionResult = (int)obj.Invoke("call", null, 10, 20);
        }

        internal static int _jsAddAsFunctionResult;
        private static void UseAsFunction(Function func)
        {
            _jsAddAsFunctionResult = (int)func.Call(null, 20, 30);
        }

        internal static int _functionResultValue;
        private static Func<int, int, int> CreateFunctionDelegate()
        {
            return (a, b) =>
            {
                _functionResultValue = a + b;
                return _functionResultValue;
            };
        }

        internal static int _intValue;
        private static void InvokeInt(int value)
        {
            _intValue = value;
        }

        internal static IntPtr _intPtrValue;
        private static void InvokeIntPtr(IntPtr i)
        {
            _intPtrValue = i;
        }

        internal static IntPtr _marshaledIntPtrValue;
        private static IntPtr InvokeMarshalIntPtr()
        {
            _marshaledIntPtrValue = (IntPtr)42;
            return _marshaledIntPtrValue;
        }

        internal static object[] _jsProperties;
        private static void RetrieveObjectProperties(JSObject obj)
        {
            _jsProperties = new object[4];
            _jsProperties[0] = obj.GetObjectProperty("myInt");
            _jsProperties[1] = obj.GetObjectProperty("myDouble");
            _jsProperties[2] = obj.GetObjectProperty("myString");
            _jsProperties[3] = obj.GetObjectProperty("myBoolean");
        }

        private static void PopulateObjectProperties(JSObject obj, bool createIfNotExist)
        {
            _jsProperties = new object[4];
            obj.SetObjectProperty("myInt", 100, createIfNotExist);
            obj.SetObjectProperty("myDouble", 4.5, createIfNotExist);
            obj.SetObjectProperty("myString", "qwerty", createIfNotExist);
            obj.SetObjectProperty("myBoolean", true, createIfNotExist);
        }

        private static void MarshalByteBufferToInts(ArrayBuffer buffer)
        {
            using (var bytes = new Uint8Array(buffer))
            {
                var byteBuffer = bytes.ToArray();
                _intBuffer = new int[bytes.Length / sizeof(int)];
                for (int i = 0; i < bytes.Length; i += sizeof(int))
                    _intBuffer[i / sizeof(int)] = BitConverter.ToInt32(byteBuffer, i);
            }
        }

        private static void MarshalInt32Array(Int32Array buffer)
        {
            _intBuffer = buffer.ToArray();
        }

        internal static float[] _floatBuffer;
        private static void MarshalFloat32Array(Float32Array buffer)
        {
            _floatBuffer = buffer.ToArray();
        }
        private static void MarshalArrayBufferToFloat32Array(ArrayBuffer buffer)
        {
            using (var floats = new Float32Array(buffer))
                _floatBuffer = floats.ToArray();
        }

        internal static double[] _doubleBuffer;
        private static void MarshalFloat64Array(Float64Array buffer)
        {
            _doubleBuffer = buffer.ToArray();
        }

        private static void MarshalArrayBufferToFloat64Array(ArrayBuffer buffer)
        {
            using (var doubles = new Float64Array(buffer))
                _doubleBuffer = doubles.ToArray();
        }

        private static void MarshalByteBufferToDoubles(ArrayBuffer buffer)
        {
            using (var doubles = new Float64Array(buffer))
                _doubleBuffer = doubles.ToArray();
        }

        private static void SetTypedArraySByte(JSObject obj)
        {
            sbyte[] buffer = Enumerable.Repeat((sbyte)0x20, 11).ToArray();
            obj.SetObjectProperty("typedArray", Int8Array.From(buffer));
        }

        internal static sbyte[] _taSByte;
        private static void GetTypedArraySByte(JSObject obj)
        {
            _taSByte = ((Int8Array)obj.GetObjectProperty("typedArray")).ToArray();
        }

        private static void SetTypedArrayByte(JSObject obj)
        {
            var dragons = "hic sunt dracones";
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(dragons);
            obj.SetObjectProperty("dracones", Uint8Array.From(buffer));
        }

        internal static byte[] _taByte;
        private static void GetTypedArrayByte(JSObject obj)
        {
            _taByte = ((Uint8Array)obj.GetObjectProperty("dracones")).ToArray();
        }

        private static void SetTypedArrayShort(JSObject obj)
        {
            short[] buffer = Enumerable.Repeat((short)0x20, 13).ToArray();
            obj.SetObjectProperty("typedArray", Int16Array.From(buffer));
        }

        internal static short[] _taShort;
        private static void GetTypedArrayShort(JSObject obj)
        {
            _taShort = ((Int16Array)obj.GetObjectProperty("typedArray")).ToArray();
        }

        private static void SetTypedArrayUShort(JSObject obj)
        {
            ushort[] buffer = Enumerable.Repeat((ushort)0x20, 14).ToArray();
            obj.SetObjectProperty("typedArray", Uint16Array.From(buffer));
        }

        internal static ushort[] _taUShort;
        private static void GetTypedArrayUShort(JSObject obj)
        {
            _taUShort = ((Uint16Array)obj.GetObjectProperty("typedArray")).ToArray();
        }

        private static void SetTypedArrayInt(JSObject obj)
        {
            int[] buffer = Enumerable.Repeat((int)0x20, 15).ToArray();
            obj.SetObjectProperty("typedArray", Int32Array.From(buffer));
        }

        internal static int[] _taInt;
        private static void GetTypedArrayInt(JSObject obj)
        {
            _taInt = ((Int32Array)obj.GetObjectProperty("typedArray")).ToArray();
        }

        public static void SetTypedArrayUInt(JSObject obj)
        {
            uint[] buffer = Enumerable.Repeat((uint)0x20, 16).ToArray();
            obj.SetObjectProperty("typedArray", Uint32Array.From(buffer));
        }

        internal static uint[] _taUInt;
        private static void GetTypedArrayUInt(JSObject obj)
        {
            _taUInt = ((Uint32Array)obj.GetObjectProperty("typedArray")).ToArray();
        }

        private static void SetTypedArrayFloat(JSObject obj)
        {
            float[] buffer = Enumerable.Repeat(3.14f, 17).ToArray();
            obj.SetObjectProperty("typedArray", Float32Array.From(buffer));
        }

        internal static float[] _taFloat;
        private static void GetTypedArrayFloat(JSObject obj)
        {
            _taFloat = ((Float32Array)obj.GetObjectProperty("typedArray")).ToArray();
        }

        private static void SetTypedArrayDouble(JSObject obj)
        {
            double[] buffer = Enumerable.Repeat(3.14d, 18).ToArray();
            obj.SetObjectProperty("typedArray", Float64Array.From(buffer));
        }

        internal static double[] _taDouble;
        private static void GetTypedArrayDouble(JSObject obj)
        {
            _taDouble = ((Float64Array)obj.GetObjectProperty("typedArray")).ToArray();
        }

        private static void SetUint8ClampedArray(JSObject obj, int length)
        {
            var clamped = new byte[length];
            obj.SetObjectProperty("clampedArray", Uint8ClampedArray.From(clamped));
        }

        internal static Uint8ClampedArray _caUInt;
        private static void GetUint8ClampedArray(JSObject obj)
        {
            _caUInt = (Uint8ClampedArray)obj.GetObjectProperty("clampedArray");
        }
        
        private static void SetUint8Array(JSObject obj, int length)
        {
            var array = new byte[length];
            obj.SetObjectProperty("uint8Array", Uint8Array.From(array));
        }

        internal static Uint8Array _uint8Array;
        private static void GetUint8Array(JSObject obj)
        {
            _uint8Array = (Uint8Array)obj.GetObjectProperty("uint8Array");
        }

        private static void SetUint16Array(JSObject obj, int length)
        {
            var array = new ushort[length];
            obj.SetObjectProperty("uint16Array", Uint16Array.From(array));
        }

        internal static Uint16Array _uint16Array;
        private static void GetUint16Array(JSObject obj)
        {
            _uint16Array = (Uint16Array)obj.GetObjectProperty("uint16Array");
        }

        private static void SetUint32Array(JSObject obj, int length)
        {
            var array = new uint[length];
            obj.SetObjectProperty("uint32Array", Uint32Array.From(array));
        }

        internal static Uint32Array _uint32Array;
        private static void GetUint32Array(JSObject obj)
        {
            _uint32Array = (Uint32Array)obj.GetObjectProperty("uint32Array");
        }

        private static void SetInt8Array(JSObject obj, int length)
        {
            var array = new sbyte[length];
            obj.SetObjectProperty("int8Array", Int8Array.From(array));
        }

        internal static Int8Array _int8Array;
        private static void GetInt8Array(JSObject obj)
        {
            _int8Array = (Int8Array)obj.GetObjectProperty("int8Array");
        }

        private static void SetInt16Array(JSObject obj, int length)
        {
            var array = new short[length];
            obj.SetObjectProperty("int16Array", Int16Array.From(array));
        }

        internal static Int16Array _int16Array;
        private static void GetInt16Array(JSObject obj)
        {
            _int16Array = (Int16Array)obj.GetObjectProperty("int16Array");
        }

        private static void SetInt32Array(JSObject obj, int length)
        {
            var array = new int[length];
            obj.SetObjectProperty("int32Array", Int32Array.From(array));
        }

        internal static Int32Array _int32Array;
        private static void GetInt32Array(JSObject obj)
        {
            _int32Array = (Int32Array)obj.GetObjectProperty("int32Array");
        }

        private static void SetFloat32Array(JSObject obj, int length)
        {
            var array = new float[length];
            obj.SetObjectProperty("float32Array", Float32Array.From(array));
        }

        internal static Float32Array _float32Array;
        private static void GetFloat32Array(JSObject obj)
        {
            _float32Array = (Float32Array)obj.GetObjectProperty("float32Array");
        }

        private static void SetFloat64Array(JSObject obj, int length)
        {
            var array = new double[length];
            obj.SetObjectProperty("float64Array", Float64Array.From(array));
        }

        internal static Float64Array _float64Array;
        private static void GetFloat64Array(JSObject obj)
        {
            _float64Array = (Float64Array)obj.GetObjectProperty("float64Array");
        }

        private static void SetUint8ClampedArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("clampedArrayFromSharedArrayBuffer", new Uint8ClampedArray(new SharedArrayBuffer(length)));
        }

        internal static Uint8ClampedArray _caFromSharedArrayBuffer;
        private static void GetUint8ClampedArrayFromSharedArrayBuffer(JSObject obj)
        {
            _caFromSharedArrayBuffer = (Uint8ClampedArray)obj.GetObjectProperty("clampedArrayFromSharedArrayBuffer");
        }

        private static void SetUint8ArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("uint8ArrayFromSharedArrayBuffer", new Uint8Array(new SharedArrayBuffer(length)));
        }

        internal static Uint8Array _uint8FromSharedArrayBuffer;
        private static void GetUint8ArrayFromSharedArrayBuffer(JSObject obj)
        {
            _uint8FromSharedArrayBuffer = (Uint8Array)obj.GetObjectProperty("uint8ArrayFromSharedArrayBuffer");
        }

        private static void SetUint16ArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("uint16ArrayFromSharedArrayBuffer", new Uint16Array(new SharedArrayBuffer(length)));
        }

        internal static Uint16Array _uint16FromSharedArrayBuffer;
        private static void GetUint16ArrayFromSharedArrayBuffer(JSObject obj)
        {
            _uint16FromSharedArrayBuffer = (Uint16Array)obj.GetObjectProperty("uint16ArrayFromSharedArrayBuffer");
        }

        private static void SetUint32ArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("uint32ArrayFromSharedArrayBuffer", new Uint32Array(new SharedArrayBuffer(length)));
        }

        internal static Uint32Array _uint32FromSharedArrayBuffer;
        private static void GetUint32ArrayFromSharedArrayBuffer(JSObject obj)
        {
            _uint32FromSharedArrayBuffer = (Uint32Array)obj.GetObjectProperty("uint32ArrayFromSharedArrayBuffer");
        }

        private static void SetInt8ArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("int8ArrayFromSharedArrayBuffer", new Int8Array(new SharedArrayBuffer(length)));
        }

        internal static Int8Array _int8fromSharedArrayBuffer;
        private static void GetInt8ArrayFromSharedArrayBuffer(JSObject obj)
        {
            _int8fromSharedArrayBuffer = (Int8Array)obj.GetObjectProperty("int8ArrayFromSharedArrayBuffer");
        }

        private static void SetInt16ArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("int16ArrayFromSharedArrayBuffer", new Int16Array(new SharedArrayBuffer(length)));
        }

        internal static Int16Array _int16fromSharedArrayBuffer;
        private static void GetInt16ArrayFromSharedArrayBuffer(JSObject obj)
        {
            _int16fromSharedArrayBuffer = (Int16Array)obj.GetObjectProperty("int16ArrayFromSharedArrayBuffer");
        }

        private static void SetInt32ArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("int32ArrayFromSharedArrayBuffer", new Int32Array(new SharedArrayBuffer(length)));
        }

        internal static Int32Array _int32fromSharedArrayBuffer;
        private static void GetInt32ArrayFromSharedArrayBuffer(JSObject obj)
        {
            _int32fromSharedArrayBuffer = (Int32Array)obj.GetObjectProperty("int32ArrayFromSharedArrayBuffer");
        }

        private static void SetFloat32ArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("float32ArrayFromSharedArrayBuffer", new Float32Array(new SharedArrayBuffer(length)));
        }

        internal static Float32Array _float32fromSharedArrayBuffer;
        private static void GetFloat32ArrayFromSharedArrayBuffer(JSObject obj)
        {
            _float32fromSharedArrayBuffer = (Float32Array)obj.GetObjectProperty("float32ArrayFromSharedArrayBuffer");
        }

        private static void SetFloat64ArrayFromSharedArrayBuffer(JSObject obj, int length)
        {
            obj.SetObjectProperty("float64ArrayFromSharedArrayBuffer", new Float64Array(new SharedArrayBuffer(length)));
        }

        internal static Float64Array _float64fromSharedArrayBuffer;
        private static void GetFloat64ArrayFromSharedArrayBuffer(JSObject obj)
        {
            _float64fromSharedArrayBuffer = (Float64Array)obj.GetObjectProperty("float64ArrayFromSharedArrayBuffer");
        }

        private static Function _sumFunction;
        private static void CreateFunctionSum()
        {
            _sumFunction = new Function("a", "b", "return a + b");
        }

        internal static int _sumValue = 0;
        private static void CallFunctionSum()
        {
            _sumValue = (int)_sumFunction.Call(null, 3, 5);
        }

        private static Function _mathMinFunction;
        private static void CreateFunctionApply()
        {
            var math = (JSObject)Runtime.GetGlobalObject("Math");
            _mathMinFunction = (Function)math.GetObjectProperty("min");

        }

        internal static int _minValue = 0;
        private static void CallFunctionApply()
        {
            _minValue = (int)_mathMinFunction.Apply(null, new object[] { 5, 6, 2, 3, 7 });
        }

        internal static Uri _blobURL;
        public static void SetBlobUrl(string blobUrl)
        {
            _blobURL = new Uri(blobUrl);
        }

        internal static Uri _blobURI;
        public static void SetBlobAsUri(Uri blobUri)
        {
            _blobURI = blobUri;
        }

    }
    
    
}
