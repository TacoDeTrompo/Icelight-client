using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Client
{
    public class UserProfile
    {
        public int idUser { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public Image photo { get; set; }
        public int status { get; set; }
        public int points { get; set; }
        private string pathImage;

        public UserProfile(int idUser, string name, string username, string email, int status, int points)
        {
            pathImage = null;
            this.idUser = idUser;
            this.name = name;
            this.username = username;
            this.email = email;
            this.status = status;
            this.points = points;
        }

    }
}
