namespace Shaman.Common.Mail
{
    public class Receiver
    {
        public Receiver(string email) : this(email, null)
        {
        }

        public Receiver(string email, string name, string phone = null)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }

        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }

        public override string ToString()
        {
            return $"{Name} <{Email}>";
        }
    }
}