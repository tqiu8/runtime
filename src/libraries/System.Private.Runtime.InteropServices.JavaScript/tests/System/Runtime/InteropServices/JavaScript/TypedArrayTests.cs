// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Xunit;

namespace System.Runtime.InteropServices.JavaScript.Tests
{
    public static class TypedArrayTests
    {
        private static Function _objectPrototype;

        public static IEnumerable<object[]> Object_Prototype()
        {
            _objectPrototype ??= new Function("return Object.prototype.toString;");
            yield return new object[] { _objectPrototype.Call() };
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ClampedArrayFrom(Function objectPrototype)
        {
            var clamped = new byte[50];
            Uint8ClampedArray from = Uint8ClampedArray.From(clamped);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Uint8ClampedArray]", objectPrototype.Call(from));
        }
        
        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ClampedArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetUint8ClampedArray"", [ obj, 50 ]);
                App.call_test_method ( ""GetUint8ClampedArray"", [ obj ]);
            ");
            var array = HelperMarshal._caUInt;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Uint8ClampedArray]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ArrayFrom(Function objectPrototype)
        {
            var array = new byte[50];
            Uint8Array from = Uint8Array.From(array);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Uint8Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetUint8Array"", [ obj, 50 ]);
                App.call_test_method ( ""GetUint8Array"", [ obj ]);
            ");
            var array = HelperMarshal._uint8Array;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Uint8Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint16ArrayFrom(Function objectPrototype)
        {
            var array = new ushort[50];
            Uint16Array from = Uint16Array.From(array);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Uint16Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint16ArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetUint16Array"", [ obj, 50 ]);
                App.call_test_method ( ""GetUint16Array"", [ obj ]);
            ");
            var array = HelperMarshal._uint16Array;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Uint16Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint32ArrayFrom(Function objectPrototype)
        {
            var array = new uint[50];
            Uint32Array from = Uint32Array.From(array);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Uint32Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint32ArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetUint32Array"", [ obj, 50 ]);
                App.call_test_method ( ""GetUint32Array"", [ obj ]);
            ");
            var array = HelperMarshal._uint32Array;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Uint32Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int8ArrayFrom(Function objectPrototype)
        {
            var array = new sbyte[50];
            Int8Array from = Int8Array.From(array);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Int8Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Int8ArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetInt8Array"", [ obj, 50 ]);
                App.call_test_method ( ""GetInt8Array"", [ obj ]);
            ");
            var array = HelperMarshal._int8Array;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Int8Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int16ArrayFrom(Function objectPrototype)
        {
            var array = new short[50];
            Int16Array from = Int16Array.From(array);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Int16Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Int16ArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetInt16Array"", [ obj, 50 ]);
                App.call_test_method ( ""GetInt16Array"", [ obj ]);
            ");
            var array = HelperMarshal._int16Array;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Int16Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int32ArrayFrom(Function objectPrototype)
        {
            var array = new int[50];
            Int32Array from = Int32Array.From(array);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Int32Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Int32ArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetInt32Array"", [ obj, 50 ]);
                App.call_test_method ( ""GetInt32Array"", [ obj ]);
            ");
            var array = HelperMarshal._int32Array;  
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Int32Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Float32ArrayFrom(Function objectPrototype)
        {
            var array = new float[50];
            Float32Array from = Float32Array.From(array);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Float32Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Float32ArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetFloat32Array"", [ obj, 50 ]);
                App.call_test_method ( ""GetFloat32Array"", [ obj ]);
            ");
            var array = HelperMarshal._float32Array;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Float32Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Float64ArrayFrom(Function objectPrototype)
        {
            var array = new double[50];
            Float64Array from = Float64Array.From(array);
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Float64Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Float64ArrayFrom_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetFloat64Array"", [ obj, 50 ]);
                App.call_test_method ( ""GetFloat64Array"", [ obj ]);
            ");
            var array = HelperMarshal._float64Array;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Float64Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ClampedArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Uint8ClampedArray from = new Uint8ClampedArray(new SharedArrayBuffer(50));
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Uint8ClampedArray]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ClampedArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetUint8ClampedArrayFromSharedArrayBuffer"", [ obj, 50 ]);
                App.call_test_method ( ""GetUint8ClampedArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._caFromSharedArrayBuffer;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Uint8ClampedArray]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Uint8Array from = new Uint8Array(new SharedArrayBuffer(50));
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Uint8Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetUint8ArrayFromSharedArrayBuffer"", [ obj, 50 ]);
                App.call_test_method ( ""GetUint8ArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._uint8FromSharedArrayBuffer;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Uint8Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint16ArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Uint16Array from = new Uint16Array(new SharedArrayBuffer(50));
            Assert.Equal(25, from.Length);
            Assert.Equal("[object Uint16Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint16ArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetUint16ArrayFromSharedArrayBuffer"", [ obj, 50 ]);
                App.call_test_method ( ""GetUint16ArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._uint16FromSharedArrayBuffer;
            Assert.Equal(25, array.Length);
            Assert.Equal("[object Uint16Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint32ArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Uint32Array from = new Uint32Array(new SharedArrayBuffer(40));
            Assert.Equal(10, from.Length);
            Assert.Equal("[object Uint32Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint32ArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetUint32ArrayFromSharedArrayBuffer"", [ obj, 40 ]);
                App.call_test_method ( ""GetUint32ArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._uint32FromSharedArrayBuffer;
            Assert.Equal(10, array.Length);
            Assert.Equal("[object Uint32Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int8ArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Int8Array from = new Int8Array(new SharedArrayBuffer(50));
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Int8Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Int8ArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetInt8ArrayFromSharedArrayBuffer"", [ obj, 50 ]);
                App.call_test_method ( ""GetInt8ArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._int8fromSharedArrayBuffer;
            Assert.Equal(50, array.Length);
            Assert.Equal("[object Int8Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int16ArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Int16Array from = new Int16Array(new SharedArrayBuffer(50));
            Assert.Equal(25, from.Length);
            Assert.Equal("[object Int16Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Int16ArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetInt16ArrayFromSharedArrayBuffer"", [ obj, 50 ]);
                App.call_test_method ( ""GetInt16ArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._int16fromSharedArrayBuffer;
            Assert.Equal(25, array.Length);
            Assert.Equal("[object Int16Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int32ArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Int32Array from = new Int32Array(new SharedArrayBuffer(40));
            Assert.Equal(10, from.Length);
            Assert.Equal("[object Int32Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Int32ArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetInt32ArrayFromSharedArrayBuffer"", [ obj, 40 ]);
                App.call_test_method ( ""GetInt32ArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._int32fromSharedArrayBuffer;
            Assert.Equal(10, array.Length);
            Assert.Equal("[object Int32Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Float32ArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Float32Array from = new Float32Array(new SharedArrayBuffer(40));
            Assert.Equal(10, from.Length);
            Assert.Equal("[object Float32Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Float32ArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetFloat32ArrayFromSharedArrayBuffer"", [ obj, 40 ]);
                App.call_test_method ( ""GetFloat32ArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._float32fromSharedArrayBuffer;
            Assert.Equal(10, array.Length);
            Assert.Equal("[object Float32Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Float64ArrayFromSharedArrayBuffer(Function objectPrototype)
        {
            Float64Array from = new Float64Array(new SharedArrayBuffer(40));
            Assert.Equal(5, from.Length);
            Assert.Equal("[object Float64Array]", objectPrototype.Call(from));
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBrowserDomSupported))]
        [MemberData(nameof(Object_Prototype))]
        public static void Float64ArrayFromSharedArrayBuffer_Browser(Function objectPrototype)
        {
            Runtime.InvokeJS(@"
                var obj = { };
                App.call_test_method ( ""SetFloat64ArrayFromSharedArrayBuffer"", [ obj, 40 ]);
                App.call_test_method ( ""GetFloat64ArrayFromSharedArrayBuffer"", [ obj ]);
            ");
            var array = HelperMarshal._float64fromSharedArrayBuffer;
            Assert.Equal(5, array.Length);
            Assert.Equal("[object Float64Array]", objectPrototype.Call(array));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ClampedArrayFromArrayBuffer(Function objectPrototype)
        {
            Uint8ClampedArray from = new Uint8ClampedArray(new ArrayBuffer(50));
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Uint8ClampedArray]", objectPrototype.Call(from));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint8ArrayFromArrayBuffer(Function objectPrototype)
        {
            Uint8Array from = new Uint8Array(new ArrayBuffer(50));
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Uint8Array]", objectPrototype.Call(from));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint16ArrayFromArrayBuffer(Function objectPrototype)
        {
            Uint16Array from = new Uint16Array(new ArrayBuffer(50));
            Assert.Equal(25, from.Length);
            Assert.Equal("[object Uint16Array]", objectPrototype.Call(from));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Uint32ArrayFromArrayBuffer(Function objectPrototype)
        {
            Uint32Array from = new Uint32Array(new ArrayBuffer(40));
            Assert.Equal(10, from.Length);
            Assert.Equal("[object Uint32Array]", objectPrototype.Call(from));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int8ArrayFromArrayBuffer(Function objectPrototype)
        {
            Int8Array from = new Int8Array(new ArrayBuffer(50));
            Assert.Equal(50, from.Length);
            Assert.Equal("[object Int8Array]", objectPrototype.Call(from));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int16ArrayFromArrayBuffer(Function objectPrototype)
        {
            Int16Array from = new Int16Array(new ArrayBuffer(50));
            Assert.Equal(25, from.Length);
            Assert.Equal("[object Int16Array]", objectPrototype.Call(from));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Int32ArrayFromArrayBuffer(Function objectPrototype)
        {
            Int32Array from = new Int32Array(new ArrayBuffer(40));
            Assert.Equal(10, from.Length);
            Assert.Equal("[object Int32Array]", objectPrototype.Call(from));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Float32ArrayFromArrayBuffer(Function objectPrototype)
        {
            Float32Array from = new Float32Array(new ArrayBuffer(40));
            Assert.Equal(10, from.Length);
            Assert.Equal("[object Float32Array]", objectPrototype.Call(from));
        }

        [Theory]
        [MemberData(nameof(Object_Prototype))]
        public static void Float64ArrayFromArrayBuffer(Function objectPrototype)
        {
            Float64Array from = new Float64Array(new ArrayBuffer(40));
            Assert.Equal(5, from.Length);
            Assert.Equal("[object Float64Array]", objectPrototype.Call(from));
        }

        [Fact]
        public static void TypedArrayTypeUint8ClampedArray()
        {
            Assert.Equal(TypedArrayTypeCode.Uint8ClampedArray, new Uint8ClampedArray().GetTypedArrayType());
        }

        [Fact]
        public static void TypedArrayTypeUint8Array()
        {
            Assert.Equal(TypedArrayTypeCode.Uint8Array, new Uint8Array().GetTypedArrayType());
        }

        [Fact]
        public static void TypedArrayTypeUint16Array()
        {
            Assert.Equal(TypedArrayTypeCode.Uint16Array, new Uint16Array().GetTypedArrayType());
        }

        [Fact]
        public static void TypedArrayTypeUint32Array()
        {
            Assert.Equal(TypedArrayTypeCode.Uint32Array, new Uint32Array().GetTypedArrayType());
        }

        [Fact]
        public static void TypedArrayTypeInt8Array()
        {
            Assert.Equal(TypedArrayTypeCode.Int8Array, new Int8Array().GetTypedArrayType());
        }

        [Fact]
        public static void TypedArrayTypeInt16Array()
        {
            Assert.Equal(TypedArrayTypeCode.Int16Array, new Int16Array().GetTypedArrayType());
        }

        [Fact]
        public static void TypedArrayTypeInt32Array()
        {
            Assert.Equal(TypedArrayTypeCode.Int32Array, new Int32Array().GetTypedArrayType());
        }

        [Fact]
        public static void TypedArrayTypeFloat32Array()
        {
            Assert.Equal(TypedArrayTypeCode.Float32Array, new Float32Array().GetTypedArrayType());
        }

        [Fact]
        public static void TypedArrayTypeFloat64Array()
        {
            Assert.Equal(TypedArrayTypeCode.Float64Array, new Float64Array().GetTypedArrayType());
        }
    }
}
