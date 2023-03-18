using Better.UnityPatterns.Runtime.MediatorModule.Interfaces;

namespace Better.UnityPatterns.Runtime.MediatorModule.Models
{
    public class MediatorEventArgs
    {
        private bool _isUsed;

        public virtual bool IsUsed => _isUsed;

        public IMediatorComponent Sender { get; }

        public bool ShouldIgnoreSender { get; } = false;

        public static MediatorEventArgs CreateFrom(IMediatorComponent mediatorComponent)
        {
            return new MediatorEventArgs(mediatorComponent);
        }

        public MediatorEventArgs(IMediatorComponent sender)
        {
            Sender = sender;
        }

        public MediatorEventArgs(IMediatorComponent sender, bool shouldIgnoreSender) : this(sender)
        {
            ShouldIgnoreSender = shouldIgnoreSender;
        }

        public virtual void Use()
        {
            _isUsed = true;
        }
    }
}