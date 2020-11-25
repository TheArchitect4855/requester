using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Requester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSendRequestButtonClicked(object sender, RoutedEventArgs e)
        {
            string targetUrl = targetUrlInput.Text.Trim();
            RequestMethod requestMethod = (RequestMethod)requestMethodInput.SelectedIndex;
            ByteArrayContent requestBody = new ByteArrayContent(
                Encoding.UTF8.GetBytes(requestBodyInput.Text)
            );

            SendRequest(targetUrl, requestMethod, requestBody, responseOutput, responseCodeLabel);
        }

        private async void SendRequest(string target, RequestMethod method, HttpContent body, TextBox contentOutput, Label statusCodeOutput)
        {
            RequestManager manager = RequestManager.Get();
            RequestManager.Response response = await manager.SendRequest(MakeAbsolute(target), method, body);
            contentOutput.Text = response.content;
            statusCodeOutput.Content = $"Response Code: {response.statusCodeName} ({response.statusCode}).";
        }

        private string MakeAbsolute(string url)
        {
            if (url.StartsWith("https://") || url.StartsWith("http://"))
            {
                return url;
            }
            else
            {
                return $"https://{url}";
            }
        }
    }
}
