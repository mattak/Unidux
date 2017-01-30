namespace Unidux
{
    public delegate void Renderer<TState>(TState state) where TState : StateBase<TState>;
}
