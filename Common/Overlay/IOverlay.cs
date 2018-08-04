﻿using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Overlay
{
    public interface IOverlay
    {
        IOverlayToken Token { get; }
        bool IsActive { get; }

        IOverlayToken DisplayOverlay<VM>(VM content) where VM : IViewModel;
    }
}