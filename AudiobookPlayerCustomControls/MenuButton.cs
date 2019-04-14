using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Commons.Controls
{
    public class MenuButton : Button
    {
        public Visibility ClickedRectVisibility
        {
            get { return (Visibility)GetValue(ClickedRectVisibilityProperty); }
            set {
                if (!ClickedRectVisibilityProperty.Equals(value))
                {
                    SetValue(ClickedRectVisibilityProperty, value);
                }
                
            }
        }

        public static readonly DependencyProperty ClickedRectVisibilityProperty =
            DependencyProperty.Register("ClickedRectVisibility", typeof(Visibility), typeof(MenuButton), new PropertyMetadata(Visibility.Hidden));

        static MenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuButton), new FrameworkPropertyMetadata(typeof(MenuButton)));            
        }
    }
}
