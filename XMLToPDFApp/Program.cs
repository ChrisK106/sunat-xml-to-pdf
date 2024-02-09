using System;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Velopack;

namespace XMLToPDFApp
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        [SupportedOSPlatform("windows")]

        static void Main()
        {
            VelopackApp.Build()
                .WithFirstRun(v => MessageBox.Show("La aplicación ha sido instalada con éxito", "SUNAT XML a PDF"))
                .Run();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _ = UpdateMyApp();

            Application.Run(new AppForm());
        }

        private static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager("https://github.com/ChrisK106/sunat-xml-to-pdf", null, null, null);

            // check for new version
            var updateInfo = await mgr.CheckForUpdatesAsync();
            if (updateInfo == null)
                return; // no update available

            string updateVersion = updateInfo.TargetFullRelease.Version.Release;
            string updateSize = ((updateInfo.TargetFullRelease.Size / 1024f) / 1024f).ToString("0.00") + " MB";

            if (MessageBox.Show(null, "¿Desea descargar e instalar la actualización?\nVersión: " + updateVersion + " (" + updateSize + ")",
                "Actualización Disponible", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // download new version
                await mgr.DownloadUpdatesAsync(updateInfo);

                // install new version and restart app
                mgr.ApplyUpdatesAndRestart();
            }
        }

    }
}
