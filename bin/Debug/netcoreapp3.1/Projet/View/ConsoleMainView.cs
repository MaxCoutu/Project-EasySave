using Projet.Infrastructure;
using Projet.Model;
using Projet.Service;
using Projet.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projet.View
{
    public class ConsoleMainView : IMainView
    {
        private readonly MainViewModel _vm;
        private readonly ILanguageService _lang;
        private readonly IAddJobView _addView;
        private readonly IBackupService _svc;        

        public ConsoleMainView(
            MainViewModel vm,
            ILanguageService lang,
            IAddJobView addView,
            IBackupService svc)        
        {
            _vm = vm;
            _lang = lang;
            _addView = addView;
            _svc = svc;

            _vm.StatusUpdated += OnStatus;
        }

        public void Show()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== EasySave ===");
                Console.WriteLine("1. List jobs");
                Console.WriteLine("2. Run selected job");
                Console.WriteLine("3. Run all jobs");
                Console.WriteLine("4. Add job");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ListJobs(); Pause(); break;

                    case "2":
                        SelectJob();
                        _vm.RunSelectedAsync().Wait();
                        Pause(); break;

                    case "3":
                        _vm.RunAllAsync().Wait();
                        Pause(); break;

                    case "4":
                        _addView.Show();
                        RefreshJobs();         
                        Pause(); break;

                    case "0":
                        exit = true; break;
                }
            }
        }

        public void Close() { }

        
        private void ListJobs()
        {
            Console.WriteLine("Jobs:");
            for (int i = 0; i < _vm.Jobs.Count; i++)
                Console.WriteLine($"{i + 1}. {_vm.Jobs[i].Name} ({_vm.Jobs[i].Strategy.Type})");
        }

        private void SelectJob()
        {
            ListJobs();
            Console.Write("Select #: ");
            if (int.TryParse(Console.ReadLine(), out int idx) &&
                idx > 0 && idx <= _vm.Jobs.Count)
            {
                _vm.SelectedJob = _vm.Jobs[idx - 1];
            }
        }

        private void RefreshJobs()
        {
            _vm.Jobs.Clear();
            foreach (var job in _svc.GetJobs())
                _vm.Jobs.Add(job);
        }

        private static void Pause()
        {
            Console.WriteLine("Press any key…");
            Console.ReadKey();
        }

        private static void OnStatus(StatusEntry s)
        {
            Console.CursorLeft = 0;
            Console.Write($"{s.Name}: {s.Progression:P0} " +
                          $"({s.TotalFilesToCopy - s.NbFilesLeftToDo}/{s.TotalFilesToCopy})   ");
        }
    }
}
