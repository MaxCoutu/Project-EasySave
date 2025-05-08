using Projet.Service;
using Projet.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projet.ViewModel
{
    public class AddJobViewModel
    {
        private readonly IBackupService _svc;
        private readonly ILogService _logService;

        public BackupJobBuilder Builder { get; } = new BackupJobBuilder();

        public AddJobViewModel(IBackupService svc, ILogService logService)
        {
            _svc = svc;
            _logService = logService;
        }

        public void AddJob()
        {
            var job = Builder.Build();
            _svc.AddBackup(job);

            // Charger les jobs existants, ajouter le nouveau job et sauvegarder
            var jobs = _logService.LoadJobs();
            jobs.Add(job);
            _backupService.AddBackup(job);
            _logService.SaveJobs(_backupService.GetJobs().ToList());
            Console.WriteLine("Sauvegarde effectuée"); // Test
        }
    }
}
