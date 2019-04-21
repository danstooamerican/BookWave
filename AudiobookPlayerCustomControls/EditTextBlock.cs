using GalaSoft.MvvmLight.Command;
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
    [TemplatePart(Name = "txb", Type = typeof(TextBox))]
    public class EditTextBlock : Control
    {

        #region Dependency Properties

        public Visibility TextBlockVisibility
        {
            get { return (Visibility)GetValue(TextBlockVisibilityProperty); }
            set { SetValue(TextBlockVisibilityProperty, value); }
        }
        public static readonly DependencyProperty TextBlockVisibilityProperty =
            DependencyProperty.Register("TextBlockVisibility", typeof(Visibility), typeof(EditTextBlock), new PropertyMetadata(Visibility.Visible));

        public Visibility TextBoxVisibility
        {
            get { return (Visibility)GetValue(TextBoxVisibilityProperty); }
            set { SetValue(TextBoxVisibilityProperty, value); }
        }
        public static readonly DependencyProperty TextBoxVisibilityProperty =
            DependencyProperty.Register("TextBoxVisibility", typeof(Visibility), typeof(EditTextBlock), new PropertyMetadata(Visibility.Collapsed));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EditTextBlock), new PropertyMetadata("Hello World"));

        #endregion

        #region Props

        private TextBox txb;

        #endregion

        #region Commands

        public ICommand StartEditCommand { get; private set; }
        public ICommand EndEditCommand { get; private set; }

        #endregion

        static EditTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditTextBlock), new FrameworkPropertyMetadata(typeof(EditTextBlock)));
        }

        public EditTextBlock()
        {
            StartEditCommand = new RelayCommand(StartEdit);
            EndEditCommand = new RelayCommand(EndEdit);
        }

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            txb = GetTemplateChild("txb") as TextBox;

            if (txb == null)
            {
                throw new NullReferenceException("Template parts not available");
            }
        }

        private void StartEdit()
        {
            TextBlockVisibility = Visibility.Collapsed;
            TextBoxVisibility = Visibility.Visible;
            txb.Focus();
            txb.CaretIndex = txb.Text.Length;
        }

        private void EndEdit()
        {
            TextBoxVisibility = Visibility.Collapsed;
            TextBlockVisibility = Visibility.Visible;
        }

        #endregion

    }
}
