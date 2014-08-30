using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesManager
{
    public partial class ClientList : ExtensibleFramework.Core.ActivityControl
    {
        public ClientList()
        {
            InitializeComponent();
        }

        /// <summary>Gets the text associated with this control.</summary>
        /// <returns>The text associated with this control.</returns>
        public override string Text { get { return "Client List"; } }
    }
}
