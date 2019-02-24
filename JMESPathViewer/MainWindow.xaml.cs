using DevLab.JmesPath;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace JMESPathViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JmesPath _jmes = new JmesPath();

        public MainWindow()
        {
            InitializeComponent();

            FilterTextBox.TextChanged += (o, e) => Transform();
            InputTextBox.TextChanged += (o, e) => Transform();
        }

        private void Transform()
        {
            ErrorTextBlock.Text = "";
            ErrorTextBlock.ToolTip = "";
            if (!string.IsNullOrWhiteSpace(FilterTextBox.Text) && !string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                try
                {
                    var text = _jmes.Transform(InputTextBox.Text, FilterTextBox.Text);
                    var obj = JsonConvert.DeserializeObject(text);
                    if (obj is null)
                    {
                        ErrorTextBlock.Text = "Output not updated due to improper filter";
                    }
                    else
                    {
                        OutTextBox.Text = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    }
                }
                catch (Exception e)
                {
                    ErrorTextBlock.Text = "[ERR]" + e.ToString();
                    ErrorTextBlock.ToolTip = e.ToString();
                }
            }
        }
    }
}
