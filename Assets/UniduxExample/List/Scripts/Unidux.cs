namespace Unidux.Example.List
{
    public class Unidux : SingletonMonoBehaviour<Unidux>
    {
        public Store<State> Store { get; private set; }

        void Awake()
        {
            this.Store = new Store<State>();
            this.Store.AddReducer<ListAddAction>(ListReducer.Reduce);
        }

        void Update()
        {
            this.Store.Update();
        }
    }
}
