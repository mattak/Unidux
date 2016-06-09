using System;
using UnityEngine;

namespace Unidux
{
    public class UniduxSubscriber : UniduxSubscriberBase
    {
        void OnEnable()
        {
            this._reduceSubscriber(true);
            this._reduceSubscriber(true);
        }

        void OnDisable()
        {
            this._reduceSubscriber(false);
            this._reduceSubscriber(false);
        }
    }
}
