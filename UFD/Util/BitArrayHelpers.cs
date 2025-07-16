using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UFD.Util;

public static class BitArrayHelpers
{
    private const int Shift = 5;
    private const int Mask = (1 << Shift) - 1;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalculateBitArrayBufferLength(int length) => (length + Mask) >>> Shift;

    [Pure]
    public static uint[] CreateBitArray(int capacity, bool setAllBits)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        var arrayLength = CalculateBitArrayBufferLength(capacity);
        if (arrayLength == 0)
            return Array.Empty<uint>();

        var result = new uint[arrayLength];
        if (setAllBits)
            PopulateBitArray(result, capacity);

        return result;
    }

    private static void PopulateBitArray(Span<uint> bitArray, int requiredPopCount)
    {
        ThrowIfInvalidCapacity(requiredPopCount, bitArray.Length);

        var requiredSpanLength = CalculateBitArrayBufferLength(requiredPopCount);

        var subSpan = bitArray[requiredSpanLength..];
        subSpan.Clear();
        subSpan = bitArray[..requiredSpanLength];
        subSpan.Fill(uint.MaxValue);

        var lastIndexPopCount = requiredPopCount & Mask;
        if (lastIndexPopCount != 0)
            subSpan[^1] = (1U << lastIndexPopCount) - 1U;
    }

    private static void ThrowIfInvalidCapacity(int requiredNumberOfItems, int bufferLength)
    {
        if (requiredNumberOfItems > (bufferLength << Shift))
            throw new ArgumentException($"Number of items for Hasher exceeds max capacity of bit buffer! Requires: {requiredNumberOfItems} bits, buffer has {bufferLength << Shift} bits");
    }

    /// <summary>
    /// Tests if a specific bit is set
    /// </summary>
    /// <param name="bits">The span to query</param>
    /// <param name="index">The bit to query</param>
    /// <returns><see langword="true" /> if the specified bit is set</returns>
    [Pure]
    public static bool GetBit(ReadOnlySpan<uint> bits, int index)
    {
        var value = bits[index >>> Shift];
        value >>>= index;
        return (value & 1U) != 0U;
    }

    /// <summary>
    /// Sets a bit to 1. Returns <see langword="true" /> if a change has occurred -
    /// i.e. if the bit was previously 0
    /// </summary>
    /// <param name="bits">The span to modify</param>
    /// <param name="index">The bit to set</param>
    /// <param name="popCount">Will be incremented if the operation changes the contents of the span</param>
    /// <returns><see langword="true" /> if the operation changed the value of the bit, <see langword="false" /> if the bit was previously set</returns>
    public static bool SetBit(Span<uint> bits, int index, ref int popCount)
    {
        ref var arrayValue = ref bits[index >>> Shift];
        var oldValue = arrayValue;
        arrayValue |= 1U << index;
        var delta = (arrayValue ^ oldValue) >>> index;
        popCount += (int)delta;
        return (delta & 1U) != 0U;
    }

    /// <summary>
    /// Sets a bit to 0. Returns <see langword="true" /> if a change has occurred -
    /// i.e. if the bit was previously 1
    /// </summary>
    /// <param name="bits">The span to modify</param>
    /// <param name="index">The bit to clear</param>v
    /// <param name="popCount">Will be decremented if the operation changes the contents of the span</param>
    /// <returns><see langword="true" /> if the operation changed the value of the bit, <see langword="false" /> if the bit was previously clear</returns>
    public static bool ClearBit(Span<uint> bits, int index, ref int popCount)
    {
        ref var arrayValue = ref bits[index >>> Shift];
        var oldValue = arrayValue;
        arrayValue &= ~(1U << index);
        var delta = (arrayValue ^ oldValue) >>> index;
        popCount -= (int)delta;
        return (delta & 1U) != 0U;
    }

    /// <summary>
    /// Sets a bit to 0. 
    /// </summary>
    /// <param name="bits">The span to modify</param>
    /// <param name="index">The bit to clear</param>
    public static void ClearBit(Span<uint> bits, int index)
    {
        bits[index >>> Shift] &= ~(1U << index);
    }

    /// <summary>
    /// Toggles the value of a bit. Returns the new value after toggling
    /// </summary>
    /// <param name="bits">The span to modify</param>
    /// <param name="index">The bit to modify</param>v
    /// <param name="popCount">Will be modified accordingly if the operation changes the contents of the span</param>
    /// <returns>The bool equivalent of the binary value (0 or 1) of the bit after toggling</returns>
    public static bool ToggleBit(Span<uint> bits, int index, ref int popCount)
    {
        ref var arrayValue = ref bits[index >>> Shift];
        var oldValue = arrayValue;
        arrayValue ^= 1U << index;
        var result = arrayValue > oldValue;
        var delta = result ? 1 : -1;
        popCount += delta;
        return result;
    }

    [Pure]
    public static int GetPopCount(ReadOnlySpan<uint> bits)
    {
        // Basic implementation is faster than using TensorPrimitives - benchmarks

        var result = 0;
        switch (bits.Length)
        {
            case 7: result += BitOperations.PopCount(bits[6]); goto case 6;
            case 6: result += BitOperations.PopCount(bits[5]); goto case 5;
            case 5: result += BitOperations.PopCount(bits[4]); goto case 4;
            case 4: result += BitOperations.PopCount(bits[3]); goto case 3;
            case 3: result += BitOperations.PopCount(bits[2]); goto case 2;
            case 2: result += BitOperations.PopCount(bits[1]); goto case 1;
            case 1: result += BitOperations.PopCount(bits[0]); return result;
            case 0: return 0;
        }

        for (int i = 0; i < bits.Length; i++)
        {
            result += BitOperations.PopCount(bits[i]);
        }

        return result;
    }
}
