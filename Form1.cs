using pryInicioSesionLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryClase27_05
{
    public partial class frmVentanaPrincipal : Form
    {
        public frmVentanaPrincipal()
        {
            InitializeComponent();
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtUsuario.Text;
            string contraseña = txtContraseña.Text;

            clsUsuario usuario = new clsUsuario();

            // Validar si el usuario ya existe
            if (usuario.ValidarUsuario(nombreUsuario, contraseña) == "Usuario EXISTE")
            {
                MessageBox.Show("El usuario ya existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Insertar el nuevo usuario en la base de datos
                usuario.InsertarUsuario(nombreUsuario, contraseña);

                // Mostrar un mensaje de éxito
                MessageBox.Show(usuario.estadoConexion, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
