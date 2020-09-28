using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ICELIGHT_POI
{
    class Usuario
    {
        public string nombreUsuario;
        private int idUsuario;
        private string email;
        string contrasena;
        private Image imagenPerfil;
        private string pathImage;
        private int points;
        //private statusContacto status;

        public Usuario(int idUsu, string nombreUsu)
        {
            pathImage = null;
            idUsuario = idUsu;
            //imagenPerfil = Image.FromFile("ImagenDefault.png");
            nombreUsuario = nombreUsu;            
        }
        public Usuario(int idUsu,string nombreUsu,string pathimagen)
        {
            this.pathImage = pathImage;
            if (File.Exists(pathImage))
                imagenPerfil = Image.FromFile(pathimagen);
            else
                imagenPerfil = null;
            nombreUsuario = nombreUsu;            
        }
        public Image GetImagenPerfil() { return imagenPerfil;}
        public string GetNombreUsuario() { return nombreUsuario;}
        public int GetIdUsuario() {return idUsuario;}
        public string GetEmail()
        {
            return email;
        }
        public void setContrasenia(string contra)
        {
            contrasena = contra;
        }
        public string GetNameImagenPerfil()
        {
            return pathImage;
        }
        public void SetEmail(string Email)
        {
            email = Email;
        }
        public void setPoints(int points)
        {
            this.points = points;
        }
        public int getPoints()
        {
            return points;
        }
        public void AddPoints(int points)
        {
            this.points += points;
        }
        //public void setStatus(statusContacto status)
        //{
        //    this.status = status;
        //}
        //public statusContacto getStatus()
        //{
        //    return status;
        //}
    }
}
