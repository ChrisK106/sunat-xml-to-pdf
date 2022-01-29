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

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _ = UpdateMyApp();

            Application.Run(new AppForm());
        }

        private static void OnAppInstall(SemanticVersion version, IAppTools tools)
        {
            tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
            tools.CreateUninstallerRegistryEntry();
        }

        private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
        {
            tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
            tools.RemoveUninstallerRegistryEntry();
        }

        private static void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
        {
            tools.SetProcessAppUserModelId();
            // show a welcome message when the app is first installed
            if (firstRun) MessageBox.Show("La aplicación ha sido instalada con éxito", "SUNAT XML ToPDF");
        }

        private static async Task UpdateMyApp()
        {
            using var mgr = new GithubUpdateManager("https://github.com/ChrisK106/sunat-xml-to-pdf", false, null);

            var updateInfo = await mgr.CheckForUpdate();

            if (updateInfo.ReleasesToApply.Count > 0)
            {
                string updateVersion = updateInfo.FutureReleaseEntry.Version.ToString();
                string updateSize = ((updateInfo.FutureReleaseEntry.Filesize / 1024f) / 1024f).ToString("0.00") + " MB";

                if (MessageBox.Show(null,"¿Desea descargar e instalar la actualización?\nVersión: " + updateVersion + " (" + updateSize + ")",
                    "Actualización Disponible", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    await mgr.DownloadReleases(updateInfo.ReleasesToApply);
                    await mgr.ApplyReleases(updateInfo);

                    UpdateManager.RestartApp();
                }
            }
        }

    }
}
