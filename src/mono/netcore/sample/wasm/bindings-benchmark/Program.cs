// -*- indent-tabs-mode: nil -*-
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;

namespace Sample
{
    public class Test
    {
        public static void Main(String[] args)
        {
            Console.WriteLine ("Hello, World!");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float BenchmarkMethod (int i, float f, double d) 
        {
            return (float)(i + f + d);
        }
    }
}
