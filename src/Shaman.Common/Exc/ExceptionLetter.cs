using System;
using System.Globalization;
using System.Text;
using Microsoft.Win32;
using Shaman.Common.Env;
using Shaman.Common.Extension;
using Shaman.Common.Mail;

namespace Shaman.Common.Exc
{
    internal class ExceptionLetter : Letter
    {
        private static string _netFrameworkVersion;

        public ExceptionLetter(Exception exception)
        {
            var user = AppEnvironment.User ?? new User();

            From = new Receiver(user.Email, user.Name, user.Phone);
            To.Add(new Receiver(AppEnvironment.DevelopersMail));
            var dbType = AppEnvironment.Db.Type.ToString()[0];
            Subject = $"{AppEnvironment.AppName}. Ошибка [{dbType}]";

            var body = new StringBuilder();

            body.AppendLine("<pre>");
            body.Append($"<h2>{AppEnvironment.HumanAppName} v.{AppEnvironment.AppVersion} | БД: {AppEnvironment.Db.Id}</h2>");
            body.AppendLine($"Пользователь:  {user.Name} {user.Department}");
            body.AppendLine($"Контакты:      {user.Email} {user.Phone}");
            body.AppendLine($"PC / AD login: {user.PcName} {user.AdLogin}");
            body.AppendLine($"DB / DB login: {AppEnvironment.Db.TnsName} {user.DbLogin}");
            body.Append("<hr />");
            body.AppendLine($".Net Framework: {GetNetFrameworkVersion()}");
            body.AppendLine($"Memory:        {Environment.WorkingSet.ToString("N0", CultureInfo.CreateSpecificCulture("en-US"))} byte");
            body.Append("<hr />");
            body.AppendLine($"<b>Ошибка:</b>\n{exception.GetMessage()}");
            body.Append("<hr />");
            body.AppendLine($"<b>Стек вызовов:</b>\n{exception.GetStackTrace()}");
            body.Append("<hr />");
            body.Append($"<b>Данные:</b>\n{exception.GetExceptionData()}");
            body.Append("<hr /></pre>");

            Body = body.ToString();
        }

        private string GetNetFrameworkVersion()
        {
            if (_netFrameworkVersion != null)
            {
                return _netFrameworkVersion;
            }

            string result;

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                var releaseKey = (int)ndpKey.GetValue("Release");

                switch (releaseKey)
                {
                    //TODO add releaseKeys
                    default: result = "unknow version. Release key=" + releaseKey; break;
                }
            }

            _netFrameworkVersion = result;
            return _netFrameworkVersion;
        }
    }
}