using System;
using System.Reactive.Subjects;
using Shaman.Common.Dialog;

namespace Shaman.Common.Messages
{
    public interface IMessenger : IDisposable
    {
        IObservable<IMessage> Messages { get; }

        void Add(IMessage msg);
        void Add(MessageType type, string msg);
        void Error(string msg);
        void Warn(string msg);
        void Info(string msg);
        void Question(string msg, DialogButtons btns, Action<DialogResult> callback);
    }

    internal class Messenger : IMessenger
    {
        private readonly Subject<IMessage> _messages = new Subject<IMessage>();

        public IObservable<IMessage> Messages => _messages;

        public void Add(IMessage msg)
        {
            _messages.OnNext(msg);
        }

        public void Add(MessageType type, string msg)
        {
            Add(new Message(type, msg));
        }

        public void Error(string msg)
        {
            Add(new Message(MessageType.Error, msg));
        }

        public void Warn(string msg)
        {
            Add(new Message(MessageType.Warn, msg));
        }

        public void Info(string msg)
        {
            Add(new Message(MessageType.Info, msg));
        }

        public void Question(string msg, DialogButtons btns, Action<DialogResult> callback)
        {
            Add(new QuestionMessage(msg, btns, callback));
        }

        public void Dispose()
        {
            _messages.Dispose();
        }
    }
}