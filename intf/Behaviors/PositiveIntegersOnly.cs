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
    public class PositiveIntegersOnly : DependencyObject
    {
        public static bool GetApply(DependencyObject obj)
        {
            return (bool)obj.GetValue(ApplyProperty);
        }


        public static void SetApply(DependencyObject obj, bool value)
        {
            obj.SetValue(ApplyProperty, value);
        }


        public static readonly DependencyProperty ApplyProperty =
            DependencyProperty.RegisterAttached(
                "Apply",
                typeof(bool),
                typeof(PositiveIntegersOnly),
                new PropertyMetadata(false, OnApplyPropertyChanged, OnCoerceApplyPropertyChanged)
            );


        private static void OnApplyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBox tb)) return;

            tb.PreviewTextInput -= Tb_PreviewTextInput;
            tb.PreviewKeyDown -= Tb_PreviewKeyDown;
            DataObject.RemovePastingHandler(tb, OnDataPastedHandler);

            tb.PreviewTextInput += Tb_PreviewTextInput;
            tb.PreviewKeyDown += Tb_PreviewKeyDown;
            DataObject.AddPastingHandler(tb, OnDataPastedHandler);
        }


        private static object OnCoerceApplyPropertyChanged(DependencyObject d, object baseValue)
        {
            return false;            
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

            if (result < 0) {
                e.CancelCommand();
                return;
            }
        }

    }
}
