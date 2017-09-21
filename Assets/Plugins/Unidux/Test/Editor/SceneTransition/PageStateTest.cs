using NUnit.Framework;
using Unidux.Util;

namespace Unidux.SceneTransition
{
    public class PageStateTest
    {
        [SetUp]
        public void Before()
        {
        }

        [Test]
        public void CloneTest()
        {
            var state = new PageState<SamplePage>();
            state.Stack.Add(new PageEntity<SamplePage>(SamplePage.Page1, null));
            state.Stack.Add(new PageEntity<SamplePage>(SamplePage.Page2, new SampleData()));

            var cloned = (PageState<SamplePage>) state.Clone();
            Assert.AreEqual(state, cloned);
        }

        [Test]
        public void EqualsTest()
        {
            var state1 = new PageState<SamplePage>();
            var state2 = new PageState<SamplePage>();
            Assert.AreEqual(state1, state2);

            state1.Stack.Add(new PageEntity<SamplePage>(SamplePage.Page1, null));
            Assert.AreNotEqual(state1, state2);

            state2.Stack.Add(new PageEntity<SamplePage>(SamplePage.Page1, null));
            Assert.AreEqual(state1, state2);

            state2.Stack.RemoveLast();
            state2.Stack.Add(new PageEntity<SamplePage>(SamplePage.Page2, null));
            Assert.AreNotEqual(state1, state2);
            
            state2.Stack.RemoveLast();
            state2.Stack.Add(new PageEntity<SamplePage>(SamplePage.Page1, new SampleData()));
            Assert.AreNotEqual(state1, state2);
        }

        class SampleData : IPageData
        {
        }

        enum SampleScene
        {
            Sample1,
            Sample2,
        }

        enum SamplePage
        {
            Page1,
            Page2,
        }
    }
}