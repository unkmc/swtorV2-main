using System;
using System.Windows.Forms;
using Memory;
using WindowsInput;
using SWTOR_External.Classes;
using MaterialSkin.Controls;
using System.Threading;

namespace SWTOR_External
{
    public partial class Form1 : MaterialForm
    {
        private readonly Mem m = new Mem();
        private readonly InputSimulator sim = new InputSimulator();
        private readonly Variables vars = new Variables();
        private readonly TimerHandlers timerHandlers;
        private readonly CheckboxHandlers checkboxHandlers;
        private readonly CoreFunctions coreFunctions;
        private readonly TrackbarHandlers trackbarHandlers;
        private readonly InputFieldHandlers inputFieldHandlers;
        private readonly ButtonHandlers buttonHandlers;
        private readonly HotkeyHandlers hotkeyHandlers;
        private readonly ScriptingEngine scriptingEngine;

        public Form1()
        {
            InitializeComponent();

            // Initialize handler classes with dependencies
            timerHandlers = new TimerHandlers(this, vars, m);
            checkboxHandlers = new CheckboxHandlers(this, vars, m);
            coreFunctions = new CoreFunctions(this, vars, m, sim);
            trackbarHandlers = new TrackbarHandlers(this, vars, m);
            inputFieldHandlers = new InputFieldHandlers(this, vars, m);
            buttonHandlers = new ButtonHandlers(this, vars, m, sim);
            hotkeyHandlers = new HotkeyHandlers(this, vars, m, sim);
            scriptingEngine = new ScriptingEngine(this, vars, m);

            // Start threads and timers via CoreFunctions
            Thread aobThread = new Thread(coreFunctions.ScanAOB) { IsBackground = true };
            Thread numpadTeleportThread = new Thread(coreFunctions.TeleportNumpad) { IsBackground = true };
            Thread hotkeyThread = new Thread(coreFunctions.HotkeysFunction) { IsBackground = true };
            aobThread.Start();
            numpadTeleportThread.Start();
            hotkeyThread.Start();

            // Initialize UI and settings via CoreFunctions
            coreFunctions.HoliCheck();
            coreFunctions.LoadHotkeys();
            coreFunctions.LoadLocations();
            coreFunctions.StartMainTimer();
            coreFunctions.StartGetBaseTimer();

            // Register process exit handler
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(coreFunctions.OnProcessExit);
        }

        // Event handlers for UI elements

        // Timer handlers
        private void pvpTimer_Tick(object sender, EventArgs e)
        {
            timerHandlers.PvpTimer_Tick(sender, e);
        }

        private void mainTimer_Tick_1(object sender, EventArgs e)
        {
            timerHandlers.MainTimer_Tick(sender, e);
        }

        private void timer_getBase_Tick(object sender, EventArgs e)
        {
            timerHandlers.TimerGetBase_Tick(sender, e);
        }

        // Checkbox handlers
        private void box_chatSpamMove_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxChatSpamMove_CheckedChanged(sender, e);
        }

        private void box_chatSpammer_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxChatSpammer_CheckedChanged(sender, e);
        }

        private void box_antiAfk_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxAntiAfk_CheckedChanged(sender, e);
        }

        private void box_infJump_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxInfJump_CheckedChanged(sender, e);
        }

        private void box_flyMode_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxFlyMode_CheckedChanged(sender, e);
        }

        private void box_noAnimations_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxNoAnimations_CheckedChanged(sender, e);
        }

        private void box_infReach_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxInfReach_CheckedChanged(sender, e);
        }

        private void box_noCollision_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxNoCollision_CheckedChanged(sender, e);
        }

        private void box_darkMode_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxDarkMode_CheckedChanged(sender, e);
        }

        private void cbox_noclip_CheckedChanged_1(object sender, EventArgs e)
        {
            checkboxHandlers.CboxNoclip_CheckedChanged(sender, e);
        }

        private void box_Freecam_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxFreecam_CheckedChanged(sender, e);
        }

        private void box_nofall_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxNofall_CheckedChanged(sender, e);
        }

        private void box_camAttach_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxCamAttach_CheckedChanged(sender, e);
        }

        private void box_alwaysInFront_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxAlwaysInFront_CheckedChanged(sender, e);
        }

        private void box_esp_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxEsp_CheckedChanged(sender, e);
        }

        private void box_dotEsp_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxDotEsp_CheckedChanged(sender, e);
        }

        private void box_wallhack_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxWallhack_CheckedChanged(sender, e);
        }

        private void box_glide_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxGlide_CheckedChanged(sender, e);
        }

        private void box_speedhack_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxSpeedhack_CheckedChanged(sender, e);
        }

        private void box_noCamCollision_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxNoCamCollision_CheckedChanged(sender, e);
        }

        private void box_noKnockback_CheckedChanged(object sender, EventArgs e)
        {
            checkboxHandlers.BoxNoKnockback_CheckedChanged(sender, e);
        }

        // Trackbar handlers
        private void trck_wFloor_Scroll(object sender, EventArgs e)
        {
            trackbarHandlers.TrckWFloor_Scroll(sender, e);
        }

        private void trckbar_camSpeed_Scroll(object sender, EventArgs e)
        {
            trackbarHandlers.TrckbarCamSpeed_Scroll(sender, e);
        }

        private void trckbr_speed_Scroll(object sender, EventArgs e)
        {
            trackbarHandlers.TrckbrSpeed_Scroll(sender, e);
        }

        private void trck_opcacity_Scroll(object sender, EventArgs e)
        {
            trackbarHandlers.TrckOpcacity_Scroll(sender, e);
        }

        // Input field handlers
        private void box_playerHeight_TextChanged(object sender, EventArgs e)
        {
            inputFieldHandlers.BoxPlayerHeight_TextChanged(sender, e);
        }

        private void txt_ZBox_TextChanged(object sender, EventArgs e)
        {
            inputFieldHandlers.TxtZBox_TextChanged(sender, e);
        }

        private void txt_YBox_TextChanged(object sender, EventArgs e)
        {
            inputFieldHandlers.TxtYBox_TextChanged(sender, e);
        }

        private void txt_XBox_TextChanged(object sender, EventArgs e)
        {
            inputFieldHandlers.TxtXbox_TextChanged(sender, e);
        }

        private void txtbox_TPUpKey_TextChanged(object sender, EventArgs e)
        {
            inputFieldHandlers.TxtboxTPUpKey_TextChanged(sender, e);
        }

        // Button handlers
        private void btn_clearConsole_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnClearConsole_Click(sender, e);
        }

        private void btn_saveLocation_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnSaveLocation_Click(sender, e);
        }

        private void btn_tpToCam_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnTpToCam_Click(sender, e);
        }

        private void btn_teleport_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnTeleport_Click(sender, e);
        }

        private void btn_saveCustomCoords_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnSaveCustomCoords_Click(sender, e);
        }

        private void btn_cancelTP_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnCancelTP_Click(sender, e);
        }

        private void btn_addCustomTP_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnAddCustomTP_Click(sender, e);
        }

        private void btn_setSelected_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnSetSelected_Click(sender, e);
        }

        private void btn_clearHotkeys_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnClearHotkeys_Click(sender, e);
        }

        private void btn_saveHotkeys_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnSaveHotkeys_Click(sender, e);
        }

        private void btn_saveLocations_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnSaveLocations_Click(sender, e);
        }

        private void btn_clearLocations_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnClearLocations_Click(sender, e);
        }

        private void btn_loadLocations_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnLoadLocations_Click(sender, e);
        }

        private void btn_removeLocation_Click(object sender, EventArgs e)
        {
            buttonHandlers.BtnRemoveLocation_Click(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            buttonHandlers.PictureBox1_Click(sender, e);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            buttonHandlers.Panel1_MouseDown(sender, e);
        }

        // Hotkey handlers
        private void txtbox_TpUpHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxTpUpHotkey_KeyDown(sender, e);
        }

        private void txtbox_TPDowNkey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxTPDowNkey_KeyDown(sender, e);
        }

        private void txtbox_TPLeftKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxTPLeftKey_KeyDown(sender, e);
        }

        private void txtbox_TPRightKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxTPRightKey_KeyDown(sender, e);
        }

        private void txtbox_TPForwardKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxTPForwardKey_KeyDown(sender, e);
        }

        private void txtbox_TPBackwardKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxTPBackwardKey_KeyDown(sender, e);
        }

        private void txtbox_freecamKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxFreecamKey_KeyDown(sender, e);
        }

        private void txtbox_tpToCamKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxTpToCamKey_KeyDown(sender, e);
        }

        private void txtbox_nofallKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxNofallKey_KeyDown(sender, e);
        }

        private void txtbox_glideKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxGlideKey_KeyDown(sender, e);
        }

        private void txtbox_speedKey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxSpeedKey_KeyDown(sender, e);
        }

        private void txtbox_flyforwardskey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxFlyforwardskey_KeyDown(sender, e);
        }

        private void txtbox_flybackwardskey_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxFlybackwardskey_KeyDown(sender, e);
        }

        private void txtbox_flyDown_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxFlyDown_KeyDown(sender, e);
        }

        private void txtbox_flyUp_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtboxFlyUp_KeyDown(sender, e);
        }

        private void txt_hotkeySpam_KeyDown(object sender, KeyEventArgs e)
        {
            hotkeyHandlers.TxtHotkeySpam_KeyDown(sender, e);
        }

        // Scripting engine handlers
        private void materialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            scriptingEngine.MaterialTabControl1_SelectedIndexChanged(sender, e);
        }

        private void lbl_Speed_Click(object sender, EventArgs e)
        {
            scriptingEngine.LblSpeed_Click(sender, e);
        }

        private void listbox_teleportLocations_DoubleClick(object sender, EventArgs e)
        {
            scriptingEngine.ListboxTeleportLocations_DoubleClick(sender, e);
        }
    }
}