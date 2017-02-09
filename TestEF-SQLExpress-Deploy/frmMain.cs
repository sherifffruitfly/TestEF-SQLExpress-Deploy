using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEF_SQLExpress_Deploy
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("sup!");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // load up the pkid listbox

            using (TestEFDeployEntities ctx = new TestEFDeployEntities())
            {
                var mypkids = (from rows in ctx.TestTables
                              select rows).ToList();

                foreach (var row in mypkids)
                {
                    lbIDs.Items.Add(row.pkid.ToString());
                }

                //list.Items.add(new ListBoxItem("name", "value"));

                // ok what type do i foreach my way thru for the results?

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // save changes to the data field to the currently selected pkid record
        }

        private void lbIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update data textbox with change in selected pkid
        }
    }
}
