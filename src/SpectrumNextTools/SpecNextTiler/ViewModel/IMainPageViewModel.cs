using SpecNextTiler.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecNextTiler.ViewModel
{
    public interface IMainPageViewModel
    {
        string Title { get; set; }

        ObservableCollection<SpecColor> Colors { get; }
    }
}
