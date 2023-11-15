using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicoTeste
{
    public partial class ServicoTeste2 : ServiceBase
    {
        public ServicoTeste2()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread.Sleep(10000);
        }

        protected override void OnStop()
        {
            Thread.Sleep(10000);
        }
    }
}
