using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Common.Behaviors
{
    public class AnimatedItemRemovalBehavior : DependencyObject
    {
        public static bool GetIsMarkedForDelete(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMarkedForDeleteProperty);
        }


        public static void SetIsMarkedForDelete(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMarkedForDeleteProperty, value);
        }


        public static readonly DependencyProperty IsMarkedForDeleteProperty =
            DependencyProperty.RegisterAttached(
                "IsMarkedForDelete",
                typeof(bool),
                typeof(AnimatedItemRemovalBehavior),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsMarkedForDeletePropertyChange))
            );


        private static void OnIsMarkedForDeletePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement el)) return;

            if ((bool)e.NewValue != true) {
                return;
            }

            Storyboard sb = GetStoryboard(d);
            if (sb == null) {
                throw new Exception("A Storyboard must be delcared!");
            }

            ICommand c = GetPerformRemoval(d);
            if (c == null) {
                throw new Exception("An action (ICommand) must be declared");
            }

            if (sb.IsSealed || sb.IsFrozen) {
                sb = sb.Clone();
            }

            Storyboard.SetTarget(sb, el);
            sb.Completed += (o, ea) => {
                var vm = el.DataContext;
                if (!c.CanExecute(vm)) {
                    return;
                }
                c.Execute(vm);
            };

            sb.Begin();
        }
        

        // -----
        

        public static Storyboard GetStoryboard(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(StoryboardProperty);
        }


        public static void SetStoryboard(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(StoryboardProperty, value);
        }


        public static readonly DependencyProperty StoryboardProperty =
            DependencyProperty.RegisterAttached(
                "Storyboard",
                typeof(Storyboard),
                typeof(AnimatedItemRemovalBehavior),
                null
            );


        // -----


        public static ICommand GetPerformRemoval(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(PerformRemovalProperty);
        }


        public static void SetPerformRemoval(DependencyObject obj, ICommand value)
        {
            obj.SetValue(PerformRemovalProperty, value);
        }

        
        public static readonly DependencyProperty PerformRemovalProperty =
            DependencyProperty.RegisterAttached(
                "PerformRemoval",
                typeof(ICommand),
                typeof(AnimatedItemRemovalBehavior),
                null
            );


    }
}
