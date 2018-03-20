using System;
using Shaman.Common.Extension;
using Shaman.Common.Messages;

namespace Shaman.Common.Exc
{
    internal class HandlerOfUserException : IExceptionHandler
    {
        private readonly IMessenger _messenger;

        public HandlerOfUserException(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public bool Handle(Exception exception)
        {
            if (exception is UserException ue)
            {
                _messenger.Warn(ue.GetMessage());
                return true;
            }

            return false;
        }
    }
}