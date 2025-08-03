using System;

namespace Glasses
{
    public class Glass : PhysicsCarry
    {
        public IGlassContent Content { get; protected set; }
        public bool IsEmpty => Content == null;

        public event Action<IGlassContent> GlassContentSetEvent;
        public event Action<IGlassContent> GlassContentClearEvent;
        
        public bool TrySetContent(IGlassContent content)
        {
            if (!IsEmpty)
                return false;
            
            Content = content;
            GlassContentSetEvent?.Invoke(Content);
            return true;
        }

        public void ClearGlass()
        {
            GlassContentClearEvent?.Invoke(Content);
            Content = null;
        }
    }

    public interface IGlassContent
    {
    }
}
