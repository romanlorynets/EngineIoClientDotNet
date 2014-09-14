﻿
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using EngineIoClientDotNet.Modules;
using Quobject.EngineIoClientDotNet.Modules;

namespace Quobject.EngineIoClientDotNet.Thread
{
    //from http://www.dailycoding.com/Posts/easytimer__javascript_style_settimeout_and_setinterval_in_c.aspx
    public class EasyTimer
    {


        private CancellationTokenSource ts;


        public EasyTimer(CancellationTokenSource ts)
        {
            this.ts = ts;
        }

        public static EasyTimer SetTimeout(Action method, int delayInMilliseconds)
        {
            var ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            var task = Task.Delay(delayInMilliseconds, ct);
            var awaiter = task.GetAwaiter();

            awaiter.OnCompleted(
                () =>
                {
                    if (!ts.IsCancellationRequested)
                    {
                        method();
                    }
                });


            // Returns a stop handle which can be used for stopping
            // the timer, if required
            return new EasyTimer(ts);
        }

        internal void Stop()
        {
            var log = LogManager.GetLogger(Global.CallerName());
            log.Info("EasyTimer stop");
            if (ts != null)
            {
                ts.Cancel();
            }          
        }


        //private DispatcherTimer timer;

        //public EasyTimer(DispatcherTimer timer)
        //{
        //    this.timer = timer;
        //}

        //public static EasyTimer SetTimeout(Action method, long delayInMilliseconds)
        //{
        //    var t = SetTimeoutAsync(method, delayInMilliseconds);
        //    t.Wait();
        //    return t.Result;
        //}

        //public static async Task<EasyTimer> SetTimeoutAsync(Action method, long delayInMilliseconds)
        //{
        //    //http://stackoverflow.com/questions/10579027/run-code-on-ui-thread-in-winrt
        //  //  var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
        //    //var dispatcher = Window.Current.Dispatcher;
        //    var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
        //    EasyTimer result = null;
        //    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        var timer1 = new DispatcherTimer();

        //        timer1.Interval = TimeSpan.FromMilliseconds(delayInMilliseconds);
        //        timer1.Tick += async (source, e) =>
        //        {
        //            timer1.Stop();
        //            //method();
        //            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => method());
        //        };

        //        timer1.Start();
        //        result = new EasyTimer(timer1);
        //    });
        //    return result;
        //}

        //internal async void Stop()
        //{
        //    var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
        //    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => timer.Stop());
        //}





    }


}

