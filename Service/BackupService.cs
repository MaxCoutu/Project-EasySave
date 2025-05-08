using Projet.Infrastructure;
using Projet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet.Service
{
    public class BackupService : IBackupService
    {
        public event Action<StatusEntry> StatusUpdated;

        private readonly ILogger _logger;
        private List<BackupJob> _jobs = new List<BackupJob>();
        private const int MaxJobs = 5;

        public BackupService(ILogger logger) => _logger = logger;

        public void AddBackup(BackupJob job)
        {
            if (_jobs.Count >= MaxJobs)
                throw new InvalidOperationException("Maximum number of backup jobs reached (5).");

            _jobs.Add(job);
        }

        public void RemoveBackup(string name) => _jobs.RemoveAll(j => j.Name == name);

        public IReadOnlyList<BackupJob> GetJobs() => _jobs.AsReadOnly();

        // Ajout pour charger les jobs depuis un fichier
        public void SetJobs(List<BackupJob> jobs)
        {
            _jobs = jobs ?? new List<BackupJob>();
        }

        public async Task ExecuteBackupAsync(string name)
        {
            BackupJob job = _jobs.First(j => j.Name == name);

            Report(new StatusEntry(job.Name, "", "", "PENDING", 0, 0, 0, 0));

            DateTime start = DateTime.UtcNow;
            await job.RunAsync(Report);
            DateTime end = DateTime.UtcNow;

            Report(new StatusEntry(job.Name, "", "", "END", 0, 0, 0, 1));

            _logger.LogEvent(new LogEntry(
                end, job.Name, job.SourceDir, job.TargetDir,
                0, (int)(end - start).TotalMilliseconds));
        }

        public async Task ExecuteAllBackupsAsync()
        {
            foreach (BackupJob job in _jobs)
                await ExecuteBackupAsync(job.Name);
        }

        private void Report(StatusEntry entry)
        {
            _logger.UpdateStatus(entry);
            StatusUpdated?.Invoke(entry);
        }
    }
}
