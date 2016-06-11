using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Unidux;

public class StoreTest
{
    [Test]
    public void RenderSubscribeTest()
    {
        var store = new Store<State>();
        var render = new SampleRender();
        store.RenderEvent += render.Render;

        store.Dispatch(new Action());
        store.ForceUpdate();
        Assert.AreEqual(1, render.Count);

        store.Dispatch(new Action());
        store.ForceUpdate();
        Assert.AreEqual(2, render.Count);

        store.RenderEvent -= render.Render;

        store.Dispatch(new Action());
        store.ForceUpdate();
        Assert.AreEqual(2, render.Count);
    }

    [Test]
    public void ReducerSubscribeTest()
    {
        var store = new Store<State>();
        var reducer = new SampleReducer();
        store.AddReducer<Action>(reducer.Reduce);

        store.Dispatch(new Action());
        Assert.AreEqual(1, reducer.Count);

        store.Dispatch(new Action());
        Assert.AreEqual(2, reducer.Count);

        store.RemoveReducer<Action>(reducer.Reduce);
        store.Dispatch(new Action());
        Assert.AreEqual(2, reducer.Count);
    }

    class State : StateBase
    {
    }

    class Action
    {
    }

    class SampleRender
    {
        public int Count = 0;
        public void Render(State state)
        {
            Count++;
        }
    }

    class SampleReducer
    {
        public int Count = 0;

        public State Reduce(State state, Action action)
        {
            Count++;
            return state;
        }
    }
}
