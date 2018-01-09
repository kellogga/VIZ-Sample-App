using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;

namespace Maestro.BaseLibrary.DBLogic
{
    internal class DDIQAccessController
    {
        //private DBDataIntegrator _ddiq;
        private BackgroundWorker _activeWorker;
        private BackgroundWorker _activeWorkerHighCnn;
        private System.Timers.Timer _timeOut = new System.Timers.Timer();
        private System.Timers.Timer _timeOutHighCnn = new System.Timers.Timer();
        private System.Timers.Timer _timeExecuteNextWorker = new System.Timers.Timer();
        private Queue<ThreadDef> _lowQueue = new Queue<ThreadDef>();
        private Queue<ThreadDef> _normalQueue = new Queue<ThreadDef>();
        private Queue<ThreadDef> _highQueue = new Queue<ThreadDef>();
        private System.Threading.Mutex _mutQueues = new System.Threading.Mutex();
        private object _activeBackWorkersLock = new object();
        private bool _exceptionAlreadyShown;

        internal enum Priority
        {
            /// <summary>
            /// Instant connection.  Reserved for user interaction.
            /// </summary>
            /// <remarks></remarks>
            High = 1,
            /// <summary>
            /// Normal priority.  Reserved for process Automatic report and decision desk simulator.
            /// </summary>
            /// <remarks></remarks>
            Normal = 2,
            /// <summary>
            /// Background work.  Autorefresh process used this priority.
            /// </summary>
            /// <remarks></remarks>
            Low = 3
        }
        public DDIQAccessController(bool useLocalConfigurationFileOnly)
        {
            //_ddiq = new DBDataIntegrator(useLocalConfigurationFileOnly);
            _timeOut.AutoReset = false;
            _timeOutHighCnn.AutoReset = false;
            _timeExecuteNextWorker.AutoReset = false;
            _timeExecuteNextWorker.Interval = 100;
            _closed = false;
        }
        private bool _closed;
        public bool Closed { get { return _closed; } }

        public void Close()
        {
            _closed = true;
            try
            {
                _timeOut.Stop();
                _timeOutHighCnn.Stop();
                _mutQueues.WaitOne();
                _lowQueue.Clear();
                _normalQueue.Clear();
                _highQueue.Clear();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _mutQueues.ReleaseMutex();
            }
        }
        private void ThreadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker SenderWorker = default(BackgroundWorker);

            lock (_activeBackWorkersLock)
            {
                SenderWorker = sender as BackgroundWorker;
                if (SenderWorker != null)
                {
                    if (object.ReferenceEquals(SenderWorker, _activeWorker))
                    {
                        _timeOut.Stop();
                        _activeWorker.RunWorkerCompleted -= ThreadCompleted;
                        _activeWorker = null;

                    }
                    else if (object.ReferenceEquals(SenderWorker, _activeWorkerHighCnn))
                    {
                        _timeOutHighCnn.Stop();
                        _activeWorkerHighCnn.RunWorkerCompleted -= ThreadCompleted;
                        _activeWorkerHighCnn = null;
                    }
                }
                //Execute next worker
                ExecuteNextWorker();
            }
        }

        private void ExecuteNextWorker()
        {
            //Execute next worker
            if (!this.Closed)
            {
                try
                {
                    //Rules:
                    //If both background worker references are null, execute the next worker
                    //If only the High worker reference is null, execute a worker in the high queue
                    //If only the ActiveWorker reference is null, does nothing.
                    _mutQueues.WaitOne();

                    if ((_activeWorker == null) & (_activeWorkerHighCnn == null))
                    {
                        if (!(ExecuteWorker(_highQueue)))
                        {
                            if (!(ExecuteWorker(_normalQueue)))
                            {
                                ExecuteWorker(_lowQueue);
                            }
                        }
                    }
                    else
                    {
                        if ((_activeWorkerHighCnn == null))
                        {
                            ExecuteWorker(_highQueue);
                        }
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    _mutQueues.ReleaseMutex();
                }
            }
        }

        internal void ExecuteAllWorkerInHighQueue()
        {
            ThreadDef T = null;

            lock (_activeBackWorkersLock)
            {
                try
                {
                    _mutQueues.WaitOne();
                    while ((_highQueue.Count > 0))
                    {
                        T = _highQueue.Dequeue();

                        if ((T.Worker != null) && !(T.RemovedFromQueue) && !(T.Worker.CancellationPending) & !(T.Worker.IsBusy))
                        {
                            if (_activeWorkerHighCnn == null)
                            {
                                _activeWorkerHighCnn = T.Worker;
                                _activeWorkerHighCnn.RunWorkerCompleted += ThreadCompleted;
                                _timeOutHighCnn.Interval = 1000 * T.TimeOut;
                                _timeOutHighCnn.Start();
                            }

                            if (T.DoWorkArgument != null)
                            {
                                T.Worker.RunWorkerAsync(T.DoWorkArgument);
                            }
                            else
                            {
                                T.Worker.RunWorkerAsync();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    _mutQueues.ReleaseMutex();
                }
            }
        }

        private void TimeOut_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_activeBackWorkersLock)
            {
                ThreadCompleted(_activeWorker, null);
            }
        }

        private void TimeOutHighCnn_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_activeBackWorkersLock)
            {
                ThreadCompleted(_activeWorkerHighCnn, null);
            }
        }

        private void TimeExecuteNextWorker_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //This timer should be started only in the ExecuteWorker method.  This
            //timer is started only when the DDIAccessController wait for a busy worker to finish.
            lock (_activeBackWorkersLock)
            {
                ExecuteNextWorker();
            }
        }

        private bool ExecuteWorker(Queue<ThreadDef> curQueue)
        {

            ThreadDef t = null;
            bool found = false;
            bool isBusy = false;

            //WARNING: This function should be called inside a synclock on ActiveBackWorkersLock

            //Find a worker in queue.  If the first worker found is busy, let the worker in queue, and do not
            //execute it.  This way, the Access controller will wait until that worker finish its execution
            //before executing a worker in this queue and lower priority queue.

            while (!(found) & (curQueue.Count > 0))
            {
                t = curQueue.Peek();
                if ((t.Worker != null) && !(t.RemovedFromQueue) && !(t.Worker.CancellationPending))
                {
                    found = true;
                    if (t.Worker.IsBusy)
                    {
                        isBusy = true;
                    }
                }

                if (!isBusy)
                {
                    t = curQueue.Dequeue();
                }
            }

            //If a worker is found, execute it
            if (found & !(isBusy))
            {
                //Depending if the queue is the high queue or not...
                if (object.ReferenceEquals(curQueue, _highQueue))
                {
                    _activeWorkerHighCnn = t.Worker;
                    _activeWorkerHighCnn.RunWorkerCompleted += ThreadCompleted;
                    _timeOutHighCnn.Interval = 1000 * t.TimeOut;
                    _timeOutHighCnn.Start();
                }
                else
                {
                    _activeWorker = t.Worker;
                    _activeWorker.RunWorkerCompleted += ThreadCompleted;
                    _timeOut.Interval = 1000 * t.TimeOut;
                    _timeOut.Start();
                }

                if (t.DoWorkArgument != null)
                {
                    t.Worker.RunWorkerAsync(t.DoWorkArgument);
                }
                else
                {
                    t.Worker.RunWorkerAsync();
                }
            }

            //Return found even if the worker has'nt been execute because IsBusy = true.  That will ensure 
            //to stop the execution of worker in this queue and lower priority queue.  In that particular case,  
            //because the ActiveWorker and/or the ActiveWorkerHighCnn could be nothing, start a timer to ensure 
            //that will cal the ExecuteNextWorker method.

            if (found & isBusy)
            {
                if (!(_timeExecuteNextWorker.Enabled))
                {
                    _timeExecuteNextWorker.Start();
                }
            }
            return found;
        }

        internal void InsertThreadInQueue(ThreadDef threadToInsert)
        {
            try
            {
                _mutQueues.WaitOne();
                if (threadToInsert.UniqueWorkerInQueue)
                {
                    //Look in all queues if a reference to same worker (ThreadToInsert.Worker) exists.
                    //When a thread for this worker already exists in a queue, remove it from the
                    //queue if the thread to insert has a higher priority.  Otherwise, replace the 
                    //information of the old thread with the one to insert.
                    if (!(ReplaceThreadInQueue(threadToInsert, _lowQueue)))
                    {
                        if (!(ReplaceThreadInQueue(threadToInsert, _normalQueue)))
                        {
                            if (!(ReplaceThreadInQueue(threadToInsert, _highQueue)))
                            {
                                //If the thread has not replaced any old thread, add a new thread to the queue
                                switch (threadToInsert.ConnectionPriority)
                                {
                                    case Priority.High:
                                        _highQueue.Enqueue(threadToInsert);
                                        break;
                                    case Priority.Normal:
                                        _normalQueue.Enqueue(threadToInsert);
                                        break;
                                    case Priority.Low:
                                        _lowQueue.Enqueue(threadToInsert);
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //If the worker is not supposed to be unique, just insert the thread in queue
                    switch (threadToInsert.ConnectionPriority)
                    {
                        case Priority.High:
                            _highQueue.Enqueue(threadToInsert);
                            break;
                        case Priority.Normal:
                            _normalQueue.Enqueue(threadToInsert);
                            break;
                        case Priority.Low:
                            _lowQueue.Enqueue(threadToInsert);
                            break;
                    }
                }

                //Will execute the inserted worker if it is the time to do so
                ExecuteNextWorker();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _mutQueues.ReleaseMutex();
            }

        }
        private bool ReplaceThreadInQueue(ThreadDef threadToInsert, Queue<ThreadDef> curQueue)
        {
            ThreadDef threadFound = null;
            bool threadReplaced = false;

            //Look for the thread with the same process
            foreach (ThreadDef t in curQueue)
            {
                if ((object.ReferenceEquals(t.Worker, threadToInsert.Worker)) & !(t.RemovedFromQueue))
                {
                    threadFound = t;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            if (threadFound != null)
            {
                //When the priority of the thread to process 
                if ((threadToInsert.ConnectionPriority - threadFound.ConnectionPriority) < 0)
                {
                    //When the priority to insert is greater than the one in queue, remove the one in queue
                    threadFound.RemovedFromQueue = true;
                }
                else
                {
                    threadFound.DoWorkArgument = threadToInsert.DoWorkArgument;
                    threadFound.TimeOut = threadToInsert.TimeOut;
                    threadReplaced = true;
                }
            }
            return threadReplaced;
        }

        //public string GetSQL(string key)
        //{
        //    NameValueCollection col = _ddiq.GetPredefinedSQLStatement(key);
        //    return col[0].ToString();
        //}

        //public string GetParameterValue(string name, string defaultValue = "", bool generateExceptionOnDDIError = true, bool showMessageOnDDIError = true)
        //{
        //    string retval = string.Empty;
        //    try
        //    {
        //        retval = _ddiq.GetParameterValue(name, defaultValue);
        //    }
        //    catch (Exception)
        //    {
        //        //Retourne l'erreur d'exécution de cette méthode si dbDataUIntegrator is started correctly 
        //        if (VerifyDbDataIntegratorStarted(_ddiq, showMessageOnDDIError, generateExceptionOnDDIError))
        //        {
        //            throw;
        //        }
        //    }
        //    return retval;
        //}

        //internal bool WorkOffLine(bool activate)
        //{
        //    bool functionReturnValue = false;
        //    bool Succeed = false;
        //    Succeed = false;
        //    try
        //    {
        //        if (activate == true)
        //        {
        //            Succeed = CheckDBDataIntegrator(_ddiq, false, true);
        //        }
        //        else
        //        {
        //            _ddiq.Stop();
        //        }
        //        functionReturnValue = Succeed;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return functionReturnValue;
        //}

        //internal int GetData(bool predefinedSQLStatement, ref string sqlStatement, DataTable dataTable, string tableName = "", bool clearData = true, NameValueCollection sqlParameters = null, string selectClause = "", string whereClause = "", string orderByClause = "", bool generateExceptionOnDDIError = true, bool ShowMessageOnDDIError = true, bool ShowMessageOnGetDataError = false)
        //{
        //    int intRows = 0;
        //    try
        //    {
        //        intRows = _ddiq.GetData(predefinedSQLStatement, ref sqlStatement, dataTable, tableName, clearData, sqlParameters, selectClause, whereClause, orderByClause);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (VerifyDbDataIntegratorStarted(_ddiq, ShowMessageOnDDIError, generateExceptionOnDDIError))
        //        {
        //            try
        //            {
        //                GenericExceptionHandler.GetInstance().Log(ex, GenericExceptionHandler.ExceptionLogLocation.Server | GenericExceptionHandler.ExceptionLogLocation.LocalFile);
        //                if (ShowMessageOnGetDataError)
        //                {
        //                    //UI.Message.Show(ApplicationMessage.UnknownErrorExecuteQuery, GenericComponentsDDIQ)
        //                    //UI.Message.Show(50010, GenericComponentsDDIQ);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //            }
        //            throw;
        //        }
        //    }
        //    return intRows;
        //}

        //public bool VerifyDBDataIntegrator(DBDataIntegrator ddiQuery, bool showMessageOnFaillure = false)
        //{
        //    return CheckDBDataIntegrator(ddiQuery, showMessageOnFaillure);
        //}

        //public bool VerifyDbDataIntegratorStarted(DBDataIntegrator ddiQuery, bool showMessageOnFaillure = false, bool generateExceptionError = true)
        //{
        //    bool succeeded = true;
        //    DbDataIntegratorException dbDataException = default(DbDataIntegratorException);
        //    try
        //    {
        //        succeeded = ddiQuery.Started;
        //        if (succeeded)
        //        {
        //            _exceptionAlreadyShown = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GenericExceptionHandler.GetInstance().Log(ex);
        //        succeeded = false;
        //    }

        //    if (!(succeeded))
        //    {
        //        if (_exceptionAlreadyShown == false)
        //        {
        //            dbDataException = new DbDataIntegratorException();
        //            _exceptionAlreadyShown = true;
        //            GenericExceptionHandler.GetInstance().Log(dbDataException, GenericExceptionHandler.ExceptionLogLocation.LocalFile);
        //            if (showMessageOnFaillure)
        //            {
        //                //MsgBox(GetTextByLanguage(cProblemStartingDDIMsgFr, cProblemStartingDDIMsgEn), MsgBoxStyle.Exclamation, "Spectrum")
        //            }

        //            if (generateExceptionError)
        //            {
        //                throw dbDataException;
        //            }
        //        }
        //    }
        //    return succeeded;
        //}

        //private bool CheckDBDataIntegrator(DBDataIntegrator ddiQuery, bool showMessageOnFaillure = false, bool forceStart = false)
        //{
        //    bool succeeded = true;
        //    try
        //    {
        //        //YM2007 Start Spectrum even DBDataIntegrator is not started 
        //        if (forceStart)
        //        {
        //            succeeded = ddiQuery.Start();
        //        }
        //        else
        //        {
        //            succeeded = ddiQuery.WaitForStatus(CBCSRC.DBDataIntegratorQuery.DBDataIntegrator.DDIStatus.CompleteDataUpdateFinished);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GenericExceptionHandler.GetInstance().Log(ex);
        //        succeeded = false;
        //    }
        //    if (!(succeeded))
        //    {
        //        GenericExceptionHandler.GetInstance().Log(new DbDataIntegratorException(), GenericExceptionHandler.ExceptionLogLocation.LocalFile | GenericExceptionHandler.ExceptionLogLocation.Server);
        //    }
        //    return succeeded;
        //}

        public void StartBackGroundWorker(int entityId, bool isUserInteractive, BackgroundWorker backWorker)
        {
            StartBackGroundWorker(entityId, isUserInteractive, backWorker);
        }

        public void StartBackGroundWorker(object doWorkArgument, bool isUserInteractive, ref BackgroundWorker backWorker)
        {
            DDIQAccessController.Priority CnnPriority = default(DDIQAccessController.Priority);
            if (isUserInteractive)
            {
                CnnPriority = DDIQAccessController.Priority.High;
            }
            else
            {
                CnnPriority = DDIQAccessController.Priority.Low;
            }
            StartBackGroundWorker(doWorkArgument, CnnPriority, ref backWorker);
        }
        internal void StartBackGroundWorker(object doWorkArgument, DDIQAccessController.Priority cnnPriority, ref BackgroundWorker backWorker)
        {
            lock (_activeBackWorkersLock)
            {
                InsertThreadInQueue(new ThreadDef(backWorker, doWorkArgument, cnnPriority, 0, true));
            }
        }
    }

    internal class ThreadDef
    {
        /// <summary>
        /// Private variable for the Worker property.
        /// </summary>
        /// <remarks></remarks>
        private BackgroundWorker _worker;
        /// <summary>
        /// Gets or sets the reference to the background worker to execute
        /// </summary>
        /// <remarks></remarks>
        public BackgroundWorker Worker
        {
            get
            {
                return _worker;
            }
            set { _worker = value; }
        }

        /// <summary>
        /// Private variable for the DoWorkArgument property.
        /// </summary>
        /// <remarks></remarks>
        private object _doWorkArgument;
        /// <summary>
        /// Gets or sets the parameter for use by the background operation to be executed in the 
        /// DoWork event handler. 
        /// </summary>
        /// <remarks></remarks>
        public object DoWorkArgument
        {
            get { return _doWorkArgument; }
            set { _doWorkArgument = value; }
        }

        /// <summary>
        /// Private variable for the ConnectionPriority property.
        /// </summary>
        /// <remarks></remarks>
        private DDIQAccessController.Priority _connectionPriority;
        /// <summary>
        /// Gets or sets the connection priority
        /// </summary>
        /// <remarks></remarks>
        public DDIQAccessController.Priority ConnectionPriority
        {
            get { return _connectionPriority; }
            set { _connectionPriority = value; }
        }

        /// <summary>
        /// Private variable for the TimeOut property.
        /// </summary>
        /// <remarks></remarks>
        private double _timeOut;
        /// <summary>
        /// Gets or sets the maximum number of seconds the DDIAccesController will wait for the
        /// RunWorkerCompleted event handler before starting another worker.
        /// </summary>
        /// <remarks></remarks>
        public double TimeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }

        /// <summary>
        /// Private variable for the RemovedFromQueue property.
        /// </summary>
        /// <remarks></remarks>
        private bool _removedFromQueue = false;
        /// <summary>
        /// Gets or sets wheter to execute or not the worker.  Used as a logical delete in the queue.
        /// </summary>
        /// <remarks></remarks>
        public bool RemovedFromQueue
        {
            get { return _removedFromQueue; }
            set { _removedFromQueue = value; }
        }

        /// <summary>
        /// Private variable for the UniqueWorkerInQueue property.
        /// </summary>
        /// <remarks></remarks>
        private bool _uniqueWorkerInQueue;
        /// <summary>
        /// Gets or sets wheter to restrict to one instance of the worker in all queues.
        /// </summary>
        /// <remarks></remarks>
        public bool UniqueWorkerInQueue
        {
            get { return _uniqueWorkerInQueue; }
            set { _uniqueWorkerInQueue = value; }
        }


        public ThreadDef(BackgroundWorker worker, object doWorkArgument, DDIQAccessController.Priority connectionPriority, double timeOut = 0, bool uniqueWorkerInQueue = false)
        {
            _worker = worker;
            _doWorkArgument = doWorkArgument;
            _connectionPriority = connectionPriority;
            if (timeOut == 0)
            {
                _timeOut = 0.5;
            }
            else
            {
                _timeOut = timeOut;
            }
            _uniqueWorkerInQueue = uniqueWorkerInQueue;
        }
    }

    public class DbDataIntegratorException : Exception
    {
        string _message;
        public override string Message
        {
            get { return _message; }
        }

        private string _stackTrace;
        public override string StackTrace
        {
            get { return _stackTrace; }
        }

        public DbDataIntegratorException() : base()
        {
            _stackTrace = Environment.StackTrace;
            _message = "The database doesn't exist or the processor (DDIP) is busy.";
            //m_Message = GetTextByLanguage("La base de données est inexistante ou le processeur (DDIP) est occupé.", "The database doesn't exists or the processor (DDIP) is busy.")
        }
    }
}
