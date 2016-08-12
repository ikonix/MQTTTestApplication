using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttTestAppl
{
    public class MqttClient
    {
        private uPLibrary.Networking.M2Mqtt.MqttClient client;

        public event Action<String> returnSuccess;
        public event Action<String> messageReceived;

        public MqttClient(string ip)
        {
            //create client instance
            client = new uPLibrary.Networking.M2Mqtt.MqttClient(ip);

            client.MqttMsgPublishReceived += mqttMessageReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
        }

        public uPLibrary.Networking.M2Mqtt.MqttClient Client {
            get
            {
                return client;
            }
        }
        
        public void subscribeTopic(string[] topic, byte qosLevel)
        {
            byte[] qos = new byte[1];
            qos[0] = qosLevel;
            client.Subscribe(topic, qos);

            returnSuccess("subscribe success");
        }

        public void publishTopic(string topic, string content, byte qosLevel, bool flag)
        {
            client.Publish(topic, Encoding.UTF8.GetBytes(content), qosLevel, flag);

            returnSuccess("publish success");
        }

        private void mqttMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string text = Encoding.UTF8.GetString(e.Message);
            messageReceived(text);
        }
    }
}
