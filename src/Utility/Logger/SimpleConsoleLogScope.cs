using System;
using System.Threading;

namespace Utility.Logger
{
    public class SimpleConsoleLogScope
    {
        private static readonly AsyncLocal<SimpleConsoleLogScope> Scope = new AsyncLocal<SimpleConsoleLogScope>();
        private readonly string _name;
        private readonly object _state;

        public SimpleConsoleLogScope Parent { get; private set; }

        public static SimpleConsoleLogScope Current
        {
            get
            {
                return Scope.Value;
            }
            set
            {
                Scope.Value = value;
            }
        }

        internal SimpleConsoleLogScope(string name, object state)
        {
            _name = name;
            _state = state;
        }

        public static IDisposable Push(string name, object state)
        {
            var current = Current;
            Current = new SimpleConsoleLogScope(name, state) {Parent = current};
            return new DisposableScope();
        }

        public override string ToString()
        {
            var state = _state;
            return state.ToString();
        }

        private class DisposableScope : IDisposable
        {
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}
