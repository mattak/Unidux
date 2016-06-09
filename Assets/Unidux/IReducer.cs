namespace Unidux
{
    // public delegate S Reducer<S>(S state, object action) where S : StateBase;
    public delegate S Reducer<S, in A>(S state, A action) where S : StateBase;
}
