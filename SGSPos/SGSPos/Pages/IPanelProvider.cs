﻿using System.Windows.Forms;

namespace SGSPos.Pages
{
    public interface IPanelProvider
    {
        Panel PanelToSwitch { get; }

        void OnPageLoad();
    }

    public interface IFormProvider : IPanelProvider
    {
        Control.ControlCollection ControlReferenceList { get; }
    }
}