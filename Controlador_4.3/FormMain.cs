using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Controlador_4._3
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //carregar combo de server

            List<string> servers = new List<string>();

            servers.Add("SWXTP0134CLD - 175.175.175.175");
            servers.Add("SWXTP0135CLD - 175.175.175.175");
            servers.Add("BBAQUANTN1 - 175.175.175.175");
            servers.Add("BBAQUANTN2 - 175.175.175.175");

            cmbServer.Items.AddRange(servers.ToArray());
            cmbServer.SelectedIndex = 0;

            //carregar linhas

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Service Name";
            dataGridView1.Columns[0].Width = 310;
            dataGridView1.Columns[1].Name = "Status";

            foreach (ServiceStatus s in GetListServiceStatus())
            {
                dataGridView1.Rows.Add(s.ServiceName, s.Status);
            }


            // config button column

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "";
            btn.Text = "Stop/Start";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btn);

            dataGridView1.CellClick += dataGridView1_CellClick;

        }

        private List<ServiceStatus> GetListServiceStatus()
        {
            List<ServiceStatus> serviceStatuses = new List<ServiceStatus>();

            serviceStatuses.Add(new ServiceStatus() { ServiceName = "MQ BPIPE ASSINATURA DINAMICA", Status = "Running" });
            serviceStatuses.Add(new ServiceStatus() { ServiceName = "MQ UMDF ASSINATURA DINAMICA", Status = "Stopped" });
            serviceStatuses.Add(new ServiceStatus() { ServiceName = "MQ Autorizador", Status = "Running" });

            return serviceStatuses;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "|" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            }
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            dataGridView1.Size = new Size(530, (this.Size.Height - (107)));
        }
    }

    public class ServiceStatus
    {
        public string ServiceName { get; set; }
        public string Status { get; set; }
    }
}