using System;
namespace FES
{
    public class FireFighter
    {
        private String username;
        private String password;
        private static FireFighter fireFighter;

        public FireFighter()
        {
        }

        public static FireFighter getFireFighter()
        {
            if (fireFighter == null)
            {
                fireFighter = new FireFighter();
            }
            return fireFighter;
        }

        public void setUsername(String username) {
            this.username = username;
        }

        public void setPassword(String password) {
            this.password = password;
        }

        public String getUsername() {
            return this.username;
        }

        public String getPassword() {
            return this.password;
        }
    }
}
