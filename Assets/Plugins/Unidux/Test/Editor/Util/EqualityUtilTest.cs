using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Unidux.Util
{
    public class EqualityUtilTest
    {
        [Test]
        public void DoubleEquals()
        {
            Assert.IsTrue(EqualityUtil.DoubleEquals(0.0, 0.0));
            Assert.IsTrue(EqualityUtil.DoubleEquals(0.0, 0.0 + Double.Epsilon));
            Assert.IsFalse(EqualityUtil.DoubleEquals(0.0, 0.0 + Double.Epsilon * 2));
            Assert.IsTrue(EqualityUtil.DoubleEquals(0.0, 0.0 + Double.Epsilon * 2, Double.Epsilon * 2));
        }

        [Test]
        public void FloatEquals()
        {
            Assert.IsTrue(EqualityUtil.FloatEquals(0.0f, 0.0f));
            Assert.IsTrue(EqualityUtil.FloatEquals(0.0f, 0.0f + Mathf.Epsilon));
            Assert.IsFalse(EqualityUtil.FloatEquals(0.0f, 0.0f + Mathf.Epsilon * 2));
            Assert.IsTrue(EqualityUtil.FloatEquals(0.0f, 0.0f + Mathf.Epsilon * 2, Mathf.Epsilon * 2));
        }

        [Test]
        public void EnumerableEquals()
        {
            // array
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new string[] { }, new string[] { }));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new string[] {""}, new string[] { }));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new string[] { }, new string[] {""}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new string[] {""}, new string[] {""}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new string[] {"Hello"}, new string[] {"Hello"}));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new string[] {"Hello"}, new string[] {"World"}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(
                new string[] {"one", "two", "theree"},
                new string[] {"one", "two", "theree"}
            ));

            // list
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new List<string> { }, new List<string> { }));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new List<string> {""}, new List<string> { }));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new List<string> { }, new List<string> {""}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new List<string> {""}, new List<string> {""}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new List<string> {"Hello"}, new List<string> {"Hello"}));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new List<string> {"Hello"}, new List<string> {"World"}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(
                new List<string> {"one", "two", "theree"},
                new List<string> {"one", "two", "theree"}
            ));
            
            // dictionary
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new Dictionary<string, int> { }, new Dictionary<string, int> { }));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new Dictionary<string,int> {{"", 0}}, new Dictionary<string,int> { }));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new Dictionary<string,int> { }, new Dictionary<string,int> {{"", 0}}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new Dictionary<string,int> {{"", 0}}, new Dictionary<string,int> {{"", 0}}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(new Dictionary<string,int> {{"Hello", 0}}, new Dictionary<string,int> {{"Hello", 0}}));
            Assert.IsFalse(EqualityUtil.EnumerableEquals(new Dictionary<string,int> {{"Hello", 0}}, new Dictionary<string,int> {{"World", 0}}));
            Assert.IsTrue(EqualityUtil.EnumerableEquals(
                new Dictionary<string,int> {{"one", 1}, {"two", 2}},
                new Dictionary<string,int> {{"one", 1}, {"two", 2}}
            ));
        }
        
        [Test]
        public void ObjectEqualsTest()
        {
            Assert.IsTrue(EqualityUtil.ObjectEquals(null, null));
            
            Assert.IsFalse(EqualityUtil.ObjectEquals(1, null));
            Assert.IsFalse(EqualityUtil.ObjectEquals(null, 1));
            Assert.IsTrue(EqualityUtil.ObjectEquals(1, 1));

            Assert.IsFalse(EqualityUtil.ObjectEquals(0.0, null));
            Assert.IsFalse(EqualityUtil.ObjectEquals(null, 0.0));
            Assert.IsTrue(EqualityUtil.ObjectEquals(0.0, 0.0));
            Assert.IsFalse(EqualityUtil.ObjectEquals(0.0 + Double.Epsilon * 2, 0.0));

            Assert.IsFalse(EqualityUtil.ObjectEquals(0.0, null));
            Assert.IsFalse(EqualityUtil.ObjectEquals(null, 0.0));
            Assert.IsTrue(EqualityUtil.ObjectEquals(0.0, 0.0));
            Assert.IsFalse(EqualityUtil.ObjectEquals(0.0 + Double.Epsilon * 2, 0.0));
            
            Assert.IsTrue(EqualityUtil.ObjectEquals(SampleEnum.one, SampleEnum.one));
            Assert.IsFalse(EqualityUtil.ObjectEquals(SampleEnum.one, SampleEnum.two));
        }

        [Test]
        public void FieldsEquals()
        {
            var entity1 = new SampleFieldEntity();
            var entity2 = new SampleFieldEntity();
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.IntValue = 1;
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.IntValue = 1;
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.DoubleValue = 1;
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.DoubleValue = 1;
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.BoolValue = true;
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.BoolValue = true;
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.StringValue = "abc";
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.StringValue = "abc";
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.ColorValue = Color.red;
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.ColorValue = Color.red;
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.Vector3Value = Vector3.one;
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.Vector3Value = Vector3.one;
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.StringArray = new string[] {"ein", "zwei", "drei"};
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.StringArray = new string[] {"ein", "zwei", "drei"};
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.ListValue = new List<string> {"ein", "zwei", "drei"};
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.ListValue = new List<string> {"ein", "zwei", "drei"};
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.DictionaryValue = new Dictionary<string, int>() {{"ein", 1}, {"zwei", 1}};
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.DictionaryValue = new Dictionary<string, int>() {{"ein", 1}, {"zwei", 1}};
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));

            entity1.IdNameState = new IdNameEntity() {Id = 1, Name = "Illya"};
            Assert.IsFalse(EqualityUtil.FieldsEquals(entity1, entity2));
            entity2.IdNameState = new IdNameEntity() {Id = 1, Name = "Illya"};
            Assert.IsTrue(EqualityUtil.FieldsEquals(entity1, entity2));
        }

        [Test]
        public void PropertiesEquals()
        {
            var entity1 = new SamplePropertyEntity();
            var entity2 = new SamplePropertyEntity();
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.IntValue = 1;
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.IntValue = 1;
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.DoubleValue = 1;
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.DoubleValue = 1;
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.BoolValue = true;
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.BoolValue = true;
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.StringValue = "abc";
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.StringValue = "abc";
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.ColorValue = Color.red;
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.ColorValue = Color.red;
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.Vector3Value = Vector3.one;
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.Vector3Value = Vector3.one;
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.StringArray = new string[] {"ein", "zwei", "drei"};
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.StringArray = new string[] {"ein", "zwei", "drei"};
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.ListValue = new List<string> {"ein", "zwei", "drei"};
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.ListValue = new List<string> {"ein", "zwei", "drei"};
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.DictionaryValue = new Dictionary<string, int>() {{"ein", 1}, {"zwei", 1}};
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.DictionaryValue = new Dictionary<string, int>() {{"ein", 1}, {"zwei", 1}};
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));

            entity1.IdNameState = new IdNameEntity() {Id = 1, Name = "Illya"};
            Assert.IsFalse(EqualityUtil.PropertiesEquals(entity1, entity2));
            entity2.IdNameState = new IdNameEntity() {Id = 1, Name = "Illya"};
            Assert.IsTrue(EqualityUtil.PropertiesEquals(entity1, entity2));
        }

        [Test]
        public void EntityEquals()
        {
            var entity1 = new SampleMixedEntity();
            var entity2 = new SampleMixedEntity();
            
            Assert.IsTrue(EqualityUtil.EntityEquals(null, null));
            Assert.IsFalse(EqualityUtil.EntityEquals(entity1, null));
            Assert.IsFalse(EqualityUtil.EntityEquals(null, entity2));
            Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));
            Assert.IsFalse(EqualityUtil.EntityEquals(new SampleFieldEntity(), new SamplePropertyEntity()));

            {
                entity1.FIntValue = 1;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FIntValue = 1;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FDoubleValue = 1;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FDoubleValue = 1;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FBoolValue = true;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FBoolValue = true;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FStringValue = "abc";
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FStringValue = "abc";
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FColorValue = Color.red;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FColorValue = Color.red;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FVector3Value = Vector3.one;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FVector3Value = Vector3.one;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FStringArray = new string[] {"ein", "zwei", "drei"};
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FStringArray = new string[] {"ein", "zwei", "drei"};
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FListValue = new List<string> {"ein", "zwei", "drei"};
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FListValue = new List<string> {"ein", "zwei", "drei"};
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FDictionaryValue = new Dictionary<string, int>() {{"ein", 1}, {"zwei", 1}};
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FDictionaryValue = new Dictionary<string, int>() {{"ein", 1}, {"zwei", 1}};
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.FIdNameState = new IdNameEntity() {Id = 1, Name = "Illya"};
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.FIdNameState = new IdNameEntity() {Id = 1, Name = "Illya"};
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));
            }
            
            {
                entity1.PIntValue = 1;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PIntValue = 1;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PDoubleValue = 1;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PDoubleValue = 1;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PBoolValue = true;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PBoolValue = true;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PStringValue = "abc";
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PStringValue = "abc";
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PColorValue = Color.red;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PColorValue = Color.red;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PVector3Value = Vector3.one;
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PVector3Value = Vector3.one;
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PStringArray = new string[] {"ein", "zwei", "drei"};
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PStringArray = new string[] {"ein", "zwei", "drei"};
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PListValue = new List<string> {"ein", "zwei", "drei"};
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PListValue = new List<string> {"ein", "zwei", "drei"};
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PDictionaryValue = new Dictionary<string, int>() {{"ein", 1}, {"zwei", 1}};
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PDictionaryValue = new Dictionary<string, int>() {{"ein", 1}, {"zwei", 1}};
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));

                entity1.PIdNameState = new IdNameEntity() {Id = 1, Name = "Illya"};
                Assert.IsFalse(EqualityUtil.EntityEquals(entity1, entity2));
                entity2.PIdNameState = new IdNameEntity() {Id = 1, Name = "Illya"};
                Assert.IsTrue(EqualityUtil.EntityEquals(entity1, entity2));
            }
        }


        class SamplePropertyEntity
        {
            public int IntValue { get; set; }
            public double DoubleValue { get; set; }
            public bool BoolValue { get; set; }

            public string StringValue { get; set; }
            public Color ColorValue { get; set; }
            public Vector3 Vector3Value { get; set; }

            public string[] StringArray { get; set; }
            public List<string> ListValue { get; set; }
            public Dictionary<string, int> DictionaryValue { get; set; }

            public IdNameEntity IdNameState { get; set; }

            public SamplePropertyEntity()
            {
                this.IntValue = 0;
                this.DoubleValue = 0.0;
                this.BoolValue = false;

                this.StringValue = "";
                this.ColorValue = Color.black;
                this.Vector3Value = Vector3.zero;

                this.StringArray = new string[] {"one", "two", "three"};
                this.ListValue = new List<string>() {"one", "two", "three"};
                this.DictionaryValue = new Dictionary<string, int>() {{"one", 1}, {"two", 2}};
                this.IdNameState = new IdNameEntity();
            }
        }

        class SampleFieldEntity
        {
            public int IntValue = 0;
            public double DoubleValue = 0;
            public bool BoolValue = false;

            public string StringValue = "";
            public Color ColorValue = Color.black;
            public Vector3 Vector3Value = Vector3.zero;

            public string[] StringArray = {"one", "two", "three"};
            public List<string> ListValue = new List<string>() {"one", "two", "three"};
            public Dictionary<string, int> DictionaryValue = new Dictionary<string, int>() {{"one", 1}, {"two", 2}};

            public IdNameEntity IdNameState = new IdNameEntity();
        }

        class SampleMixedEntity
        {
            public int PIntValue { get; set; }
            public double PDoubleValue { get; set; }
            public bool PBoolValue { get; set; }
            public string PStringValue { get; set; }
            public Color PColorValue { get; set; }
            public Vector3 PVector3Value { get; set; }
            public string[] PStringArray { get; set; }
            public List<string> PListValue { get; set; }
            public Dictionary<string, int> PDictionaryValue { get; set; }
            public IdNameEntity PIdNameState { get; set; }
            
            public int FIntValue = 0;
            public double FDoubleValue = 0;
            public bool FBoolValue = false;
            public string FStringValue = "";
            public Color FColorValue = Color.black;
            public Vector3 FVector3Value = Vector3.zero;
            public string[] FStringArray = {"one", "two", "three"};
            public List<string> FListValue = new List<string>() {"one", "two", "three"};
            public Dictionary<string, int> FDictionaryValue = new Dictionary<string, int>() {{"one", 1}, {"two", 2}};
            public IdNameEntity FIdNameState = new IdNameEntity();

            public SampleMixedEntity()
            {
                this.PIntValue = 0;
                this.PDoubleValue = 0.0;
                this.PBoolValue = false;
                this.PStringValue = "";
                this.PColorValue = Color.black;
                this.PVector3Value = Vector3.zero;
                this.PStringArray = new string[] {"one", "two", "three"};
                this.PListValue = new List<string>() {"one", "two", "three"};
                this.PDictionaryValue = new Dictionary<string, int>() {{"one", 1}, {"two", 2}};
                this.PIdNameState = new IdNameEntity();
            }
        }

        public class IdNameEntity : StateElement
        {
            public int Id = 0;
            public string Name = "";

            public override bool Equals(object obj)
            {
                if (!(obj is IdNameEntity))
                {
                    return false;
                }

                IdNameEntity state = (IdNameEntity) obj;
                return obj != null && this.Id == state.Id && this.Name == state.Name;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        enum SampleEnum
        {
            one,
            two,
            three,
        }
    }
}