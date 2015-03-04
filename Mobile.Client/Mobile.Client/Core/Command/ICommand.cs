using System;

namespace AvantCredit.Uploader.Core.Command
{
    interface ICommand
    {
        bool IsExecuting { get; }
        bool LastExecutionCancelled { get; }
        Exception LastExecutionError { get; }
        bool RethrowException { get; set; }
        string Name { get; set; }
        object Argument { get; set; }
        object Tag { get; set; }

        event Command.BeforeExecutionHandler BeforeExecution;
        event Command.ExecutionHandler Execution;
        event Command.AfterExecutionHandler AfterExecution;

        void Execute();
    }
}