using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet.Infrastructure;
using Projet.Model;

namespace Projet.Service
{

    public class BackupService : IBackupService
    {
        public event Action<StatusEntry> StatusUpdated;

        private readonly ILogger _logger;
        private readonly IJobRepository _repo;
        private readonly List<BackupJob> _jobs;

        private const int MaxJobs = 5;

        public BackupService(ILogger logger, IJobRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _jobs = new List<BackupJob>(_repo.Load());
        }

        public void AddBackup(BackupJob job)
        {
            if (_jobs.Count >= MaxJobs)
                throw new InvalidOperationException("Maximum number of backup jobs reached (5).");

            _jobs.Add(job);
            _repo.Save(_jobs);
        }

        public void RemoveBackup(string name)
        {
            _jobs.RemoveAll(j => j.Name == name);
            _repo.Save(_jobs);
        }

        public IReadOnlyList<BackupJob> GetJobs() => _jobs.AsReadOnly();


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
