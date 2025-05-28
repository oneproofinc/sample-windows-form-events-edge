using System;
using System.Threading;
using System.Windows.Forms;

namespace EdgeUSBEvents
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); // Loads UI components from Designer
        }

        private void AppendTextSafe(TextBox tb, string text)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke(new Action(() => tb.AppendText(text)));
            }
            else
            {
                tb.AppendText(text);
            }
        }

        private void SetTextSafe(TextBox tb, string text)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke(new Action(() => tb.Text = text));
            }
            else
            {
                tb.Text = text;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            string json = @"
    {
        ""org.iso.18013.5.1"": {
            ""family_name"": false,
            ""given_name"": false,
            ""portrait"": false,
            ""issuing_country"": true,
            ""birth_date"": false,
            ""issuing_authority"": true
        }
    }";

            var service = new UsbEventService();
            bool verify = verifyCheckBox.Checked;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                service.StartMonitoring(json, (type, msg) =>
                {
                    string time = DateTime.Now.ToString("HH:mm:ss.fff");
                    string name = type == 0 ? "INFO" : service.GetEventTypeName(type);
                    string fullMsg = $"[{time}] {name}: {msg}";

                    AppendTextSafe(outputTextBox, fullMsg + Environment.NewLine);

                    if (type == 9) // DATA RECEIVED
                    {
                        SetTextSafe(dataTextBox, msg);
                    }
                }, verify, timeoutSeconds: 30);

            });
        }

    }
}
