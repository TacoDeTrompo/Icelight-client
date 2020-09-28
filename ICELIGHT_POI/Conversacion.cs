using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICELIGHT_POI
{
    class Conversacion
    {
        static List<int> allConvo = new List<int>();
        public int id;
        public Usuario mainUser;
        public List<Usuario> usuarios;
        public List<Mensaje> mensajes;
        
        public Conversacion(Usuario MainUser, Usuario user2)
        {
            //Cargar las Cosas
            usuarios = new List<Usuario>();
            mensajes = new List<Mensaje>();
            mainUser = MainUser;
            //Esto talvez tenga que se cambiado por una mejor forma de asignar ID en caso de 
            //que las conversaciones necesiten una id Constante Atravez de sesiones 
            
            id = allConvo.Count;
            allConvo.Add(id);

            usuarios.Add(user2);
        }
        public Conversacion(Usuario MainUser, int id)
        {
            usuarios = new List<Usuario>();
            mensajes = new List<Mensaje>();
            this.id = id;
            mainUser = MainUser;
        }
        public void AddUsuario(Usuario usernew)
        {
            //AddUsuario a la convo
            usuarios.Add(usernew);
        }
        public void addMensaje(Mensaje newMen)
        {
            mensajes.Add(newMen);
        }

        internal string GetNameUsuarios()
        {
            string res = "";

            res += usuarios[0].GetNombreUsuario();
            for (int i = 1; i < usuarios.Count; i++)
            {
                res += "," + usuarios[i].GetNombreUsuario();
            }
            return res;
        }

        //    internal statusContacto getStatus()
        //    {

        //        if (usuarios.Count > 1)
        //        {
        //            bool conectado = false;
        //            bool ausente = false;
        //            foreach (Usuario us in usuarios)
        //            {
        //                if (us.getStatus() == statusContacto.conectado)
        //                    conectado = true;
        //                else if (us.getStatus() == statusContacto.ausente)
        //                    ausente = true;
        //            }
        //            //return usuarios.get
        //            if (conectado)
        //            {
        //                return statusContacto.conectado;
        //            }
        //            else if (ausente)
        //            {
        //                return statusContacto.ausente;
        //            }
        //            else return statusContacto.desconectado;
        //        }
        //        else
        //        {
        //            return usuarios[0].getStatus();
        //        }

        //    }
    }

}
