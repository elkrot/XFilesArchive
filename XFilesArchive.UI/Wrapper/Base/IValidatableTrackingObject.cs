﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.UI.Wrapper
{
  public interface IValidatableTrackingObject :
    IRevertibleChangeTracking,
    INotifyPropertyChanged
  {
    bool IsValid { get; }
  }
}
