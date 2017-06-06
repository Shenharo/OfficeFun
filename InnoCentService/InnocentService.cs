using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using KeyReplacer;

namespace InnoCentService
{
    public partial class InnocentService : ServiceBase
    {
        private KeyReplacerService m_service;
        private KeyReplacerServiceSettings m_settingsObject;

        public InnocentService()
        {
            var conf = ConfigurationManager.AppSettings["Setting"];
            m_settingsObject = KeyReplacer.KeyReplacerServiceSettings.FromJson(conf);
            
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            m_service = new KeyReplacer.KeyReplacerService(m_settingsObject);
        }

        protected override void OnStop()
        {
            m_service.Dispose();
        }
    }
}
