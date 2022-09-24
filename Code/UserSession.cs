using System;

namespace QLNS.Code
{
    [Serializable]
    public class UserSession
    {
        private string username;

        public string Get()
        {
            return username;
        }

        private void Set(string value)
        {
            username = value;
        }

        public UserSession(string Username)
        {
            this.Set(Username);
        }

    }
}