using Bespoke.Sph.Domain;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bespoke.PosEntt.Ipos.FileWatcher
{
    public class IposPemFileDrop : IReceiveLocation, IDisposable
    {
        private FileSystemWatcher m_watcher;
        private bool m_paused;

        private async void FswChanged(object sender, FileSystemEventArgs e)
        {
            if (m_paused) return;

            var file = e.FullPath;
            var wip = file + ".wip";
            await WaitReadyAsync(file);

            File.Move(file, wip);
            await WaitReadyAsync(wip);
            await Task.Delay(100);

            var collectionFolder = ConfigurationManager.AppSettings["CentralLocation"];
            if (!string.IsNullOrWhiteSpace(collectionFolder) && Directory.Exists(collectionFolder))
            {
                File.Copy(wip, Path.Combine(collectionFolder, Path.GetFileName(file)), true);
                Console.WriteLine($"File [{Path.GetFileName(file)}] copied to central.");
            }

            var archivedFolder = ConfigurationManager.AppSettings["ArchiveLocation"];
            if (!string.IsNullOrWhiteSpace(archivedFolder) && Directory.Exists(archivedFolder))
            {
                File.Move(wip, Path.Combine(archivedFolder, Path.GetFileName(file)));
                Console.WriteLine($"File [{Path.GetFileName(file)}] archived.");
            }
            else
                File.Delete(wip);
        }

        public async Task WaitReadyAsync(string fileName)
        {
            while (true)
            {
                try
                {
                    using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        if (stream != null)
                        {
                            Console.WriteLine($"Output file {Path.GetFileName(fileName)} ready.");
                            break;
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                await Task.Delay(500);
            }
        }

        public void Dispose()
        {
            m_watcher?.Dispose();
            m_watcher = null;
        }

        public void Pause()
        {
            Console.WriteLine($"File listening is paused.");
            m_paused = true;
        }

        public void Resume()
        {
            Console.WriteLine($"File listening is resumed.");
            m_paused = false;
        }

        public bool Start()
        {
            var path = ConfigurationManager.AppSettings["DropLocation"] ?? @"C:\temp\ipos-drop";
            var filter = ConfigurationManager.AppSettings["FileFilter"] ?? @"*.txt*";
            m_watcher = new FileSystemWatcher(path, filter) { EnableRaisingEvents = true };
            m_watcher.Created += FswChanged;

            Console.WriteLine($"Started file listening in [{path}] for any file drop...");
            return true;
        }

        public bool Stop()
        {
            this.Dispose();
            Console.WriteLine($"File listening is stopped.");
            return true;
        }
    }
}
