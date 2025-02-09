﻿using BeatTogether.LiteNetLib.Enums;
using BeatTogether.LiteNetLib.Headers;
using Krypton.Buffers;
using System;
using System.Net;

namespace BeatTogether.LiteNetLib.Dispatchers
{
    public class UnconnectedMessageDispatcher
    {
        private readonly LiteNetServer _server;

        public UnconnectedMessageDispatcher(
            LiteNetServer server)
        {
            _server = server;
        }

        public void Send(EndPoint endPoint, ReadOnlySpan<byte> message, UnconnectedMessageType type)
        {
            var bufferWriter = new SpanBufferWriter(stackalloc byte[412]);
            if (type == UnconnectedMessageType.BasicMessage)
                new UnconnectedHeader().WriteTo(ref bufferWriter);
            else
                new BroadcastHeader().WriteTo(ref bufferWriter);
            bufferWriter.WriteBytes(message);
            _server.SendAsync(endPoint, bufferWriter);
        }
    }
}
