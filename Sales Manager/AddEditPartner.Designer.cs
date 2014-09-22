namespace SalesManager
{
    partial class AddEditPartner
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TableLayoutPanel tlpMain;
            System.Windows.Forms.Label lblID;
            System.Windows.Forms.Label lblName;
            System.Windows.Forms.TableLayoutPanel tlpDetails;
            System.Windows.Forms.GroupBox grpContact;
            System.Windows.Forms.Label lblAddress;
            System.Windows.Forms.Label lblPhoneNumber;
            System.Windows.Forms.GroupBox grpPartnerInfo;
            System.Windows.Forms.Label lblNotes;
            System.Windows.Forms.TableLayoutPanel tlpButtons;
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.chkIsClient = new System.Windows.Forms.CheckBox();
            this.chkIsSupplier = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            tlpMain = new System.Windows.Forms.TableLayoutPanel();
            lblID = new System.Windows.Forms.Label();
            lblName = new System.Windows.Forms.Label();
            tlpDetails = new System.Windows.Forms.TableLayoutPanel();
            grpContact = new System.Windows.Forms.GroupBox();
            lblAddress = new System.Windows.Forms.Label();
            lblPhoneNumber = new System.Windows.Forms.Label();
            grpPartnerInfo = new System.Windows.Forms.GroupBox();
            lblNotes = new System.Windows.Forms.Label();
            tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            tlpMain.SuspendLayout();
            tlpDetails.SuspendLayout();
            grpContact.SuspendLayout();
            grpPartnerInfo.SuspendLayout();
            tlpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpMain.Controls.Add(lblID, 0, 0);
            tlpMain.Controls.Add(this.txtID, 1, 0);
            tlpMain.Controls.Add(lblName, 0, 1);
            tlpMain.Controls.Add(this.txtName, 1, 1);
            tlpMain.Controls.Add(tlpDetails, 0, 2);
            tlpMain.Controls.Add(tlpButtons, 1, 3);
            tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpMain.Location = new System.Drawing.Point(0, 0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 4;
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.Size = new System.Drawing.Size(480, 280);
            tlpMain.TabIndex = 0;
            // 
            // lblID
            // 
            lblID.AutoSize = true;
            lblID.Dock = System.Windows.Forms.DockStyle.Fill;
            lblID.Location = new System.Drawing.Point(3, 0);
            lblID.Name = "lblID";
            lblID.Size = new System.Drawing.Size(62, 29);
            lblID.TabIndex = 0;
            lblID.Text = "Partner &ID:";
            lblID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(71, 3);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(192, 23);
            this.txtID.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            lblName.Location = new System.Drawing.Point(3, 29);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(62, 29);
            lblName.TabIndex = 2;
            lblName.Text = "&Name:";
            lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorProvider.SetIconPadding(this.txtName, -20);
            this.txtName.Location = new System.Drawing.Point(71, 32);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(406, 23);
            this.txtName.TabIndex = 3;
            // 
            // tlpDetails
            // 
            tlpDetails.ColumnCount = 2;
            tlpMain.SetColumnSpan(tlpDetails, 2);
            tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpDetails.Controls.Add(grpContact, 0, 0);
            tlpDetails.Controls.Add(grpPartnerInfo, 1, 0);
            tlpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpDetails.Location = new System.Drawing.Point(3, 61);
            tlpDetails.Name = "tlpDetails";
            tlpDetails.RowCount = 1;
            tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpDetails.Size = new System.Drawing.Size(474, 187);
            tlpDetails.TabIndex = 4;
            // 
            // grpContact
            // 
            grpContact.Controls.Add(this.txtAddress);
            grpContact.Controls.Add(lblAddress);
            grpContact.Controls.Add(this.txtPhoneNumber);
            grpContact.Controls.Add(lblPhoneNumber);
            grpContact.Dock = System.Windows.Forms.DockStyle.Fill;
            grpContact.Location = new System.Drawing.Point(3, 3);
            grpContact.Name = "grpContact";
            grpContact.Size = new System.Drawing.Size(231, 181);
            grpContact.TabIndex = 0;
            grpContact.TabStop = false;
            grpContact.Text = "Contact Details";
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorProvider.SetIconAlignment(this.txtAddress, System.Windows.Forms.ErrorIconAlignment.TopRight);
            this.errorProvider.SetIconPadding(this.txtAddress, -40);
            this.txtAddress.Location = new System.Drawing.Point(6, 87);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAddress.Size = new System.Drawing.Size(219, 88);
            this.txtAddress.TabIndex = 3;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new System.Drawing.Point(6, 69);
            lblAddress.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new System.Drawing.Size(52, 15);
            lblAddress.TabIndex = 2;
            lblAddress.Text = "&Address:";
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorProvider.SetIconPadding(this.txtPhoneNumber, -20);
            this.txtPhoneNumber.Location = new System.Drawing.Point(6, 37);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(219, 23);
            this.txtPhoneNumber.TabIndex = 1;
            // 
            // lblPhoneNumber
            // 
            lblPhoneNumber.AutoSize = true;
            lblPhoneNumber.Location = new System.Drawing.Point(6, 19);
            lblPhoneNumber.Name = "lblPhoneNumber";
            lblPhoneNumber.Size = new System.Drawing.Size(91, 15);
            lblPhoneNumber.TabIndex = 0;
            lblPhoneNumber.Text = "&Phone Number:";
            // 
            // grpPartnerInfo
            // 
            grpPartnerInfo.Controls.Add(this.txtNotes);
            grpPartnerInfo.Controls.Add(this.chkIsClient);
            grpPartnerInfo.Controls.Add(lblNotes);
            grpPartnerInfo.Controls.Add(this.chkIsSupplier);
            grpPartnerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            grpPartnerInfo.Location = new System.Drawing.Point(240, 3);
            grpPartnerInfo.Name = "grpPartnerInfo";
            grpPartnerInfo.Size = new System.Drawing.Size(231, 181);
            grpPartnerInfo.TabIndex = 1;
            grpPartnerInfo.TabStop = false;
            grpPartnerInfo.Text = "Partner Info";
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(6, 87);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(219, 88);
            this.txtNotes.TabIndex = 3;
            // 
            // chkIsClient
            // 
            this.chkIsClient.AutoSize = true;
            this.chkIsClient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsClient.Location = new System.Drawing.Point(6, 41);
            this.chkIsClient.MinimumSize = new System.Drawing.Size(96, 0);
            this.chkIsClient.Name = "chkIsClient";
            this.chkIsClient.Size = new System.Drawing.Size(96, 19);
            this.chkIsClient.TabIndex = 1;
            this.chkIsClient.Text = "Is C&lient";
            this.chkIsClient.UseVisualStyleBackColor = true;
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Location = new System.Drawing.Point(6, 69);
            lblNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new System.Drawing.Size(41, 15);
            lblNotes.TabIndex = 2;
            lblNotes.Text = "&Notes:";
            // 
            // chkIsSupplier
            // 
            this.chkIsSupplier.AutoSize = true;
            this.chkIsSupplier.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsSupplier.Location = new System.Drawing.Point(6, 22);
            this.chkIsSupplier.MinimumSize = new System.Drawing.Size(96, 0);
            this.chkIsSupplier.Name = "chkIsSupplier";
            this.chkIsSupplier.Size = new System.Drawing.Size(96, 19);
            this.chkIsSupplier.TabIndex = 0;
            this.chkIsSupplier.Text = "&Is Supplier";
            this.chkIsSupplier.UseVisualStyleBackColor = true;
            // 
            // tlpButtons
            // 
            tlpButtons.AutoSize = true;
            tlpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpButtons.ColumnCount = 2;
            tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpButtons.Controls.Add(this.btnSave, 0, 0);
            tlpButtons.Controls.Add(this.btnCancel, 1, 0);
            tlpButtons.Dock = System.Windows.Forms.DockStyle.Right;
            tlpButtons.Location = new System.Drawing.Point(318, 251);
            tlpButtons.Margin = new System.Windows.Forms.Padding(0);
            tlpButtons.Name = "tlpButtons";
            tlpButtons.RowCount = 1;
            tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpButtons.Size = new System.Drawing.Size(162, 29);
            tlpButtons.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(84, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // AddEditPartner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tlpMain);
            this.Name = "AddEditPartner";
            this.Size = new System.Drawing.Size(480, 280);
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            tlpDetails.ResumeLayout(false);
            grpContact.ResumeLayout(false);
            grpContact.PerformLayout();
            grpPartnerInfo.ResumeLayout(false);
            grpPartnerInfo.PerformLayout();
            tlpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.CheckBox chkIsClient;
        private System.Windows.Forms.CheckBox chkIsSupplier;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
