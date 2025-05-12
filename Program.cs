using System;
using Projet.Infrastructure;
using Projet.Model;
using Projet.Service;
using Projet.View;
using Projet.ViewModel;

namespace Projet
{
    internal class Program
    {
        private static void Main()
        {
            
            Console.Write("Choose language (en/fr) [en] : ");
            string langCode = Console.ReadLine()?.Trim().ToLower();
            if (langCode != "fr") langCode = "en";    
            string dictPath = $"Languages/{langCode}.json";

      
            IPathProvider paths = new DefaultPathProvider();     
            ILogger logger = new JsonLogger(paths);
            IJobRepository repo = new TxtJobRepository(paths);

        
            IBackupService backup = new BackupService(logger, repo);
            ILanguageService lang = new JsonLanguageService(dictPath);

          
            MainViewModel mainVm = new MainViewModel(backup);
            AddJobViewModel addVm = new AddJobViewModel(backup);
            RemoveJobViewModel remVm = new RemoveJobViewModel(backup);

           
            IAddJobView addView = new ConsoleAddJobView(addVm, lang);
            IRemoveJobView remView = new ConsoleRemoveJobView(remVm, backup);

            IMainView mainView = new ConsoleMainView(
                mainVm, lang, addView, remView, backup);

           
            mainView.Show();
        }
    }
}
