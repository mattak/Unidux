using NUnit.Framework;

namespace Unidux.SceneTransition
{
    public class SceneDuckTest
    {
        [Test]
        public void ResetTest()
        {
            var reducer = new SceneDuck<SampleScene>.Reducer();
            var state = new SceneState<SampleScene>();

            // Adjust
            {
                var result = reducer.Reduce(
                    state,
                    SceneDuck<SampleScene>.ActionCreator.Adjust(new[] {SampleScene.Scene1})
                );
                Assert.AreEqual(2, result.ActiveMap.Count);
                Assert.IsTrue(result.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(result.ActiveMap[SampleScene.Scene2]);
            }
            
            // Add
            {
                var result = reducer.Reduce(
                    state,
                    SceneDuck<SampleScene>.ActionCreator.Add(new[] {SampleScene.Scene1})
                );
                Assert.AreEqual(2, result.ActiveMap.Count);
                Assert.IsTrue(result.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(result.ActiveMap[SampleScene.Scene2]);
                
                result = reducer.Reduce(
                    state,
                    SceneDuck<SampleScene>.ActionCreator.Add(new[] {SampleScene.Scene2})
                );
                Assert.AreEqual(2, result.ActiveMap.Count);
                Assert.IsTrue(result.ActiveMap[SampleScene.Scene1]);
                Assert.IsTrue(result.ActiveMap[SampleScene.Scene2]);
            }
            
            // Remove
            {
                var result = reducer.Reduce(
                    state,
                    SceneDuck<SampleScene>.ActionCreator.Remove(new[] {SampleScene.Scene1})
                );
                Assert.AreEqual(2, result.ActiveMap.Count);
                Assert.IsFalse(result.ActiveMap[SampleScene.Scene1]);
                Assert.IsTrue(result.ActiveMap[SampleScene.Scene2]);
                
                result = reducer.Reduce(
                    state,
                    SceneDuck<SampleScene>.ActionCreator.Remove(new[] {SampleScene.Scene2})
                );
                Assert.AreEqual(2, result.ActiveMap.Count);
                Assert.IsFalse(result.ActiveMap[SampleScene.Scene1]);
                Assert.IsFalse(result.ActiveMap[SampleScene.Scene2]);
            }
        }

        enum SampleScene
        {
            Scene1,
            Scene2,
        }
    }
}