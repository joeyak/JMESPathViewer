using DevLab.JmesPath;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.IO;

namespace JMESPathViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string _path;
        private JmesPath _jmes = new JmesPath();

        public MainWindow()
        {

            InitializeComponent();

            FilterTextBox.TextChanged += (o, e) => Transform();
            InputTextBox.TextChanged += (o, e) => Transform();
            TogglePath.IsCheckedChanged += (o, e) =>
            {
                PathTextBox.IsEnabled = TogglePath.IsChecked ?? false;
                _path = PathTextBox.IsEnabled ? PathTextBox.Text : null;
            };
            PathTextBox.TextChanged += (o, e) =>
            {
                var path = PathTextBox.Text;
                if (File.Exists(path) && Path.GetExtension(path).ToLower() == ".json")
                {
                    ReadFile(path);
                }
            };
        }

        private void ReadFile(string path) => InputTextBox.Text = File.ReadAllText(path);

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
