static void Main(string[] args)
{
    IPathProvider paths = new DefaultPathProvider();
    ILogger logger = new JsonLogger(paths);

    ILogService logService = new LogService(); // D'abord le log service
    IBackupService backupSvc = new BackupService(logger);

    // Charger les jobs sauvegardés
    var savedJobs = logService.LoadJobs();
    foreach (var job in savedJobs)
    {
        backupSvc.AddBackup(job);
    }

    ILanguageService langSvc = new JsonLanguageService("Languages/en.json");

    MainViewModel mainVm = new MainViewModel(backupSvc, logService);
    AddJobViewModel addVm = new AddJobViewModel(backupSvc, logService);
    RemoveJobViewModel removeVm = new RemoveJobViewModel(backupSvc);

    IAddJobView addView = new ConsoleAddJobView(addVm, langSvc);
    IRemoveJobView removeView = new ConsoleRemoveJobView(removeVm, backupSvc);
    IMainView mainView = new ConsoleMainView(mainVm, langSvc, addView, backupSvc);

    mainView.Show();

    // Sauvegarde automatique à la fin du programme
    logService.SaveJobs(backupSvc.GetJobs().ToList());
}
