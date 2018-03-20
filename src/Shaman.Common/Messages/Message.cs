using System;
using Shaman.Common.Dialog;

namespace Shaman.Common.Messages
{
    public interface IMessage
    {
        MessageType Type { get; set; }
        string Body { get; set; }
    }

    public enum MessageType
    {
        Error,
        Warn,
        Info,
        Question,
        InputDialog
    }

    public class Message : IMessage
    {
        public Message(MessageType type, string body)
        {
            Type = type;
            Body = body;
        }

        public MessageType Type { get; set; }
        public string Body { get; set; }
    }

    public interface IQuestionMessage : IMessage
    {
        DialogButtons Buttons { get; }
        Action<DialogResult> Callback { get; }
    }

    public class QuestionMessage : Message, IQuestionMessage
    {
        public QuestionMessage(string body, DialogButtons buttons, Action<DialogResult> callback)
            : base(MessageType.Question, body)
        {
            Buttons = buttons;
            Callback = callback;
        }

        public DialogButtons Buttons { get; set; }
        public Action<DialogResult> Callback { get; set; }
    }
}
