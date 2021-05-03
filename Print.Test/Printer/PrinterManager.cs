using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.IO;

namespace Print.Test.Printer
{
    class PrinterManager
    {
        public void printTestOrder()
        {
            var printFilesDirectoryWatcherPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Orders");

            string fileName = "1_1.jpg";

            FileSystemEventArgs eventArgs = new FileSystemEventArgs(WatcherChangeTypes.Created, printFilesDirectoryWatcherPath, fileName);

            System.Threading.Tasks.Task.Run(() =>
            {
                var printDocument = new PrintDocument
                {
                    DocumentName = eventArgs.Name
                };

                printDocument.DefaultPageSettings.Landscape = true;

                printDocument.PrintPage += (printPageSender, printPageEventArgs) =>
                {
                    var printableImage = System.Drawing.Image.FromFile(eventArgs.FullPath);

                    System.Drawing.PointF position = new System.Drawing.PointF(0, 0);

                    var rectangle = printPageEventArgs.MarginBounds;

                    rectangle.Location = new System.Drawing.Point(0, 0);

                    rectangle.Width = printPageEventArgs.PageSettings.Bounds.Width;

                    rectangle.Height = printPageEventArgs.PageSettings.Bounds.Height;

                    printPageEventArgs.Graphics.DrawImage(printableImage, rectangle);
                };

                var fileNameParts = System.IO.Path.GetFileNameWithoutExtension(eventArgs.Name).Split('_');

                for (int i = 0; i < short.Parse(fileNameParts[1]); i++)
                {
                    printDocument.Print();
                }
            }).ContinueWith(task =>
            {
                if (task.Exception != null || task.IsCanceled)
                {
                    foreach (var exception in task.Exception.InnerExceptions)
                    {
                        Log("Error", exception.Message);
                    }

                    return;
                }

                try
                {

                    // System.IO.File.Delete(eventArgs.FullPath);

                }
                catch { }
            });
        }

        public void printOrder(FileSystemEventArgs eventArgs)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                var printDocument = new PrintDocument
                {
                    DocumentName = eventArgs.Name
                };

                printDocument.DefaultPageSettings.Landscape = true;

                printDocument.PrintPage += (printPageSender, printPageEventArgs) =>
                {
                    var printableImage = System.Drawing.Image.FromFile(eventArgs.FullPath);

                    System.Drawing.PointF position = new System.Drawing.PointF(0, 0);

                    var rectangle = printPageEventArgs.MarginBounds;

                    rectangle.Location = new System.Drawing.Point(0, 0);

                    rectangle.Width = printPageEventArgs.PageSettings.Bounds.Width;
                    rectangle.Height = printPageEventArgs.PageSettings.Bounds.Height;

                    printPageEventArgs.Graphics.DrawImage(printableImage, rectangle);
                };

                var fileNameParts = System.IO.Path.GetFileNameWithoutExtension(eventArgs.Name).Split('_');

                for (int i = 0; i < short.Parse(fileNameParts[1]); i++)
                {
                    printDocument.Print();
                }
            }).ContinueWith(task =>
            {
                if (task.Exception != null || task.IsCanceled)
                {
                    foreach (var exception in task.Exception.InnerExceptions)
                    {
                        Log("Error", exception.Message);
                    }

                    return;
                }

                try
                {
                    System.IO.File.Delete(eventArgs.FullPath);
                }
                catch { }
            });
        }

        private void Log(string level, string message)
        {
            var logDirectoryPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Logs");

            if (!System.IO.Directory.Exists(logDirectoryPath))
            {
                System.IO.Directory.CreateDirectory(logDirectoryPath);
            }

            var logFilePath = System.IO.Path.Combine(logDirectoryPath, string.Format("{0}.log", System.DateTime.Now.Date.ToShortDateString().Replace('/', '_')));

            if (!System.IO.File.Exists(logFilePath))
            {
                // Create a file to write to.   
                using (System.IO.StreamWriter streamWriter = System.IO.File.CreateText(logFilePath))
                {
                    streamWriter.WriteLine(string.Format("[PrintEnqueuerLog - {0}] - {1}: {2}", level, System.DateTime.Now.ToShortTimeString(), message));
                }
            }
            else
            {
                using (System.IO.StreamWriter streamWriter = System.IO.File.AppendText(logFilePath))
                {
                    streamWriter.WriteLine(string.Format("[PrintEnqueuerLog - {0}] - {1}: {2}", level, System.DateTime.Now.ToShortTimeString(), message));
                }
            }
        }
    }
}
