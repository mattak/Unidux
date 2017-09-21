using System.Collections.Generic;
using NUnit.Framework;

namespace Unidux.SceneTransition
{
    public class SceneConfigTest
    {
        [Test]
        public void GetPageScenesTest()
        {
            var config = new SampleConfig();
            Assert.AreEqual(new[]
            {
                SampleScene.PageScene1,
                SampleScene.PageScene2,
            }, config.GetPageScenes());
        }

        enum SampleScene
        {
            PageScene1,
            PageScene2,
            ModalScene1,
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
                        {SampleScene.PageScene1, SceneCategory.Page},
                        {SampleScene.PageScene2, SceneCategory.Page},
                        {SampleScene.ModalScene1, SceneCategory.Modal},
                    };
                }
            }

            public IDictionary<SamplePage, SampleScene[]> PageMap
            {
                get
                {
                    return new Dictionary<SamplePage, SampleScene[]>()
                    {
                        {SamplePage.Page1, new[] {SampleScene.PageScene1}},
                        {SamplePage.Page2, new[] {SampleScene.PageScene2}},
                    };
                }
            }
        }
    }
}