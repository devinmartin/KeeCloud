using KeeCloud.Forms;
using KeePass.Plugins;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeeCloud
{
    public class KeeCloudExt : Plugin
    {
        private IPluginHost host = null;
        private ToolStripItem seperator;
        private ToolStripItem wizardItem;

        public override bool Initialize(IPluginHost host)
        {
#if DEVELOPING
            // Obviously you can attatch directly to the KeePass process from Visual Studio as well if you prefer
            try
            {
                if (!Debugger.IsAttached)
                    Debugger.Launch();
            }
            catch { } // do nothing if we can't debug
#endif
            this.host = host;

            this.seperator = new ToolStripSeparator();
            host.MainWindow.ToolsMenu.DropDown.Items.Add(this.seperator);

            this.wizardItem = host.MainWindow.ToolsMenu.DropDown.Items.Add("URL Credential Wizard", Resource1.key_go, (sender, e) => this.LaunchWizard());

            ProviderRegistry.RegisterAllWithContext(host);
            return true;
        }

        public override void Terminate()
        {
            host.MainWindow.ToolsMenu.DropDown.Items.Remove(this.seperator);
            host.MainWindow.ToolsMenu.DropDown.Items.Remove(this.wizardItem);

            base.Terminate();
        }

        private void LaunchWizard()
        {
            FormCredentialWizard form = new FormCredentialWizard(this.host);
            form.Show();
        }

        /// <summary>
        /// URL that contains the current version. This is used by KeePass to report to the user when updates are available
        /// </summary>
        public override string UpdateUrl
        {
            get { return "https://s3.amazonaws.com/KeeCloud/version_manifest.txt"; }
        }
    }
}