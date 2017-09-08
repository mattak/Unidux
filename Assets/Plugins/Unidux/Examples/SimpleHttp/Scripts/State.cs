using System;

namespace Unidux.Example.SimpleHttp
{
    [Serializable]
    public partial class State : StateBase
    {
        public string Url = "";
        public int StatusCode = -1;
        public string Body = "";

        public override bool Equals(object obj)
        {
            if (obj is State)
            {
                var target = (State) obj;
                return
                    this.Url == target.Url &&
                    this.StatusCode == target.StatusCode &&
                    this.Body == target.Body;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}