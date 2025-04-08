using System;

namespace SufniBrowser
{
    public class Credential
    {
        public string Website { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class CookieInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime? Expires { get; set; }
        public bool IsSecure { get; internal set; }
        public bool IsHttpOnly { get; internal set; }
    }
}
