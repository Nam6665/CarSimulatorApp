using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLogicLibrary.DirectionStrategies.Interfaces;
using DataLogicLibrary.DTO;
using DataLogicLibrary.Infrastructure.Enums;

namespace DataLogicLibrary.DirectionStrategies
{
    public class DirectionContext : IDirectionContext
    {
        private IDirectionStrategy? _strategy;

        public void SetStrategy(IDirectionStrategy strategy)
        {
            _strategy = strategy;
        }

        public StatusDTO ExecuteStrategy(StatusDTO currentStatus)
        {
            if (_strategy is null)
                throw new InvalidOperationException("Strategy not set. Call SetStrategy() first."); //försäkra att _strategy inte är null

            return _strategy.Execute(currentStatus);
        }

    }
}
