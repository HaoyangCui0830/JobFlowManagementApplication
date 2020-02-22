using System;
namespace SWEN90013.Models
{
    public class User
    {
        public int userID { get; set; }
        public string userCode { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string passwordHash { get; set; }
        public string passwordSalt { get; set; }
        public string token { get; set; }
        public DateTime registerDate { get; set; }
        public DateTime lastLoginDate { get; set; }


        public string getUserName()
        {
            return this.userName;
        }

        public string getUserCode()
        {
            return this.userCode;
        }

        public string getToken()
        {
            return this.token;
        }
    }
}
