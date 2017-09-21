using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Unidux.Performance
{
    public class EqualsPerformanceTest
    {
        [Test]
        public void EqualsTest()
        {
            var loopCount = 1000;
            var state1 = SampleState.Create(loopCount);
            var state2 = SampleState.Create(loopCount);
            var watch = new Stopwatch();

            var timeOfEquality = 0;
            var timeOfCustomEquals = 0;

            {
                watch.Reset();
                watch.Start();
                Assert.IsTrue(state1.Equals(state2));
                watch.Stop();
                timeOfEquality = watch.Elapsed.Milliseconds;
            }
            {
                watch.Reset();
                watch.Start();
                Assert.IsTrue(state1.CustomEquals(state2));
                watch.Stop();
                timeOfCustomEquals = watch.Elapsed.Milliseconds;
            }

            var gain = 100 * (double) timeOfCustomEquals / timeOfEquality;

            UnityEngine.Debug.Log(string.Format(
                "[Perf] loops: {0}, CustomEquals: {1}[ms], AutoEquals: {2}[ms], gain {3}%",
                loopCount,
                timeOfCustomEquals,
                timeOfEquality,
                gain.ToString("F2")
            ));
        }

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

            public bool CustomEquals(object targetObject)
            {
                var target = (SampleState) targetObject;
                return CustomListEquals(target.List);
            }

            private bool CustomListEquals(List<SampleEntity> targets)
            {
                if (this.List.Count != targets.Count)
                {
                    return false;
                }

                for (var i = 0; i < this.List.Count; i++)
                {
                    if (!this.List[i].CustomEquals(targets[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

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

            public bool CustomEquals(object targetObject)
            {
                var target = (SampleEntity) targetObject;

                return
                    this.IntValue.Equals(target.IntValue) &&
                    this.LongValue.Equals(target.LongValue) &&
                    this.FloatValue.Equals(target.FloatValue) &&
                    this.DoubleValue.Equals(target.DoubleValue) &&
                    this.StringValue.Equals(target.StringValue) &&
                    this.StringListValue.SequenceEqual(target.StringListValue) &&
                    this.DictionaryStringValue.SequenceEqual(target.DictionaryStringValue)
                    ;
            }
        }
    }
}