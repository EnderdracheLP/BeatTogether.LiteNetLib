﻿using Krypton.Buffers;
using System;
using System.Buffers;
using System.Collections.Concurrent;

namespace BeatTogether.LiteNetLib.Models
{
    public class FragmentBuilder
    {
        private int _fragmentsReceived;

        private readonly int _totalFragments;
        private readonly ConcurrentDictionary<int, ReadOnlyMemory<byte>> _fragments = new();

        public FragmentBuilder(int totalFragments)
        {
            _totalFragments = totalFragments;
        }

        public bool AddFragment(int fragmentId, ReadOnlySpan<byte> buffer)
        {
            if (_fragments.TryAdd(fragmentId, new ReadOnlyMemory<byte>(buffer.ToArray())))
                _fragmentsReceived++;
            return _fragmentsReceived >= _totalFragments;
        }

        public void WriteTo(ref SpanBufferWriter writer)
        {
            for (int i = 0; i < _totalFragments; i++)
                if (_fragments.TryGetValue(i, out var memory))
                    writer.WriteBytes(memory.Span);
                else
                    throw new Exception(); // TODO
        }
    }
}
