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
using System.Configuration;
using DatabaseUtilities;

namespace TestEF_SQLExpress_Deploy
{
	public partial class frmMain : Form
	{
		private string connstr = string.Empty;

		public frmMain()
		{
			SetConnectionString();
			InitializeComponent();
            this.Text = "Test EF-Deploy - " + System.Environment.MachineName;

        }

		private void btnExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			DatabaseEnumerator dbenum = new DatabaseEnumerator();
			if (dbenum.EnumerateSQLInstances())
			{
				MessageBox.Show(dbenum.displaythis);
			}
			else
			{
				MessageBox.Show("nothing found");
			}
		}

		private void ResetDatabase()
		{
			using (TestEFDeployEntities db = new TestEFDeployEntities(connstr))
			{
				db.Database.ExecuteSqlCommand("truncate table TestTable");
				db.SaveChanges();

				TestTable tt1 = new TestTable();
				tt1.DataValue = "this is record 1";
				db.TestTables.Add(tt1);
				db.SaveChanges();

				TestTable tt2 = new TestTable();
				tt2.DataValue = "this is record 2";
				db.TestTables.Add(tt2);
				db.SaveChanges();

				TestTable tt3 = new TestTable();
				tt3.DataValue = "this is record 3";
				db.TestTables.Add(tt3);
				db.SaveChanges();

				LoadListbox();
				lbIDs.SelectedIndex = 0;
			}
		}

		private void SetConnectionString()
		{
			if (System.Environment.MachineName == "CANTOR")
			{
				this.connstr = ConfigurationManager.ConnectionStrings["TestEFDeployEntities-away"].ConnectionString;

			}
			else if (System.Environment.MachineName == "GAUSS")
			{
				this.connstr = ConfigurationManager.ConnectionStrings["TestEFDeployEntities-home"].ConnectionString;
			}
			else
			{
				MessageBox.Show("Unrecognized computer; database not available.");
				this.connstr = string.Empty;
			}
		}

		private void LoadListbox()
		{
			// load up the pkid listbox
			lbIDs.Items.Clear();
			using (TestEFDeployEntities ctx = new TestEFDeployEntities(this.connstr))
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

            if (param > 0)
            {
                using (TestEFDeployEntities ctx = new TestEFDeployEntities(this.connstr))
                {
                    TestTable savethis = (from rows in ctx.TestTables
                                          where rows.pkid == param
                                          select rows).FirstOrDefault();

                    savethis.DataValue = txtData.Text;

                    ctx.SaveChanges();
                }
            }
		}

		private int GetSelectedPKID()
		{
			int ret = 0;

			var theone = lbIDs.SelectedItem;
            //MessageBox.Show("selected: " + theone.ToString());
            if (theone != null)
            {
                ret = Int32.Parse(theone.ToString());
            }

            return ret;
		}

		private void lbIDs_SelectedIndexChanged(object sender, EventArgs e)
		{
			int param = GetSelectedPKID();

			// update data textbox with change in selected pkid
			using (TestEFDeployEntities ctx = new TestEFDeployEntities(this.connstr))
			{
				var mypkids = (from rows in ctx.TestTables
							   where rows.pkid == param
							   select rows).ToList();

                if (mypkids != null && mypkids.Count > 0)
                {
                    txtData.Text = mypkids[0].DataValue;
                }
			}
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			//create new testtable object
			//save it, letting the identity 1, 1 get set
			//reload listbox, go to appropriate index
			//focus one data textbox

			int myid = 0;
			TestTable newtt = new TestTable();

			//create the new record, get the pkid
			using (TestEFDeployEntities db = new TestEFDeployEntities(connstr))
			{
				db.TestTables.Add(newtt);
				db.SaveChanges();

				myid = newtt.pkid;
			}

			//reload the listbox
			LoadListbox();
		}

		private void btnResetDB_Click(object sender, EventArgs e)
		{
			ResetDatabase();
		}
	}
}
