using System.Collections.Generic;
using NUnit.Framework;

namespace Unidux.SceneTransition
{
    public class PageDuckTest
    {
        private PageDuck<SamplePage, SampleScene>.Reducer reducer;
        private SceneState<SampleScene> sceneState;
        private PageState<SamplePage> pageState;

        [SetUp]
        public void Before()
        {
            var config = new SampleConfig();
            this.reducer = new SampleReducer(config);
            this.sceneState = new SceneState<SampleScene>();
            this.pageState = new PageState<SamplePage>();
        }

        [Test]
        public void PushTest()
        {
            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Push(SamplePage.Page1)
                );

                Assert.IsTrue(result.IsReady);
                Assert.AreEqual(SamplePage.Page1, result.Current.Page);
                Assert.IsNull(result.Current.Data);
                Assert.AreEqual(1, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsTrue(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene2]);
            }

            // duplicate push check
            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Push(SamplePage.Page1)
                );

                Assert.IsTrue(result.IsReady);
                Assert.AreEqual(SamplePage.Page1, result.Current.Page);
                Assert.IsNull(result.Current.Data);
                Assert.AreEqual(1, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsTrue(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene2]);
            }

            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Push(SamplePage.Page2)
                );

                Assert.IsTrue(result.IsReady);
                Assert.AreEqual(SamplePage.Page2, result.Current.Page);
                Assert.IsNull(result.Current.Data);
                Assert.AreEqual(2, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsTrue(sceneState.ActiveMap[SampleScene.Scene2]);
            }
        }

        [Test]
        public void PopTest()
        {
            reducer.Reduce(
                pageState,
                sceneState,
                PageDuck<SamplePage, SampleScene>.ActionCreator.Push(SamplePage.Page1)
            );
            reducer.Reduce(
                pageState,
                sceneState,
                PageDuck<SamplePage, SampleScene>.ActionCreator.Push(SamplePage.Page2)
            );

            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Pop()
                );

                Assert.IsTrue(result.IsReady);
                Assert.AreEqual(SamplePage.Page1, result.Current.Page);
                Assert.IsNull(result.Current.Data);
                Assert.AreEqual(1, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsTrue(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene2]);
            }

            // empty pop check
            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Pop()
                );

                Assert.IsTrue(result.IsReady);
                Assert.AreEqual(SamplePage.Page1, result.Current.Page);
                Assert.IsNull(result.Current.Data);
                Assert.AreEqual(1, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsTrue(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene2]);
            }
        }

        [Test]
        public void ReplaceTest()
        {
            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Replace(SamplePage.Page1)
                );

                Assert.IsTrue(result.IsReady);
                Assert.AreEqual(SamplePage.Page1, result.Current.Page);
                Assert.IsNull(result.Current.Data);
                Assert.AreEqual(1, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsTrue(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene2]);
            }

            // empty pop check
            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Replace(SamplePage.Page2)
                );

                Assert.IsTrue(result.IsReady);
                Assert.AreEqual(SamplePage.Page2, result.Current.Page);
                Assert.IsNull(result.Current.Data);
                Assert.AreEqual(1, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsTrue(sceneState.ActiveMap[SampleScene.Scene2]);
            }
        }

        [Test]
        public void ResetTest()
        {
            // empty check
            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Reset()
                );
                Assert.IsFalse(result.IsReady);
                Assert.AreEqual(0, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene1]);
            }

            reducer.Reduce(
                pageState,
                sceneState,
                PageDuck<SamplePage, SampleScene>.ActionCreator.Push(SamplePage.Page1)
            );
            reducer.Reduce(
                pageState,
                sceneState,
                PageDuck<SamplePage, SampleScene>.ActionCreator.Push(SamplePage.Page2)
            );

            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.Reset()
                );
                Assert.IsFalse(result.IsReady);
                Assert.AreEqual(0, result.Stack.Count);

                Assert.AreEqual(2, sceneState.ActiveMap.Count);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene1]);
            }
        }

        [Test]
        public void SetDataTest()
        {
            // error check
            {
                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.SetData(new SamplePageData(1))
                );
                Assert.IsFalse(result.IsReady);
                Assert.AreEqual(0, result.Stack.Count);
            }

            reducer.Reduce(
                pageState,
                sceneState,
                PageDuck<SamplePage, SampleScene>.ActionCreator.Push(SamplePage.Page1, new SamplePageData(1))
            );

            {
                Assert.IsTrue(pageState.IsReady);
                Assert.AreEqual(1, pageState.Stack.Count);
                Assert.AreNotEqual(new SamplePageData(2), pageState.Current.Data);

                var result = reducer.Reduce(
                    pageState,
                    sceneState,
                    PageDuck<SamplePage, SampleScene>.ActionCreator.SetData(new SamplePageData(2))
                );
                Assert.IsTrue(result.IsReady);
                Assert.AreEqual(1, result.Stack.Count);
                Assert.AreEqual(new SamplePageData(2), result.Current.Data);
            }
        }


        [Test]
        public void AdjustTest()
        {
            pageState.Stack.Add(new PageEntity<SamplePage>(SamplePage.Page1, null));
            Assert.AreEqual(0, sceneState.ActiveMap.Count);
            
            reducer.Reduce(
                pageState,
                sceneState,
                PageDuck<SamplePage, SampleScene>.ActionCreator.Adjust()
            );
            Assert.AreEqual(2, sceneState.ActiveMap.Count);
            Assert.IsTrue(sceneState.ActiveMap[SampleScene.Scene1]);
            Assert.IsFalse(sceneState.ActiveMap[SampleScene.Scene2]);
        }

        enum SampleScene
        {
            Scene1,
            Scene2,
            Scene3,
        }

        enum SamplePage
        {
            Page1,
            Page2,
        }

        class SampleConfig : ISceneConfig<SampleScene, SamplePage>
        {
            public IDictionary<SampleScene, int> CategoryMap
            {
                get
                {
                    return new Dictionary<SampleScene, int>()
                    {
                        {SampleScene.Scene1, SceneCategory.Page},
                        {SampleScene.Scene2, SceneCategory.Page},
                    };
                }
            }

            public IDictionary<SamplePage, SampleScene[]> PageMap
            {
                get
                {
                    return new Dictionary<SamplePage, SampleScene[]>()
                    {
                        {SamplePage.Page1, new[] {SampleScene.Scene1}},
                        {SamplePage.Page2, new[] {SampleScene.Scene2}},
                    };
                }
            }
        }

        class SampleReducer : PageDuck<SamplePage, SampleScene>.Reducer
        {
            public SampleReducer(ISceneConfig<SampleScene, SamplePage> config) : base(config)
            {
            }

            public override object ReduceAny(object state, object action)
            {
                return state;
            }
        }

        class SamplePageData : IPageData
        {
            public int Value;

            public SamplePageData(int value)
            {
                this.Value = value;
            }

            public override bool Equals(object obj)
            {
                if (obj is SamplePageData)
                {
                    var target = (SamplePageData) obj;
                    return this.Value.Equals(target.Value);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}