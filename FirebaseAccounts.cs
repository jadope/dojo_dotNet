using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dojo_dotNet.Models;

namespace dojo_dotNet
{
    public class FirebaseAccounts
    {
        private readonly static FirebaseAccounts _instancia = new FirebaseAccounts();
        FirestoreDb _db;
        Usuario user = new Usuario();
        public FirebaseAccounts()

        {
            String path = AppDomain.CurrentDomain.BaseDirectory + @"Firebase-SDK.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            _db = FirestoreDb.Create("dojo-net-core");
            Console.WriteLine("Se conectó correctamente");
        }

        public static FirebaseAccounts Instancia
        {
            get
            {
                return _instancia;
            }

        }
        public async Task<string> AddUser(Usuario user){
            DocumentReference coll = _db.Collection("Usuarios").Document();
            Dictionary <String,object> data = new Dictionary<string, object>()
            {
                {"Cédula",user.Cedula},
                {"Nombre",user.Nombre},
                {"Correo",user.Correo},
                {"Carrera",user.Carrera}
            };
            await coll.SetAsync(data);
            return "Usuario guarado con ID: "+coll.Id;
        }
        public async Task<List<Usuario>> GetUser()
        {
            CollectionReference userRef = _db.Collection("Usuarios");
            QuerySnapshot queryUser = await userRef.GetSnapshotAsync();
            List<Usuario> userList = new List<Usuario>();
            foreach(DocumentSnapshot documentSnapshot in queryUser.Documents)
            {
                Dictionary<String,Object> usuario = documentSnapshot.ToDictionary();
                Usuario user = new Usuario();
                foreach (var item in usuario)
                {
                    if(item.Key == "Nombre")
                    {
                        user.Nombre = (String)item.Value;
                    } else if (item.Key=="Cedula"){
                        user.Cedula = (String)item.Value;
                    }
                    else if (item.Key=="Correo"){
                        user.Correo = (String)item.Value;
                    }
                    else if (item.Key=="Carrera"){
                        user.Carrera = (String)item.Value;
                    }
                }
                userList.Add(user);
            }
            return userList;
        }
        public async Task<String> DeleteUser (String id)
        {
            DocumentReference userDelete = _db.Collection("Usuarios").Document(id);
            await userDelete.DeleteAsync();
            return "Usario con Id " +id+" eliminado correctamente";
        }
    }
}