using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeepAwake
{
    // ReSharper disable InconsistentNaming
    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
        // ReSharper restore InconsistentNaming
    }
    
    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        
        public MainForm()
        {
            InitializeComponent();
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            if ( SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | 
                                         EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                                         EXECUTION_STATE.ES_CONTINUOUS) == 0 )
            {
                MessageBox.Show("Error",
                                "SetThreadExecutionState failed.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                
                Environment.Exit(1);
            }
        }

        void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }
    }
}