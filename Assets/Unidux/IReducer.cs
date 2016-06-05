namespace Unidux
{
    public delegate S Reducer<S>(S state, object action) where S : StateBase;
}
