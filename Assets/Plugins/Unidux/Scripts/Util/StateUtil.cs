using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Unidux.Util
{
    public static class StateUtil
    {
        public static bool ApplyStateChanged(IStateChanged oldState, IStateChanged newState)
        {
            if (oldState == null || newState == null)
            {
                return oldState != null || newState != null;
            }

            bool stateChanged = false;

            var properties = newState.GetType().GetProperties();
            foreach (var property in properties)
            {
                var newValue = property.GetValue(newState, null);
                var oldValue = property.GetValue(oldState, null);

                if (newValue is IStateChanged)
                {
                    stateChanged |= ApplyStateChanged((IStateChanged) oldValue, (IStateChanged) newValue);
                }
                else if (newValue == null && oldValue != null || newValue != null && !newValue.Equals(oldValue))
                {
                    stateChanged = true;
                }
            }

            var fields = newState.GetType().GetFields();
            foreach (var field in fields)
            {
                var newValue = field.GetValue(newState);
                var oldValue = field.GetValue(oldState);

                if (newValue is IStateChanged)
                {
                    stateChanged |= ApplyStateChanged((IStateChanged) oldValue, (IStateChanged) newValue);
                }
                else if (newValue == null && oldValue != null || newValue != null && !newValue.Equals(oldValue))
                {
                    stateChanged = true;
                }
            }

            if (stateChanged)
            {
                newState.SetStateChanged();
            }

            return stateChanged;
        }

        public static void ResetStateChanged(IStateChanged state)
        {
            if (state != null)
            {
                state.SetStateChanged(false);
            }

            var properties = state.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(state, null);

                if (value != null && value is IStateChanged)
                {
                    var changedValue = (IStateChanged) value;
                    changedValue.SetStateChanged(false);
                }
            }

            var fields = state.GetType().GetFields();
            foreach (var field in fields)
            {
                var value = field.GetValue(state);
                if (value != null && value is IStateChanged)
                {
                    var changedValue = (IStateChanged) value;
                    changedValue.SetStateChanged(false);
                }
            }
        }

        public static object MemoryClone(object clonee)
        {
            return MemoryClone(clonee, CreateDefaultSurrogateSelector());
        }

        public static object MemoryClone(object clonee, SurrogateSelector selector)
        {
            object result;
            IFormatter formatter = new BinaryFormatter();
            formatter.SurrogateSelector = selector;

            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(stream, clonee);
                    stream.Position = 0;
                    result = formatter.Deserialize(stream);
                }
                finally
                {
                    stream.Close();
                }
            }

            return result;
        }

        public static SurrogateSelector CreateDefaultSurrogateSelector()
        {
            SurrogateSelector selector = new SurrogateSelector();
            selector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All),
                new Vector2SerializationSurrogate());
            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All),
                new Vector3SerializationSurrogate());
            selector.AddSurrogate(typeof(Vector4), new StreamingContext(StreamingContextStates.All),
                new Vector4SerializationSurrogate());
            selector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All),
                new ColorSerializationSurrogate());
            return selector;
        }
    }
}