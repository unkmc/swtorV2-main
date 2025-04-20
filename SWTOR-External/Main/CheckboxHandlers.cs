using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MaterialSkin;
using Memory;

namespace SWTOR_External.Classes
{
    public class CheckboxHandlers
    {
        private readonly Form1 _form;
        private readonly Variables _vars;
        private readonly Mem _mem;

        public CheckboxHandlers(Form1 form, Variables vars, Mem mem)
        {
            _form = form;
            _vars = vars;
            _mem = mem;
        }

        public void BoxChatSpamMove_CheckedChanged(object sender, EventArgs e)
        {
            _vars.moveAfterMsg = !_vars.moveAfterMsg;
        }

        public void BoxChatSpammer_CheckedChanged(object sender, EventArgs e)
        {
            Thread msgSpammerThread = new Thread(_form.chatSpamLoop) { IsBackground = true };
            _vars.msgSpamFlag = !_vars.msgSpamFlag;

            if (_vars.msgSpamFlag)
            {
                msgSpammerThread.Start();
            }
        }

        public void BoxAntiAfk_CheckedChanged(object sender, EventArgs e)
        {
            if (!_vars.antiAFK)
            {
                MessageBox.Show("Make sure you are tabbed into the game", "Quick tip", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            _vars.antiAFK = !_vars.antiAFK;
            _vars.dateTimeBuffer = DateTime.Now;
        }

        public void BoxInfJump_CheckedChanged(object sender, EventArgs e)
        {
            _vars.isInfJumpActivated = !_vars.isInfJumpActivated;
        }

        public void BoxFlyMode_CheckedChanged(object sender, EventArgs e)
        {
            _form.toggle_FlyMode();

            if (_vars.flyModeEnabled)
            {
                _form.box_noCollision.Checked = true;
                _form.box_noCamCollision.Checked = true;
            }
            else
            {
                _form.box_noCollision.Checked = false;
                _form.box_noCamCollision.Checked = false;
            }
        }

        public void BoxNoAnimations_CheckedChanged(object sender, EventArgs e)
        {
            if (!_vars.noAnimationPatched)
            {
                _mem.WriteMemory(_vars.noAnimationAddrString, "bytes", "90 90 90 90 90 90 90 90");
                _vars.noAnimationPatched = true;
            }
            else
            {
                _mem.WriteMemory(_vars.noAnimationAddrString, "bytes", "F3 0F 11 8B 70 02 00 00");
                _vars.noAnimationPatched = false;
            }
        }

        public void BoxInfReach_CheckedChanged(object sender, EventArgs e)
        {
            _form.infReachFunction();
        }

        public void BoxNoCollision_CheckedChanged(object sender, EventArgs e)
        {
            _form.toggleNoCollision();
        }

        public void BoxDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!_vars.darkmodeEnabled)
            {
                Color fColor = Color.FromArgb(220, 220, 220);
                Color bColor = Color.FromArgb(50, 50, 50);
                MaterialSkinManager.Instance.ColorScheme = new ColorScheme(Primary.Green800, Primary.Green900, Primary.Green900, Accent.Green700, TextShade.BLACK);
                MaterialSkinManager.Instance.Theme = MaterialSkinManager.Themes.DARK;
                _form.BackColor = bColor;
                _form.ForeColor = fColor;

                _form.tabPage1.BackColor = bColor;
                _form.tabPage1.ForeColor = fColor;
                _form.tabPage2.BackColor = bColor;
                _form.tabPage2.ForeColor = fColor;
                _form.tabPage4.BackColor = bColor;
                _form.tabPage4.ForeColor = fColor;
                _form.tabPage5.BackColor = bColor;
                _form.tabPage5.ForeColor = fColor;
                _form.tabPage6.ForeColor = fColor;
                _form.tabPage6.BackColor = bColor;
                _form.trckbr_speed.BackColor = bColor;
                _form.listbox_teleportLocations.BackColor = bColor;
                _form.listbox_teleportLocations.ForeColor = fColor;

                _form.SetControlColors(_form.Controls, bColor, fColor);

                _vars.darkmodeEnabled = true;
            }
            else
            {
                MaterialSkinManager.Instance.ColorScheme = new ColorScheme(Primary.Green800, Primary.Green900, Primary.Green900, Accent.Green700, TextShade.WHITE);
                MaterialSkinManager.Instance.Theme = MaterialSkinManager.Themes.LIGHT;
                _form.BackColor = Color.FromArgb(255, 255, 255);
                _form.ForeColor = Color.FromArgb(0, 0, 0);
                Color fColor = Color.FromArgb(0, 0, 0);
                Color bColor = Color.FromArgb(255, 255, 255);
                _form.tabPage1.BackColor = bColor;
                _form.tabPage1.ForeColor = fColor;
                _form.tabPage2.BackColor = bColor;
                _form.tabPage2.ForeColor = fColor;
                _form.tabPage4.BackColor = bColor;
                _form.tabPage4.ForeColor = fColor;
                _form.tabPage5.BackColor = bColor;
                _form.tabPage5.ForeColor = fColor;
                _form.trckbr_speed.BackColor = bColor;
                _form.trckbr_speed.BackColor = fColor;
                _form.listbox_teleportLocations.BackColor = bColor;
                _form.listbox_teleportLocations.ForeColor = fColor;

                _form.SetControlColors(_form.Controls, Color.FromArgb(250, 250, 250), Color.FromArgb(0, 0, 0));

                _vars.darkmodeEnabled = false;
            }
        }

        public void CboxNoclip_CheckedChanged(object sender, EventArgs e)
        {
            Thread codeCaveThread = new Thread(_form.createCodeCave);

            if (_form.cbox_noclip.ForeColor != Color.Green)
            {
                Cursor.Current = Cursors.WaitCursor;
                _form.cbox_noclip.ForeColor = Color.Green;
            }
            else
            {
                _form.cbox_noclip.ForeColor = Color.Red;
            }

            codeCaveThread.Start();
        }

        public void BoxFreecam_CheckedChanged(object sender, EventArgs e)
        {
            _form.toggle_freecam();
        }

        public void BoxNofall_CheckedChanged(object sender, EventArgs e)
        {
            _form.nofallFunction();
        }

        public void BoxCamAttach_CheckedChanged(object sender, EventArgs e)
        {
            _vars.attachToCamEnabled = !_vars.attachToCamEnabled;
        }

        public void BoxAlwaysInFront_CheckedChanged(object sender, EventArgs e)
        {
            if (!_vars.alwaysOnTop)
            {
                _form.TopMost = true;
                _vars.alwaysOnTop = true;
            }
            else
            {
                _form.TopMost = false;
                _vars.alwaysOnTop = false;
            }
        }

        public void BoxEsp_CheckedChanged(object sender, EventArgs e)
        {
            if (!_vars.devEspEnabled)
            {
                _mem.WriteMemory($"{_vars.devESPAddrString}", "bytes", "0F 85");
                _vars.devEspEnabled = true;
            }
            else
            {
                _mem.WriteMemory($"{_vars.devESPAddrString}", "bytes", "0F 84");
                _vars.devEspEnabled = false;
            }
        }

        public void BoxDotEsp_CheckedChanged(object sender, EventArgs e)
        {
            if (!_vars.devVelEnabled)
            {
                _mem.WriteMemory($"{_vars.velocityIndAddrStr}", "bytes", "75 1F B9");
                _vars.devVelEnabled = true;
            }
            else
            {
                string espAddr = ConvertUintToHexString(_vars.EspUint);
                _mem.WriteMemory($"{_vars.velocityIndAddrStr}", "bytes", "74 1F B9");
                _vars.devVelEnabled = false;
            }
        }

        public void BoxWallhack_CheckedChanged(object sender, EventArgs e)
        {
            if (!_vars.wallhackPatched)
            {
                _mem.WriteMemory($"{_vars.wallhackAddress}", "bytes", "90 90");
                _vars.wallhackPatched = true;
            }
            else
            {
                _mem.WriteMemory($"{_vars.wallhackAddress}", "bytes", "74 0D");
                _vars.wallhackPatched = false;
            }

            if (!_vars.wallhack2Patched)
            {
                _mem.WriteMemory($"{_vars.wallhack2Address}", "bytes", "90 90");
                _vars.wallhack2Patched = true;
            }
            else
            {
                _mem.WriteMemory($"{_vars.wallhack2Address}", "bytes", "0F 84");
                _vars.wallhack2Patched = false;
            }
        }

        public void BoxGlide_CheckedChanged(object sender, EventArgs e)
        {
            _form.doglide();
        }

        public void BoxSpeedhack_CheckedChanged(object sender, EventArgs e)
        {
            _form.speedhackFunction();
        }

        public void BoxNoCamCollision_CheckedChanged(object sender, EventArgs e)
        {
            _form.toggle_camCollision();
        }

        public void BoxNoKnockback_CheckedChanged(object sender, EventArgs e)
        {
            _vars.noKnockbackEnabled = !_vars.noKnockbackEnabled;
        }

        private string ConvertUintToHexString(UIntPtr uintToConvert)
        {
            string placeholder1 = uintToConvert.ToString();
            long placeholder2 = long.Parse(placeholder1);
            string hexstring = placeholder2.ToString("X2");
            return hexstring;
        }
    }
}