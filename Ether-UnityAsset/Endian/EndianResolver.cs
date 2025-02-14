﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether_UnityAsset.Endian
{
    public enum EndianType
    {
        //
        // 摘要:
        //     Little endian.
        LittleEndian,
        //
        // 摘要:
        //     Big endian.
        BigEndian
    }
    public class EndianBinaryReader : BinaryReader
    {
        //
        // 摘要:
        //     Simple read 2 byte buffer.
        private readonly byte[] buffer2;

        //
        // 摘要:
        //     Simple read 4 byte buffer.
        private readonly byte[] buffer4;

        //
        // 摘要:
        //     Simple read 8 byte buffer.
        private readonly byte[] buffer8;

        //
        // 摘要:
        //     The EndianType to read.
        public EndianType Endian { get; set; }

        //
        // 摘要:
        //     The BaseStream Position.
        public long Position
        {
            get
            {
                return BaseStream.Position;
            }
            set
            {
                BaseStream.Position = value;
            }
        }

        //
        // 摘要:
        //     The EndianType of the BitConverter.
        private EndianType BitConverterEndian
        {
            get
            {
                if (!BitConverter.IsLittleEndian)
                {
                    return EndianType.BigEndian;
                }

                return EndianType.LittleEndian;
            }
        }

        //
        // 摘要:
        //     The endian binary reader needs a stream to read from and the endian.
        //
        // 参数:
        //   stream:
        //
        //   endian:
        public EndianBinaryReader(Stream stream, EndianType endian = EndianType.BigEndian)
            : base(stream)
        {
            Endian = endian;
            buffer2 = new byte[2];
            buffer4 = new byte[4];
            buffer8 = new byte[8];
        }

        //
        // 摘要:
        //     Unity uses 4 byte blocks. Align the stream if uneven to it.
        public void AlignStream()
        {
            AlignStream(4);
        }

        //
        // 摘要:
        //     Align the stream to _Alignment if uneven to it.
        public void AlignStream(int _Alignment)
        {
            long num = Position % _Alignment;
            if (num != 0L)
            {
                Position += _Alignment - num;
            }
        }

        //
        // 摘要:
        //     Read a Int16 depend on Endian.
        public override short ReadInt16()
        {
            Read(buffer2, 0, 2);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(buffer2);
            }

            return BitConverter.ToInt16(buffer2, 0);
        }

        //
        // 摘要:
        //     Read a UInt16 depend on Endian.
        public override ushort ReadUInt16()
        {
            Read(buffer2, 0, 2);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(buffer2);
            }

            return BitConverter.ToUInt16(buffer2, 0);
        }

        //
        // 摘要:
        //     Read a Int32 depend on Endian.
        public override int ReadInt32()
        {
            Read(buffer4, 0, 4);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(buffer4);
            }

            return BitConverter.ToInt32(buffer4, 0);
        }

        //
        // 摘要:
        //     Read a UInt32 depend on Endian.
        public override uint ReadUInt32()
        {
            Read(buffer4, 0, 4);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(buffer4);
            }

            return BitConverter.ToUInt32(buffer4, 0);
        }

        //
        // 摘要:
        //     Read a Int64 depend on Endian.
        public override long ReadInt64()
        {
            Read(buffer8, 0, 8);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(buffer8);
            }

            return BitConverter.ToInt64(buffer8, 0);
        }

        //
        // 摘要:
        //     Read a UInt64 depend on Endian.
        public override ulong ReadUInt64()
        {
            Read(buffer8, 0, 8);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(buffer8);
            }

            return BitConverter.ToUInt64(buffer8, 0);
        }

        //
        // 摘要:
        //     Read a Float depend on Endian.
        public override float ReadSingle()
        {
            Read(buffer4, 0, 4);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(buffer4);
            }

            return BitConverter.ToSingle(buffer4, 0);
        }

        //
        // 摘要:
        //     Read a Double depend on Endian.
        public override double ReadDouble()
        {
            Read(buffer8, 0, 8);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(buffer8);
            }

            return BitConverter.ToDouble(buffer8, 0);
        }

        //
        // 摘要:
        //     Read an aligned string.
        public string ReadAlignedString()
        {
            int num = ReadInt32();
            if (num > 0 && num <= BaseStream.Length - BaseStream.Position)
            {
                byte[] bytes = ReadBytes(num);
                string @string = Encoding.UTF8.GetString(bytes);
                AlignStream();
                return @string;
            }

            return "";
        }

        //
        // 摘要:
        //     Read a string until read null byte.
        //
        // 参数:
        //   _MaxLength:
        public string ReadStringToNull(int _MaxLength = 32767)
        {
            List<byte> list = new List<byte>();
            int num = 0;
            while (BaseStream.Position != BaseStream.Length && num < _MaxLength)
            {
                byte b = ReadByte();
                if (b == 0)
                {
                    break;
                }

                list.Add(b);
                num++;
            }

            return Encoding.UTF8.GetString(list.ToArray());
        }
    }
    public class EndianBinaryWriter : BinaryWriter
    {
        //
        // 摘要:
        //     The EndianType to read.
        public EndianType Endian { get; set; }

        //
        // 摘要:
        //     The BaseStream Position.
        public long Position
        {
            get
            {
                return BaseStream.Position;
            }
            set
            {
                BaseStream.Position = value;
            }
        }

        //
        // 摘要:
        //     The EndianType of the BitConverter.
        private EndianType BitConverterEndian
        {
            get
            {
                if (!BitConverter.IsLittleEndian)
                {
                    return EndianType.BigEndian;
                }

                return EndianType.LittleEndian;
            }
        }

        //
        // 摘要:
        //     The endian binary writer needs a stream to write to and the endian.
        //
        // 参数:
        //   stream:
        //
        //   endian:
        public EndianBinaryWriter(Stream stream, EndianType endian = EndianType.BigEndian)
            : base(stream)
        {
            Endian = endian;
        }

        //
        // 摘要:
        //     Unity uses 4 byte blocks. Align the stream if uneven to it.
        public void AlignStream()
        {
            AlignStream(4);
        }

        //
        // 摘要:
        //     Align the stream to _Alignment if uneven to it.
        public void AlignStream(int _Alignment)
        {
            long num = BaseStream.Position % _Alignment;
            if (num != 0L)
            {
                Write(new byte[_Alignment - num]);
            }
        }

        //
        // 摘要:
        //     Write a Int16 depend on Endian.
        //
        // 参数:
        //   _Value:
        public override void Write(short _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(bytes);
            }

            Write(bytes);
        }

        //
        // 摘要:
        //     Write a UInt16 depend on Endian.
        //
        // 参数:
        //   _Value:
        public override void Write(ushort _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(bytes);
            }

            Write(bytes);
        }

        //
        // 摘要:
        //     Write a Int32 depend on Endian.
        //
        // 参数:
        //   _Value:
        public override void Write(int _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(bytes);
            }

            Write(bytes);
        }

        //
        // 摘要:
        //     Write a UInt32 depend on Endian.
        //
        // 参数:
        //   _Value:
        public override void Write(uint _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(bytes);
            }

            Write(bytes);
        }

        //
        // 摘要:
        //     Write a Int64 depend on Endian.
        //
        // 参数:
        //   _Value:
        public override void Write(long _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(bytes);
            }

            Write(bytes);
        }

        //
        // 摘要:
        //     Write a UInt64 depend on Endian.
        //
        // 参数:
        //   _Value:
        public override void Write(ulong _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(bytes);
            }

            Write(bytes);
        }

        //
        // 摘要:
        //     Write a float depend on Endian.
        //
        // 参数:
        //   _Value:
        public override void Write(float _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(bytes);
            }

            Write(bytes);
        }

        //
        // 摘要:
        //     Write a double depend on Endian.
        //
        // 参数:
        //   _Value:
        public override void Write(double _Value)
        {
            byte[] bytes = BitConverter.GetBytes(_Value);
            if (Endian != BitConverterEndian)
            {
                Array.Reverse(bytes);
            }

            Write(bytes);
        }

        //
        // 摘要:
        //     Write an aligned string and write the length.
        //
        // 参数:
        //   _String:
        public void WriteAlignedString(string _String)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(_String);
            Write(bytes.Length);
            Write(bytes);
            AlignStream();
        }

        //
        // 摘要:
        //     Write a string without writing the length. And add a NULL at the end.
        //
        // 参数:
        //   _String:
        public void WriteStringToNull(string _String)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(_String);
            Write(bytes);
            Write(new byte[1]);
        }
    }
}

