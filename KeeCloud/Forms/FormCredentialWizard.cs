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
            ExchangeCode,
            AwaitingAuthorization,
            Success
        }

        readonly FormSelectProvider selectionForm = new FormSelectProvider();
        readonly FormExchangeCode exchangeForm = new FormExchangeCode();
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
                "Select a service to go through the authentication process");

            this.Icon = host.MainWindow.Icon;

            this.panelContainer.Controls.Add(this.selectionForm);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (this.wizardState != WizardState.UrlEntry)
            {
                if (MessageBox.Show("Are you sure you wish to cancel?", "Are you sure?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    this.Close();
            }
            else
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
                        if (this.credentialConfigurationProvier is IOAuth1CredentialConfigurationProvider)
                        {
                            this.panelContainer.Controls.Add(this.authorizationForm);
                            // invoke authentication process
                            this.wizardState = WizardState.AwaitingAuthorization;
                        }
                        else if (this.credentialConfigurationProvier is IOAuth2CredentialConfigurationProvider)
                        {
                            this.panelContainer.Controls.Add(this.exchangeForm);
                            this.wizardState = WizardState.ExchangeCode;
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
                    urlOperation.Run();

                    break;
                case WizardState.ExchangeCode:
                    {
                        this.BackgroundDisplay(true);
                        CredentialClaimResult result = null;
                        var exchangeCodeOperation = new AsyncOperationWithSynchronizationContext(() =>
                        {
                            var provider = this.credentialConfigurationProvier as IOAuth2CredentialConfigurationProvider;
                            result = provider.ExchangeCode(this.exchangeForm.textBoxCode.Text);
                        },
                        // continuation
                        () =>
                        {
                            if (result.IsSuccess)
                            {
                                this.panelContainer.Controls.Remove(this.exchangeForm);
                                this.successForm.SetResult(result, this.selectionForm.SelectedProvider);
                                this.panelContainer.Controls.Add(this.successForm);
                                this.wizardState = WizardState.Success;
                                this.buttonCancel.Visible = false;
                                this.buttonNext.Text = "Done";
                            }
                            else
                            {
                                MessageBox.Show("Couldn't exchange authorization code for an access token", "Error");
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
                        exchangeCodeOperation.Run();
                        break;
                    }
                case WizardState.AwaitingAuthorization:
                    {
                        this.BackgroundDisplay(true);
                        CredentialClaimResult result = null;
                        var awaitingAuthorizationOperation = new AsyncOperationWithSynchronizationContext(() =>
                        {
                            var provider = this.credentialConfigurationProvier as IOAuth1CredentialConfigurationProvider;
                            result = provider.Claim();
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
                    }
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