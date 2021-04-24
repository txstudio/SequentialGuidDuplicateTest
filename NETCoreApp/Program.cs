using SequentialGuid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace NETCoreApp
{
    class Program
    {
        const string rootPath = "../../../files";
        const int taskNumber = 20;
        const int guidValueCount = 500;

        static void Main(string[] args)
        {
            List<Task> _tasks;

            _tasks = new List<Task>();

            var _files = Directory.GetFiles(rootPath, "*.guid");

            //Dictionary not allow duplicate key
            var _dictionary = new Dictionary<string, string>();

            foreach (var _filePath in _files)
                File.Delete(_filePath);

            Stopwatch _watch = new Stopwatch();

            _watch.Start();

            for (int i = 0; i < taskNumber; i++)
            {
                Task _task = new Task(() => {
                    var _fileName = $"{Guid.NewGuid()}.guid";
                    var _fullPath = Path.Combine(rootPath, _fileName);

                    for (int i = 0; i < guidValueCount; i++)
                    {
                        var _guidValue = SequentialGuidGenerator.Instance.NewGuid();
                        //var _guidValue = Guid.NewGuid();
                        var _content = $"{_guidValue}\n";

                        File.AppendAllText(_fullPath, _content);
                    }
                });
                _tasks.Add(_task);
                _task.Start();
            }

            Task.WaitAll(_tasks.ToArray());

            _watch.Stop();

            _files = Directory.GetFiles(rootPath, "*.guid");

            foreach (var _filePath in _files)
            {
                using(var _reader = new StreamReader(_filePath))
                {
                    while(true)
                    {
                        if (_reader.Peek() == -1)
                            break;

                        var _value = _reader.ReadLine();

                        if (string.IsNullOrWhiteSpace(_value))
                            break;

                        _dictionary.Add(_value, _value);
                    }
                }
            }

            //foreach (var _item in _dictionary)
            //    Console.WriteLine(_item.Key);

            Console.WriteLine($"{_dictionary.Count} SequentialGuids Created");
            Console.WriteLine("Not duplicate Guid found !");
            Console.WriteLine($"Elapsed Time: {_watch.Elapsed}");
            Console.WriteLine();

            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }
    }
}
