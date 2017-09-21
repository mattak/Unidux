using NUnit.Framework;

namespace Unidux.SceneTransition
{
    public class SceneStateTest
    {
        SceneState<SampleScene> state = new SceneState<SampleScene>();

        [SetUp]
        public void Before()
        {
            this.state = new SceneState<SampleScene>();
        }

        [Test]
        public void NeedsAdjustTest()
        {
            var allScenes = new[] {SampleScene.Sample1, SampleScene.Sample2};
            state.ActiveMap[SampleScene.Sample1] = false;
            state.ActiveMap[SampleScene.Sample2] = false;

            Assert.IsTrue(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample1, SampleScene.Sample2}));
            Assert.IsTrue(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample1}));
            Assert.IsTrue(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample2}));
            Assert.IsFalse(state.NeedsAdjust(allScenes, new SampleScene[] { }));

            state.ActiveMap[SampleScene.Sample1] = true;
            state.ActiveMap[SampleScene.Sample2] = false;

            Assert.IsTrue(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample1, SampleScene.Sample2}));
            Assert.IsFalse(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample1}));
            Assert.IsTrue(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample2}));
            Assert.IsTrue(state.NeedsAdjust(allScenes, new SampleScene[] { }));

            state.ActiveMap[SampleScene.Sample1] = true;
            state.ActiveMap[SampleScene.Sample2] = true;

            Assert.IsFalse(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample1, SampleScene.Sample2}));
            Assert.IsTrue(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample1}));
            Assert.IsTrue(state.NeedsAdjust(allScenes, new[] {SampleScene.Sample2}));
            Assert.IsTrue(state.NeedsAdjust(allScenes, new SampleScene[] { }));
        }

        [Test]
        public void AdditionalsTest()
        {
            state.ActiveMap[SampleScene.Sample1] = false;
            state.ActiveMap[SampleScene.Sample2] = false;
            Assert.AreEqual(new SampleScene[] { }, state.Additionals(new SampleScene[] { }));

            state.ActiveMap[SampleScene.Sample1] = true;
            state.ActiveMap[SampleScene.Sample2] = false;
            Assert.AreEqual(
                new SampleScene[] {SampleScene.Sample1},
                state.Additionals(new SampleScene[] { })
            );
            Assert.AreEqual(
                new SampleScene[] { },
                state.Additionals(new SampleScene[] {SampleScene.Sample1})
            );

            state.ActiveMap[SampleScene.Sample1] = true;
            state.ActiveMap[SampleScene.Sample2] = true;
            Assert.AreEqual(
                new SampleScene[] {SampleScene.Sample1, SampleScene.Sample2},
                state.Additionals(new SampleScene[] { })
            );
            Assert.AreEqual(
                new SampleScene[] {SampleScene.Sample2},
                state.Additionals(new SampleScene[] {SampleScene.Sample1})
            );
            Assert.AreEqual(
                new SampleScene[] { },
                state.Additionals(new SampleScene[] {SampleScene.Sample1, SampleScene.Sample2})
            );
        }

        [Test]
        public void RemovalsTest()
        {
            state.ActiveMap[SampleScene.Sample1] = false;
            state.ActiveMap[SampleScene.Sample2] = false;
            Assert.AreEqual(new SampleScene[] { }, state.Removals(new SampleScene[] { }));
            Assert.AreEqual(new SampleScene[] {SampleScene.Sample1,},
                state.Removals(new SampleScene[] {SampleScene.Sample1,}));
            Assert.AreEqual(new SampleScene[] {SampleScene.Sample1, SampleScene.Sample2,},
                state.Removals(new SampleScene[] {SampleScene.Sample1, SampleScene.Sample2,}));

            state.ActiveMap[SampleScene.Sample1] = true;
            state.ActiveMap[SampleScene.Sample2] = false;
            Assert.AreEqual(
                new SampleScene[] { },
                state.Removals(new SampleScene[] {SampleScene.Sample1,})
            );
            Assert.AreEqual(
                new SampleScene[] {SampleScene.Sample2},
                state.Removals(new SampleScene[] {SampleScene.Sample1, SampleScene.Sample2})
            );

            state.ActiveMap[SampleScene.Sample1] = true;
            state.ActiveMap[SampleScene.Sample2] = true;
            Assert.AreEqual(
                new SampleScene[] { },
                state.Removals(new SampleScene[] {SampleScene.Sample1, SampleScene.Sample2})
            );
        }

        [Test]
        public void CloneTest()
        {
        }

        [Test]
        public void EqualsTest()
        {
        }

        enum SampleScene
        {
            Sample1,
            Sample2,
        }
    }
}