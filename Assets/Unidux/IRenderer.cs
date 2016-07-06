namespace Unidux
{
    public delegate void Renderer<S>(S state) where S : StateBase<S>;
}
