using System;
using Android.Content;

namespace AvantCredit.Uploader.Core.Command
{
    class Command : NotifyPropertyChangedBase, ICommand
    {
        private string _name;
        private bool _isExecuting;
        private Exception _lastExecutionError;
        private bool _lastExecutionCancelled;
        private bool _rethrowException = true;

        public Command(Context context, string name)
        {
            Context = context;
            this.Name = name;
        }

        public Command(Context context) : this(context, null)
        {
        }

        /// <summary>
        /// Indicates that the command is currently executing
        /// </summary>
        public bool IsExecuting
        {
            get { return _isExecuting; }
            protected set
            {
                _isExecuting = value;
                this.NotifyPropertyChanged("IsExecuting");
            }
        }

        /// <summary>
        /// The indicates previous execution was cancelled
        /// </summary>
        public bool LastExecutionCancelled
        {
            get { return _lastExecutionCancelled; }
            protected set
            {
                _lastExecutionCancelled = value;
                this.NotifyPropertyChanged("LastExecutionCancelled");
            }
        }

        /// <summary>
        /// Exception that happened during last execution (null if no exception was encountered)
        /// </summary>
        public Exception LastExecutionError
        {
            get { return _lastExecutionError; }
            protected set
            {
                _lastExecutionError = value;
                this.NotifyPropertyChanged("LastExecutionError");
            }
        }

        /// <summary>
        /// Propagates exception errors up the stack without calling afterexecution
        /// Defaults to true (to preserve existing functionality)
        /// </summary>
        /// <remarks>this must be set to false for AfterExecution event to fire</remarks>
        public bool RethrowException
        {
            get { return _rethrowException; }
            set
            {
                _rethrowException = value;
                this.NotifyPropertyChanged("RethrowException");
            }
        }

        public Context Context { get; private set; }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Arguments to be passed to the command for execution
        /// </summary>
        public object Argument { get; set; }

        /// <summary>
        /// User-defined data associated with the command
        /// </summary>
        public object Tag { get; set; }

        public delegate void BeforeExecutionHandler(object sender, BeforeCommandEventArgs e);
        public delegate void ExecutionHandler(object sender, ExecuteCommandEventArgs e);
        public delegate void AfterExecutionHandler(object sender, AfterCommandEventArgs e);

        public event BeforeExecutionHandler BeforeExecution;
        public event ExecutionHandler Execution;
        public event AfterExecutionHandler AfterExecution;

        public void Execute()
        {
            var beforeEventArgs = new BeforeCommandEventArgs();
            this.OnBeforeExecution(beforeEventArgs);

            this.LastExecutionCancelled = beforeEventArgs.Cancel;
            this.LastExecutionError = null;

            if (!beforeEventArgs.Cancel)
            {
                if (beforeEventArgs.Argument != null)
                {
                    this.Argument = beforeEventArgs.Argument;
                }
                var executeArgs = new ExecuteCommandEventArgs { Argument = this.Argument };
                try
                {
                    IsExecuting = true;
                    this.OnExecution(executeArgs);

                }
                catch (Exception exc)
                {
                    executeArgs.Error = exc;
                    this.LastExecutionError = exc;
                    if (this.RethrowException)
                    {
                        throw;
                    }
                }
                finally
                {
                    IsExecuting = false;
                }

                if (!executeArgs.Cancel)
                {
                    var afterArgs = new AfterCommandEventArgs(executeArgs.Result, executeArgs.Error);
                    this.OnAfterExecution(afterArgs);
                }
            }
        }

        protected virtual void OnBeforeExecution(BeforeCommandEventArgs e)
        {
            if (BeforeExecution != null)
            {
                BeforeExecution(this, e);
            }
        }

        protected virtual void OnExecution(ExecuteCommandEventArgs e)
        {
            if (Execution != null)
            {
                Execution(this, e);
            }
        }

        protected virtual void OnAfterExecution(AfterCommandEventArgs e)
        {
            if (AfterExecution != null)
            {
                AfterExecution(this, e);
            }
        }
    }
}