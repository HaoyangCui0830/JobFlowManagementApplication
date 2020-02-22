using System;
namespace SWEN90013.Models
{
    public class Login
    {
        public string userName { get; set; }
        public string password { get; set; }
        public Login(string name, string pwd)
        {
            userName = name;
            password = pwd;
        }
    }
}
