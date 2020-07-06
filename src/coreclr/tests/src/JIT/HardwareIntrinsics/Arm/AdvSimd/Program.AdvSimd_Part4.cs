// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace JIT.HardwareIntrinsics.Arm
{
    public static partial class Program
    {
        static Program()
        {
            TestList = new Dictionary<string, Action>() {
                ["ShiftArithmeticRoundedSaturate.Vector64.Int16"] = ShiftArithmeticRoundedSaturate_Vector64_Int16,
                ["ShiftArithmeticRoundedSaturate.Vector64.Int32"] = ShiftArithmeticRoundedSaturate_Vector64_Int32,
                ["ShiftArithmeticRoundedSaturate.Vector64.SByte"] = ShiftArithmeticRoundedSaturate_Vector64_SByte,
                ["ShiftArithmeticRoundedSaturate.Vector128.Int16"] = ShiftArithmeticRoundedSaturate_Vector128_Int16,
                ["ShiftArithmeticRoundedSaturate.Vector128.Int32"] = ShiftArithmeticRoundedSaturate_Vector128_Int32,
                ["ShiftArithmeticRoundedSaturate.Vector128.Int64"] = ShiftArithmeticRoundedSaturate_Vector128_Int64,
                ["ShiftArithmeticRoundedSaturate.Vector128.SByte"] = ShiftArithmeticRoundedSaturate_Vector128_SByte,
                ["ShiftArithmeticRoundedSaturateScalar.Vector64.Int64"] = ShiftArithmeticRoundedSaturateScalar_Vector64_Int64,
                ["ShiftArithmeticRoundedScalar.Vector64.Int64"] = ShiftArithmeticRoundedScalar_Vector64_Int64,
                ["ShiftArithmeticSaturate.Vector64.Int16"] = ShiftArithmeticSaturate_Vector64_Int16,
                ["ShiftArithmeticSaturate.Vector64.Int32"] = ShiftArithmeticSaturate_Vector64_Int32,
                ["ShiftArithmeticSaturate.Vector64.SByte"] = ShiftArithmeticSaturate_Vector64_SByte,
                ["ShiftArithmeticSaturate.Vector128.Int16"] = ShiftArithmeticSaturate_Vector128_Int16,
                ["ShiftArithmeticSaturate.Vector128.Int32"] = ShiftArithmeticSaturate_Vector128_Int32,
                ["ShiftArithmeticSaturate.Vector128.Int64"] = ShiftArithmeticSaturate_Vector128_Int64,
                ["ShiftArithmeticSaturate.Vector128.SByte"] = ShiftArithmeticSaturate_Vector128_SByte,
                ["ShiftArithmeticSaturateScalar.Vector64.Int64"] = ShiftArithmeticSaturateScalar_Vector64_Int64,
                ["ShiftArithmeticScalar.Vector64.Int64"] = ShiftArithmeticScalar_Vector64_Int64,
                ["ShiftLeftAndInsert.Vector64.Byte"] = ShiftLeftAndInsert_Vector64_Byte,
                ["ShiftLeftAndInsert.Vector64.Int16"] = ShiftLeftAndInsert_Vector64_Int16,
                ["ShiftLeftAndInsert.Vector64.Int32"] = ShiftLeftAndInsert_Vector64_Int32,
                ["ShiftLeftAndInsert.Vector64.SByte"] = ShiftLeftAndInsert_Vector64_SByte,
                ["ShiftLeftAndInsert.Vector64.UInt16"] = ShiftLeftAndInsert_Vector64_UInt16,
                ["ShiftLeftAndInsert.Vector64.UInt32"] = ShiftLeftAndInsert_Vector64_UInt32,
                ["ShiftLeftAndInsert.Vector128.Byte"] = ShiftLeftAndInsert_Vector128_Byte,
                ["ShiftLeftAndInsert.Vector128.Int16"] = ShiftLeftAndInsert_Vector128_Int16,
                ["ShiftLeftAndInsert.Vector128.Int32"] = ShiftLeftAndInsert_Vector128_Int32,
                ["ShiftLeftAndInsert.Vector128.Int64"] = ShiftLeftAndInsert_Vector128_Int64,
                ["ShiftLeftAndInsert.Vector128.SByte"] = ShiftLeftAndInsert_Vector128_SByte,
                ["ShiftLeftAndInsert.Vector128.UInt16"] = ShiftLeftAndInsert_Vector128_UInt16,
                ["ShiftLeftAndInsert.Vector128.UInt32"] = ShiftLeftAndInsert_Vector128_UInt32,
                ["ShiftLeftAndInsert.Vector128.UInt64"] = ShiftLeftAndInsert_Vector128_UInt64,
                ["ShiftLeftAndInsertScalar.Vector64.Int64"] = ShiftLeftAndInsertScalar_Vector64_Int64,
                ["ShiftLeftAndInsertScalar.Vector64.UInt64"] = ShiftLeftAndInsertScalar_Vector64_UInt64,
                ["ShiftLeftLogical.Vector64.Byte.1"] = ShiftLeftLogical_Vector64_Byte_1,
                ["ShiftLeftLogical.Vector64.Int16.1"] = ShiftLeftLogical_Vector64_Int16_1,
                ["ShiftLeftLogical.Vector64.Int32.1"] = ShiftLeftLogical_Vector64_Int32_1,
                ["ShiftLeftLogical.Vector64.SByte.1"] = ShiftLeftLogical_Vector64_SByte_1,
                ["ShiftLeftLogical.Vector64.UInt16.1"] = ShiftLeftLogical_Vector64_UInt16_1,
                ["ShiftLeftLogical.Vector64.UInt32.1"] = ShiftLeftLogical_Vector64_UInt32_1,
                ["ShiftLeftLogical.Vector128.Byte.1"] = ShiftLeftLogical_Vector128_Byte_1,
                ["ShiftLeftLogical.Vector128.Int16.1"] = ShiftLeftLogical_Vector128_Int16_1,
                ["ShiftLeftLogical.Vector128.Int64.1"] = ShiftLeftLogical_Vector128_Int64_1,
                ["ShiftLeftLogical.Vector128.SByte.1"] = ShiftLeftLogical_Vector128_SByte_1,
                ["ShiftLeftLogical.Vector128.UInt16.1"] = ShiftLeftLogical_Vector128_UInt16_1,
                ["ShiftLeftLogical.Vector128.UInt32.1"] = ShiftLeftLogical_Vector128_UInt32_1,
                ["ShiftLeftLogical.Vector128.UInt64.1"] = ShiftLeftLogical_Vector128_UInt64_1,
                ["ShiftLeftLogicalSaturate.Vector64.Byte.1"] = ShiftLeftLogicalSaturate_Vector64_Byte_1,
                ["ShiftLeftLogicalSaturate.Vector64.Int16.1"] = ShiftLeftLogicalSaturate_Vector64_Int16_1,
                ["ShiftLeftLogicalSaturate.Vector64.Int32.1"] = ShiftLeftLogicalSaturate_Vector64_Int32_1,
                ["ShiftLeftLogicalSaturate.Vector64.SByte.1"] = ShiftLeftLogicalSaturate_Vector64_SByte_1,
                ["ShiftLeftLogicalSaturate.Vector64.UInt16.1"] = ShiftLeftLogicalSaturate_Vector64_UInt16_1,
                ["ShiftLeftLogicalSaturate.Vector64.UInt32.1"] = ShiftLeftLogicalSaturate_Vector64_UInt32_1,
                ["ShiftLeftLogicalSaturate.Vector128.Byte.1"] = ShiftLeftLogicalSaturate_Vector128_Byte_1,
                ["ShiftLeftLogicalSaturate.Vector128.Int16.1"] = ShiftLeftLogicalSaturate_Vector128_Int16_1,
                ["ShiftLeftLogicalSaturate.Vector128.Int32.1"] = ShiftLeftLogicalSaturate_Vector128_Int32_1,
                ["ShiftLeftLogicalSaturate.Vector128.Int64.1"] = ShiftLeftLogicalSaturate_Vector128_Int64_1,
                ["ShiftLeftLogicalSaturate.Vector128.SByte.1"] = ShiftLeftLogicalSaturate_Vector128_SByte_1,
                ["ShiftLeftLogicalSaturate.Vector128.UInt16.1"] = ShiftLeftLogicalSaturate_Vector128_UInt16_1,
                ["ShiftLeftLogicalSaturate.Vector128.UInt32.1"] = ShiftLeftLogicalSaturate_Vector128_UInt32_1,
                ["ShiftLeftLogicalSaturate.Vector128.UInt64.1"] = ShiftLeftLogicalSaturate_Vector128_UInt64_1,
                ["ShiftLeftLogicalSaturateScalar.Vector64.Int64.1"] = ShiftLeftLogicalSaturateScalar_Vector64_Int64_1,
                ["ShiftLeftLogicalSaturateScalar.Vector64.UInt64.1"] = ShiftLeftLogicalSaturateScalar_Vector64_UInt64_1,
                ["ShiftLeftLogicalSaturateUnsigned.Vector64.Int16.1"] = ShiftLeftLogicalSaturateUnsigned_Vector64_Int16_1,
                ["ShiftLeftLogicalSaturateUnsigned.Vector64.Int32.1"] = ShiftLeftLogicalSaturateUnsigned_Vector64_Int32_1,
                ["ShiftLeftLogicalSaturateUnsigned.Vector64.SByte.1"] = ShiftLeftLogicalSaturateUnsigned_Vector64_SByte_1,
                ["ShiftLeftLogicalSaturateUnsigned.Vector128.Int16.1"] = ShiftLeftLogicalSaturateUnsigned_Vector128_Int16_1,
                ["ShiftLeftLogicalSaturateUnsigned.Vector128.Int32.1"] = ShiftLeftLogicalSaturateUnsigned_Vector128_Int32_1,
                ["ShiftLeftLogicalSaturateUnsigned.Vector128.Int64.1"] = ShiftLeftLogicalSaturateUnsigned_Vector128_Int64_1,
                ["ShiftLeftLogicalSaturateUnsigned.Vector128.SByte.1"] = ShiftLeftLogicalSaturateUnsigned_Vector128_SByte_1,
                ["ShiftLeftLogicalSaturateUnsignedScalar.Vector64.Int64.1"] = ShiftLeftLogicalSaturateUnsignedScalar_Vector64_Int64_1,
                ["ShiftLeftLogicalScalar.Vector64.Int64.1"] = ShiftLeftLogicalScalar_Vector64_Int64_1,
                ["ShiftLeftLogicalScalar.Vector64.UInt64.1"] = ShiftLeftLogicalScalar_Vector64_UInt64_1,
                ["ShiftLeftLogicalWideningLower.Vector64.Byte.1"] = ShiftLeftLogicalWideningLower_Vector64_Byte_1,
                ["ShiftLeftLogicalWideningLower.Vector64.Int16.1"] = ShiftLeftLogicalWideningLower_Vector64_Int16_1,
                ["ShiftLeftLogicalWideningLower.Vector64.Int32.1"] = ShiftLeftLogicalWideningLower_Vector64_Int32_1,
                ["ShiftLeftLogicalWideningLower.Vector64.SByte.1"] = ShiftLeftLogicalWideningLower_Vector64_SByte_1,
                ["ShiftLeftLogicalWideningLower.Vector64.UInt16.1"] = ShiftLeftLogicalWideningLower_Vector64_UInt16_1,
                ["ShiftLeftLogicalWideningLower.Vector64.UInt32.1"] = ShiftLeftLogicalWideningLower_Vector64_UInt32_1,
                ["ShiftLeftLogicalWideningUpper.Vector128.Byte.1"] = ShiftLeftLogicalWideningUpper_Vector128_Byte_1,
                ["ShiftLeftLogicalWideningUpper.Vector128.Int16.1"] = ShiftLeftLogicalWideningUpper_Vector128_Int16_1,
                ["ShiftLeftLogicalWideningUpper.Vector128.Int32.1"] = ShiftLeftLogicalWideningUpper_Vector128_Int32_1,
                ["ShiftLeftLogicalWideningUpper.Vector128.SByte.1"] = ShiftLeftLogicalWideningUpper_Vector128_SByte_1,
                ["ShiftLeftLogicalWideningUpper.Vector128.UInt16.1"] = ShiftLeftLogicalWideningUpper_Vector128_UInt16_1,
                ["ShiftLeftLogicalWideningUpper.Vector128.UInt32.1"] = ShiftLeftLogicalWideningUpper_Vector128_UInt32_1,
                ["ShiftLogical.Vector64.Byte"] = ShiftLogical_Vector64_Byte,
                ["ShiftLogical.Vector64.Int16"] = ShiftLogical_Vector64_Int16,
                ["ShiftLogical.Vector64.Int32"] = ShiftLogical_Vector64_Int32,
                ["ShiftLogical.Vector64.SByte"] = ShiftLogical_Vector64_SByte,
                ["ShiftLogical.Vector64.UInt16"] = ShiftLogical_Vector64_UInt16,
                ["ShiftLogical.Vector64.UInt32"] = ShiftLogical_Vector64_UInt32,
                ["ShiftLogical.Vector128.Byte"] = ShiftLogical_Vector128_Byte,
                ["ShiftLogical.Vector128.Int16"] = ShiftLogical_Vector128_Int16,
                ["ShiftLogical.Vector128.Int32"] = ShiftLogical_Vector128_Int32,
                ["ShiftLogical.Vector128.Int64"] = ShiftLogical_Vector128_Int64,
                ["ShiftLogical.Vector128.SByte"] = ShiftLogical_Vector128_SByte,
                ["ShiftLogical.Vector128.UInt16"] = ShiftLogical_Vector128_UInt16,
                ["ShiftLogical.Vector128.UInt32"] = ShiftLogical_Vector128_UInt32,
                ["ShiftLogical.Vector128.UInt64"] = ShiftLogical_Vector128_UInt64,
                ["ShiftLogicalRounded.Vector64.Byte"] = ShiftLogicalRounded_Vector64_Byte,
                ["ShiftLogicalRounded.Vector64.Int16"] = ShiftLogicalRounded_Vector64_Int16,
                ["ShiftLogicalRounded.Vector64.Int32"] = ShiftLogicalRounded_Vector64_Int32,
                ["ShiftLogicalRounded.Vector64.SByte"] = ShiftLogicalRounded_Vector64_SByte,
                ["ShiftLogicalRounded.Vector64.UInt16"] = ShiftLogicalRounded_Vector64_UInt16,
                ["ShiftLogicalRounded.Vector64.UInt32"] = ShiftLogicalRounded_Vector64_UInt32,
                ["ShiftLogicalRounded.Vector128.Byte"] = ShiftLogicalRounded_Vector128_Byte,
                ["ShiftLogicalRounded.Vector128.Int16"] = ShiftLogicalRounded_Vector128_Int16,
                ["ShiftLogicalRounded.Vector128.Int32"] = ShiftLogicalRounded_Vector128_Int32,
                ["ShiftLogicalRounded.Vector128.Int64"] = ShiftLogicalRounded_Vector128_Int64,
                ["ShiftLogicalRounded.Vector128.SByte"] = ShiftLogicalRounded_Vector128_SByte,
                ["ShiftLogicalRounded.Vector128.UInt16"] = ShiftLogicalRounded_Vector128_UInt16,
                ["ShiftLogicalRounded.Vector128.UInt32"] = ShiftLogicalRounded_Vector128_UInt32,
                ["ShiftLogicalRounded.Vector128.UInt64"] = ShiftLogicalRounded_Vector128_UInt64,
                ["ShiftLogicalRoundedSaturate.Vector64.Byte"] = ShiftLogicalRoundedSaturate_Vector64_Byte,
                ["ShiftLogicalRoundedSaturate.Vector64.Int16"] = ShiftLogicalRoundedSaturate_Vector64_Int16,
                ["ShiftLogicalRoundedSaturate.Vector64.Int32"] = ShiftLogicalRoundedSaturate_Vector64_Int32,
                ["ShiftLogicalRoundedSaturate.Vector64.SByte"] = ShiftLogicalRoundedSaturate_Vector64_SByte,
                ["ShiftLogicalRoundedSaturate.Vector64.UInt16"] = ShiftLogicalRoundedSaturate_Vector64_UInt16,
                ["ShiftLogicalRoundedSaturate.Vector64.UInt32"] = ShiftLogicalRoundedSaturate_Vector64_UInt32,
                ["ShiftLogicalRoundedSaturate.Vector128.Byte"] = ShiftLogicalRoundedSaturate_Vector128_Byte,
                ["ShiftLogicalRoundedSaturate.Vector128.Int16"] = ShiftLogicalRoundedSaturate_Vector128_Int16,
                ["ShiftLogicalRoundedSaturate.Vector128.Int32"] = ShiftLogicalRoundedSaturate_Vector128_Int32,
                ["ShiftLogicalRoundedSaturate.Vector128.Int64"] = ShiftLogicalRoundedSaturate_Vector128_Int64,
                ["ShiftLogicalRoundedSaturate.Vector128.SByte"] = ShiftLogicalRoundedSaturate_Vector128_SByte,
                ["ShiftLogicalRoundedSaturate.Vector128.UInt16"] = ShiftLogicalRoundedSaturate_Vector128_UInt16,
                ["ShiftLogicalRoundedSaturate.Vector128.UInt32"] = ShiftLogicalRoundedSaturate_Vector128_UInt32,
                ["ShiftLogicalRoundedSaturate.Vector128.UInt64"] = ShiftLogicalRoundedSaturate_Vector128_UInt64,
                ["ShiftLogicalRoundedSaturateScalar.Vector64.Int64"] = ShiftLogicalRoundedSaturateScalar_Vector64_Int64,
                ["ShiftLogicalRoundedSaturateScalar.Vector64.UInt64"] = ShiftLogicalRoundedSaturateScalar_Vector64_UInt64,
                ["ShiftLogicalRoundedScalar.Vector64.Int64"] = ShiftLogicalRoundedScalar_Vector64_Int64,
                ["ShiftLogicalRoundedScalar.Vector64.UInt64"] = ShiftLogicalRoundedScalar_Vector64_UInt64,
                ["ShiftLogicalSaturate.Vector64.Byte"] = ShiftLogicalSaturate_Vector64_Byte,
                ["ShiftLogicalSaturate.Vector64.Int16"] = ShiftLogicalSaturate_Vector64_Int16,
                ["ShiftLogicalSaturate.Vector64.Int32"] = ShiftLogicalSaturate_Vector64_Int32,
                ["ShiftLogicalSaturate.Vector64.SByte"] = ShiftLogicalSaturate_Vector64_SByte,
                ["ShiftLogicalSaturate.Vector64.UInt16"] = ShiftLogicalSaturate_Vector64_UInt16,
                ["ShiftLogicalSaturate.Vector64.UInt32"] = ShiftLogicalSaturate_Vector64_UInt32,
                ["ShiftLogicalSaturate.Vector128.Byte"] = ShiftLogicalSaturate_Vector128_Byte,
                ["ShiftLogicalSaturate.Vector128.Int16"] = ShiftLogicalSaturate_Vector128_Int16,
                ["ShiftLogicalSaturate.Vector128.Int32"] = ShiftLogicalSaturate_Vector128_Int32,
                ["ShiftLogicalSaturate.Vector128.Int64"] = ShiftLogicalSaturate_Vector128_Int64,
                ["ShiftLogicalSaturate.Vector128.SByte"] = ShiftLogicalSaturate_Vector128_SByte,
                ["ShiftLogicalSaturate.Vector128.UInt16"] = ShiftLogicalSaturate_Vector128_UInt16,
                ["ShiftLogicalSaturate.Vector128.UInt32"] = ShiftLogicalSaturate_Vector128_UInt32,
                ["ShiftLogicalSaturate.Vector128.UInt64"] = ShiftLogicalSaturate_Vector128_UInt64,
                ["ShiftLogicalSaturateScalar.Vector64.Int64"] = ShiftLogicalSaturateScalar_Vector64_Int64,
                ["ShiftLogicalSaturateScalar.Vector64.UInt64"] = ShiftLogicalSaturateScalar_Vector64_UInt64,
                ["ShiftLogicalScalar.Vector64.Int64"] = ShiftLogicalScalar_Vector64_Int64,
                ["ShiftLogicalScalar.Vector64.UInt64"] = ShiftLogicalScalar_Vector64_UInt64,
                ["ShiftRightAndInsert.Vector64.Byte"] = ShiftRightAndInsert_Vector64_Byte,
                ["ShiftRightAndInsert.Vector64.Int16"] = ShiftRightAndInsert_Vector64_Int16,
                ["ShiftRightAndInsert.Vector64.Int32"] = ShiftRightAndInsert_Vector64_Int32,
                ["ShiftRightAndInsert.Vector64.SByte"] = ShiftRightAndInsert_Vector64_SByte,
                ["ShiftRightAndInsert.Vector64.UInt16"] = ShiftRightAndInsert_Vector64_UInt16,
                ["ShiftRightAndInsert.Vector64.UInt32"] = ShiftRightAndInsert_Vector64_UInt32,
                ["ShiftRightAndInsert.Vector128.Byte"] = ShiftRightAndInsert_Vector128_Byte,
                ["ShiftRightAndInsert.Vector128.Int16"] = ShiftRightAndInsert_Vector128_Int16,
                ["ShiftRightAndInsert.Vector128.Int32"] = ShiftRightAndInsert_Vector128_Int32,
                ["ShiftRightAndInsert.Vector128.Int64"] = ShiftRightAndInsert_Vector128_Int64,
                ["ShiftRightAndInsert.Vector128.SByte"] = ShiftRightAndInsert_Vector128_SByte,
                ["ShiftRightAndInsert.Vector128.UInt16"] = ShiftRightAndInsert_Vector128_UInt16,
                ["ShiftRightAndInsert.Vector128.UInt32"] = ShiftRightAndInsert_Vector128_UInt32,
                ["ShiftRightAndInsert.Vector128.UInt64"] = ShiftRightAndInsert_Vector128_UInt64,
                ["ShiftRightAndInsertScalar.Vector64.Int64"] = ShiftRightAndInsertScalar_Vector64_Int64,
                ["ShiftRightAndInsertScalar.Vector64.UInt64"] = ShiftRightAndInsertScalar_Vector64_UInt64,
                ["ShiftRightArithmetic.Vector64.Int16.1"] = ShiftRightArithmetic_Vector64_Int16_1,
                ["ShiftRightArithmetic.Vector64.Int32.1"] = ShiftRightArithmetic_Vector64_Int32_1,
                ["ShiftRightArithmetic.Vector64.SByte.1"] = ShiftRightArithmetic_Vector64_SByte_1,
                ["ShiftRightArithmetic.Vector128.Int16.1"] = ShiftRightArithmetic_Vector128_Int16_1,
                ["ShiftRightArithmetic.Vector128.Int32.1"] = ShiftRightArithmetic_Vector128_Int32_1,
                ["ShiftRightArithmetic.Vector128.Int64.1"] = ShiftRightArithmetic_Vector128_Int64_1,
                ["ShiftRightArithmetic.Vector128.SByte.1"] = ShiftRightArithmetic_Vector128_SByte_1,
                ["ShiftRightArithmeticAdd.Vector64.Int16.1"] = ShiftRightArithmeticAdd_Vector64_Int16_1,
                ["ShiftRightArithmeticAdd.Vector64.Int32.1"] = ShiftRightArithmeticAdd_Vector64_Int32_1,
                ["ShiftRightArithmeticAdd.Vector64.SByte.1"] = ShiftRightArithmeticAdd_Vector64_SByte_1,
                ["ShiftRightArithmeticAdd.Vector128.Int16.1"] = ShiftRightArithmeticAdd_Vector128_Int16_1,
                ["ShiftRightArithmeticAdd.Vector128.Int32.1"] = ShiftRightArithmeticAdd_Vector128_Int32_1,
                ["ShiftRightArithmeticAdd.Vector128.Int64.1"] = ShiftRightArithmeticAdd_Vector128_Int64_1,
                ["ShiftRightArithmeticAdd.Vector128.SByte.1"] = ShiftRightArithmeticAdd_Vector128_SByte_1,
                ["ShiftRightArithmeticAddScalar.Vector64.Int64.1"] = ShiftRightArithmeticAddScalar_Vector64_Int64_1,
                ["ShiftRightArithmeticNarrowingSaturateLower.Vector64.Int16.1"] = ShiftRightArithmeticNarrowingSaturateLower_Vector64_Int16_1,
                ["ShiftRightArithmeticNarrowingSaturateLower.Vector64.Int32.1"] = ShiftRightArithmeticNarrowingSaturateLower_Vector64_Int32_1,
                ["ShiftRightArithmeticNarrowingSaturateLower.Vector64.SByte.1"] = ShiftRightArithmeticNarrowingSaturateLower_Vector64_SByte_1,
                ["ShiftRightArithmeticNarrowingSaturateUnsignedLower.Vector64.Byte.1"] = ShiftRightArithmeticNarrowingSaturateUnsignedLower_Vector64_Byte_1,
                ["ShiftRightArithmeticNarrowingSaturateUnsignedLower.Vector64.UInt16.1"] = ShiftRightArithmeticNarrowingSaturateUnsignedLower_Vector64_UInt16_1,
                ["ShiftRightArithmeticNarrowingSaturateUnsignedLower.Vector64.UInt32.1"] = ShiftRightArithmeticNarrowingSaturateUnsignedLower_Vector64_UInt32_1,
                ["ShiftRightArithmeticNarrowingSaturateUnsignedUpper.Vector128.Byte.1"] = ShiftRightArithmeticNarrowingSaturateUnsignedUpper_Vector128_Byte_1,
                ["ShiftRightArithmeticNarrowingSaturateUnsignedUpper.Vector128.UInt16.1"] = ShiftRightArithmeticNarrowingSaturateUnsignedUpper_Vector128_UInt16_1,
                ["ShiftRightArithmeticNarrowingSaturateUnsignedUpper.Vector128.UInt32.1"] = ShiftRightArithmeticNarrowingSaturateUnsignedUpper_Vector128_UInt32_1,
                ["ShiftRightArithmeticNarrowingSaturateUpper.Vector128.Int16.1"] = ShiftRightArithmeticNarrowingSaturateUpper_Vector128_Int16_1,
                ["ShiftRightArithmeticNarrowingSaturateUpper.Vector128.Int32.1"] = ShiftRightArithmeticNarrowingSaturateUpper_Vector128_Int32_1,
                ["ShiftRightArithmeticNarrowingSaturateUpper.Vector128.SByte.1"] = ShiftRightArithmeticNarrowingSaturateUpper_Vector128_SByte_1,
                ["ShiftRightArithmeticRounded.Vector64.Int16.1"] = ShiftRightArithmeticRounded_Vector64_Int16_1,
                ["ShiftRightArithmeticRounded.Vector64.Int32.1"] = ShiftRightArithmeticRounded_Vector64_Int32_1,
                ["ShiftRightArithmeticRounded.Vector64.SByte.1"] = ShiftRightArithmeticRounded_Vector64_SByte_1,
                ["ShiftRightArithmeticRounded.Vector128.Int16.1"] = ShiftRightArithmeticRounded_Vector128_Int16_1,
                ["ShiftRightArithmeticRounded.Vector128.Int32.1"] = ShiftRightArithmeticRounded_Vector128_Int32_1,
                ["ShiftRightArithmeticRounded.Vector128.Int64.1"] = ShiftRightArithmeticRounded_Vector128_Int64_1,
                ["ShiftRightArithmeticRounded.Vector128.SByte.1"] = ShiftRightArithmeticRounded_Vector128_SByte_1,
                ["ShiftRightArithmeticRoundedAdd.Vector64.Int16.1"] = ShiftRightArithmeticRoundedAdd_Vector64_Int16_1,
                ["ShiftRightArithmeticRoundedAdd.Vector64.Int32.1"] = ShiftRightArithmeticRoundedAdd_Vector64_Int32_1,
                ["ShiftRightArithmeticRoundedAdd.Vector64.SByte.1"] = ShiftRightArithmeticRoundedAdd_Vector64_SByte_1,
                ["ShiftRightArithmeticRoundedAdd.Vector128.Int16.1"] = ShiftRightArithmeticRoundedAdd_Vector128_Int16_1,
                ["ShiftRightArithmeticRoundedAdd.Vector128.Int32.1"] = ShiftRightArithmeticRoundedAdd_Vector128_Int32_1,
                ["ShiftRightArithmeticRoundedAdd.Vector128.Int64.1"] = ShiftRightArithmeticRoundedAdd_Vector128_Int64_1,
                ["ShiftRightArithmeticRoundedAdd.Vector128.SByte.1"] = ShiftRightArithmeticRoundedAdd_Vector128_SByte_1,
                ["ShiftRightArithmeticRoundedAddScalar.Vector64.Int64.1"] = ShiftRightArithmeticRoundedAddScalar_Vector64_Int64_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateLower.Vector64.Int16.1"] = ShiftRightArithmeticRoundedNarrowingSaturateLower_Vector64_Int16_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateLower.Vector64.Int32.1"] = ShiftRightArithmeticRoundedNarrowingSaturateLower_Vector64_Int32_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateLower.Vector64.SByte.1"] = ShiftRightArithmeticRoundedNarrowingSaturateLower_Vector64_SByte_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUnsignedLower.Vector64.Byte.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUnsignedLower_Vector64_Byte_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUnsignedLower.Vector64.UInt16.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUnsignedLower_Vector64_UInt16_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUnsignedLower.Vector64.UInt32.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUnsignedLower_Vector64_UInt32_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUnsignedUpper.Vector128.Byte.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUnsignedUpper_Vector128_Byte_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUnsignedUpper.Vector128.UInt16.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUnsignedUpper_Vector128_UInt16_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUnsignedUpper.Vector128.UInt32.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUnsignedUpper_Vector128_UInt32_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUpper.Vector128.Int16.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUpper_Vector128_Int16_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUpper.Vector128.Int32.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUpper_Vector128_Int32_1,
                ["ShiftRightArithmeticRoundedNarrowingSaturateUpper.Vector128.SByte.1"] = ShiftRightArithmeticRoundedNarrowingSaturateUpper_Vector128_SByte_1,
                ["ShiftRightArithmeticRoundedScalar.Vector64.Int64.1"] = ShiftRightArithmeticRoundedScalar_Vector64_Int64_1,
                ["ShiftRightArithmeticScalar.Vector64.Int64.1"] = ShiftRightArithmeticScalar_Vector64_Int64_1,
                ["ShiftRightLogical.Vector64.Byte.1"] = ShiftRightLogical_Vector64_Byte_1,
                ["ShiftRightLogical.Vector64.Int16.1"] = ShiftRightLogical_Vector64_Int16_1,
                ["ShiftRightLogical.Vector64.Int32.1"] = ShiftRightLogical_Vector64_Int32_1,
                ["ShiftRightLogical.Vector64.SByte.1"] = ShiftRightLogical_Vector64_SByte_1,
                ["ShiftRightLogical.Vector64.UInt16.1"] = ShiftRightLogical_Vector64_UInt16_1,
                ["ShiftRightLogical.Vector64.UInt32.1"] = ShiftRightLogical_Vector64_UInt32_1,
                ["ShiftRightLogical.Vector128.Byte.1"] = ShiftRightLogical_Vector128_Byte_1,
                ["ShiftRightLogical.Vector128.Int16.1"] = ShiftRightLogical_Vector128_Int16_1,
                ["ShiftRightLogical.Vector128.Int32.1"] = ShiftRightLogical_Vector128_Int32_1,
                ["ShiftRightLogical.Vector128.Int64.1"] = ShiftRightLogical_Vector128_Int64_1,
                ["ShiftRightLogical.Vector128.SByte.1"] = ShiftRightLogical_Vector128_SByte_1,
                ["ShiftRightLogical.Vector128.UInt16.1"] = ShiftRightLogical_Vector128_UInt16_1,
                ["ShiftRightLogical.Vector128.UInt32.1"] = ShiftRightLogical_Vector128_UInt32_1,
                ["ShiftRightLogical.Vector128.UInt64.1"] = ShiftRightLogical_Vector128_UInt64_1,
                ["ShiftRightLogicalAdd.Vector64.Byte.1"] = ShiftRightLogicalAdd_Vector64_Byte_1,
                ["ShiftRightLogicalAdd.Vector64.Int16.1"] = ShiftRightLogicalAdd_Vector64_Int16_1,
                ["ShiftRightLogicalAdd.Vector64.Int32.1"] = ShiftRightLogicalAdd_Vector64_Int32_1,
                ["ShiftRightLogicalAdd.Vector64.SByte.1"] = ShiftRightLogicalAdd_Vector64_SByte_1,
                ["ShiftRightLogicalAdd.Vector64.UInt16.1"] = ShiftRightLogicalAdd_Vector64_UInt16_1,
                ["ShiftRightLogicalAdd.Vector64.UInt32.1"] = ShiftRightLogicalAdd_Vector64_UInt32_1,
                ["ShiftRightLogicalAdd.Vector128.Byte.1"] = ShiftRightLogicalAdd_Vector128_Byte_1,
                ["ShiftRightLogicalAdd.Vector128.Int16.1"] = ShiftRightLogicalAdd_Vector128_Int16_1,
                ["ShiftRightLogicalAdd.Vector128.Int32.1"] = ShiftRightLogicalAdd_Vector128_Int32_1,
                ["ShiftRightLogicalAdd.Vector128.Int64.1"] = ShiftRightLogicalAdd_Vector128_Int64_1,
                ["ShiftRightLogicalAdd.Vector128.SByte.1"] = ShiftRightLogicalAdd_Vector128_SByte_1,
                ["ShiftRightLogicalAdd.Vector128.UInt16.1"] = ShiftRightLogicalAdd_Vector128_UInt16_1,
                ["ShiftRightLogicalAdd.Vector128.UInt32.1"] = ShiftRightLogicalAdd_Vector128_UInt32_1,
                ["ShiftRightLogicalAdd.Vector128.UInt64.1"] = ShiftRightLogicalAdd_Vector128_UInt64_1,
                ["ShiftRightLogicalAddScalar.Vector64.Int64.1"] = ShiftRightLogicalAddScalar_Vector64_Int64_1,
                ["ShiftRightLogicalAddScalar.Vector64.UInt64.1"] = ShiftRightLogicalAddScalar_Vector64_UInt64_1,
                ["ShiftRightLogicalNarrowingLower.Vector64.Byte.1"] = ShiftRightLogicalNarrowingLower_Vector64_Byte_1,
                ["ShiftRightLogicalNarrowingLower.Vector64.Int16.1"] = ShiftRightLogicalNarrowingLower_Vector64_Int16_1,
                ["ShiftRightLogicalNarrowingLower.Vector64.Int32.1"] = ShiftRightLogicalNarrowingLower_Vector64_Int32_1,
                ["ShiftRightLogicalNarrowingLower.Vector64.SByte.1"] = ShiftRightLogicalNarrowingLower_Vector64_SByte_1,
                ["ShiftRightLogicalNarrowingLower.Vector64.UInt16.1"] = ShiftRightLogicalNarrowingLower_Vector64_UInt16_1,
            };
        }
    }
}
