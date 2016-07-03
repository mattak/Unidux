using UnityEngine;
using System.Collections;

namespace Unidux
{
    public interface IStateClone<T>
    {
        T Clone();
    }
}