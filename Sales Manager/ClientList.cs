using System;
using System.Linq;
using System.Windows.Forms;

namespace SalesManager
{
    public partial class ClientList : ExtensibleFramework.Core.ActivityControl
    {
        public ClientList()
        {
            InitializeComponent();

            this.Load += ClientList_Load;
        }

        /// <summary>Gets the text associated with this control.</summary>
        /// <returns>The text associated with this control.</returns>
        public override string Text { get { return "Client List"; } }

        private void ClientList_Load(object sender, EventArgs e)
        {
            LoadClientList();
        }

        private void dgvClients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // trigger the edit button's action
            if (dgvClients.SelectedRows.Count > 0)
                tsbEdit.PerformClick();
        }

        private void dgvClients_DataSourceChanged(object sender, EventArgs e)
        {
            // hide ID column
            foreach (var col in dgvClients.Columns.OfType<DataGridViewColumn>()
                                                  .Where(c => c.HeaderText.ToLower().Contains("id")))
                col.Visible = false;
        }

        private void dgvClients_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (var i = 0; i < dgvClients.Rows.Count; i++)
                dgvClients.Rows[i].HeaderCell.Value = string.Format("{0}", i + 1);
        }

        private void dgvClients_SelectionChanged(object sender, EventArgs e)
        {
            // enable/disable edit & delete buttons
            var enable = dgvClients.SelectedRows.Count > 0;

            tsbEdit.Enabled = enable;
            tsbRemove.Enabled = enable;
        }

        /// <summary>Gets the selected client's ID.</summary>
        private string GetSelectedClientID()
        {
            // exit early if there is no data being displayed
            if (dgvClients.DataSource == null)
                return "";

            var dt = (System.Data.DataTable)dgvClients.DataSource;
            var rowIdx = dgvClients.SelectedRows[0].Index;
            return dt.Rows[rowIdx]["ID"].ToString();
        }

        /// <summary>Loads the client list.</summary>
        private void LoadClientList()
        {
            // load the client list
            dgvClients.DataSource = Dinofage.Data.XpressData.ExecuteSelect(
                SalesManager.Instance.ConnectionString,
                "\r\n".Join(new[] {"SELECT ",
                                   "  PartnerID AS [ID],",
                                   "  PartnerName AS [Name],",
                                   "  PhoneNumber AS [Phone Number],",
                                   "  Address,",
                                   "  Email",
                                   "FROM Partners",
                                   "WHERE IsClient=TRUE"}));

            dgvClients.AutoResizeColumns();
            dgvClients.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            // HIGH: Add code to add clients to the database
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            // HIGH: Add code to edit selected client
        }

        private void tsbRemove_Click(object sender, EventArgs e)
        {
            // TODO: Test code to delete selected client
            using (var xd = new Dinofage.Data.XpressData(SalesManager.Instance.ConnectionString))
            {
                var query = string.Format("SELECT * FROM Partners WHERE PartnerID = '{0}'", GetSelectedClientID());
                var update = true;

                if (xd.ExecuteSelect(query).Rows.Count > 0)
                {
                    // set the client column to false
                    xd.ResultsTable.Rows[0]["IsClient"] = "FALSE";

                    // delete the entire row if partner is not client or supplier
                    if (xd.ResultsTable.Rows[0]["IsClient"].ToString().ToUpper() == "FALSE" &&
                        xd.ResultsTable.Rows[0]["IsSupplier"].ToString().ToUpper() == "FALSE")
                        xd.ResultsTable.Rows[0].Delete();

                    update = xd.Update();
                }

                // display update to user
                if (update)
                {
                    MessageBox.Show("The client has been successfully removed from the database.",
                          "Client Successfully Removed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LoadClientList();
                }
                else
                    MessageBox.Show("The client could not be removed from the database.",
                        "Client Removal Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}