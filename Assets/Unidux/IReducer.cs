namespace Unidux
{
    public delegate S Reducer<S, in A>(S state, A action) where S : StateBase<S>;
}
