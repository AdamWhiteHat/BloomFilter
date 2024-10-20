﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BloomFilterCore.Filters
{
    [Serializable]
    public class RunLengthEncodedFilter : IFilter, ISerializable
    {
        public bool this[int index]
        {
            get { return GetBitValueAt(index); }
            set { SetBitValueAt(index, value); }
        }

        public int Length { get; private set; }
        public int SetBitCount
        {
            get
            {
                return _filter.Where(run => run.Value == true)
                              .Sum(run => run.Length);
            }
        }

        private List<Run> _filter;
        private static string SerializedNodeName = "FilterArray";

        public bool[] GetArray()
        {
            return _filter.SelectMany(run => run.GetArray()).ToArray();
        }

        public RunLengthEncodedFilter()
        {
            _filter = new List<Run>();
        }

        public RunLengthEncodedFilter(int filterSizeInBits)
            : this()
        {
            Length = filterSizeInBits;
            _filter.Add(new Run(0, filterSizeInBits - 1, false));
        }

        private bool GetBitValueAt(int index)
        {
            int retrieveIndex = FindRunIndexFromBitIndex(index);
            return _filter[retrieveIndex].Value;
        }

        private void SetBitValueAt(int targetIndex, bool value)
        {
            int runIndex = FindRunIndexFromBitIndex(targetIndex);
            Run found = _filter[runIndex];

            // If the bit at the position is already set this value, do nothing.
            if (found.Value == value)
            {
                return; // Bail early.
            }

            // Beyond this point, it is determined that the
            // run our target index is in, consists of unlike values.

            // If flipping bit at the beginning of the run,
            // then modify the previous run to be 1 bit longer,
            // and this run 1 bit shorter, instead.
            if (targetIndex > 0 && targetIndex == found.StartIndex)
            {
                Run previous = _filter[runIndex - 1];
                bool check1 = (previous.Value == value);
                bool check2 = (previous.EndIndex + 1 == targetIndex);
                if (check1 && check2)
                {
                    found.StartIndex += 1;
                    previous.EndIndex += 1;
                    return; // We are done, bail
                }
            }
            // If flipping bit at the end of the run,
            // then modify the subsequent run to begin 1 bit earlier,
            // and this run 1 bit shorter, instead.
            else if (targetIndex < Length - 1 && targetIndex == found.EndIndex)
            {
                Run subsequent = _filter[runIndex + 1];
                bool check1 = (subsequent.Value == value);
                bool check2 = (subsequent.StartIndex - 1 == targetIndex);
                if (check1 && check2)
                {
                    found.EndIndex -= 1;
                    subsequent.StartIndex -= 1;
                    return; // We are done, bail
                }
            }

            Run before = new Run(found.StartIndex, targetIndex - 1, found.Value);
            Run at = new Run(targetIndex, targetIndex, value);
            Run after = new Run(targetIndex + 1, found.EndIndex, found.Value);
            found = null;

            _filter.RemoveAt(runIndex);

            _filter.Insert(runIndex, after);
            _filter.Insert(runIndex, at);
            _filter.Insert(runIndex, before);
        }

        private int FindRunIndexFromBitIndex(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException("Index can not be less than zero.");
            }
            if (index >= Length)
            {
                throw new IndexOutOfRangeException("Index can not be greater than or equal to the filter length.");
            }

            int result = _filter.FindIndex(run => run.IsIndexInRange(index));
            if (result == -1)
            {
                throw new Exception("Unable to find filter segment who's range include the supplied index.");
            }
            return result;
        }

        // This method is called during serialization.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue(nameof(Length), Length, typeof(int));
            info.AddValue(SerializedNodeName, Serialize(), typeof(string));
        }

        // This special constructor is used during deserialization.
        public RunLengthEncodedFilter(SerializationInfo info, StreamingContext context)
        {
            // Use the GetValue method to retrieve serialized values.
            Length = (int)info.GetValue(nameof(Length), typeof(int));
            string serializedValue = (string)info.GetValue(SerializedNodeName, typeof(string));
            Deserialize(serializedValue);
        }

        private void Deserialize(string value)
        {
            _filter = new List<Run>();

            string[] runs = value.Split(new char[] { '|' });

            int index = 0;
            foreach (string runString in runs)
            {
                Run newRun = Run.Deserialize(runString, index);
                index += newRun.Length;

                _filter.Add(newRun);
            }

            if (index != Length)
            {
                throw new SerializationException($"index != {nameof(Length)} ({index} != {Length})");
            }
        }

        private string Serialize()
        {
            return string.Join("|", _filter.Select(run => run.ToString()));
        }

        private class Run
        {
            public bool Value { get; private set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
            public int Length { get { return (EndIndex - StartIndex) + 1; } }

            private Run()
            { }

            public Run(int startIndex, int endIndex, bool value)
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
                Value = value;
            }

            public bool IsIndexInRange(int index)
            {
                return (index >= StartIndex && index <= EndIndex);
            }

            public IEnumerable<bool> GetArray()
            {
                return Enumerable.Repeat(Value, Length);
            }

            public static Run Deserialize(string value, int index)
            {
                return new Run() // "T32"
                {
                    Value = value[0] == 'T',
                    StartIndex = index,
                    EndIndex = index + int.Parse(value.Substring(1)) - 1
                };
            }

            public bool[] Serialize()
            {
                // # bytes used to represent length | length | value
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return $"{(Value ? "T" : "F")}{Length}";
            }
        }
    }
}
