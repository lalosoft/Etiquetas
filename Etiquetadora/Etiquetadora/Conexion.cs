using System.Data.SqlClient;

namespace Etiquetadora
{
    class Conexion
    {
        //public string db = "direccion bd";

        public SqlConnection con;

        public Conexion()
        {
            con = null;
        }

        public SqlConnection getConexion()
        {
            con = new SqlConnection(db);
            return con;
        }
    }
}