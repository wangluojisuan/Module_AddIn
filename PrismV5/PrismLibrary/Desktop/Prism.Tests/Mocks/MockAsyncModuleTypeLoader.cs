// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.Composition.Tests.Mocks
{
    public class MockAsyncModuleTypeLoader : IModuleTypeLoader
    {
        private ManualResetEvent callbackEvent;

        public MockAsyncModuleTypeLoader(ManualResetEvent callbackEvent)
        {
            this.callbackEvent = callbackEvent;
        }

        public int SleepTimeOut { get; set; }

        public Exception CallbackArgumentError { get; set; }

        public bool CanLoadModuleType(ModuleInfo moduleInfo)
        {
            return true;
        }

        public void LoadModuleType(ModuleInfo moduleInfo)
        {
            Thread retrieverThread = new Thread(() =>
            {
                Thread.Sleep(SleepTimeOut);

                this.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, CallbackArgumentError));
                callbackEvent.Set();
            });
            retrieverThread.Start();
        }

        
        public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        private void RaiseLoadModuleProgressChanged(ModuleDownloadProgressChangedEventArgs e)
        {
            if (this.ModuleDownloadProgressChanged != null)
            {
                this.ModuleDownloadProgressChanged(this, e);
            }
        }

        public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

        private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
        {
            if (this.LoadModuleCompleted != null)
            {
                this.LoadModuleCompleted(this, e);
            }
        }
    }
}
