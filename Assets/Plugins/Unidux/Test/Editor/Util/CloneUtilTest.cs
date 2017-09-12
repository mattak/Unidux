using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Unidux.Util
{
    public class CloneUtilTest
    {
        [Test]
        public void MemoryCloneTest()
        {
            var sample1 = new SampleEntity();
            sample1.SetStateChanged();
            var sample2 = CloneUtil.MemoryClone(sample1) as SampleEntity;

            Assert.AreEqual(sample1.Id, sample2.Id);
            Assert.AreEqual(sample1.Name, sample2.Name);
            Assert.AreEqual(sample1.IsStateChanged, sample2.IsStateChanged);

            sample2.Id = sample1.Id + 1;
            sample2.Name = "John";

            Assert.AreNotEqual(sample1.Id, sample2.Id);
            Assert.AreNotEqual(sample1.Name, sample2.Name);
        }

        [Test]
        public void CopyEntityTest()
        {
            var sample1 = new SampleNonCloneable();
            var sample2 = new SampleNonCloneable();

            sample1.FieldValue = 1;
            sample2.FieldValue = 2;
            
            sample1.PropertyValue = 1;
            sample2.PropertyValue = 2;

            Assert.AreNotEqual(sample1, sample2);

            CloneUtil.CopyEntity(sample1, sample2);
            Assert.AreEqual(sample1, sample2);
            Assert.AreEqual(1, sample2.FieldValue);
            Assert.AreEqual(1, sample2.PropertyValue);
        }

        [Test]
        public void ListCloneTest()
        {
            {
                var list = new List<string>();
                var cloned = CloneUtil.ListClone(list);
                Assert.AreEqual(0, cloned.Count);
            }

            {
                var list = new List<string>() {"a", "b", "c"};
                var cloned = CloneUtil.ListClone(list);
                Assert.AreEqual(3, cloned.Count);
                Assert.AreEqual("a", cloned[0]);
                Assert.AreEqual("b", cloned[1]);
                Assert.AreEqual("c", cloned[2]);
            }
        }

        [Test]
        public void ArrayCloneTest()
        {
            {
                var list = new string[] { };
                var cloned = CloneUtil.ArrayClone(list);
                Assert.AreEqual(0, cloned.Length);
            }

            {
                var list = new string[] {"a", "b", "c"};
                var cloned = (string[]) CloneUtil.ArrayClone(list);
                Assert.AreEqual(3, cloned.Length);
                Assert.AreEqual("a", cloned[0]);
                Assert.AreEqual("b", cloned[1]);
                Assert.AreEqual("c", cloned[2]);
            }
        }

        [Test]
        public void DictionaryCloneTest()
        {
            {
                var dict = new Dictionary<string, int> { };
                var cloned = CloneUtil.DictionaryClone(dict);
                Assert.AreEqual(0, cloned.Count);
            }

            {
                var dict = new Dictionary<string, int> {{"one", 1}, {"two", 2}};
                var cloned = CloneUtil.DictionaryClone(dict);
                Assert.AreEqual(2, cloned.Count);
                Assert.AreEqual(1, cloned["one"]);
                Assert.AreEqual(2, cloned["two"]);
            }
        }

        [Test]
        public void ObjectCloneTest()
        {
            Assert.AreEqual(null, CloneUtil.ObjectClone(null));

            Assert.AreEqual(1, CloneUtil.ObjectClone(1));
            Assert.AreNotEqual(0, CloneUtil.ObjectClone(1));

            Assert.AreEqual(true, CloneUtil.ObjectClone(true));
            Assert.AreNotEqual(false, CloneUtil.ObjectClone(true));

            Assert.AreEqual("abc", CloneUtil.ObjectClone("abc"));
            Assert.AreNotEqual("", CloneUtil.ObjectClone("abc"));

            Assert.AreEqual(Animal.ProcyonLotor, CloneUtil.ObjectClone(Animal.ProcyonLotor));
            Assert.AreNotEqual(Animal.LeptailurusServal, CloneUtil.ObjectClone(Animal.ProcyonLotor));

            Assert.AreEqual(new int[] {1}, CloneUtil.ObjectClone(new int[] {1}));
            Assert.AreNotEqual(new int[] {0}, CloneUtil.ObjectClone(new int[] {1}));

            Assert.AreEqual(new List<int> {1}, CloneUtil.ObjectClone(new List<int> {1}));
            Assert.AreNotEqual(new List<int> {0}, CloneUtil.ObjectClone(new List<int> {1}));

            Assert.AreEqual(new Dictionary<int, int> {{1, 2}},
                CloneUtil.ObjectClone(new Dictionary<int, int> {{1, 2}}));
            Assert.AreNotEqual(new Dictionary<int, int> {{1, 0}},
                CloneUtil.ObjectClone(new Dictionary<int, int> {{1, 2}}));

            Assert.AreEqual(
                new SampleCloneable() {IntValue = 1},
                CloneUtil.ObjectClone(new SampleCloneable() {IntValue = 1})
            );
            Assert.AreNotEqual(
                new SampleCloneable() {IntValue = 0},
                CloneUtil.ObjectClone(new SampleCloneable() {IntValue = 1})
            );

            Assert.AreEqual(
                new SampleNonCloneable() {FieldValue = 1, PropertyValue = 2},
                CloneUtil.ObjectClone(new SampleNonCloneable() {FieldValue = 1, PropertyValue = 2})
            );
            Assert.AreNotEqual(
                new SampleNonCloneable() {FieldValue = 0, PropertyValue = 1},
                CloneUtil.ObjectClone(new SampleNonCloneable() {FieldValue = 1, PropertyValue = 2})
            );
        }

        enum Animal
        {
            LeptailurusServal,
            ProcyonLotor,
        }

        class SampleNonCloneable
        {
            public int FieldValue;
            public int PropertyValue { get; set; }

            public override bool Equals(object obj)
            {
                var target = (SampleNonCloneable) obj;
                return
                    this.FieldValue == target.FieldValue &&
                    this.PropertyValue == target.PropertyValue;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        class SampleCloneable : ICloneable
        {
            public int IntValue;

            public SampleCloneable()
            {
            }

            public SampleCloneable(SampleCloneable instance)
            {
                this.IntValue = instance.IntValue;
            }

            public object Clone()
            {
                return new SampleCloneable(this);
            }

            public override bool Equals(object obj)
            {
                var target = (SampleCloneable) obj;
                return this.IntValue.Equals(target.IntValue);
            }
            
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}