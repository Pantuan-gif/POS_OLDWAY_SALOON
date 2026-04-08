using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_OLDWAY_SALOON.MVVM.MODELS
{
    class FlyoutMenuItem
    {
        //Flyout Models
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetPage { get; set; }

        public string currentId { get; set; }  
    }

}
