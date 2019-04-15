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
        #region DependencyProperties

        /// <summary>
        /// If set to visible a small rectangle on the left side of the button is shown. 
        /// This property triggers a fadeIn animation only if the value is changed.
        /// </summary>
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

        /// <summary>
        /// Page which is loaded after the MenuButton is clicked.
        /// </summary>
        public string Page
        {
            get { return (string)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(string), typeof(MenuButton), new PropertyMetadata(String.Empty));

        #endregion

        static MenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuButton), new FrameworkPropertyMetadata(typeof(MenuButton)));            
        }
    }
}
