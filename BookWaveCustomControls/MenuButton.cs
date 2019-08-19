using System;
using System.Windows;
using System.Windows.Controls;

namespace BookWave.Controls
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
               
        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }
        public static readonly DependencyProperty PageTitleProperty =
            DependencyProperty.Register("PageTitle", typeof(string), typeof(MenuButton), new PropertyMetadata(String.Empty));




        public string TitleBarTemplate
        {
            get { return (string)GetValue(TitleBarTemplateProperty); }
            set { SetValue(TitleBarTemplateProperty, value); }
        }

        public static readonly DependencyProperty TitleBarTemplateProperty =
            DependencyProperty.Register("TitleBarTemplate", typeof(string), typeof(MenuButton), new PropertyMetadata("DefaultTitle"));



        #endregion

        static MenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuButton), new FrameworkPropertyMetadata(typeof(MenuButton)));            
        }
    }
}
