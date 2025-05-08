using Projet.Infrastructure;
using Projet.Service;
using Projet.View;
using Projet.ViewModel;
using System;

namespace Projet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPathProvider paths = new DefaultPathProvider();
            ILogger logger = new JsonLogger(paths);

            
            IBackupService backupSvc = new BackupService(logger);
            ILanguageService langSvc = new JsonLanguageService("Languages/en.json");

            
            MainViewModel mainVm = new MainViewModel(backupSvc);
            AddJobViewModel addVm = new AddJobViewModel(backupSvc);
            RemoveJobViewModel removeVm = new RemoveJobViewModel(backupSvc);

            
            IAddJobView addView = new ConsoleAddJobView(addVm, langSvc);
            IRemoveJobView removeView = new ConsoleRemoveJobView(removeVm, backupSvc);

            
            IMainView mainView = new ConsoleMainView(mainVm, langSvc, addView, backupSvc);

           
            mainView.Show();
        }
    }
}
