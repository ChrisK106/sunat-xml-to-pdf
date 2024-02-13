using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Velopack;
using Velopack.Sources;

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
            var mgr = new UpdateManager(new GithubSource("https://github.com/ChrisK106/sunat-xml-to-pdf", String.Empty, false));

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();

            if (newVersion == null)
                return; // no update available

            string updateVersion = newVersion.TargetFullRelease.Version.Release;
            string updateSize = ((newVersion.TargetFullRelease.Size / 1024f) / 1024f).ToString("0.00") + " MB";
            string updateMessage = "¿Desea descargar e instalar la actualización?\nVersión: " + updateVersion + "\nTamaño de actualización: " + updateSize;
            string updateReleaseNotes = "";

            // get release info from a Github Release using Github API
            string releaseApiUrl = "https://api.github.com/repos/ChrisK106/sunat-xml-to-pdf/releases/tags/" + updateVersion;

            // make a web request to the releaseApiUrl and parse the json response to get the release notes
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "sunat-xml-to-pdf");

            var releaseJson = await httpClient.GetStringAsync(releaseApiUrl);
            var releaseInfo = JsonConvert.DeserializeObject<GithubRelease>(releaseJson);

            if (releaseInfo != null) updateReleaseNotes = releaseInfo.Body;

            // check if release notes are available and append to update message
            if (!String.IsNullOrEmpty(updateReleaseNotes)) updateMessage += "\n\nNotas de esta versión:\n" + updateReleaseNotes;

            // show message box to user with option to install new version
            if (MessageBox.Show(null, updateMessage, "Actualización Disponible", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // download new version
                await mgr.DownloadUpdatesAsync(newVersion);

                // install new version and restart app
                mgr.ApplyUpdatesAndRestart(newVersion);
            }
        }

    }
}
