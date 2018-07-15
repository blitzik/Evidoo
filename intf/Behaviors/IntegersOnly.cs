using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace intf.Behaviors
{
    public enum IntegersOnlyMode
    {
        ALL,
        POSITIVE,
        NEGATIVE
    }


    public class IntegersOnly : DependencyObject
    {
        public static IntegersOnlyMode GetMode(DependencyObject obj)
        {
            return (IntegersOnlyMode)obj.GetValue(ModeProperty);
        }


        public static void SetMode(DependencyObject obj, IntegersOnlyMode value)
        {
            obj.SetValue(ModeProperty, value);
        }


        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.RegisterAttached(
                "Mode",
                typeof(IntegersOnlyMode),
                typeof(IntegersOnly),
                new PropertyMetadata(default(IntegersOnlyMode), null, OnCoerceModePropertyChanged)
            );


        private static object OnCoerceModePropertyChanged(DependencyObject d, object baseValue)
        {
            if (!(d is TextBox tb)) return baseValue;

            tb.PreviewTextInput -= Tb_PreviewTextInput;
            tb.PreviewKeyDown -= Tb_PreviewKeyDown;
            DataObject.RemovePastingHandler(tb, OnDataPastedHandler);

            tb.PreviewTextInput += Tb_PreviewTextInput;
            tb.PreviewKeyDown += Tb_PreviewKeyDown;
            DataObject.AddPastingHandler(tb, OnDataPastedHandler);

            return baseValue;            
        }


        private static void Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(sender is TextBox tb)) return;

            if (!int.TryParse(e.Text, out int result)) {
                e.Handled = true;
                return;
            }
        }


        private static void Tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox tb)) return;

            if (e.Key == Key.Space)
                e.Handled = true;
        }


        private static void OnDataPastedHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (!(sender is TextBox tb)) return;

            if (!e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true)) {
                e.CancelCommand();
                return;
            }

            string data = e.SourceDataObject.GetData(DataFormats.UnicodeText, true).ToString();
            if (!int.TryParse(data, out int result)) {
                e.CancelCommand();
                return;
            }
        }

    }
}
