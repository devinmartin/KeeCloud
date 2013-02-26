using KeeCloud.Providers;
using KeeCloud.Utilities;
using KeePass.Plugins;
using KeePass.UI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeeCloud.Forms
{
    public partial class FormCredentialWizard : Form
    {
        private enum WizardState
        {
            UrlEntry,
            AwaitingAuthorization,
            Success
        }

        readonly FormSelectProvider selectionForm = new FormSelectProvider();
        readonly FormAwaitAuthorization authorizationForm = new FormAwaitAuthorization();
        readonly FormAuthorizationSuccess successForm;
        ICredentialConfigurationProvider credentialConfigurationProvier;

        WizardState wizardState = WizardState.UrlEntry;

        public FormCredentialWizard(IPluginHost host)
        {
            this.successForm = new FormAuthorizationSuccess(host);

            InitializeComponent();

            this.pictureBoxBanner.Image = BannerFactory.CreateBanner(this.pictureBoxBanner.Width,
                this.pictureBoxBanner.Height,
                BannerStyle.Default,
                Resource1.key_go,
                this.Text,
                "Enter a URL to go through the authentication process");

            this.Icon = host.MainWindow.Icon;

            this.panelContainer.Controls.Add(this.selectionForm);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to cancel?", "Are you sure?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                this.Close();
        }

        #region event handlers

        private void EntryForm_StateChanged(bool allowNext)
        {
            this.buttonNext.Enabled = allowNext;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            switch (this.wizardState)
            {
                case WizardState.UrlEntry:
                    this.BackgroundDisplay(true);
                    var urlOperation = new AsyncOperationWithSynchronizationContext(() =>
                    {
                        this.credentialConfigurationProvier = this.selectionForm.SelectedProvider.CredentialConfigurationProvider;
                        this.credentialConfigurationProvier.Initialize();
                    },
                        // continuation
                    () =>
                    {
                        LaunchUrl(this.credentialConfigurationProvier.GetExternalAuthorizationUrl());

                        this.panelContainer.Controls.Remove(this.selectionForm);
                        this.panelContainer.Controls.Add(this.authorizationForm);
                        // invoke authentication process
                        this.wizardState = WizardState.AwaitingAuthorization;
                    },
                        // catch
                    ecxeption =>
                    {
                        MessageBox.Show(ecxeption.Message, "Error");
                        this.StartOver();
                    },
                        // finally
                    () =>
                    {
                        this.BackgroundDisplay(false);
                    });
                    urlOperation.Run();

                    break;
                case WizardState.AwaitingAuthorization:
                    this.BackgroundDisplay(true);
                    CredentialClaimResult result = null;
                    var awaitingAuthorizationOperation = new AsyncOperationWithSynchronizationContext(() =>
                    {
                        result = this.credentialConfigurationProvier.Claim();
                    },
                        // continuation
                    () =>
                    {
                        if (result.IsSuccess)
                        {
                            this.panelContainer.Controls.Remove(this.authorizationForm);
                            this.successForm.SetResult(result, this.selectionForm.SelectedProvider);
                            this.panelContainer.Controls.Add(this.successForm);
                            this.wizardState = WizardState.Success;
                            this.buttonCancel.Visible = false;
                            this.buttonNext.Text = "Done";
                        }
                        else
                        {
                            MessageBox.Show("Couldn't authorize with account", "Error");
                            this.StartOver();
                        }
                    },
                        // catch
                    ecxeption =>
                    {
                        MessageBox.Show(ecxeption.Message, "Error");
                        this.StartOver();
                    },
                        // finally
                    () =>
                    {
                        this.BackgroundDisplay(false);
                    });
                    awaitingAuthorizationOperation.Run();
                    break;
                case WizardState.Success:
                    this.Close();
                    break;
            }
        }

        #endregion

        private void StartOver()
        {
            this.panelContainer.Controls.Remove(this.authorizationForm);
            this.panelContainer.Controls.Remove(this.selectionForm);
            this.panelContainer.Controls.Remove(this.successForm);

            this.buttonNext.Text = "Next";
            this.buttonNext.Enabled = true;
            this.buttonCancel.Visible = true;
            this.wizardState = WizardState.UrlEntry;
            this.panelContainer.Controls.Add(this.selectionForm);
        }

        private void BackgroundDisplay(bool isRunningBackground)
        {
            this.panelContainer.Visible = !isRunningBackground;
            this.progressBar.Visible = isRunningBackground;
        }

        private static void LaunchUrl(Uri uri)
        {
            Process.Start(uri.ToString());
        }
    }
}