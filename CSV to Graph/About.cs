using System;
using System.Deployment.Application;
using System.Windows.Forms;

namespace CSV_Graph
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.sansel.co.in/");
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:support@dicot.in");
        }

        private void update_Click(object sender, EventArgs e)
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException ex1)
                {
                    MessageBox.Show("Not able to download the latest update right now! \n\nPlease check your internet connection, or try again later.");
                }
                catch (InvalidDeploymentException ex2)
                {
                    MessageBox.Show("Not able to download the latest update! This installation is corrupt! Please reinstall and try again!");
                }
                catch (InvalidOperationException ex3)
                {
                    MessageBox.Show("Cannot update the application please contact Customer support!");
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }


                    }
                    else
                    {
                        MessageBox.Show("The application has detected mandatory update from current version "
                            + "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install update and restart.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            DialogResult restart = MessageBox.Show("The application has been updated, and will require a restart. Please press Yes to restart and No to close the software.", "Software updated", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (restart == DialogResult.Yes)
                            {
                                Application.Restart();
                            }
                            else
                            { Environment.Exit(0); }
                        }
                        catch (DeploymentDownloadException ex1)
                        {
                            MessageBox.Show("Cannot install the latest version of the application \n\nPlease check your internet connection, or try again later.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The application is updated to its latest version!");
                }
            }
        }
    }
}
