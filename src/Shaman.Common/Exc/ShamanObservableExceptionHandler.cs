using System;
using System.Diagnostics;
using System.Net.Mail;
using Shaman.Common.Extension;
using Shaman.Common.Mail;
using Shaman.Common.Messages;

namespace Shaman.Common.Exc
{
    public interface IObservableExceptionHandler : IObserver<Exception>
    {
        void SetDefaultMethodToShowError(Action<Exception> action);
    }

    internal class ShamanObservableExceptionHandler : IObservableExceptionHandler
    {
        private readonly IExceptionHandlersManager _exceptionHandlersManager;
        private readonly IScreenCapture _screenCapture;
        private readonly IMessenger _messenger;
        private readonly IMailer _mailer;
        private Action<Exception> _defaultMethodToShowError;

        public ShamanObservableExceptionHandler(
            IExceptionHandlersManager exceptionHandlersManager,
            IScreenCapture screenCapture,
            IMessenger messenger,
            IMailer mailer)
        {
            _exceptionHandlersManager = exceptionHandlersManager;
            _screenCapture = screenCapture;
            _messenger = messenger;
            _mailer = mailer;
        }

        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();

            OnException(value);
            //RxApp.MainThreadScheduler.Schedule(() => { throw value; });
        }

        public void OnError(Exception error)
        {
            if (Debugger.IsAttached) Debugger.Break();

            OnException(error);
            //RxApp.MainThreadScheduler.Schedule(() => { throw error; });
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
            //RxApp.MainThreadScheduler.Schedule(() => { throw new NotImplementedException(); });
        }

        public void SetDefaultMethodToShowError(Action<Exception> action)
        {
            _defaultMethodToShowError = action;
        }

        private void OnException(Exception exception)
        {
            try
            {
                if (!_exceptionHandlersManager.Handle(exception))
                {
                    ShowException(exception);
                }
            }
            catch (Exception innerExc)
            {
                if (Debugger.IsAttached) Debugger.Break();

                var agrExc = new AggregateException(
                    "Возникла ошибка при обработке уже имеющегося исключентя. Детали обоих исключений прилогаются",
                    innerExc,
                    exception);

                if (_defaultMethodToShowError != null)
                {
                    _defaultMethodToShowError(agrExc);
                    return;
                }

                if (Debugger.IsAttached)
                {
                    Debug.Print("\n=====================================\n" +
                                agrExc.GetMessage() +
                                "=====================================\n");
                }
            }
        }

        private void ShowException(Exception exception)
        {
            Log.Error(exception, "Упс!");
            _mailer.SendMail(GetErrorMail(exception));
            _messenger.Error(exception.GetMessage());
        }

        private Letter GetErrorMail(Exception exception)
        {
            var letter = new ExceptionLetter(exception);
            letter.Attachments.Add(new Attachment(_screenCapture.CaptureScreenToStream(), "screen.jpg", "image/jpeg"));
            return letter;
        }
    }
}