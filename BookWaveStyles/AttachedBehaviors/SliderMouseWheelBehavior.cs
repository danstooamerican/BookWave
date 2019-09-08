using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookWave.Styles.AttachedBehaviors
{
    public class SliderMouseWheelBehavior
    {
        public static double GetValue(Slider slider)
        {
            return (double)slider.GetValue(ValueProperty);
        }

        public static void SetValue(Slider slider, double value)
        {
            slider.SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(
            "Value",
            typeof(double),
            typeof(SliderMouseWheelBehavior),
            new UIPropertyMetadata(0.0, OnValueChanged));

        public static Slider GetSlider(UIElement parentElement)
        {
            return (Slider)parentElement.GetValue(SliderProperty);
        }

        public static readonly DependencyProperty SliderProperty =
            DependencyProperty.RegisterAttached(
            "Slider",
            typeof(Slider),
            typeof(SliderMouseWheelBehavior));


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Slider slider = d as Slider;
            if (slider != null)
            {
                slider.Loaded += (ss, ee) =>
                {
                    slider.PreviewMouseWheel += Slider_PreviewMouseWheel;
                };

                slider.Unloaded += (ss, ee) =>
                {
                    slider.PreviewMouseWheel -= Slider_PreviewMouseWheel;
                };
            }
        }

        private static void Slider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Slider slider = sender as Slider;

            double value = GetValue(slider);
            if (slider != null && value != 0)
            {
                slider.Value += slider.SmallChange * e.Delta / value;
            }
        }
    }
}
