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

        private void LoadListbox()
        {
            // load up the pkid listbox
            lbIDs.Items.Clear();
            using (TestEFDeployEntities ctx = new TestEFDeployEntities())
            {
                var mypkids = (from rows in ctx.TestTables
                               select rows).ToList();

                foreach (var row in mypkids)
                {
                    lbIDs.Items.Add(row.pkid.ToString());
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadListbox();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // save changes to the data field to the currently selected pkid record
            int param = GetSelectedPKID();

            using (TestEFDeployEntities ctx = new TestEFDeployEntities())
            {
                TestTable savethis = (from rows in ctx.TestTables
                                where rows.pkid == param
                                select rows).FirstOrDefault();

                savethis.DataValue = txtData.Text;

                ctx.SaveChanges();
            }
        }

        private int GetSelectedPKID()
        {
            int ret = 0;

            var theone = lbIDs.SelectedItem;
            //MessageBox.Show("selected: " + theone.ToString());
            ret = Int32.Parse(theone.ToString());

            return ret;
        }

        private void lbIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int param = GetSelectedPKID();

            // update data textbox with change in selected pkid
            using (TestEFDeployEntities ctx = new TestEFDeployEntities())
            {
                var mypkids = (from rows in ctx.TestTables
                               where rows.pkid == param
                               select rows).ToList();

                txtData.Text = mypkids[0].DataValue;
            }
        }
    }
}
