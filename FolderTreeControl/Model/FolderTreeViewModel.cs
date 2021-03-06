﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace GeekJ.FolderTreeControl.Model
{
    public class FolderTreeViewModel : ViewModelBase
    {
        private ObservableCollection<Drive> _drives;
        public ObservableCollection<Drive> Drives
        {
            get { return _drives; }
            set
            {
                _drives = value;
                OnPropertyChanged("Drives");
            }
        }

        public FolderTreeViewModel()
        {
            _drives = new ObservableCollection<Drive>();
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                _drives.Add(new Drive(driveInfo));
            }
        }
    }
}
