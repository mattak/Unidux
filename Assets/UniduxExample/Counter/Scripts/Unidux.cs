namespace Unidux.Example.Counter
{
    public class Unidux : SingletonMonoBehaviour<Unidux>
    {
        public Store<State> Store { get; private set; }

        void Awake()
        {
            this.Store = new Store<State>();
            this.Store.AddReducer<CountAction>(CountReducer.Reduce);
        }

        void Update()
        {
            this.Store.Update();
        }
    }
}
