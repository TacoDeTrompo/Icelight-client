using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICELIGHT_POI
{
    class Mensaje
    {
        private string textoMensaje;
        private DateTime fechaHora;
        private Usuario usuario;

        public int idUsuario;

        private List<string> emoticonos; 

        public string GettxtMenssage()
        { return textoMensaje; }

        public void SettxtMenssage(string value)
        { textoMensaje = value; }
        
        //public DateTime FechaHora => fechaHora;
        public DateTime getFechaHora()
        {
            return fechaHora;
        }
        public Mensaje(int idEnv, string men)
        {
            fechaHora = DateTime.Now;
            idUsuario = idEnv;
            textoMensaje = men;
        }
        public Mensaje(int idEnv, string men, DateTime hora)
        {
            fechaHora = hora;
            idUsuario = idEnv;
            textoMensaje = men;
        }
        public Mensaje(Usuario usuEnv, string men, DateTime hora)
        {
            fechaHora = hora;
            usuario = usuEnv;
            textoMensaje = men;
        }
        public Mensaje(Usuario usuEnv, string men)
        {
            fechaHora = DateTime.Now;
            usuario = usuEnv;
            textoMensaje = men;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return base.ToString();
        }
        public Usuario getUsuario()
        {
            return usuario;
        }

        public List<string> getemoticonos()
        {
            return emoticonos;
        }

        public void setemoticonos(List<string> emo3)
        {
            emoticonos = emo3;
        }
    }
}
