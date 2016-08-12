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
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttTestAppl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MqttClient _cl;

        public MainWindow()
        {
            InitializeComponent();
        }

        public delegate void UpdateTextCallback(string message);
        private void publishClick(object sender, RoutedEventArgs e)
        {
            _cl.publishTopic(topicTextBox.Text, messageTextBox.Text, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        private void subscribeClick(object sender, RoutedEventArgs e)
        {
            _cl.subscribeTopic(contentTextBox.Text.Split(), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE);
        }

        private void connectClick(object sender, RoutedEventArgs e)
        {
            _cl = new MqttClient(ipTextBox.Text);
            _cl.returnSuccess += updateStatusTextBox;
            _cl.messageReceived += setTextBox;
        }

        private void updateStatusTextBox(String text)
        {
            statusTextBox.Text = text;
        }

        private void setTextBox(String text)
        {
            if (!receivedTextBox.CheckAccess()) { 
                receivedTextBox.Dispatcher.Invoke(new UpdateTextCallback(setTextBox), text);
                return;
            }

            receivedTextBox.Text = text;
        }
    }
}
