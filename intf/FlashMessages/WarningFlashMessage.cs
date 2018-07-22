﻿using Common.FlashMessages;
using intf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.FlashMessages
{
    public class WarningFlashMessage : FlashMessage
    {
        public WarningFlashMessage(string message) : base(message)
        {
            _type = Common.FlashMessages.Type.WARNING;
            _viewModel = new FlashMessageWarningViewModel(this);
        }
    }
}
