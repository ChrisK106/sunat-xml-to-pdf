using System;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Squirrel;

namespace XMLToPDFApp
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // run Squirrel first, as the app may exit after these run
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: OnAppInstall,
                onAppUninstall: OnAppUninstall,
                onEveryRun: OnAppRun);

            Task task = UpdateMyApp();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppForm()); 
        }

        private static void OnAppInstall(SemanticVersion version, IAppTools tools)
        {
            tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
        {
            tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
        {
            tools.SetProcessAppUserModelId();
            // show a welcome message when the app is first installed
            if (firstRun) MessageBox.Show("La aplicación 'SUNAT XML To PDF' ha sido instalada con éxito");
        }

        private static async Task UpdateMyApp()
        {
            using var mgr = new GithubUpdateManager("https://github.com/ChrisK106/sunat-xml-to-pdf", false, null);

            var updateInfo = await mgr.CheckForUpdate();

            string updateVersion, updateSize = null;

            if (updateInfo != null)
            {
                updateVersion = updateInfo.FutureReleaseEntry.Version.Version.Major + "."
                    + updateInfo.FutureReleaseEntry.Version.Version.Minor + "."
                    + updateInfo.FutureReleaseEntry.Version.Version.Revision;

                updateSize = updateInfo.FutureReleaseEntry.Filesize.ToString() + " bytes";

                if(MessageBox.Show("¿Desea descargar e instalar la actualización? \nVersión: " + updateVersion + " (" + updateSize + ")", "Actualización Disponible", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var newVersion = await mgr.UpdateApp();

                    // optionally restart the app automatically, or ask the user if/when they want to restart
                    if (newVersion != null)
                    {
                        UpdateManager.RestartApp();
                    }
                }
            }
        }
    }
}
