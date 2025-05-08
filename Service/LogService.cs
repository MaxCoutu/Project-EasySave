using Newtonsoft.Json;
using Projet.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Projet.Service
{
    public interface ILogService
    {
        List<BackupJob> LoadJobs();
        void SaveJobs(List<BackupJob> jobs);
    }

    public class LogService : ILogService
    {
        private readonly string _logFilePath = Path.Combine(Environment.CurrentDirectory, "backupJobs.json");

        public LogService()
        {
            Console.WriteLine("Fichier de sauvegarde : " + _logFilePath); // Affichage du chemin
        }

        public List<BackupJob> LoadJobs()
        {
            if (File.Exists(_logFilePath))
            {
                var json = File.ReadAllText(_logFilePath);
                return JsonConvert.DeserializeObject<List<BackupJob>>(json) ?? new List<BackupJob>();
            }
            return new List<BackupJob>();
        }

        public void SaveJobs(List<BackupJob> jobs)
        {
            var json = JsonConvert.SerializeObject(jobs, Formatting.Indented);
            File.WriteAllText(_logFilePath, json);
            Console.WriteLine("Backup jobs sauvegardés dans : " + _logFilePath); // Confirmer la sauvegarde
        }
    }
}
