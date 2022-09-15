using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace ElectronicLib
{
    public class AsyncOperation : IDisposable
    {
        public class Retrace
        {
            public bool IsBreak { get; set; }
        }

        private static int _ids;

        public static AsyncOperation CreateAsync(IEnumerator enumerator)
        {
            var s = new AsyncOperation(enumerator);
            s.Start();
            return s;
        }

        public static AsyncOperation CreateAsync(Action<Retrace> enter)
        {
            IEnumerator rator()
            {
                Retrace trace = new Retrace();
                trace.IsBreak = false;
                while (!trace.IsBreak)
                {
                    enter(trace);
                    yield return 0;
                }
            }
            return CreateAsync(rator());

        }

        public Dispatcher Dispatcher { get; }
        public IEnumerator Enumerator { get; private set; }
        /// <summary>
        /// Состояние асинхронности
        /// </summary>
        public bool IsAsync { get; private set; }
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int id { get; private set; }
        public bool IsDisposed { get; private set; }
        public AsyncOperation(IEnumerator enumerator)
        {
            if ((this.Enumerator = enumerator) == null)
            {
                throw new NullReferenceException("Аргумент возвратил Null");
            }
            this.Dispatcher = Dispatcher.CurrentDispatcher;
            this.id = _ids++;
        }

        private void _AsyncHandle()
        {
            IsAsync = true;
            void begin()
            {
                Dispatcher.BeginInvoke((Action)_async, DispatcherPriority.Background);
            }
            void _async()
            {
                if (!IsDisposed && IsAsync)
                {
                    IsAsync = this.Enumerator.MoveNext();
                    begin();
                }

            };

            begin();
        }

        public void Start()
        {
            if (IsAsync)
                throw new InvalidOperationException("Процесс уже выполняется. Вызовите метод Stop()");

            _AsyncHandle();
        }

        public void Stop()
        {
            IsAsync = false;
        }


        public void Dispose()
        {
            if (IsDisposed)
                return;

            Stop();
            id = -1;
            Enumerator = null;
            IsDisposed = true;

        }
    }
}
