using System.Collections.Generic;
using System.Linq;

namespace Shaman.Common.Env
{
    public enum DatabaseId : byte
    {
        Unknow = 0,
        Work = 1,
        Test = 2,
        Main = 3
    }

    public enum DatabaseType : byte
    {
        Unknow = 0,
        Dev = 1,
        Test = 2,
        Release = 3
    }

    public class Database
    {
        static Database()
        {
            DbList = new List<Database>
            {
                new Database(DatabaseId.Unknow, DatabaseType.Unknow, "Unknow", "Unknow TNS"),

                new Database(DatabaseId.Work, DatabaseType.Dev, "Work", "work.ora.vz"),
                new Database(DatabaseId.Test, DatabaseType.Test, "Test", "test.ora.vz"),
                new Database(DatabaseId.Main, DatabaseType.Release, "Main", "main.ora.vz"),
            };
        }

        public Database(DatabaseId id, DatabaseType type, string humanName, string tnsName)
        {
            Id = id;
            Type = type;
            HumanName = humanName;
            TnsName = tnsName;
        }

        public static IList<Database> DbList { get; }

        public DatabaseId Id { get; set; }
        public DatabaseType Type { get; set; }
        public string HumanName { get; set; }
        public string TnsName { get; set; }

        public static Database GetById(DatabaseId id)
        {
            return DbList.First(x => x.Id == id);
        }
    }
}