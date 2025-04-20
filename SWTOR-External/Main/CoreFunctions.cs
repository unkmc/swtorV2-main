using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Memory;
using WindowsInput;

namespace SWTOR_External.Classes
{
    public class CoreFunctions
    {
        private readonly Form1 _form;
        private readonly Variables _vars;
        private readonly Mem _mem;
        private readonly InputSimulator _sim;

        public CoreFunctions(Form1 form, Variables vars, Mem mem, InputSimulator sim)
        {
            _form = form;
            _vars = vars;
            _mem = mem;
            _sim = sim;
        }

        public void HoliCheck()
        {
            DateTime today = DateTime.Today;
            string emoji = "";

            if (today.Month == 4 && IsEasterSunday(today))
            {
                emoji = " (Happy 4/20) 🐣 🥦";
            }
            else if (today.Month == 12 && today.Day == 25)
            {
                emoji = "🎄";
            }
            else if (today.Month == 10 && today.Day == 31)
            {
                emoji = "🎃";
            }

            if (!string.IsNullOrEmpty(emoji))
            {
                _form.lbl_title.Text = _form.lbl_title.Text + " " + emoji;
            }
        }

        private bool IsEasterSunday(DateTime date)
        {
            int year = date.Year;
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;

            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;

            return date.Month == month && date.Day == day;
        }

        public void ChatSpamLoop()
        {
            while (true)
            {
                if (_vars.enableSpam)
                {
                    if (!_form.box_chatSpammer.Checked) break;

                    _sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                    Thread.Sleep(100);

                    foreach (char c in _form.txtbox_chatSpammer.Text)
                    {
                        _sim.Keyboard.TextEntry(c);
                        Thread.Sleep(10);
                    }

                    _sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);

                    if (_vars.moveAfterMsg)
                    {
                        Thread.Sleep(50);
                        _sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                        Thread.Sleep(50);
                        _sim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                    }

                    try
                    {
                        Thread.Sleep(int.Parse(_form.txtbox_chatSpamDelay.Text));
                    }
                    catch
                    {
                        Thread.Sleep(2000);
                    }
                }
            }
        }

        public void OnProcessExit(object sender, EventArgs e)
        {
            try
            {
                _form.restoreOriginalCode();
            }
            catch
            {
                MessageBox.Show($"Restoring code failed! Please restart the game.");
            }
        }

        public void InfJumpPatch()
        {
            if (!_vars.infJumpPatched)
            {
                _vars.infJumpPatched = true;
                _mem.WriteMemory(_vars.infJumpAddrStr, "bytes", "90 90 90 90 90");
            }
            else
            {
                _vars.infJumpPatched = false;
                _mem.WriteMemory(_vars.infJumpAddrStr, "bytes", "F2 0F 11 47 0C");
            }
        }

        public void ToggleNoCollision()
        {
            if (!_vars.noCollisionEnabled)
            {
                _mem.WriteMemory($"{_vars.movementModeAddrStr}", "int", "6");
                _vars.noCollisionEnabled = true;
            }
            else
            {
                _mem.WriteMemory($"{_vars.movementModeAddrStr}", "int", "1");
                _vars.noCollisionEnabled = false;
            }
        }

        public void ToggleCamCollision()
        {
            if (!_vars.camCollisionEnabled)
            {
                _vars.camCollisionEnabled = true;
                _mem.WriteMemory(_vars.camCollisionAddrStr, "bytes", "90 90 90 90 90 90 90 90");
            }
            else
            {
                _vars.camCollisionEnabled = false;
                _mem.WriteMemory(_vars.camCollisionAddrStr, "bytes", "F3 0F 11 8F 50 03 00 00");
            }
        }

        public void LoadHotkeys()
        {
            if (File.Exists("hotkeys.dat"))
            {
                using (FileStream fs = new FileStream("hotkeys.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    HotkeySettings settings = (HotkeySettings)formatter.Deserialize(fs);

                    _vars.TPUpKey = settings.TPUpKey;
                    _vars.TPDownKey = settings.TPDownKey;
                    _vars.TPLeftKey = settings.TPLeftKey;
                    _vars.TPRightKey = settings.TPRightKey;
                    _vars.TPForwardKey = settings.TPForwardKey;
                    _vars.TPBackwardKey = settings.TPBackwardKey;
                    _vars.FreecamKey = settings.FreecamKey;
                    _vars.TPToCamKey = settings.TPToCamKey;
                    _vars.NofallKey = settings.NofallKey;
                    _vars.GlideKey = settings.GlideKey;
                    _vars.SpeedKey = settings.SpeedKey;
                    _vars.forwardsKey = settings.flyforwardsKey;
                    _vars.backwardsKey = settings.flybackwardsKey;
                    _vars.flyDownKey = settings.flyDownKey;
                    _vars.flyUpKey = settings.flyupKey;
                    _vars.enableSpamKey = settings.enableSpamKey;

                    _form.txtbox_TPUpKey.Text = _vars.TPUpKey.ToString();
                    _form.txtbox_TPDowNkey.Text = _vars.TPDownKey.ToString();
                    _form.txtbox_TPLeftKey.Text = _vars.TPLeftKey.ToString();
                    _form.txtbox_TPRightKey.Text = _vars.TPRightKey.ToString();
                    _form.txtbox_TPForwardKey.Text = _vars.TPForwardKey.ToString();
                    _form.txtbox_TPBackwardKey.Text = _vars.TPBackwardKey.ToString();
                    _form.txtbox_freecamKey.Text = _vars.FreecamKey.ToString();
                    _form.txtbox_tpToCamKey.Text = _vars.TPToCamKey.ToString();
                    _form.txtbox_nofallKey.Text = _vars.NofallKey.ToString();
                    _form.txtbox_glideKey.Text = _vars.GlideKey.ToString();
                    _form.txtbox_speedKey.Text = _vars.SpeedKey.ToString();
                    _form.txtbox_flyforwardskey.Text = _vars.forwardsKey.ToString();
                    _form.txtbox_flybackwardskey.Text = _vars.backwardsKey.ToString();
                    _form.txtbox_flyDown.Text = _vars.flyDownKey.ToString();
                    _form.txtbox_flyUp.Text = _vars.flyUpKey.ToString();
                    _form.txt_hotkeySpam.Text = _vars.enableSpamKey.ToString();
                }
            }
            else
            {
                _form.logToConsole("No hotkey settings found.\n");
            }
        }

        public void ToggleFreecam()
        {
            if (!_vars.cameraYPatched)
            {
                _mem.WriteBytes(_vars.cameraYUInt, _vars.cameraYPatchedBytes);
                _vars.cameraYPatched = true;
            }
            else
            {
                _mem.WriteBytes(_vars.cameraYUInt, _vars.cameraYBytes);
                _vars.cameraYPatched = false;
            }

            if (!_vars.cameraZPatched)
            {
                _mem.WriteBytes(_vars.cameraZUInt, _vars.cameraZPatchedBytes);
                _vars.cameraZPatched = true;
            }
            else
            {
                _mem.WriteBytes(_vars.cameraZUInt, _vars.cameraZBytes);
                _vars.cameraZPatched = false;
            }

            _vars.freeCamEnabled = !_vars.freeCamEnabled;
        }

        public void ToggleFlyMode()
        {
            _vars.flyModeEnabled = !_vars.flyModeEnabled;
        }

        public void ScanAOB()
        {
            try
            {
                IntPtr moduleStart = _mem.GetModuleAddressByName("swtor.exe");
                long moduleStartLong = long.Parse(moduleStart.ToString());
                _vars.infJumpAddrStr = _mem.AoBScan(_vars.infJumpAOB).Result.Sum().ToString("X2");
                _vars.noclipAddressStr = _mem.AoBScan(_vars.noclipAOB).Result.Sum().ToString("X2");
                _vars.cameraAddress = _mem.AoBScan(moduleStartLong, moduleStartLong + 10000000, _vars.cameraAOB).Result.Sum().ToString("X2");
                _vars.cameraZAddress = _mem.AoBScan(_vars.cameraZAOB).Result.Sum().ToString("X2");
                _vars.cameraYAddress = _mem.AoBScan(_vars.cameraYAOB).Result.Sum().ToString("X2");
                _vars.nofallAddrString = _mem.AoBScan(_vars.nofallAOB).Result.Sum().ToString("X2");
                _vars.speedHackAddrString = _mem.AoBScan(_vars.speedHackAOB).Result.Sum().ToString("X2");
                _vars.devESPAddrString = _mem.AoBScan(_vars.devESPAob).Result.Sum().ToString("X2");
                _vars.velocityIndAddrStr = _mem.AoBScan(_vars.velocityIndicatorAOB).Result.Sum().ToString("X2");
                _vars.glideAddrString = _mem.AoBScan(_vars.glideAOB).Result.Sum().ToString("X2");
                _vars.wallhackAddress = _mem.AoBScan(_vars.wallhackAOB).Result.Sum().ToString("X2");
                _vars.wallhack2Address = _mem.AoBScan(_vars.wallhack2AOB).Result.Sum().ToString("X2");
                _vars.infReachAddressStr = _mem.AoBScan(_vars.infReachAOB).Result.Sum().ToString("X2");
                _vars.camCollisionAddrStr = _mem.AoBScan(_vars.camCollisionAOB).Result.Sum().ToString("X2");
                _vars.noAnimationAddrString = _mem.AoBScan(_vars.noAnimationAOB).Result.Sum().ToString("X2");
                _vars.stuckAddrStr = _mem.AoBScan(_vars.stuckAOB).Result.Sum().ToString("X2");

                _vars.stuckAddrUint = _mem.Get64BitCode(_vars.stuckAddrStr);
                _vars.cameraYUInt = _mem.Get64BitCode(_vars.cameraYAddress);
                _vars.cameraZUInt = _mem.Get64BitCode(_vars.cameraZAddress);

                _vars.infReachAddress = _mem.Get64BitCode(_vars.infReachAddressStr);
                _vars.infReachAddress = (UIntPtr)((ulong)_vars.infReachAddress + 0x5);
                _vars.infReachAddressStr = ConvertUintToHexString(_vars.infReachAddress);

                _vars.noclipAddress = _mem.Get64BitCode(_vars.noclipAddressStr);
                _vars.noclipAddressStr = ConvertUintToHexString(_vars.noclipAddress);

                _form.log_console.Invoke((MethodInvoker)delegate
                {
                    _form.log_console.Text = _form.log_console.Text + $"\r\nInitialization success";
                    _form.cbox_noclip.Enabled = true;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AOB's not found. Please restart the game and the tool.\nError: {ex.Message}");
            }
        }

        public void Freecam()
        {
            float pitch = _mem.ReadFloat(_vars.pitchAddrString);
            float yaw = _mem.ReadFloat(_vars.yawAddrString);

            float siny = (float)Math.Sin(yaw);
            float cosy = (float)Math.Cos(yaw);
            float sinp = (float)Math.Sin(pitch);
            float cosp = (float)Math.Cos(pitch);

            float camx = _mem.ReadFloat(_vars.xCamAddrString);
            float camy = _mem.ReadFloat(_vars.yCamAddrString);
            float camz = _mem.ReadFloat(_vars.zCamAddrString);

            float speedX = 0;
            float speedY = 0;
            float speedZ = 0;

            float speed = _vars.camSpeed;
            if (_vars.isSpeedBoostActive)
            {
                speed *= _vars.speedBoostMultiplier;
            }

            bool isArrowUpPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.forwardsKey);
            bool isArrowDownPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.backwardsKey);

            if (isArrowUpPressed)
            {
                speedX = -speed * cosp * siny;
                speedZ = -speed * cosp * cosy;
                speedY = -speed * sinp;
            }
            else if (isArrowDownPressed)
            {
                speedX = speed * cosp * siny;
                speedZ = speed * cosp * cosy;
                speedY = speed * sinp;
            }

            if (_sim.InputDeviceState.IsHardwareKeyDown(WindowsInput.Native.VirtualKeyCode.LEFT) ||
                _sim.InputDeviceState.IsHardwareKeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT))
            {
                _vars.isSpeedBoostActive = true;
            }
            else
            {
                _vars.isSpeedBoostActive = false;
            }

            if (_sim.InputDeviceState.IsHardwareKeyDown(_vars.flyUpKey))
            {
                speedY += speed * 0.2f;
            }
            else if (_sim.InputDeviceState.IsHardwareKeyDown(_vars.flyDownKey))
            {
                speedY -= speed * 0.2f;
            }

            camx += speedX;
            camy += speedY;
            camz += speedZ;

            string camxString = camx.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string camyString = camy.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string camzString = camz.ToString(System.Globalization.CultureInfo.InvariantCulture);

            _mem.WriteMemory(_vars.xCamAddrString, "float", camxString);
            _mem.WriteMemory(_vars.yCamAddrString, "float", camyString);
            _mem.WriteMemory(_vars.zCamAddrString, "float", camzString);

            if (_vars.attachToCamEnabled)
            {
                _vars.savedX = camx;
                _vars.savedY = camy;
                _vars.savedZ = camz;
                _vars.tpflag = true;
            }
        }

        public void FlyMode()
        {
            bool valueManipulated = false;
            float pitch = _mem.ReadFloat(_vars.pitchAddrString);
            float yaw = _mem.ReadFloat(_vars.yawAddrString);

            float siny = (float)Math.Sin(yaw);
            float cosy = (float)Math.Cos(yaw);
            float sinp = (float)Math.Sin(pitch);
            float cosp = (float)Math.Cos(pitch);

            float playerX = _mem.ReadFloat(_vars.xAddrString);
            float playerY = _mem.ReadFloat(_vars.yAddrString);
            float playerZ = _mem.ReadFloat(_vars.zAddrString);

            float speedX = 0;
            float speedY = 0;
            float speedZ = 0;

            float speed = _vars.camSpeed;
            if (_vars.isSpeedBoostActive)
            {
                speed *= _vars.speedBoostMultiplier;
            }

            bool isArrowUpPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.forwardsKey);
            bool isArrowDownPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.backwardsKey);

            if (isArrowUpPressed)
            {
                speedX = -speed * cosp * siny;
                speedZ = -speed * cosp * cosy;
                speedY = -speed * sinp;
                valueManipulated = true;
            }
            else if (isArrowDownPressed)
            {
                speedX = speed * cosp * siny;
                speedZ = speed * cosp * cosy;
                speedY = speed * sinp;
                valueManipulated = true;
            }

            if (_sim.InputDeviceState.IsHardwareKeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT))
            {
                _vars.isSpeedBoostActive = true;
            }
            else
            {
                _vars.isSpeedBoostActive = false;
            }

            if (_sim.InputDeviceState.IsHardwareKeyDown(_vars.flyUpKey))
            {
                speedY += speed * 0.2f;
                valueManipulated = true;
            }
            else if (_sim.InputDeviceState.IsHardwareKeyDown(_vars.flyDownKey))
            {
                speedY -= speed * 0.2f;
                valueManipulated = true;
            }

            playerX += speedX;
            playerY += speedY;
            playerZ += speedZ;

            string playerXString = playerX.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string playerYString = playerY.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string playerZString = playerZ.ToString(System.Globalization.CultureInfo.InvariantCulture);

            if (valueManipulated)
            {
                _mem.WriteMemory(_vars.xAddrString, "float", playerXString);
                _mem.WriteMemory(_vars.yAddrString, "float", playerYString);
                _mem.WriteMemory(_vars.zAddrString, "float", playerZString);
            }
        }

        public void Teleport()
        {
            bool isArrived = false;

            if (!_vars.attachToCamEnabled)
            {
                if (_vars.lastPos.X == _vars.xCoord && _vars.lastPos.Y == _vars.yCoord && _vars.lastPos.Z == _vars.zCoord)
                {
                    _vars.rubberbandCount++;
                }

                if (_vars.rubberbandCount > 3)
                {
                    _form.doglide();
                    _form.nofallFunction();
                    _vars.tpflag = false;
                    return;
                }
            }
            _vars.lastPos.X = _vars.xCoord;
            _vars.lastPos.Y = _vars.yCoord;
            _vars.lastPos.Z = _vars.zCoord;

            if (!_vars.glideEnabled)
            {
                _form.doglide();
                _mem.WriteMemory(_vars.movementModeAddrStr, "int", "6");
            }

            if (_vars.savedX == 0 || _vars.savedY == 0 || _vars.savedZ == 0)
            {
                _form.log_console.Text = _form.log_console.Text + "\n\r\n\rInvalid Value!";
                return;
            }

            float distance = (float)Math.Sqrt(Math.Pow(_vars.xCoord - _vars.savedX, 2) + Math.Pow(_vars.yCoord - _vars.savedY, 2) + Math.Pow(_vars.zCoord - _vars.savedZ, 2));

            if (distance <= 1.0f && distance > 0)
            {
                _mem.WriteMemory(_vars.xAddrString, "float", (_vars.savedX).ToString(System.Globalization.CultureInfo.InvariantCulture));
                _mem.WriteMemory(_vars.yAddrString, "float", (_vars.savedY).ToString(System.Globalization.CultureInfo.InvariantCulture));
                _mem.WriteMemory(_vars.zAddrString, "float", (_vars.savedZ).ToString(System.Globalization.CultureInfo.InvariantCulture));
                Thread.Sleep(50);
                _mem.WriteMemory(_vars.movementModeAddrStr, "int", "1");

                isArrived = true;
                _form.doglide();
            }
            else if (distance > 1.0f)
            {
                float moveX = (_vars.savedX - _vars.xCoord) / distance;
                float moveY = (_vars.savedY - _vars.yCoord) / distance;
                float moveZ = (_vars.savedZ - _vars.zCoord) / distance;

                _mem.WriteMemory(_vars.xAddrString, "float", (_vars.xCoord + moveX).ToString(System.Globalization.CultureInfo.InvariantCulture));
                _mem.WriteMemory(_vars.yAddrString, "float", (_vars.yCoord + moveY).ToString(System.Globalization.CultureInfo.InvariantCulture));
                _mem.WriteMemory(_vars.zAddrString, "float", (_vars.zCoord + moveZ).ToString(System.Globalization.CultureInfo.InvariantCulture));

                Thread.Sleep(100);

                isArrived = (distance <= 1.0f);
            }
            if (isArrived)
            {
                isArrived = false;
                _vars.tpflag = false;
            }
        }

        public void TeleportNumpad()
        {
            while (true)
            {
                float playerXCoord = _mem.ReadFloat(_vars.xAddrString);
                float playerYCoord = _mem.ReadFloat(_vars.yAddrString);
                float playerZCoord = _mem.ReadFloat(_vars.zAddrString);
                bool isTPUpPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.TPUpKey);
                bool isTPDownPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.TPDownKey);
                bool isTPXUpPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.TPLeftKey);
                bool isTPXDownPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.TPRightKey);
                bool isTPZUpPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.TPForwardKey);
                bool isTPZDownPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.TPBackwardKey);

                if (isTPUpPressed)
                {
                    _mem.WriteMemory(_vars.yAddrString, "float", (playerYCoord + 0.5f).ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (isTPDownPressed)
                {
                    _mem.WriteMemory(_vars.yAddrString, "float", (playerYCoord - 0.25f).ToString());
                }
                if (isTPXUpPressed)
                {
                    _mem.WriteMemory(_vars.xAddrString, "float", (playerXCoord + 0.25f).ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (isTPXDownPressed)
                {
                    _mem.WriteMemory(_vars.xAddrString, "float", (playerXCoord - 0.25f).ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (isTPZUpPressed)
                {
                    _mem.WriteMemory(_vars.zAddrString, "float", (playerZCoord + 0.25f).ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (isTPZDownPressed)
                {
                    _mem.WriteMemory(_vars.zAddrString, "float", (playerZCoord - 0.25f).ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                Thread.Sleep(100);
            }
        }

        public string ConvertUintToHexString(UIntPtr uintToConvert)
        {
            string placeholder1 = uintToConvert.ToString();
            long placeholder2 = long.Parse(placeholder1);
            string hexstring = placeholder2.ToString("X2");
            return hexstring;
        }

        public static UIntPtr ParseHexToUIntPtr(string hexValue)
        {
            ulong numericValue = ulong.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            return new UIntPtr(numericValue);
        }

        public void NofallFunction()
        {
            if (!_vars.nofallEnabled)
            {
                if (!_vars.nofallPatched)
                {
                    _mem.CreateCodeCave(_vars.nofallAddrString, _vars.patchedBytes, 6, 120);
                    _vars.gotoCaveBytes = _mem.ReadBytes(_vars.nofallAddrString, 6);
                    _vars.nofallEnabled = true;
                    _vars.nofallPatched = true;
                }
                else
                {
                    _mem.WriteBytes(_vars.nofallAddrString, _vars.gotoCaveBytes);
                    _vars.nofallEnabled = true;
                }
            }
            else
            {
                _vars.nofallAddr = ParseHexToUIntPtr(_vars.nofallAddrString);
                _mem.WriteBytes(_vars.nofallAddr, _vars.originalBytes);
                _vars.nofallEnabled = false;
            }
        }

        public void InfReachFunction()
        {
            if (!_vars.infReachEnabled)
            {
                if (!_vars.infReachPatched)
                {
                    _mem.CreateCodeCave(_vars.infReachAddressStr, _vars.infReachPatchedBytes, 6, 300);
                    _vars.infReachAlreadyPatchedBytes = _mem.ReadBytes(_vars.infReachAddressStr, 6);
                    _vars.infReachEnabled = true;
                    _vars.infReachPatched = true;
                }
                else
                {
                    _mem.WriteBytes(_vars.infReachAddressStr, _vars.infReachAlreadyPatchedBytes);
                    _vars.infReachEnabled = true;
                }
            }
            else
            {
                _mem.WriteBytes(_vars.infReachAddressStr, _vars.infReachOriginalBytes);
                _vars.infReachEnabled = false;
            }
        }

        public bool OnlineCheck(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    _form.log_console.Text = _form.log_console.Text + "Tool is Online";
                }
                else
                {
                    char[] msg = { 'T', 'o', 'o', 'l', ' ', 'O', 'f', 'f', 'l', 'i', 'n', 'e' };
                    StringBuilder prnt = new StringBuilder();
                    foreach (char c in msg)
                    {
                        prnt.Append(c);
                    }
                    MessageBox.Show(prnt.ToString());
                    Environment.Exit(0);
                }
                return response.IsSuccessStatusCode;
            }
        }

        public void UpdateCheck(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    DialogResult openDc = MessageBox.Show("Your tool is outdated. Please download a new version from the discord.\r\nWould you like to join the discord?", "Error", MessageBoxButtons.YesNo);
                    if (openDc == DialogResult.Yes)
                    {
                        _form.openDiscord();
                    }
                    Environment.Exit(1);
                }
                else
                {
                    _form.log_console.Text = _form.log_console.Text + "\r\n\r\nYou are running the latest version\r\n\r\n";
                }
            }
        }

        public void StartMainTimer()
        {
            try
            {
                _form.mainTimer.Start();
            }
            catch (Exception ex)
            {
                _form.log_console.Text = _form.log_console.Text + ($"\r\nException in Main Thread: {ex.Message}");
            }
        }

        public void StartPvpTimer()
        {
            try
            {
                _form.pvpTimer.Start();
            }
            catch (Exception ex)
            {
                _form.log_console.Text = _form.log_console.Text + ($"\r\nException in PVP Thread: {ex.Message}");
            }
        }

        public void StartGetBaseTimer()
        {
            try
            {
                _form.timer_getBase.Start();
            }
            catch (Exception ex)
            {
                _form.log_console.Text = _form.log_console.Text + ($"\r\nException in getBase Thread: {ex.Message}");
            }
        }

        public void CreateCodeCave()
        {
            if (!_vars.noclipCave)
            {
                try
                {
                    byte[] patched_bytes = { 0x83, 0xB9, 0x1C, 0x03, 0x00, 0x00, 0x02, 0x0F, 0x85, 0x07, 0x00, 0x00, 0x00, 0x48, 0x89, 0x0D, 0x0C, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x01, 0x48, 0x8B, 0x40, 0x58 };
                    _vars.pbasecaveAddr = _mem.CreateCodeCave(_vars.noclipAddressStr, patched_bytes, 7, 120);

                    _form.log_console.Invoke((MethodInvoker)delegate
                    {
                        _form.log_console.Text = _form.log_console.Text + "\r\n\r\nHooked";
                    });

                    _vars.noclipPatchedBytes = _mem.ReadBytes(_vars.noclipAddressStr, 7);
                    _vars.noclipPatched = true;
                    _vars.noclipCave = true;
                }
                catch (Exception ex)
                {
                    _form.log_console.Invoke((MethodInvoker)delegate
                    {
                        _form.log_console.Text = _form.log_console.Text + $"\r\n\r\nHook failed\r\n\r\nPlease restart the game\r\n\r\nError: {ex.Message}";
                    });
                    return;
                }
            }
            else
            {
                if (_vars.noclipPatched)
                {
                    _mem.WriteBytes(_vars.noclipAddressStr, _vars.noclipBytes);
                    _form.log_console.Invoke((MethodInvoker)delegate
                    {
                        _form.log_console.Text = _form.log_console.Text + "\r\n\r\nUnhooked";
                    });
                    _vars.noclipPatched = false;
                }
                else
                {
                    _mem.WriteBytes(_vars.noclipAddressStr, _vars.noclipPatchedBytes);
                    _form.log_console.Invoke((MethodInvoker)delegate
                    {
                        _form.log_console.Text = _form.log_console.Text + "\r\n\r\nRe-Hooked";
                    });
                    _vars.noclipPatched = true;
                }
            }

            if (!_vars.camCave)
            {
                byte[] patched_bytes = { 0x83, 0xBF, 0xCC, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x85, 0x07, 0x00, 0x00, 0x00, 0x48, 0x89, 0x3D, 0x0D, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x87, 0xD0, 0x03, 0x00, 0x00 };
                UIntPtr caveAddr = _mem.CreateCodeCave(_vars.cameraAddress, patched_bytes, 8, 220);

                string caveAddrString = ConvertUintToHexString(caveAddr);
                _vars.camBaseUInt = (UIntPtr)UIntPtr.Add(caveAddr, 0x21);
                string CambaseUIntString = ConvertUintToHexString(_vars.camBaseUInt);

                Thread.Sleep(100);

                long camBaselong = _mem.ReadLong(CambaseUIntString);
                _vars.CamBaseAddress = camBaselong.ToString("X2");
                _vars.camBaseUInt = ParseHexToUIntPtr(_vars.CamBaseAddress);

                _vars.cameraPatchedBytes = _mem.ReadBytes(_vars.cameraAddress, 8);
                _vars.cameraPatched = true;
                _vars.camCave = true;
            }
            else
            {
                if (_vars.cameraPatched)
                {
                    _mem.WriteBytes(_vars.cameraAddress, _vars.cameraBytes);
                    _vars.cameraPatched = false;
                }
                else
                {
                    _mem.WriteBytes(_vars.cameraAddress, _vars.cameraPatchedBytes);
                    _vars.cameraPatched = true;
                }
            }

            if (!_vars.speedHackCave)
            {
                byte[] patched_bytes = { 0x83, 0x3D, 0x23, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x84, 0x10, 0x00, 0x00, 0x00, 0x53, 0x48, 0x8B, 0x1D, 0x15, 0x00, 0x00, 0x00, 0x48, 0x89, 0x9E, 0xDC, 0x00, 0x00, 0x00, 0x5B, 0xF3, 0x0F, 0x10, 0xBE, 0xDC, 0x00, 0x00, 0x00 };
                UIntPtr caveAddr = _mem.CreateCodeCave(_vars.speedHackAddrString, patched_bytes, 8, 120);

                string caveAddrString = ConvertUintToHexString(caveAddr);
                _vars.speedValueUInt = (UIntPtr)UIntPtr.Add(caveAddr, 0x2A);
                _vars.speedValueUIntString = ConvertUintToHexString(_vars.speedValueUInt);

                Thread.Sleep(100);

                _vars.speedPatchedBytes = _mem.ReadBytes(_vars.speedHackAddrString, 8);
                _vars.speedPatched = true;
                _vars.speedHackCave = true;
            }
            else
            {
                if (_vars.speedPatched)
                {
                    _mem.WriteBytes(_vars.speedHackAddrString, _vars.speedBytes);
                    _vars.speedPatched = false;
                }
                else
                {
                    _mem.WriteBytes(_vars.speedHackAddrString, _vars.speedPatchedBytes);
                    _vars.speedPatched = true;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        public void LogToConsole(string textToLog)
        {
            _form.log_console.Text = _form.log_console.Text + $"\r\n{textToLog}";
        }

        public void Doglide()
        {
            if (!_vars.glideEnabled)
            {
                _vars.glideEnabled = true;
                _mem.WriteMemory(_vars.glideAddrString, "bytes", "90 90 90 90 90 90");
                _mem.WriteMemory(_vars.movementModeAddrStr, "int", "6");
            }
            else
            {
                _vars.glideEnabled = false;
                _mem.WriteMemory(_vars.glideAddrString, "bytes", _vars.glideAOB);
                _mem.WriteMemory(_vars.movementModeAddrStr, "int", "1");
            }
        }

        public void SpeedhackFunction()
        {
            if (!_vars.isSpeedhackEnabled)
            {
                _form.box_speedhack.Invoke((MethodInvoker)delegate
                {
                    _form.box_speedhack.Text = "On";
                    _form.box_speedhack.Checked = true;
                });
                _vars.isSpeedhackEnabled = true;
            }
            else
            {
                _form.box_speedhack.Invoke((MethodInvoker)delegate
                {
                    _form.box_speedhack.Text = "Off";
                    _form.box_speedhack.Checked = false;
                });
                _vars.isSpeedhackEnabled = false;
            }
        }

        public void TpToCamera()
        {
            _vars.rubberbandCount = 0;
            _vars.savedX = _mem.ReadFloat(_vars.xCamAddrString);
            _vars.savedY = _mem.ReadFloat(_vars.yCamAddrString);
            _vars.savedZ = _mem.ReadFloat(_vars.zCamAddrString);

            _form.log_console.Text = _form.log_console.Text + $"\r\n\r\nTeleported to camera";
            _form.log_console.Focus();
            _form.log_console.ScrollToCaret();
            _form.log_console.SelectionLength = 0;

            _vars.saveflag = true;
            _vars.tpflag = true;
            _form.box_nofall.Checked = true;
        }

        public void HotkeysFunction()
        {
            while (true)
            {
                bool isinfJumpKeyPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.infJumpKey);
                bool isinfJumpKeyUP = _sim.InputDeviceState.IsHardwareKeyUp(_vars.infJumpKey);
                bool isFreecamKeyPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.FreecamKey);
                bool isTPToCamKeyPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.TPToCamKey);
                bool isNofallKeyPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.NofallKey);
                bool isGlideKeyPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.GlideKey);
                bool isSpeedKeyPressed = _sim.InputDeviceState.IsHardwareKeyDown(_vars.SpeedKey);
                bool spamKeyPressed = _sim.InputDeviceState.IsKeyDown(_vars.enableSpamKey);

                try
                {
                    if (spamKeyPressed)
                    {
                        _vars.enableSpam = !_vars.enableSpam;
                        Thread.Sleep(200);
                    }
                    if (_vars.isInfJumpActivated)
                    {
                        if (isinfJumpKeyPressed && !_vars.isInfJumpToggled)
                        {
                            _form.Invoke((MethodInvoker)delegate
                            {
                                InfJumpPatch();
                            });
                            _vars.isInfJumpToggled = true;
                            Thread.Sleep(200);
                        }
                        if (isinfJumpKeyUP && _vars.isInfJumpToggled)
                        {
                            _form.Invoke((MethodInvoker)delegate
                            {
                                InfJumpPatch();
                            });
                            _vars.isInfJumpToggled = false;
                            Thread.Sleep(200);
                        }
                    }
                    if (isFreecamKeyPressed)
                    {
                        _form.Invoke((MethodInvoker)delegate
                        {
                            ToggleFreecam();
                        });
                        Thread.Sleep(200);
                    }
                    if (isTPToCamKeyPressed)
                    {
                        _form.btn_tpTo