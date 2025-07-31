using System;

namespace CSharpTools.HTTP.Architecture
{
    public abstract class ErrorHandler
    {
        public event Action<string> OnError;

        protected abstract void LogError(string error);

        public void HandleError(string error)
        {
            OnError?.Invoke(error);
            LogError(error);
        }
    }
}