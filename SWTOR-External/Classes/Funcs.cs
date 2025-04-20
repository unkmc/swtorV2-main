using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWTOR_External
{
    public class Funcs
    {
        private readonly Form1 _form;

        public Funcs(Form1 form)
        {
            _form = form;
        }

        #region Functions
        public void holicheck()
        {
            DateTime today = DateTime.Today;
            string emoji = "";

            // Check for specific holidays
            if (today.Month == 4 && IsEasterSunday(today)) // Easter (approximated)
            {
                emoji = " (Happy 4/20) 🐣 🥦";
            }
            else if (today.Month == 12 && today.Day == 25) // Christmas
            {
                emoji = "🎄";
            }
            else if (today.Month == 10 && today.Day == 31) // Halloween
            {
                emoji = "🎃";
            }
            // Add more holidays as needed

            // Append emoji to lbl_title if a holiday is detected
            if (!string.IsNullOrEmpty(emoji))
            {
                _form. lbl_title.Text = lbl_title.Text + " " + emoji;
            }
        }

        // Helper method to approximate Easter Sunday
        private bool IsEasterSunday(DateTime date)
        {
            // Simplified Easter calculation (Western Christian Easter)
            // Uses the Anonymous Gregorian algorithm for approximation
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
        private void chatSpamLoop()
        {
            while (true)
            {
                if (enableSpam)
                {
                    if (!box_chatSpammer.Checked) break;

                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN); // go into chat
                    Thread.Sleep(100);

                    foreach (char c in txtbox_chatSpammer.Text)   // type every char of string
                    {
                        sim.Keyboard.TextEntry(c);
                        Thread.Sleep(10);
                    }

                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN); // send message

                    if (moveAfterMsg)
                    {
                        Thread.Sleep(50);
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                        Thread.Sleep(50);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                    }

                    try
                    {
                        Thread.Sleep(int.Parse(txtbox_chatSpamDelay.Text));
                    }
                    catch
                    {
                        Thread.Sleep(2000);
                    }
                }
                else
                {

                }
            }
        }
        private void OnProcessExit(Object sender, EventArgs e)
        {
            try
            {
                restoreOriginalCode();
            }
            catch
            {
                MessageBox.Show($"Restoring code failed! Please restart the game.");
            }

        }
        private void infJumpPatch()
        {
            if (!infJumpPatched)
            {
                infJumpPatched = true;

                m.WriteMemory(infJumpAddrStr, "bytes", "90 90 90 90 90");
            }
            else
            {
                infJumpPatched = false;
                m.WriteMemory(infJumpAddrStr, "bytes", "F2 0F 11 47 0C");
            }
        }
        //private void patchStuck()
        //{
        //    //add additional bytes to scanned location because the AOB starts earlier than the target location
        //    UIntPtr addressToPatch = (stuckAddrUint + 0x0A);
        //    string addressToPatchStr = convertUintToHexString(addressToPatch);
        //
        //    if (!stuckPatched)
        //    {
        //        m.WriteMemory(addressToPatchStr, "bytes", "C3 90 90");
        //        stuckPatched = true;
        //    }
        //    else
        //    {
        //        m.WriteMemory(addressToPatchStr, "bytes", "48 8B C4");
        //        stuckPatched = false;
        //    }
        //}
        private void toggleNoCollision()
        {
            if (!noCollisionEnabled)
            {
                m.WriteMemory($"{movementModeAddrStr}", "int", "6");
                noCollisionEnabled = true;
            }
            else
            {
                m.WriteMemory($"{movementModeAddrStr}", "int", "1");
                noCollisionEnabled = false;
            }
        }
        private void toggle_camCollision()
        {
            if (!camCollisionEnabled)
            {
                camCollisionEnabled = true;

                m.WriteMemory(camCollisionAddrStr, "bytes", "90 90 90 90 90 90 90 90");
            }
            else
            {
                camCollisionEnabled = false;
                m.WriteMemory(camCollisionAddrStr, "bytes", "F3 0F 11 8F 50 03 00 00");
            }
        }
        private void loadHotkeys()
        {
            if (File.Exists("hotkeys.dat"))
            {
                using (FileStream fs = new FileStream("hotkeys.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    HotkeySettings settings = (HotkeySettings)formatter.Deserialize(fs);

                    TPUpKey = settings.TPUpKey;
                    TPDownKey = settings.TPDownKey;
                    TPLeftKey = settings.TPLeftKey;
                    TPRightKey = settings.TPRightKey;
                    TPForwardKey = settings.TPForwardKey;
                    TPBackwardKey = settings.TPBackwardKey;
                    FreecamKey = settings.FreecamKey;
                    TPToCamKey = settings.TPToCamKey;
                    NofallKey = settings.NofallKey;
                    GlideKey = settings.GlideKey;
                    SpeedKey = settings.SpeedKey;
                    forwardsKey = settings.flyforwardsKey;
                    backwardsKey = settings.flybackwardsKey;
                    flyDownKey = settings.flyDownKey;
                    flyUpKey = settings.flyupKey;
                    enableSpamKey = settings.enableSpamKey;

                    txtbox_TPUpKey.Text = TPUpKey.ToString();
                    txtbox_TPDowNkey.Text = TPDownKey.ToString();
                    txtbox_TPLeftKey.Text = TPLeftKey.ToString();
                    txtbox_TPRightKey.Text = TPRightKey.ToString();
                    txtbox_TPForwardKey.Text = TPForwardKey.ToString();
                    txtbox_TPBackwardKey.Text = TPBackwardKey.ToString();
                    txtbox_freecamKey.Text = FreecamKey.ToString();
                    txtbox_tpToCamKey.Text = TPToCamKey.ToString();
                    txtbox_nofallKey.Text = NofallKey.ToString();
                    txtbox_glideKey.Text = GlideKey.ToString();
                    txtbox_speedKey.Text = SpeedKey.ToString();
                    txtbox_flyforwardskey.Text = forwardsKey.ToString();
                    txtbox_flybackwardskey.Text = backwardsKey.ToString();
                    txtbox_flyDown.Text = flyDownKey.ToString();
                    txtbox_flyUp.Text = flyUpKey.ToString();
                    txt_hotkeySpam.Text = enableSpamKey.ToString();
                }
            }
            else
            {
                logToConsole("No hotkey settings found.\n");
            }
        }
        private void SetControlColors(Control.ControlCollection controls, Color backColor, Color foreColor)
        {
            foreach (Control control in controls)
            {
                if (control is Button || control is TextBox || control is TrackBar)
                {
                    control.BackColor = backColor;
                    control.ForeColor = foreColor;
                }
                else if (control.HasChildren)
                {
                    SetControlColors(control.Controls, backColor, foreColor);
                }
            }
        }
        private void toggle_freecam()
        {
            if (!cameraYPatched)
            {
                m.WriteBytes(cameraYUInt, cameraYPatchedBytes);
                //log_console.Text = log_console.Text + "\r\n\r\nCamYBytes Restored";
                cameraYPatched = true;
            }
            else
            {
                m.WriteBytes(cameraYUInt, cameraYBytes);
                //log_console.Text = log_console.Text + "\r\n\r\nCamYBytes Patched";
                cameraYPatched = false;
            }

            if (!cameraZPatched)
            {
                m.WriteBytes(cameraZUInt, cameraZPatchedBytes);
                //log_console.Text = log_console.Text + "\r\n\r\nCamXBytes Restored";
                cameraZPatched = true;
            }
            else
            {
                m.WriteBytes(cameraZUInt, cameraZBytes);
                cameraZPatched = false;
            }
            //Dont touch code above, important for freecam

            if (!freeCamEnabled)
            {
                freeCamEnabled = true;
            }
            else
            {
                freeCamEnabled = false;
            }
        }
        private void toggle_FlyMode()
        {
            if (!flyModeEnabled)
            {
                flyModeEnabled = true;
            }
            else
            {
                flyModeEnabled = false;
            }
        }
        private void scanAOB()
        {
            try
            {
                //PvP Scan
                //long startingAddr = 0x000000000000;
                //long endingAddr = 0x7fffffffffff;
                //pvpAddrStr = m.AoBScan(startingAddr, endingAddr, PVPAOB, true, false).Result.Sum().ToString("X2");
                //pvpAddr = m.Get64BitCode(pvpAddrStr);
                //if (pvpAddrStr == "00")
                //{
                //    MessageBox.Show("Please start the tool, once you are ingame");
                //    Environment.Exit(0);
                //}

                //AOB Scans
                IntPtr moduleStart = m.GetModuleAddressByName("swtor.exe");
                long moduleStartLong = long.Parse(moduleStart.ToString());
                infJumpAddrStr = m.AoBScan(infJumpAOB).Result.Sum().ToString("X2");
                noclipAddressStr = m.AoBScan(noclipAOB).Result.Sum().ToString("X2");
                cameraAddress = m.AoBScan(moduleStartLong, moduleStartLong + 10000000, cameraAOB).Result.Sum().ToString("X2");
                cameraZAddress = m.AoBScan(cameraZAOB).Result.Sum().ToString("X2");
                cameraYAddress = m.AoBScan(cameraYAOB).Result.Sum().ToString("X2");
                nofallAddrString = m.AoBScan(nofallAOB).Result.Sum().ToString("X2");
                speedHackAddrString = m.AoBScan(speedHackAOB).Result.Sum().ToString("X2");
                devESPAddrString = m.AoBScan(devESPAob).Result.Sum().ToString("X2");
                velocityIndAddrStr = m.AoBScan(velocityIndicatorAOB).Result.Sum().ToString("X2");
                glideAddrString = m.AoBScan(glideAOB).Result.Sum().ToString("X2");
                wallhackAddress = m.AoBScan(wallhackAOB).Result.Sum().ToString("X2");
                wallhack2Address = m.AoBScan(wallhack2AOB).Result.Sum().ToString("X2");
                infReachAddressStr = m.AoBScan(infReachAOB).Result.Sum().ToString("X2");
                camCollisionAddrStr = m.AoBScan(camCollisionAOB).Result.Sum().ToString("X2");
                noAnimationAddrString = m.AoBScan(noAnimationAOB).Result.Sum().ToString("X2");
                stuckAddrStr = m.AoBScan(stuckAOB).Result.Sum().ToString("X2");


                stuckAddrUint = m.Get64BitCode(stuckAddrStr);
                cameraYUInt = m.Get64BitCode(cameraYAddress);
                cameraZUInt = m.Get64BitCode(cameraZAddress);

                //fixInfReachAddr to +5 bytes
                infReachAddress = m.Get64BitCode(infReachAddressStr);
                infReachAddress = (infReachAddress + 0x5);
                infReachAddressStr = convertUintToHexString(infReachAddress);

                //fixPbaseAddr to +4 bytes
                noclipAddress = m.Get64BitCode(noclipAddressStr);
                noclipAddressStr = convertUintToHexString(noclipAddress);

                log_console.Invoke((MethodInvoker)delegate
                {
                    log_console.Text = log_console.Text + $"\r\nInitialization success";
                    cbox_noclip.Enabled = true;
                });

                //MessageBox.Show("AOB scan success");
            }
            catch (Exception ex)
            {
                //log_console.Text = log_console.Text + $"\r\n\r\nAOB's not found. Please restart the game";
                MessageBox.Show($"AOB's not found. Please restart the game and the tool.\nError: {ex.Message}");
            }
        }
        private void Freecam()
        {
            float pitch = m.ReadFloat(pitchAddrString);
            float yaw = m.ReadFloat(yawAddrString);

            float siny = (float)Math.Sin(yaw);
            float cosy = (float)Math.Cos(yaw);
            float sinp = (float)Math.Sin(pitch);
            float cosp = (float)Math.Cos(pitch);

            float camx = m.ReadFloat(xCamAddrString);
            float camy = m.ReadFloat(yCamAddrString);
            float camz = m.ReadFloat(zCamAddrString);

            float speedX = 0;
            float speedY = 0;
            float speedZ = 0;

            float speed = camSpeed;
            if (isSpeedBoostActive)
            {
                speed *= speedBoostMultiplier;
            }

            bool isArrowUpPressed = sim.InputDeviceState.IsHardwareKeyDown(forwardsKey);
            bool isArrowDownPressed = sim.InputDeviceState.IsHardwareKeyDown(backwardsKey);

            if (isArrowUpPressed)
            {
                speedX = -speed * cosp * siny;
                speedZ = -speed * cosp * cosy;
                speedY = -speed * sinp;  // Adjusted to move correctly with pitch
            }
            else if (isArrowDownPressed)
            {
                speedX = speed * cosp * siny;
                speedZ = speed * cosp * cosy;
                speedY = speed * sinp;   // Adjusted to move correctly with pitch
            }

            if (sim.InputDeviceState.IsHardwareKeyDown(WindowsInput.Native.VirtualKeyCode.LEFT) ||
                sim.InputDeviceState.IsHardwareKeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT))
            {
                isSpeedBoostActive = true;
            }
            else
            {
                isSpeedBoostActive = false;
            }

            if (sim.InputDeviceState.IsHardwareKeyDown(flyUpKey))
            {
                speedY += speed * 0.2f;
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(flyDownKey))
            {
                speedY -= speed * 0.2f;
            }

            camx += speedX;
            camy += speedY;
            camz += speedZ;

            // Ensure the float values are converted to strings using the invariant culture
            string camxString = camx.ToString(CultureInfo.InvariantCulture);
            string camyString = camy.ToString(CultureInfo.InvariantCulture);
            string camzString = camz.ToString(CultureInfo.InvariantCulture);

            // Write the memory
            m.WriteMemory(xCamAddrString, "float", camxString);
            m.WriteMemory(yCamAddrString, "float", camyString);
            m.WriteMemory(zCamAddrString, "float", camzString);

            if (attachToCamEnabled)
            {
                savedX = camx;
                savedY = camy;
                savedZ = camz;
                tpflag = true;
            }
        }
        private void FlyMode()
        {
            //based on the idea of jrazorx ;)

            bool valueManipulated = false;
            float pitch = m.ReadFloat(pitchAddrString);
            float yaw = m.ReadFloat(yawAddrString);

            float siny = (float)Math.Sin(yaw);
            float cosy = (float)Math.Cos(yaw);
            float sinp = (float)Math.Sin(pitch);
            float cosp = (float)Math.Cos(pitch);

            float playerX = m.ReadFloat(xAddrString);
            float playerY = m.ReadFloat(yAddrString);
            float playerZ = m.ReadFloat(zAddrString);

            float speedX = 0;
            float speedY = 0;
            float speedZ = 0;

            float speed = camSpeed;
            if (isSpeedBoostActive)
            {
                speed *= speedBoostMultiplier;
            }

            bool isArrowUpPressed = sim.InputDeviceState.IsHardwareKeyDown(forwardsKey);
            bool isArrowDownPressed = sim.InputDeviceState.IsHardwareKeyDown(backwardsKey);

            if (isArrowUpPressed)
            {
                speedX = -speed * cosp * siny;
                speedZ = -speed * cosp * cosy;
                speedY = -speed * sinp;  // Adjusted to move correctly with pitch
                valueManipulated = true;

            }
            else if (isArrowDownPressed)
            {
                speedX = speed * cosp * siny;
                speedZ = speed * cosp * cosy;
                speedY = speed * sinp;   // Adjusted to move correctly with pitch
                valueManipulated = true;

            }

            if (sim.InputDeviceState.IsHardwareKeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT))
            {
                isSpeedBoostActive = true;
            }
            else
            {
                isSpeedBoostActive = false;
            }

            if (sim.InputDeviceState.IsHardwareKeyDown(flyUpKey))
            {
                speedY += speed * 0.2f;
                valueManipulated = true;
            }
            else if (sim.InputDeviceState.IsHardwareKeyDown(flyDownKey))
            {
                speedY -= speed * 0.2f;
                valueManipulated = true;
            }

            playerX += speedX;
            playerY += speedY;
            playerZ += speedZ;

            //ensure the float values are converted to strings using the invariant culture
            string playerXString = playerX.ToString(CultureInfo.InvariantCulture);
            string playerYString = playerY.ToString(CultureInfo.InvariantCulture);
            string playerZString = playerZ.ToString(CultureInfo.InvariantCulture);

            if (valueManipulated)
            {
                //write the memory
                m.WriteMemory(xAddrString, "float", playerXString);
                m.WriteMemory(yAddrString, "float", playerYString);
                m.WriteMemory(zAddrString, "float", playerZString);
            }
        }
        private void teleport()
        {
            bool isArrived = false;

            #region autoCancelTP
            if (!attachToCamEnabled)
            {
                if (lastPos.X == xCoord && lastPos.Y == yCoord && lastPos.Z == zCoord)
                {
                    rubberbandCount++;
                }

                if (rubberbandCount > 3)
                {
                    doglide();
                    nofallFunction();
                    tpflag = false;
                    return;
                }
            }
            lastPos.X = xCoord;
            lastPos.Y = yCoord;
            lastPos.Z = zCoord;
            #endregion

            if (!glideEnabled)
            {
                doglide(); //to enable glide while teleport
                m.WriteMemory(movementModeAddrStr, "int", "6"); //to enable noclip while teleport
            }

            if (savedX == 0 || savedY == 0 || savedZ == 0)
            {
                log_console.Text = log_console.Text + "\n\r\n\rInvalid Value!";
                return;
            }

            float distance = (float)Math.Sqrt(Math.Pow(xCoord - savedX, 2) + Math.Pow(yCoord - savedY, 2) + Math.Pow(zCoord - savedZ, 2));

            if (distance <= 1.0f && distance > 0)
            {
                //if player is already close to the destination tp directly
                m.WriteMemory(xAddrString, "float", (savedX).ToString(CultureInfo.InvariantCulture));
                m.WriteMemory(yAddrString, "float", (savedY).ToString(CultureInfo.InvariantCulture));
                m.WriteMemory(zAddrString, "float", (savedZ).ToString(CultureInfo.InvariantCulture));
                Thread.Sleep(50);
                //m.WriteMemory(yAddrString, "float", (floorYValue).ToString(CultureInfo.InvariantCulture)); //bad idea (freecamtp)
                m.WriteMemory(movementModeAddrStr, "int", "1");

                isArrived = true;
                doglide();
            }
            else if (distance > 1.0f)
            {
                //calculate the normalized movement increments
                float moveX = (savedX - xCoord) / distance;
                float moveY = (savedY - yCoord) / distance;
                float moveZ = (savedZ - zCoord) / distance;

                //move towards the saved coordinates using normalized values
                m.WriteMemory(xAddrString, "float", (xCoord + moveX).ToString(CultureInfo.InvariantCulture));
                m.WriteMemory(yAddrString, "float", (yCoord + moveY).ToString(CultureInfo.InvariantCulture));
                m.WriteMemory(zAddrString, "float", (zCoord + moveZ).ToString(CultureInfo.InvariantCulture));

                Thread.Sleep(100);

                //check if the player has arrived
                isArrived = (distance <= 1.0f);
            }
            if (isArrived)
            {
                isArrived = false;
                tpflag = false;
            }
        }
        private void teleportNumpad()
        {
            while (true)
            {
                float playerXCoord = m.ReadFloat(xAddrString);
                float playerYCoord = m.ReadFloat(yAddrString);
                float playerZCoord = m.ReadFloat(zAddrString);
                bool isTPUpPressed = sim.InputDeviceState.IsHardwareKeyDown(TPUpKey);
                bool isTPDownPressed = sim.InputDeviceState.IsHardwareKeyDown(TPDownKey);
                bool isTPXUpPressed = sim.InputDeviceState.IsHardwareKeyDown(TPLeftKey);
                bool isTPXDownPressed = sim.InputDeviceState.IsHardwareKeyDown(TPRightKey);
                bool isTPZUpPressed = sim.InputDeviceState.IsHardwareKeyDown(TPForwardKey);
                bool isTPZDownPressed = sim.InputDeviceState.IsHardwareKeyDown(TPBackwardKey);

                //numpad teleport
                if (isTPUpPressed)
                {
                    m.WriteMemory(yAddrString, "float", (playerYCoord + 0.5f).ToString(CultureInfo.InvariantCulture));
                }
                if (isTPDownPressed)
                {
                    m.WriteMemory(yAddrString, "float", (playerYCoord - 0.25f).ToString());
                }
                if (isTPXUpPressed)
                {
                    m.WriteMemory(xAddrString, "float", (playerXCoord + 0.25f).ToString(CultureInfo.InvariantCulture));
                }
                if (isTPXDownPressed)
                {
                    m.WriteMemory(xAddrString, "float", (playerXCoord - 0.25f).ToString(CultureInfo.InvariantCulture));
                }
                if (isTPZUpPressed)
                {
                    m.WriteMemory(zAddrString, "float", (playerZCoord + 0.25f).ToString(CultureInfo.InvariantCulture));
                }
                if (isTPZDownPressed)
                {
                    m.WriteMemory(zAddrString, "float", (playerZCoord - 0.25f).ToString(CultureInfo.InvariantCulture));
                }
                Thread.Sleep(100);
            }
        }
        private string convertUintToHexString(UIntPtr uintToConvert)
        {
            string placeholder1 = uintToConvert.ToString();
            long placeholder2 = long.Parse(placeholder1);
            string hexstring = placeholder2.ToString("X2");
            return hexstring;
        }
        private static UIntPtr ParseHexToUIntPtr(string hexValue)
        {
            // Parse the hexadecimal string into a UIntPtr
            ulong numericValue = ulong.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

            return new UIntPtr(numericValue);
        }
        private void nofallFunction()
        {
            if (!nofallEnabled)
            {
                if (!nofallPatched)
                {
                    m.CreateCodeCave(nofallAddrString, patchedBytes, 6, 120);

                    gotoCaveBytes = m.ReadBytes(nofallAddrString, 6);

                    nofallEnabled = true;
                    nofallPatched = true;
                }
                else
                {
                    m.WriteBytes(nofallAddr, gotoCaveBytes);
                    nofallEnabled = true;
                }
            }
            else
            {
                nofallAddr = ParseHexToUIntPtr(nofallAddrString);
                m.WriteBytes(nofallAddr, originalBytes);
                nofallEnabled = false;
            }
        }
        private void infReachFunction()
        {
            if (!infReachEnabled)
            {
                if (!infReachPatched)
                {
                    m.CreateCodeCave(infReachAddressStr, infReachPatchedBytes, 6, 300);

                    infReachAlreadyPatchedBytes = m.ReadBytes(infReachAddressStr, 6);

                    infReachEnabled = true;
                    infReachPatched = true;
                }
                else
                {
                    m.WriteBytes(infReachAddressStr, infReachAlreadyPatchedBytes);
                    infReachEnabled = true;
                }
            }
            else
            {
                m.WriteBytes(infReachAddressStr, infReachOriginalBytes);
                infReachEnabled = false;
            }
        }
        private bool onlineCheck(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    log_console.Text = log_console.Text + "Tool is Online";
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
        private void updateCheck(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    DialogResult openDc = MessageBox.Show("Your tool is outdated. Please download a new version from the discord.\r\nWould you like to join the discord?", "Error", MessageBoxButtons.YesNo);
                    if (openDc == DialogResult.Yes)
                    {
                        openDiscord();
                    }
                    Environment.Exit(1);
                }
                else
                {
                    log_console.Text = log_console.Text + "\r\n\r\nYou are running the latest version\r\n\r\n";
                }
            }
        }
        //private void PreventProgramFromBeingDebuged()
        //{
        //    var a = DateTime.Now;
        //    Thread.Sleep(10);
        //    var b = DateTime.Now;
        //    if ((b - a).TotalSeconds > 2)
        //    {
        //        Application.ExitThread();
        //        Environment.Exit(0);
        //        this.Close();
        //    }
        //}
        private void startMainTimer()
        {
            try
            {
                mainTimer.Start();
            }
            catch (Exception ex)
            {
                log_console.Text = log_console.Text + ($"\r\nException in Main Thread: {ex.Message}");
            }
        }
        private void startPvPTimer()
        {
            try
            {
                pvpTimer.Start();
            }
            catch (Exception ex)
            {
                log_console.Text = log_console.Text + ($"\r\nException in PVP Thread: {ex.Message}");
            }
        }
        private void startgetBaseTimer()
        {
            try
            {
                timer_getBase.Start();
            }
            catch (Exception ex)
            {
                log_console.Text = log_console.Text + ($"\r\nException in getBase Thread: {ex.Message}");
            }
        }
        private void createCodeCave()
        {
            if (!noclipCave)
            {
                try
                {
                    byte[] patched_bytes = { 0x83, 0xB9, 0x1C, 0x03, 0x00, 0x00, 0x02, 0x0F, 0x85, 0x07, 0x00, 0x00, 0x00, 0x48, 0x89, 0x0D, 0x0C, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x01, 0x48, 0x8B, 0x40, 0x58 };

                    //Create Codecave
                    pbasecaveAddr = m.CreateCodeCave(noclipAddressStr, patched_bytes, 7, 120);

                    log_console.Invoke((MethodInvoker)delegate
                    {
                        log_console.Text = log_console.Text + "\r\n\r\nHooked";
                    });

                    //rest of code
                    noclipPatchedBytes = m.ReadBytes(noclipAddressStr, 7);
                    noclipPatched = true;
                    noclipCave = true;
                    //log_console.Text = log_console.Text + "\r\n\r\nCave Created";;
                }
                catch (Exception ex)
                {
                    log_console.Invoke((MethodInvoker)delegate
                    {
                        log_console.Text = log_console.Text + $"\r\n\r\nHook failed\r\n\r\nPlease restart the game\r\n\r\nError: {ex.Message}";
                    });
                    return;
                }

            }
            else
            {
                if (noclipPatched)
                {
                    m.WriteBytes(noclipAddressStr, noclipBytes);
                    log_console.Invoke((MethodInvoker)delegate
                    {
                        log_console.Text = log_console.Text + "\r\n\r\nUnhooked";
                    });
                    noclipPatched = false;
                }
                else
                {
                    m.WriteBytes(noclipAddressStr, noclipPatchedBytes);
                    log_console.Invoke((MethodInvoker)delegate
                    {
                        log_console.Text = log_console.Text + "\r\n\r\nRe-Hooked";
                    });
                    noclipPatched = true;
                }

            }

            //camera

            if (!camCave)
            {
                byte[] patched_bytes = { 0x83, 0xBF, 0xCC, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x85, 0x07, 0x00, 0x00, 0x00, 0x48, 0x89, 0x3D, 0x0D, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x87, 0xD0, 0x03, 0x00, 0x00 }; // patched bytes from the asm above

                //Create Codecave
                UIntPtr caveAddr = m.CreateCodeCave(cameraAddress, patched_bytes, 8, 220); //crash

                //logCaveAddr
                string caveAddrString = convertUintToHexString(caveAddr);
                //log_console.Text = log_console.Text + "\r\n\r\nCaveAddr = " + caveAddrString;

                //Add offset 0x12 to the cave addr (that's where the ptr for pbase is stored)
                camBaseUInt = (UIntPtr)UIntPtr.Add(caveAddr, 0x21); //caveAddr == UIntPtr
                //log caveAddr + offset
                string CambaseUIntString = convertUintToHexString(camBaseUInt);
                //log_console.Text = log_console.Text + "\r\n\r\nAddress of the Cam PTR = " + CambaseUIntString;

                Thread.Sleep(100);

                //readValue & convert to hex string
                long camBaselong = m.ReadLong(CambaseUIntString);
                CamBaseAddress = camBaselong.ToString("X2");
                camBaseUInt = ParseHexToUIntPtr(CamBaseAddress);

                //log to console
                //log_console.Text = log_console.Text + "\r\n\r\nCamBaseAddress = " + CamBaseAddress;


                //rest of code
                cameraPatchedBytes = m.ReadBytes(cameraAddress, 8);
                cameraPatched = true;
                camCave = true;
                //log_console.Text = log_console.Text + "\r\n\r\nCave Created";
            }
            else
            {
                if (cameraPatched)
                {
                    m.WriteBytes(cameraAddress, cameraBytes);
                    //log_console.Text = log_console.Text + "\r\n\r\nBytes Restored";
                    cameraPatched = false;
                }
                else
                {
                    m.WriteBytes(cameraAddress, cameraPatchedBytes);
                    //log_console.Text = log_console.Text + "\r\n\r\nBytes Patched";
                    cameraPatched = true;
                }
            }

            if (!speedHackCave)
            {
                byte[] patched_bytes = { 0x83, 0x3D, 0x23, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x84, 0x10, 0x00, 0x00, 0x00, 0x53, 0x48, 0x8B, 0x1D, 0x15, 0x00, 0x00, 0x00, 0x48, 0x89, 0x9E, 0xDC, 0x00, 0x00, 0x00, 0x5B, 0xF3, 0x0F, 0x10, 0xBE, 0xDC, 0x00, 0x00, 0x00 }; // patched bytes from the asm above

                //Create Codecave
                UIntPtr caveAddr = m.CreateCodeCave(speedHackAddrString, patched_bytes, 8, 120);

                //logCaveAddr
                string caveAddrString = convertUintToHexString(caveAddr);
                //log_console.Text = log_console.Text + "\r\n\r\nCaveAddr = " + caveAddrString;

                //Add offset 0x12 to the cave addr (that's where the ptr for pbase is stored)
                speedValueUInt = (UIntPtr)UIntPtr.Add(caveAddr, 0x2A); //caveAddr == UIntPtr
                //log caveAddr + offset
                speedValueUIntString = convertUintToHexString(speedValueUInt);
                //log_console.Text = log_console.Text + "\r\n\r\nAddress of the Speed PTR = " + speedValueUIntString;

                Thread.Sleep(100);

                //rest of code
                speedPatchedBytes = m.ReadBytes(speedHackAddrString, 8);
                speedPatched = true;
                speedHackCave = true;
                //log_console.Text = log_console.Text + "\r\n\r\nCave Created";
            }
            else
            {
                if (speedPatched)
                {
                    m.WriteBytes(speedHackAddrString, speedBytes);
                    //log_console.Text = log_console.Text + "\r\n\r\nBytes Restored";
                    speedPatched = false;
                }
                else
                {
                    m.WriteBytes(speedHackAddrString, speedPatchedBytes);
                    //log_console.Text = log_console.Text + "\r\n\r\nBytes Patched";
                    speedPatched = true;
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private void logToConsole(string textToLog)
        {
            log_console.Text = log_console.Text + $"\r\n{textToLog}";
        }
        /*
        private void checkForPvP()
        {
            ////Takes up to much performance
            
            //while (!isPVPEnabled)
            //{
            //    Thread.Sleep(10000);

            //    string aobScanResult = m.AoBScan(PVPAOB, true, false).Result.Sum().ToString("X2");

            //    if (aobScanResult == "00")
            //    {
            //        isPVPEnabled = true;
            //        MessageBox.Show("No PvP allowed ;)");
            //        Environment.Exit(0);
            //    }
            //}
        }
        */
        private void doglide()
        {
            if (!glideEnabled)
            {
                glideEnabled = true;

                m.WriteMemory(glideAddrString, "bytes", "90 90 90 90 90 90");
                m.WriteMemory(movementModeAddrStr, "int", "6");
            }
            else
            {
                glideEnabled = false;

                m.WriteMemory(glideAddrString, "bytes", glideAOB);
                m.WriteMemory(movementModeAddrStr, "int", "1");
            }
        }
        private void speedhackFunction()
        {
            if (!isSpeedhackEnabled)
            {
                box_speedhack.Invoke((MethodInvoker)delegate
                {
                    box_speedhack.Text = "On";
                    box_speedhack.Checked = true;
                });
                isSpeedhackEnabled = true;
            }
            else
            {
                box_speedhack.Invoke((MethodInvoker)delegate
                {
                    box_speedhack.Text = "Off";
                    box_speedhack.Checked = false;
                });
                isSpeedhackEnabled = false;
            }
        }
        private void tpToCam()
        {
            rubberbandCount = 0;

            savedX = m.ReadFloat(xCamAddrString);
            savedY = m.ReadFloat(yCamAddrString);
            savedZ = m.ReadFloat(zCamAddrString);

            log_console.Text = log_console.Text + $"\r\n\r\nTeleported to camera";

            //BottomScroll
            log_console.Focus();
            log_console.ScrollToCaret();
            log_console.SelectionLength = 0;
            //BottomScroll

            saveflag = true;

            tpflag = true;
            box_nofall.Checked = true;
        }
        private void hotkeysFunction()
        {
            while (true)
            {

                bool isinfJumpKeyPressed = sim.InputDeviceState.IsHardwareKeyDown(infJumpKey);
                bool isinfJumpKeyUP = sim.InputDeviceState.IsHardwareKeyUp(infJumpKey);
                bool isFreecamKeyPressed = sim.InputDeviceState.IsHardwareKeyDown(FreecamKey);
                bool isTPToCamKeyPressed = sim.InputDeviceState.IsHardwareKeyDown(TPToCamKey);
                bool isNofallKeyPressed = sim.InputDeviceState.IsHardwareKeyDown(NofallKey);
                bool isGlideKeyPressed = sim.InputDeviceState.IsHardwareKeyDown(GlideKey);
                bool isSpeedKeyPressed = sim.InputDeviceState.IsHardwareKeyDown(SpeedKey);
                bool spamKeyPressed = sim.InputDeviceState.IsKeyDown(enableSpamKey);

                try
                {
                    if (spamKeyPressed)
                    {
                        enableSpam = !enableSpam;
                        Thread.Sleep(200);
                    }
                    if (isInfJumpActivated)
                    {
                        if (isinfJumpKeyPressed && !isInfJumpToggled)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                //box_infJump.Checked = !box_infJump.Checked;
                                infJumpPatch();
                            });
                            isInfJumpToggled = true;
                            Thread.Sleep(200);
                        }
                        if (isinfJumpKeyUP && isInfJumpToggled)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                //box_infJump.Checked = !box_infJump.Checked;
                                infJumpPatch();
                            });
                            isInfJumpToggled = false;
                            Thread.Sleep(200);
                        }
                    }
                    if (isFreecamKeyPressed)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            toggle_freecam();
                        });
                        Thread.Sleep(200);
                    }
                    if (isTPToCamKeyPressed)
                    {
                        btn_tpToCam.PerformClick();
                        Thread.Sleep(200);
                    }
                    if (isNofallKeyPressed)
                    {
                        nofallFunction();
                        Thread.Sleep(200);
                    }
                    if (isGlideKeyPressed)
                    {
                        doglide();
                        Thread.Sleep(200);
                    }
                    if (isSpeedKeyPressed)
                    {
                        speedhackFunction();
                        Thread.Sleep(200);
                    }
                }
                catch { }
            }
        }
        private void openDiscord()
        {
            //Open discord
            string tempFile = Path.GetTempFileName() + ".bat";

            // Command to run Python script
            string blockCmd = @"start https://discord.gg/6MMEgcHJea";

            // Write the commands to the temporary batch file
            File.WriteAllText(tempFile, blockCmd);

            // Configure the process start info
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",  // Specify cmd.exe as the program to run
                Arguments = "/c \"" + tempFile + "\"",  // Pass the batch file as an argument to cmd.exe
                WindowStyle = ProcessWindowStyle.Hidden,  // Hide the window
                CreateNoWindow = true,  // Do not create a window
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false
            };

            // Start the process
            var process = Process.Start(startInfo);

            // Wait for the process to exit asynchronously
            System.Threading.Tasks.Task.Run(() => process.WaitForExit());
        }
        //private void saveLocations()
        //{
        //    using (FileStream fs = new FileStream("locations.dat", FileMode.Create))
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(fs, locationList);
        //    }
        //    MessageBox.Show("Saved locations");
        //}
        private void saveLocations()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("locations.xml"))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<customLocation>));
                    serializer.Serialize(writer, locationList);
                }

                MessageBox.Show("Saved locations");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving locations: {ex.Message}");
            }
        }
        private void loadLocations()
        {
            try
            {
                if (File.Exists("locations.xml"))
                {
                    using (StreamReader reader = new StreamReader("locations.xml"))
                    {
                        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<customLocation>));
                        locationList = (List<customLocation>)serializer.Deserialize(reader);

                        // Sort the list by customName
                        locationList = locationList.OrderBy(loc => loc.customName).ToList();

                        // Update the listbox
                        listbox_teleportLocations.Items.Clear();
                        foreach (var loc in locationList)
                        {
                            listbox_teleportLocations.Items.Add(loc.customName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading locations: {ex.Message}");
            }
        }
        private void restoreOriginalCode()
        {
            try
            {
                m.WriteBytes(noclipAddressStr, noclipBytes);
                noclipPatched = false;

                m.WriteBytes(cameraAddress, cameraBytes); //crash
                cameraPatched = false;

                m.WriteBytes(speedHackAddrString, speedBytes);
                speedPatched = false;

                m.WriteMemory(nofallAddrString, "bytes", "F3 44 0F 10 4F 10 44 0F 28 DF");
                nofallEnabled = false;

                m.WriteMemory(glideAddrString, "bytes", glideAOB);
                glideEnabled = false;

                m.WriteMemory(camCollisionAddrStr, "bytes", "F3 0F 11 8F 50 03 00 00");
                camCollisionEnabled = false;

                m.WriteMemory(noAnimationAddrString, "bytes", "F3 0F 11 8B 70 02 00 00");
                noAnimationPatched = false;

                m.WriteMemory(infJumpAddrStr, "bytes", "F2 0F 11 47 0C");
                infJumpPatched = false;

                m.WriteBytes(cameraYUInt, cameraYBytes);
                cameraYPatched = false;

                m.WriteBytes(cameraZUInt, cameraZBytes);
                cameraZPatched = false;

                m.WriteMemory($"{wallhackAddress}", "bytes", "74 0D");
                wallhackPatched = false;

                m.WriteMemory($"{wallhack2Address}", "bytes", "0F 84");
                wallhack2Patched = false;

                m.WriteMemory(movementModeAddrStr, "int", "1");

                m.WriteBytes(infReachAddressStr, infReachOriginalBytes);
            }
            catch
            {
                MessageBox.Show("Code restoration failed. You might want to restart your game.");
            }
        }
        private void antiAFKfunc()
        {
            var timePassed = DateTime.Now - dateTimeBuffer;  // Calculate the time difference

            if (timePassed.TotalMinutes > 5)
            {
                sim.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                Thread.Sleep(100);
                sim.Keyboard.KeyUp(VirtualKeyCode.VK_W);

                dateTimeBuffer = DateTime.Now;
            }

        }

        #endregion

    }
}
