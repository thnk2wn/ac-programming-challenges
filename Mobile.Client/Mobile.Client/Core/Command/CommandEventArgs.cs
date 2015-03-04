using System;

namespace AvantCredit.Uploader.Core.Command
{
    class CommandEventArgs : EventArgs
    {
        public CommandEventArgs()
        {
            Cancel = false;
        }

        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Event args that get passed to command execution
    /// </summary>
    class ExecuteCommandEventArgs : CommandEventArgs
    {
        /// <summary>
        ///  Argument to pass to command
        /// </summary>
        public object Argument;

        /// <summary>
        /// Result of Command Execution
        /// </summary>
        public object Result;

        /// <summary>
        /// Exception that occurred during execution
        /// </summary>
        public Exception Error;

    }

    class BeforeCommandEventArgs : CommandEventArgs
    {
        /// <summary>
        /// Arguments to pass to command execution
        /// </summary>
        public object Argument;
    }

    /// <summary>
    /// Event args that get fired off after command execution
    /// </summary>
    class AfterCommandEventArgs : CommandEventArgs
    {
        /// <summary>
        /// Constructor that takes any exceptions that might have occurred and the results
        /// </summary>
        /// <param name="result">Result of command execution</param>
        /// <param name="error">Errors that occured during execution</param>
        public AfterCommandEventArgs(object result, Exception error)
            : base()
        {
            this.Result = result;
            this.Error = error;
        }

        /// <summary>
        /// Result of Command Execution
        /// </summary>
        public object Result;

        /// <summary>
        /// Exception that occurred during execution
        /// </summary>
        public Exception Error;
    }
}