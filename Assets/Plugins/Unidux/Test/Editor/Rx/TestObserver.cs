using System;
using System.Collections.Generic;
using UniRx;

namespace Unidux.Rx
{
    public class TestObserver<TValue> : IObserver<TValue>
    {
        public bool IsOnNext = false;
        public bool IsOnError = false;
        public bool IsOnCompleted = false;
        public int OnNextCount = 0;
        public int OnErrorCount = 0;
        public int OnCompletedCount = 0;
        public List<TValue> OnNextValues = new List<TValue>();
        public Exception OnErrorValue = null;
        
        public void OnCompleted()
        {
            this.IsOnCompleted = true;
            this.OnCompletedCount++;
        }

        public void OnError(Exception error)
        {
            this.IsOnError = true;
            this.OnErrorCount++;
            this.OnErrorValue = error;
        }

        public void OnNext(TValue value)
        {
            this.IsOnNext = true;
            this.OnNextCount++;
            this.OnNextValues.Add(value);
        }
    }
}