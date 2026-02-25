using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fullProject.Applications.Applications_Type
{
    public partial class frmManageApplicationTypes : Form
    {

        private DataTable _dtAllApplicationTypes;
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationTypes();
            dgvApplicationTypes.DataSource = _dtAllApplicationTypes;
            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();

            if (dgvApplicationTypes.Rows.Count>0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "ID";
                dgvApplicationTypes.Columns[0].Width = 110;

                dgvApplicationTypes.Columns[1].HeaderText = "Title";
                dgvApplicationTypes.Columns[1].Width = 300;

                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvApplicationTypes.Columns[2].Width = 100;
            }
           
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appID = (int)dgvApplicationTypes.CurrentRow.Cells[0].Value;
            Form frm= new frmEditApplicationType(appID);
            frm.ShowDialog();
            frmManageApplicationTypes_Load(null,null);
        }
    }
}
