using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kort.MyStates
{
    public class MySingletonState
    {
        public event EventHandler<RrChangedEventArgs> RrChanged;
        // To subscribe this event handler
        // MySingltonState.RrChanged += localFunction to call
        private void RrHasChanged(RrChangedEventArgs e)
        {
            // This will update any subscribers
            // that the counter state has changed
            // so they can update themselves
            RrChanged?.Invoke(this, e);
        }

        public void RrChange(Guid guid, int aaID, DateTime trh)
        {
            RrHasChanged(new RrChangedEventArgs { Guid = guid, AaID = aaID, Trh = trh });
        }
    }

    public class RrChangedEventArgs : EventArgs
    {
        public Guid Guid { get; set; }
        public int AaID { get; set; }
        public DateTime Trh { get; set; }
    }
}
