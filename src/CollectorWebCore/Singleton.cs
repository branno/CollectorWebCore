using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
//using System.Threading;
//using CollectorWeb.Models;
using CollectorWebCore.Models;
using Npgsql;
//using Buffer = CollectorWeb.Models.Buffer;
//using Timer = System.Timers.Timer;
//using EntityFramework.BulkInsert.Extensions;

namespace CollectorWebCore
{
    public class Singleton
    {
        private static Singleton _instance;
        private readonly Timer _timer;

        //private ConcurrentBag<Element> _list;
        public BlockingCollection<Start> block;

        public ConcurrentBag<Start> _startsBag;
        //private ConcurrentBag<Stop> _stopsBag;
        //private ConcurrentBag<Pause> _pausesBag;
        //private ConcurrentBag<Resume> _resumesBag;
        //private ConcurrentBag<Buffer> _buffersBag;
        //private ConcurrentBag<Seek> _seeksBag;
        public DateTime? _dateStarted;
        public DateTime? _dateFinished;

        private int _counter;
        private int _counterStops;

        private string connString = string.Format("Server={0};Port={1};" +
                    "User Id={2};Password={3};Database={4};",
                    "localhost", "5432",
                    "postgres", "", "collectorDB");

        private NpgsqlConnection _connection;

        /*Server = 13.94.157.104; Database=postgres;
        User Id = postgres; Password=carpediem*/

        private Singleton()
        {
            //temp
            //_list = new ConcurrentBag<Element>();
            block = new BlockingCollection<Start>();

            _startsBag = new ConcurrentBag<Start>();
            //_stopsBag = new ConcurrentBag<Stop>();
            //_pausesBag = new ConcurrentBag<Pause>();
            //_resumesBag = new ConcurrentBag<Resume>();
            //_buffersBag = new ConcurrentBag<Buffer>();
            //_seeksBag = new ConcurrentBag<Seek>();
            _dateStarted = null;
            _connection = new NpgsqlConnection(connString);

            _timer = new Timer(OnTimeElapsedEvent, null, 0, 5000);
            // Hook up the Elapsed event for the timer. 
            //_timer.Elapsed += OnTimeElapsedEvent;
            //// Have the timer fire repeated events (true is the default)
            //_timer.AutoReset = true;
            //_timer.Enabled = true;

        }

        public static Singleton Instance
        {
            get { return _instance ?? (_instance = new Singleton()); }
        }
        /*
        public ConcurrentBag<Element> GetElements()
        {
            return _list;
        }

        public void AddElement(Element element)
        {
            _list.Add(element);
        }
        */
        public ConcurrentBag<Start> GetStarts()
        {
            return _startsBag;
        }

        public void AddStart(Start start)
        {
            _startsBag.Add(start);
        }

        public void AddBlock(Start start)
        {
            block.Add(start);
        }
        /*
        public ConcurrentBag<Stop> GetStops()
        {
            return _stopsBag;
        }

        public void AddStop(Stop stop)
        {
            _stopsBag.Add(stop);
        }

        public void AddPause(Pause pause)
        {
            _pausesBag.Add(pause);
        }

        public void AddResume(Resume resume)
        {
            _resumesBag.Add(resume);
        }

        public void AddBuffer(Buffer buffer)
        {
            _buffersBag.Add(buffer);
        }

        public void AddSeek(Seek seek)
        {
            _seeksBag.Add(seek);
        }
        */
        private void OnTimeElapsedEvent(Object stateInfo)
        {
            //if (_startsBag.Count > 9800)
            //{
            //_timer.Enabled = false;
            //}
            //else
            //{
            //    return;
            //}

            var startsToProcess = new ConcurrentBag<Start>();

            Interlocked.Exchange(ref startsToProcess, _startsBag);
            _startsBag = new ConcurrentBag<Start>();
            using (var connection = new NpgsqlConnection(connString))
            {
                connection.Open();
                using (var writer = connection.BeginTextImport("COPY dbo.\"Starts\"(datetimeadded) FROM STDIN;"))
                {
                    foreach (var start in startsToProcess)
                    {
                        writer.WriteLine(start.DateTimeAdded);
                    }
                    writer.Flush();
                }
            }
        }
        /*
        private bool HasAnyDataToSend()
        {
            return _list.Any() || _startsBag.Any() || _stopsBag.Any() ||
                _pausesBag.Any() || _resumesBag.Any() || _buffersBag.Any() || _seeksBag.Any();
        }
        */
    }
}