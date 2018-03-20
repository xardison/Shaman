using Ninject.Modules;
using Shaman.Common.Exc;
using Shaman.Common.Mail;
using Shaman.Common.Messages;
using Splat;

namespace Shaman.Common
{
    public class DiModuleCommon : NinjectModule
    {
        public override void Load()
        {
            Bind<IExceptionHandlersManager>().To<ExceptionHandlersManager>().InSingletonScope();
            Bind<IObservableExceptionHandler>().To<ShamanObservableExceptionHandler>().InSingletonScope();
            Bind<IExceptionHandler>().To<HandlerOfUserException>();

            Rebind<ILogger>().ToConstant(new LogSplatAdapter());
            Bind<IMessenger>().To<Messenger>().InSingletonScope();
            Bind<IMailer>().To<Mailer>().When(request =>
            {
                var aqn = request.ParentRequest?.Service.AssemblyQualifiedName;
                return aqn != null && (aqn.StartsWith("Shaman.Common") || aqn.StartsWith("Shaman.Server"));
            }).InSingletonScope();
        }
    }
}