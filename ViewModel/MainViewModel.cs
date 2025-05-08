using Projet.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Projet.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IBackupService _svc;
        private readonly ILogService _logService;

        public ObservableCollection<BackupJob> Jobs { get; }
        private BackupJob _selectedJob;
        public BackupJob SelectedJob
        {
            get => _selectedJob;
            set { _selectedJob = value; OnPropertyChanged(); }
        }

        public event Action<StatusEntry> StatusUpdated;

        public MainViewModel(IBackupService svc, ILogService logService)
        {
            _svc = svc;
            _logService = logService;
            Jobs = new ObservableCollection<BackupJob>(_logService.LoadJobs());
            _svc.StatusUpdated += s => StatusUpdated?.Invoke(s);
        }

        public Task RunSelectedAsync() => _selectedJob == null
            ? Task.CompletedTask
            : _svc.ExecuteBackupAsync(_selectedJob.Name);

        public Task RunAllAsync() => _svc.ExecuteAllBackupsAsync();
    }
}
