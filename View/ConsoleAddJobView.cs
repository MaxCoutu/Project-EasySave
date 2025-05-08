using Projet.Model;
using Projet.Service;
using Projet.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projet.View
{
    public class ConsoleAddJobView : IAddJobView
    {
        private readonly AddJobViewModel _vm;
        private readonly ILanguageService _lang;

        public ConsoleAddJobView(AddJobViewModel vm, ILanguageService lang)
        {
            _vm = vm;
            _lang = lang;
        }

        public void Show()
        {
            Console.Write("Job name      : ");
            string name = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Source folder : ");
            string src = Console.ReadLine()?.Trim().Trim('"') ?? "";
            if (!Directory.Exists(src))
            {
                Console.WriteLine($"Source directory not found: {src}");
                return;
            }

            Console.Write("Target folder : ");
            string dst = Console.ReadLine()?.Trim().Trim('"') ?? "";
            if (!Directory.Exists(dst))
            {
                Console.WriteLine($"Target directory not found: {dst}");
                return;
            }

            Console.Write("Type (full/diff): ");
            string t = Console.ReadLine()?.Trim().ToLower() ?? "full";

            IBackupStrategy strat = t == "diff"
                ? (IBackupStrategy)new DifferentialBackupStrategy()
                : new FullBackupStrategy();

            _vm.Builder
               .WithName(name)
               .WithSource(src)
               .WithTarget(dst)
               .WithStrategy(strat);

            _vm.AddJob();
            Console.WriteLine("Job added.");
        }

        public void Close() { }
    }
}