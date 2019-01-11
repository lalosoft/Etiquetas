using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Etiquetadora
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] datos = new string[24];

        private void Form1_Load(object sender, EventArgs e)
        {
            lbl_fecha.Text = DateTime.Now.ToString("dd-MM-yyyy");
            this.ActiveControl = txt_NumFact;
            cmb_Opcion.SelectedIndex = 0;
        }

        private void btn_Buscar_Click(object sender, EventArgs e)
        {
            reiniciaCuentas();

            Conexion con = new Conexion();
            SqlConnection my_con = con.getConexion();
            if (my_con == null) MessageBox.Show("No se pudo conectar a la Base de Datos", "Fallo de Conexion", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else
            {
                if (txt_NumFact.Text.Equals("")) MessageBox.Show("Ingrese el numero de Factura", "Número de Factura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    Reporte etiq = new Reporte(datos);
                    etiq.borraArchivos();
                    buscarFactura();
                    buscaCliente();
                    buscaAgente();
                    buscaRuta();

                    txt_hiel.Focus();
                }
            }
        }

        public void buscarFactura()
        {
            string q = "";
            switch(cmb_Opcion.SelectedIndex)
            {
                case 0: /* *** FACTURAS *** */ 
                    q = "SELECT TIP_DOC, CVE_DOC, CVE_CLPV, CVE_PEDI, CVE_VEND FROM FACTF01 WHERE CVE_DOC LIKE '%" + txt_NumFact.Text.Trim() + "%'";

                    break;

                case 1: /* *** REMISIONES *** */
                    q = "SELECT TIP_DOC, CVE_DOC, CVE_CLPV, CVE_PEDI, CVE_VEND FROM FACTR01 WHERE CVE_DOC LIKE '%" + txt_NumFact.Text.Trim() + "%'";

                    break;

                case 2: /* *** DEVOLUCIONES *** */
                    q = "SELECT TIP_DOC, CVE_DOC, CVE_CLPV, CVE_PEDI, CVE_VEND FROM FACTD01 WHERE CVE_DOC LIKE '%" + txt_NumFact.Text.Trim() + "%'";

                    break;

                case 3: /* *** NOTAS DE VENTA *** */
                    q = "SELECT TIP_DOC, CVE_DOC, CVE_CLPV, CVE_PEDI, CVE_VEND FROM FACTV01 WHERE CVE_DOC LIKE '%" + txt_NumFact.Text.Trim() + "%'";

                    break;
            }

            Conexion con = new Conexion();
            SqlConnection mi_conexion = con.getConexion();
            string tip_doc = "";

            try
            {
                mi_conexion.Open();
                SqlCommand cmd = new SqlCommand(q, mi_conexion);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tip_doc = dr.GetString(0).Trim();
                    lbl_Factura.Text = dr.GetString(1).Trim();
                    txt_CveCte.Text = dr.GetString(2);
                    txt_Pedido.Text = dr.GetString(3).Trim();
                    txt_CveAgnt.Text = dr.GetString(4).Trim();
                }
                dr.Close();
                mi_conexion.Close();

                if (tip_doc.Equals("F")) txt_Doc.Text = "FACTURA";
                else if (tip_doc.Equals("R")) txt_Doc.Text = "REMISION";
                else if (tip_doc.Equals("D")) txt_Doc.Text = "DEVOLUCION";
                else if (tip_doc.Equals("V")) txt_Doc.Text = "NOTA DE VENTA";
            }
            catch (Exception e) { }
        }

        public void buscaCliente()
        {
            string q = "SELECT NOMBRE, CALLE, NUMINT, NUMEXT, COLONIA, ESTADO, MUNICIPIO, CVE_ZONA FROM CLIE01 WHERE CLAVE = '" + txt_CveCte.Text + "'";
            Conexion con = new Conexion();
            SqlConnection mi_conexion = con.getConexion();

            try
            {
                mi_conexion.Open();
                SqlCommand cmd = new SqlCommand(q, mi_conexion);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    txt_NomClte.Text = dr.IsDBNull(0)? String.Empty : dr.GetString(0).Trim();
                    txt_Calle.Text = dr.IsDBNull(1) ? String.Empty : dr.GetString(1).Trim();
                    txt_NumInt.Text = dr.IsDBNull(2) ? String.Empty : dr.GetString(2).Trim();
                    txt_NumExt.Text = dr.IsDBNull(3) ? String.Empty : dr.GetString(3).Trim();
                    txt_Colonia.Text = dr.IsDBNull(4) ? String.Empty : dr.GetString(4).Trim();
                    txt_Edo.Text = dr.IsDBNull(5) ? String.Empty : dr.GetString(5).Trim();
                    txt_Mpo.Text = dr.IsDBNull(6) ? String.Empty : dr.GetString(6).Trim();
                    txt_CveZona.Text = dr.IsDBNull(7) ? String.Empty : dr.GetString(7).Trim();
                }
                dr.Close();
                mi_conexion.Close();

                getFcia();
            }
            catch (Exception e) { }
        }

        public void getFcia()
        {
            if (txt_CveCte.Text != String.Empty)
            {
                string q = "SELECT CAMPLIB3 FROM CLIE_CLIB01 WHERE CVE_CLIE = '" + txt_CveCte.Text + "'";
                Conexion con = new Conexion();
                SqlConnection mi_conexion = con.getConexion();

                try
                {
                    mi_conexion.Open();
                    SqlCommand cmd = new SqlCommand(q, mi_conexion);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while(dr.Read())
                    {
                        txt_Fcia.Text = dr.IsDBNull(0) ? String.Empty : dr.GetString(0).Trim();
                    }
                    dr.Close();
                    mi_conexion.Close();
                }
                catch (Exception e) { }
            }
        }

        public void buscaAgente()
        {
            string q = "";
            if (txt_CveAgnt.Text.Equals(""))
                q = "SELECT NOMBRE FROM VEND01 WHERE CVE_VEND = 1";

            else q = "SELECT NOMBRE FROM VEND01 WHERE CVE_VEND = " + int.Parse(txt_CveAgnt.Text);

            Conexion con = new Conexion();
            SqlConnection mi_conexion = con.getConexion();

            try
            {
                mi_conexion.Open();
                SqlCommand cmd = new SqlCommand(q, mi_conexion);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    txt_NomAgnt.Text = dr.IsDBNull(0) ? String.Empty : dr.GetString(0).Trim();
                }
                dr.Close();
                mi_conexion.Close();
            }
            catch (Exception e) { }
        }

        public void buscaRuta()
        {
            string q = "";

            if(txt_CveZona.Text.Equals(""))
                q = "SELECT TEXTO FROM ZONA01 WHERE CVE_ZONA = 1";

            else q = "SELECT TEXTO FROM ZONA01 WHERE CVE_ZONA = " + int.Parse(txt_CveZona.Text);

            Conexion con = new Conexion();
            SqlConnection mi_conexion = con.getConexion();

            try
            {
                mi_conexion.Open();
                SqlCommand cmd = new SqlCommand(q, mi_conexion);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    txt_NomRuta.Text = dr.IsDBNull(0) ? String.Empty : dr.GetString(0).Trim();
                }
                dr.Close();
                mi_conexion.Close();
            }
            catch (Exception e) { }
        }

        public void getDatos()
        {
            datos[0] = txt_Pedido.Text.Trim();
            string fact = lbl_Factura.Text.Trim();

            if (fact.Contains("A"))
                fact = fact.Replace("A", "");

            else if(fact.Contains("NV"))
                fact = fact.Replace("NV", "");
            
            else if(fact.Contains("NC"))
                fact = fact.Replace("NC", "");

            datos[1] = txt_NumFact.Text;//fact;
            datos[2] = txt_NomAgnt.Text.Trim();
            datos[3] = txt_NomRuta.Text.Trim();
            datos[4] = txt_CveCte.Text.Trim();
            datos[5] = txt_NomClte.Text.Trim();
            datos[6] = txt_Calle.Text.Trim();
            datos[7] = txt_NumInt.Text.Trim();
            datos[8] = txt_NumExt.Text.Trim();
            datos[9] = txt_Colonia.Text.Trim();
            datos[10] = txt_Mpo.Text.Trim() + ", " + txt_Edo.Text.Trim();
            datos[11] = txt_Lab.Text.Trim();
            datos[12] = txt_Cja00.Text.Trim();
            datos[13] = txt_Cja01.Text.Trim();
            datos[14] = txt_Cja02.Text.Trim();
            datos[15] = txt_Cja03.Text.Trim();
            datos[16] = txt_Cja04.Text.Trim();
            datos[17] = txt_Total.Text.Trim();
            datos[18] = lbl_fecha.Text.Trim();
            datos[19] = txt_Dcto.Text.Trim();
            datos[20] = txt_Empaq.Text.Trim();

            if (check_fcia.Checked) datos[21] = txt_Fcia.Text.Trim();
            else datos[21] = txt_Fcia.Text = "";

            datos[22] = txt_Obs.Text;
            datos[23] = txt_hiel.Text;
        }

        public void reiniciaCuentas()
        {
            txt_hiel.Text = "00";
            txt_Lab.Text = "00";
            txt_Cja00.Text = "00";
            txt_Cja01.Text = "00";
            txt_Cja02.Text = "00";
            txt_Cja03.Text = "00";
            txt_Cja04.Text = "00";
            txt_Total.Text = "00";

            txt_CveCte.Text = "";
            txt_NomClte.Text = "";
            txt_Calle.Text = "";
            txt_NumInt.Text = "";
            txt_NumExt.Text = "";
            txt_Colonia.Text = "";
            txt_Fcia.Text = "";
            txt_Mpo.Text = "";
            txt_Edo.Text = "";
            txt_CveAgnt.Text = "";
            txt_NomAgnt.Text = "";
            txt_CveZona.Text = "";
            txt_NomRuta.Text = "";
            txt_Obs.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getDatos();
            Reporte etiq = new Reporte(datos);
            reiniciaCuentas();
            if (!etiq.generaEtiqueta()) MessageBox.Show("NO SE PUDO CREAR");
        }

        public void sumas()
        {
            try
            {
                int hiel = int.Parse(txt_hiel.Text.Trim());
                int lab = int.Parse(txt_Lab.Text.Trim());
                int cja0 = int.Parse(txt_Cja00.Text.Trim());
                int cja1 = int.Parse(txt_Cja01.Text.Trim());
                int cja2 = int.Parse(txt_Cja02.Text.Trim());
                int cja3 = int.Parse(txt_Cja03.Text.Trim());
                int cja4 = int.Parse(txt_Cja04.Text.Trim());

                int total = hiel + lab + cja0 + cja1 + cja2 + cja3 + cja4;
                txt_Total.Text = total.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("INGRESE SOLO NUMEROS", "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_hiel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                sumas();
                txt_Lab.Focus();
            }
        }

        private void txt_Lab_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                sumas();
                txt_Cja00.Focus();
            }
        }

        private void txt_Cja00_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                sumas();
                txt_Cja01.Focus();
            }
        }

        private void txt_Cja01_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                sumas();
                txt_Cja02.Focus();
            }
        }

        private void txt_Cja02_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                sumas();
                txt_Cja03.Focus();
            }
        }

        private void txt_Cja03_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                sumas();
                txt_Cja04.Focus();
            }
        }

        private void txt_Cja04_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                sumas();
                txt_Dcto.Focus();
            }
        }

        private void txt_NumFact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((int)e.KeyChar == (int)Keys.Enter)
            {
                reiniciaCuentas();

                Conexion con = new Conexion();
                SqlConnection my_con = con.getConexion();
                if (my_con == null) MessageBox.Show("No se pudo conectar a la Base de Datos", "Fallo de Conexion", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                {
                    if (txt_NumFact.Text.Equals("")) MessageBox.Show("Ingrese el numero de Factura", "Número de Factura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        Reporte etiq = new Reporte(datos);
                        etiq.borraArchivos();
                        buscarFactura();
                        buscaCliente();
                        buscaAgente();
                        buscaRuta();

                        txt_hiel.Focus();
                    }
                }
            }
        }

        private void txt_Dcto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                txt_Empaq.Focus();
            }
        }

        private void txt_Total_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                sumas();
                txt_Dcto.Focus();
            }
        }

        private void txt_Empaq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                txt_Obs.Focus();
            }
        }
    }
}