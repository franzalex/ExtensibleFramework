using System.Windows.Forms;

namespace SalesManager
{
    public partial class AddEditPartner : ExtensibleFramework.Core.ActivityControl
    {
        private enum ControlMode { Unknown = 0, Add = 1, Edit = 2 };

        private const string AddPartner = "add";
        private ControlMode mode;

        /// <summary>Initializes a new instance of the <see cref="AddEditPartner" /> class.</summary>
        public AddEditPartner()
        {
            InitializeComponent();

            // set mode to unknown until it is initialized from the [Re]startControl
            this.mode = ControlMode.Unknown;
            this.Start += AddEditPartner_Start;
            this.Stopping += AddEditPartner_Stopping;
            this.Stopped += AddEditPartner_Stopped;
            this.Restart += AddEditPartner_Restart;
        }

        /// <summary>Gets the text associated with this control.</summary>
        /// <returns>The text associated with this control.</returns>
        public override string Text
        {
            get
            {
                switch (mode)
                {
                    case ControlMode.Add:
                        return "Add Partner";

                    case ControlMode.Edit:
                        return "Edit: " + "{clientID}"; //HIGH: Add code to display client ID on edit

                    default:
                        return "Client Editor";
                }
            }
        }

        private void AddEditPartner_Restart(object sender, ExtensibleFramework.Core.RestartEventArgs e)
        {
            // TODO: restore state on restart
            throw new System.NotImplementedException();
        }

        private void AddEditPartner_Start(object sender, ExtensibleFramework.Core.StartEventArgs e)
        {
            if (e.InitializationCommand == AddPartner)
            {
                this.mode = ControlMode.Add;
            }
        }

        private void AddEditPartner_Stopped(object sender, ExtensibleFramework.Core.StoppedEventArgs e)
        {
            // TODO: save state when user is stopping the control
            throw new System.NotImplementedException();
        }

        private void AddEditPartner_Stopping(object sender, ExtensibleFramework.Core.StoppingEventArgs e)
        {
            // HIGH: Add code to detect whether changes have occurred.
            // prompt user to save changes made before closing
            var resp = MessageBox.Show("Do you want to save changes to this client?", "Save Changes",
                                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

            if (resp == DialogResult.Yes)
                this.Save();

            e.Cancel = resp == DialogResult.Cancel;
        }

        private void Save()
        {
            // HIGH: Add code to save partner (client/supplier)
            throw new System.NotImplementedException();
        }
    }
}