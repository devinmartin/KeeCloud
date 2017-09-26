using System;
using System.Linq;
using System.Windows.Forms;
using KeeCloud.Providers;
using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Security;
using KeeCloud.Utilities;

namespace KeeCloud.Forms
{
    public partial class FormAuthorizationSuccess : UserControl
    {
        private readonly IPluginHost host;
        private CredentialClaimResult claimResult;
        private IProvider provider;
        private bool shown = false;

        public FormAuthorizationSuccess(IPluginHost host)
        {
            this.host = host;
            InitializeComponent();
        }

        internal void SetResult(CredentialClaimResult result, IProvider provider)
        {
            this.claimResult = result;
            this.provider = provider;

            this.labelUsername.Text = result.UserName;

            this.SetShown(false);

            if (!this.host.Database.IsOpen)
                this.buttonSave.Visible = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.host.Database.IsOpen)
            {
                PwEntry entry = new PwEntry(true, true);

                entry.Strings.Set(StandardProtectedStrings.Title, new ProtectedString(false, string.Format("KeeCloud {0} credentials", this.provider.FriendlyName)));
                entry.Strings.Set(StandardProtectedStrings.Url, new ProtectedString(false, this.provider.Uri.ToString()));
                entry.Strings.Set(StandardProtectedStrings.Username, new ProtectedString(false, this.claimResult.UserName));
                entry.Strings.Set(StandardProtectedStrings.Password, new ProtectedString(true, this.claimResult.Password));

                if (this.host.Database.LastSelectedGroup != null)
                {
                    var uuid = this.host.Database.LastSelectedGroup.UuidBytes;
                    var group = (from g in this.host.Database.GetAllGroups()
                                 where g.Uuid.UuidBytes.SequenceEqual(uuid)
                                 select g).FirstOrDefault();

                    if (group == null)
                        this.host.Database.RootGroup.AddEntry(entry, true);
                    else
                        group.AddEntry(entry, true);
                }
                else
                    this.host.Database.RootGroup.AddEntry(entry, true);

                this.host.MainWindow.UpdateUI(false, null, true, host.Database.RootGroup, true, null, true);
            }
            else
                MessageBox.Show("Database isn't open");
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            SetShown(!this.shown);
        }

        private void SetShown(bool show)
        {
            if (show)
            {
                this.buttonShow.Text = "Hide Password";
                this.textBoxPassword.Text = this.claimResult.Password;
            }
            else
            {
                this.buttonShow.Text = "Show Password";
                this.textBoxPassword.Text = string.Empty.PadLeft(this.claimResult.Password.Length, '*');
            }
            this.shown = show;
        }
    }
}
