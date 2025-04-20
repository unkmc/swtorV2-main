using System;
using System.Numerics;
using System.Windows.Forms;
using Memory;

namespace SWTOR_External.Classes
{
    public class TimerHandlers
    {
        private readonly Form1 _form;
        private readonly Variables _vars;
        private readonly Mem _mem;

        public TimerHandlers(Form1 form, Variables vars, Mem mem)
        {
            _form = form;
            _vars = vars;
            _mem = mem;
        }

        public void PvpTimer_Tick(object sender, EventArgs e)
        {
            _vars.pvpAddrStr = _vars.pvpAddrStr ?? "00";
            string isPvPEnabledAddr = ConvertUintToHexString(_vars.pvpAddr + 0x4);
            if (_vars.pvpAddrStr != "00")
            {
                int pvpByte = _mem.ReadByte(isPvPEnabledAddr);
                if (pvpByte != 0x45)
                {
                    _form.pvpTimer.Stop();
                    _form.cbox_noclip.Checked = false;
                    MessageBox.Show("PvP detected!\nPlease disable PvP to continue using the tool");
                    Environment.Exit(0);
                }
            }
        }

        public void MainTimer_Tick(object sender, EventArgs e)
        {
            if (_vars.PbaseUintString != null)
            {
                _vars.xAddr = _vars.playerBaseUInt + 0x68;
                _vars.xAddrString = ConvertUintToHexString(_vars.xAddr);

                _vars.yAddr = _vars.playerBaseUInt + 0x6C;
                _vars.yAddrString = ConvertUintToHexString(_vars.yAddr);

                _vars.zAddr = _vars.playerBaseUInt + 0x70;
                _vars.zAddrString = ConvertUintToHexString(_vars.zAddr);

                _vars.zCamAddr = _vars.camBaseUInt + 0x210;
                _vars.zCamAddrString = ConvertUintToHexString(_vars.zCamAddr);

                _vars.yCamAddr = _vars.camBaseUInt + 0x20C;
                _vars.yCamAddrString = ConvertUintToHexString(_vars.yCamAddr);

                _vars.xCamAddr = _vars.camBaseUInt + 0x208;
                _vars.xCamAddrString = ConvertUintToHexString(_vars.xCamAddr);

                _vars.pitchAddr = _vars.camBaseUInt + 0x290;
                _vars.pitchAddrString = ConvertUintToHexString(_vars.pitchAddr);

                _vars.movementModeAddr = _vars.playerBaseUInt + 0x390;
                _vars.movementModeAddrStr = ConvertUintToHexString(_vars.movementModeAddr);

                _vars.yawAddr = _vars.camBaseUInt + 0x218;
                _vars.yawAddrString = ConvertUintToHexString(_vars.yawAddr);

                _vars.heightAddr = _vars.playerBaseUInt + 0x84;
                _vars.heightAddrString = ConvertUintToHexString(_vars.heightAddr);

                _vars.floorYAddr = _vars.playerBaseUInt + 0x348;
                _vars.floorYAddrStr = ConvertUintToHexString(_vars.floorYAddr);

                UIntPtr wFloorAddr = _mem.Get64BitCode($"{_vars.PbaseUintString},0x330,0x2E0");
                _vars.wFloorAddrStr = ConvertUintToHexString(wFloorAddr);
                _vars.walkableFloor = _mem.ReadFloat(_vars.wFloorAddrStr);

                UIntPtr velocityBaseAddr = _mem.Get64BitCode($"{_vars.PbaseUintString},0x470");
                string velocityBaseAddrStr = ConvertUintToHexString(velocityBaseAddr);

                _vars.playerXVelocity = _mem.ReadFloat($"{velocityBaseAddrStr},0x0C");
                _vars.playerYVelocity = _mem.ReadFloat($"{velocityBaseAddrStr},0x10");
                _vars.playerZVelocity = _mem.ReadFloat($"{velocityBaseAddrStr},0x14");

                _vars.throwbackValue = _mem.ReadInt($"{_vars.PbaseUintString},0x64C");

                _vars.xCoord = _mem.ReadFloat(_vars.xAddrString);
                _vars.yCoord = _mem.ReadFloat(_vars.yAddrString);
                _vars.zCoord = _mem.ReadFloat(_vars.zAddrString);

                _vars.floorYValue = _mem.ReadFloat(_vars.floorYAddrStr);
                _vars.playerHeight = _mem.ReadFloat(_vars.heightAddrString);

                _form.lbl_coords.Text = $"X: {_vars.xCoord}\nY: {_vars.yCoord}\nZ: {_vars.zCoord}";
                _form.lbl_savedCoords.Text = $"X: {_vars.savedX}\nY: {_vars.savedY}\nZ: {_vars.savedZ}";
                _form.lbl_yFloorValue.Text = $"FloorY: {_vars.floorYValue}";

                if (_vars.tpflag)
                {
                    _form.teleport();
                }

                if (_vars.freeCamEnabled)
                {
                    _form.Freecam();
                }

                if (_vars.flyModeEnabled)
                {
                    _form.FlyMode();
                }

                if (_vars.noKnockbackEnabled)
                {
                    if (_vars.throwbackValue == 256)
                    {
                        _vars.lastThrowbackTime = DateTime.Now;
                        Thread.Sleep(100);
                        if (!_vars.throwBackSaved)
                        {
                            _vars.throwSavedX = _vars.xCoord;
                            _vars.throwSavedY = _vars.yCoord;
                            _vars.throwSavedZ = _vars.zCoord;
                            _vars.throwBackSaved = true;
                        }
                        _mem.WriteMemory(_vars.xAddrString, "float", _vars.throwSavedX.ToString());
                        _mem.WriteMemory(_vars.yAddrString, "float", _vars.throwSavedY.ToString());
                        _mem.WriteMemory(_vars.zAddrString, "float", _vars.throwSavedZ.ToString());
                    }
                    else
                    {
                        if ((DateTime.Now - _vars.lastThrowbackTime).TotalSeconds >= 3)
                        {
                            _vars.throwSavedX = 0;
                            _vars.throwSavedY = 0;
                            _vars.throwSavedZ = 0;
                            _vars.throwBackSaved = false;
                        }
                    }
                }

                if (_vars.antiAFK)
                {
                    _form.antiAFKfunc();
                }

                if (_vars.enableSpam)
                {
                    _form.lbl_chatspamstatus.Text = "On";
                    _form.lbl_chatspamstatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    _form.lbl_chatspamstatus.Text = "Off";
                    _form.lbl_chatspamstatus.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        public void TimerGetBase_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_vars.isSpeedhackEnabled)
                {
                    _mem.WriteMemory(_vars.speedValueUIntString, "float", _form.trckbr_speed.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (!_vars.isSpeedhackEnabled)
                {
                    _mem.WriteMemory(_vars.speedValueUIntString, "float", "0");
                }

                string caveAddrString = ConvertUintToHexString(_vars.pbasecaveAddr);
                _vars.playerBaseUInt = (UIntPtr)UIntPtr.Add(_vars.pbasecaveAddr, 0x20);
                _vars.PbaseUintString = ConvertUintToHexString(_vars.playerBaseUInt);

                long playerBaselong = _mem.ReadLong(_vars.PbaseUintString);
                _vars.PlayerBaseAddress = playerBaselong.ToString("X2");
                _vars.playerBaseUInt = ParseHexToUIntPtr(_vars.PlayerBaseAddress);
            }
            catch { }
        }

        private string ConvertUintToHexString(UIntPtr uintToConvert)
        {
            string placeholder1 = uintToConvert.ToString();
            long placeholder2 = long.Parse(placeholder1);
            string hexstring = placeholder2.ToString("X2");
            return hexstring;
        }

        private static UIntPtr ParseHexToUIntPtr(string hexValue)
        {
            ulong numericValue = ulong.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            return new UIntPtr(numericValue);
        }
    }
}