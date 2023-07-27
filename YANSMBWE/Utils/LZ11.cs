using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YANSMBWE.Utils
{
    public static class LZ11
    {
        public static readonly byte MAGIC = 0x11;

        public static (bool, uint) FindLastOccurrenceOf(byte[] data, byte[] sequence, uint start, uint end)
        {
            bool match = false;
            uint res = 0;
            for (uint i = start; i < end - sequence.Length; i++)
            {
                if (data[i] == sequence[0])
                    for (uint offset = 1; offset <= sequence.Length; offset++)
                    {
                        if (offset == sequence.Length) // Match
                        {
                            res = i;
                            match = true;
                            break;
                        }
                        if (data[i + offset] != sequence[offset])
                            break;
                    }
            }

            return (match, res);
        }

        public static (uint, uint) CompressionSearch(byte[] data, uint offset, uint length, uint windowSize = 0x1000, uint maxMatchAmount=18)
        {
            if (windowSize > offset)
                windowSize = offset;
            uint start = offset - windowSize;

            if (windowSize < maxMatchAmount)
                maxMatchAmount = windowSize;
            if ((length - offset) < maxMatchAmount)
                maxMatchAmount = length - offset;

            uint lower = 3;
            uint upper = maxMatchAmount;

            uint bestMatchOffset = 0;
            uint bestMatchLength = 0;

            while (lower <= upper)
            {
                uint matchLength = (lower + upper) / 2;

                byte[] match = data[(int)offset..(int)(Math.Min(offset + matchLength, data.Length - 1))];

                (bool hasMatch, uint matchOffset) = FindLastOccurrenceOf(data, match, start, offset);

                if (!hasMatch)
                {
                    upper = matchLength - 1;
                    continue;
                }

                if (matchLength > bestMatchLength)
                {
                    bestMatchLength = matchLength;
                    bestMatchOffset = matchOffset;

                    lower = matchLength + 1;
                }
            }

            if (bestMatchLength == 0)
                return (0, 0);
            return (offset - bestMatchOffset, bestMatchLength);
        }

        public static byte[] Compress(byte[] data)
        {
            uint dataLength = (uint)data.LongLength;

            if (dataLength > 0xffffff)
                throw new ArgumentException();

            byte[] result = new byte[dataLength * 9 / 8 + 1000];

            using MemoryStream resultStream = new(result);

            resultStream.WriteByte(0x11);

            //TODO Use CustomEndianBitConverter
            resultStream.WriteByte((byte)(dataLength & 0xff));
            resultStream.WriteByte((byte)((dataLength >> 8) & 0xff));
            resultStream.WriteByte((byte)((dataLength >> 16) & 0xff));


            uint src = 0;
            uint dst = 4;

            while (src < dataLength)
            {
                byte flag = 0;
                uint flagPos = dst;

                resultStream.WriteByte(flag);
                dst++;

                for (int i = 0; i < 8; i++)
                {
                    (uint matchOffset, uint matchLength) = CompressionSearch(data, src, dataLength, 0x1000, 0xffff + 273);
                    if (matchLength == 0) 
                    {
                        resultStream.WriteByte(data[src]);
                        
                        src++;
                        dst++;
                    }
                    else
                    {
                        flag |= (byte)(0b10000000 >> i);

                        matchOffset--;

                        if (matchLength <= 0x10)
                        {
                            resultStream.WriteByte((byte)((((matchLength - 1) & 0xf) << 4) | ((matchOffset >> 8) & 0xf)));
                            resultStream.WriteByte((byte)(matchOffset & 0xff));

                            dst += 2;
                        }
                        else if (matchLength <= 0x110)
                        {
                            uint tLen = matchLength - 17;
                            resultStream.WriteByte((byte)((tLen & 0xFF) >> 4));
                            resultStream.WriteByte((byte)(((tLen & 0xF) << 4) | ((matchOffset & 0xFFF) >> 8)));
                            resultStream.WriteByte((byte)(matchOffset & 0xFF));

                            dst += 3;
                        }
                        else
                        {
                            uint tLen = matchLength - 273;
                            resultStream.WriteByte((byte)(0x10 | ((tLen >> 12) & 0xF)));
                            resultStream.WriteByte((byte)((tLen >> 4) & 0xFF));
                            resultStream.WriteByte((byte)(((tLen & 0xF) << 4) | ((matchOffset >> 8) & 0xF)));
                            resultStream.WriteByte((byte)(matchOffset & 0xFF));

                            dst += 4;
                        }

                        src += matchLength;
                    }

                    if (src >= dataLength)
                        break;
                }

                result[flagPos] = flag;
            }

            Array.Resize(ref result, (int)dst);
            return result;
        }

        public static byte[] Decompress(byte[] data)
        {
            if (data.Length > 0x4000 * 0x4000 * 2)
                throw new ArgumentException();
            if (data[0] != MAGIC)
                throw new ArgumentException();
            
            int index = 0;

            uint decompressedSize = CustomEndianBitConverter.ToUInt32(data, index, true) >> 8;
            index += 4;
            if (decompressedSize == 0)
            {
                decompressedSize = CustomEndianBitConverter.ToUInt32(data, 4, true);
                index += 4;
            }

            byte[] result = new byte[decompressedSize];

            using MemoryStream resultStream = new(result);
            uint currentSize = 0;

            while (currentSize < decompressedSize && index < data.Length)
            {
                byte flags = data[index];
                index++;

                for (int i = 0; i < 8; i++)
                {
                    if (currentSize >= decompressedSize)
                        break;

                    if ((flags & (0b10000000 >> i)) == 0)
                    {
                        resultStream.WriteByte(data[index]);
                        index++;
                        currentSize++;

                        continue;
                    }

                    uint copyPos, copyLength;

                    uint first = data[index];
                    index++;

                    uint second = data[index];
                    index++;

                    if (first < 0x20)
                    {
                        uint third = data[index];
                        index++;

                        if (first >= 0x10)
                        {
                            uint fourth = data[index];
                            index++;

                            copyPos = (((third & 0xF) << 8) | fourth) + 1;
                            copyLength = ((second << 4) | ((first & 0xF) << 12) | (third >> 4)) + 273;
                        }
                        else
                        {
                            copyPos = (((second & 0xF) << 8) | third) + 1;
                            copyLength = (((first & 0xF) << 4) | (second >> 4)) + 17;
                        }
                    }
                    else
                    {
                        copyPos = (((first & 0xF) << 8) | second) + 1;
                        copyLength = (first >> 4) + 1;
                    }

                    uint start = currentSize - copyPos;
                    uint length = Math.Min(copyLength, currentSize - start);
                    Span<byte> copyBuffer = result.AsSpan((int)start, (int)length);
                    for (int j = 0; j < (copyLength / copyBuffer.Length); j++)
                        resultStream.Write(copyBuffer);
                    resultStream.Write(copyBuffer[..(int)(copyLength % copyBuffer.Length)]);

                    currentSize += copyLength;

                    if (index >= data.Length || currentSize >= decompressedSize)
                        break;
                }
            }

            //Would zero fill the remaining space but that's default

            return result;
        }
    }
}
