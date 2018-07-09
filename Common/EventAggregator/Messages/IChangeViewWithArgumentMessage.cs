using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventAggregator.Messages
{
    public interface IChangeViewWithArgumentMessage<T> : IChangeViewMessage<T>
    {
        void Apply(T viewModel);
        void Apply(IEnumerable<T> viewModels);
    }
}
