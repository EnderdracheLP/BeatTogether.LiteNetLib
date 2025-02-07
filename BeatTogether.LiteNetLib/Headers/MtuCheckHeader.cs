﻿using BeatTogether.LiteNetLib.Enums;
using BeatTogether.LiteNetLib.Headers.Abstractions;
using Krypton.Buffers;

namespace BeatTogether.LiteNetLib.Headers
{
    public class MtuCheckHeader : BaseLiteNetHeader
    {
        public override PacketProperty Property { get; set; } = PacketProperty.MtuCheck;
        public int Mtu { get; set; }
        public int PadSize { get; set; }
        public int CheckEnd { get; set; }

        public override void ReadFrom(ref SpanBufferReader bufferReader)
        {
            base.ReadFrom(ref bufferReader);
            Mtu = bufferReader.ReadInt32();
            PadSize = bufferReader.RemainingSize - 4; // 4 = int32 size
            bufferReader.SkipBytes(PadSize);
            CheckEnd = bufferReader.ReadInt32();
        }

        public override void WriteTo(ref SpanBufferWriter bufferWriter)
        {
            base.WriteTo(ref bufferWriter);
            bufferWriter.WriteInt32(Mtu);
            bufferWriter.PadBytes(PadSize);
            bufferWriter.WriteInt32(CheckEnd);
        }
    }
}
