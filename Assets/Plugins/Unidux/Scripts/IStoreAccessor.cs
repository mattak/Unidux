using System;

namespace Unidux
{
    public interface IStoreAccessor
    {
        object StateObject { get; set; }
        Type StateType { get; }
    }
}