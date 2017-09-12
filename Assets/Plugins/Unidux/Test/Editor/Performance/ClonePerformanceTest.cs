using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Unidux.Util;

namespace Unidux.Performance
{
    public class ClonePerformanceTest
    {
        [Test]
        public void CloneTest()
        {
            var loopCount = 100;
            var state = SampleState.Create(loopCount);
            var watch = new Stopwatch();

            var timeOfCustomClone = 0;
            var timeOfAutoClone = 0;
            var timeOfReflectionClone = 0;

            {
                watch.Reset();
                watch.Start();
                state.CustomClone();
                watch.Stop();
                timeOfCustomClone = watch.Elapsed.Milliseconds;
            }
            {
                watch.Reset();
                watch.Start();
                state.Clone();
                watch.Stop();
                timeOfAutoClone = watch.Elapsed.Milliseconds;
            }
            {
                watch.Reset();
                watch.Start();
                state.ReflectionClone();
                watch.Stop();
                timeOfReflectionClone = watch.Elapsed.Milliseconds;
            }

            // dummy check
            Assert.IsTrue(true);

            {
                UnityEngine.Debug.Log(string.Format(
                    "[Perf] loop: {0}, CustomClone: {1}[ms], ReflectionClone: {2}[ms], AutClone: {3}[ms]",
                    loopCount,
                    timeOfCustomClone,
                    timeOfReflectionClone,
                    timeOfAutoClone
                ));
            }
        }

        [Serializable]
        class SampleState : StateBase
        {
            public List<SampleEntity> List = new List<SampleEntity>();

            public static SampleState Create(int loop)
            {
                var state = new SampleState();

                for (int i = 0; i < loop; i++)
                {
                    state.List.Add(SampleEntity.Create(loop));
                }

                return state;
            }

            // custom implementation of Clone is faster than default BinaryFormatter implementation.
//            public override object Clone()
//            {
//                return CustomClone();
//            }

            public SampleState CustomClone()
            {
                var state = new SampleState();

                foreach (var entity in state.List)
                {
                    state.List.Add(entity.CustomClone());
                }

                return state;
            }

            public SampleState ReflectionClone()
            {
                return CloneUtil.CopyEntity(this, new SampleState());
            }
        }

        [Serializable]
        class SampleEntity : StateElement
        {
            public int IntValue = 0;
            public long LongValue = 0;
            public float FloatValue = 0f;
            public double DoubleValue = 0;
            public string StringValue = "";
            public List<string> StringListValue = new List<string>();
            public Dictionary<int, string> DictionaryStringValue = new Dictionary<int, string>();

            public static SampleEntity Create(int loop)
            {
                var entity = new SampleEntity();
                entity.IntValue = 1;
                entity.LongValue = 1;
                entity.FloatValue = 1;
                entity.DoubleValue = 1;
                entity.StringValue = "abcdefghijklmnopqrstu";

                for (var i = 0; i < loop; i++)
                {
                    entity.StringListValue.Add("abc");
                }

                for (var i = 0; i < loop; i++)
                {
                    entity.DictionaryStringValue[i] = "abc";
                }

                return entity;
            }

            public SampleEntity CustomClone()
            {
                var entity = new SampleEntity();
                entity.IntValue = this.IntValue;
                entity.LongValue = this.LongValue;
                entity.FloatValue = this.FloatValue;
                entity.DoubleValue = this.DoubleValue;
                entity.StringValue = this.StringValue;

                foreach (var value in this.StringListValue)
                {
                    entity.StringListValue.Add(value);
                }

                foreach (var entry in this.DictionaryStringValue)
                {
                    entity.DictionaryStringValue[entry.Key] = entry.Value;
                }

                return entity;
            }
        }
    }
}