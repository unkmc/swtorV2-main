using System;
using System.Numerics;
using WindowsInput.Native;

namespace SWTOR_External
{
    public class Variables
    {
        public bool moveAfterMsg = false;
        public bool msgSpamFlag = false;
        public bool enableSpam = false;
        public DateTime dateTimeBuffer = DateTime.Now;
        public bool antiAFK = false;
        public int rubberbandCount;
        public int creditClickerCount;
        public Vector3 lastPos;
        public bool darkmodeEnabled = false;
        public string urlRunning = "https://github.com/NightfallChease/s/blob/main/isRunning.sw";
        public string urlUpdate = "https://github.com/NightfallChease/s/blob/main/version9.2.sw";
        public string currentVersion = "v9.2";
        public bool noclipPatched = false;
        public bool cameraPatched = false;
        public bool cameraZPatched = false;
        public bool cameraYPatched = false;
        public bool noCollisionEnabled = false;
        public bool noclipCave = false;
        public bool camCave = false;
        public bool freeCamEnabled = false;
        public bool flyModeEnabled = false;
        public bool attachToCamEnabled = false;
        public bool nofallEnabled = false;
        public bool nofallPatched = false;
        public bool alwaysOnTop = false;
        public bool devEspEnabled = false;
        public bool devVelEnabled = false;
        public string noAnimationAddrString;
        public bool noAnimationPatched = false;
        public bool infJumpPatched = false;
        public string infJumpAddrStr;
        public string infJumpAOB = "F2 0F 11 47 0C 89 47 14 80";
        public string stuckAOB = "66 00 CC CC CC CC CC CC CC CC 48 8B C4 55 56";
        public string noAnimationAOB = "F3 0F 11 8B 70 02 00 00";
        public string noclipAOB = "48 8B 01 48 8B 40 58 FF 15 ?? ?? ?? ?? 48 8B C8";
        public string cameraAOB = "F3 0F 10 87 D0 03 00 00";
        public string cameraYAOB = "89 87 10 02 00 00 F3";
        public string cameraZAOB = "F2 0F 11 87 08 02 00 00";
        public string nofallAOB = "F3 44 0F 10 4F 10 44 0F 28 DF";
        public string speedHackAOB = "F3 0F 10 BE DC 00 00 00 0F 28 F7";
        public string devESPAob = "0F 84 ?? ?? ?? ?? B9 06 00 00 00 41 FF D4 48 BE";
        public string velocityIndicatorAOB = "74 1F B9 ?? ?? ?? ?? 41 FF D6 4C 8D 45 D0 B9 ?? ?? ?? ?? 48 89 F2 FF D7 48 8B 4D D0 FF D0 41 FF D7";
        public string glideAOB = "F3 44 0F 11 43 14 F3 0F";
        public string wallhackAOB = "74 0D 83 4B 68 01";
        public string wallhack2AOB = "0F 84 76 02 00 00 49 8B CE";
        public string infReachAOB = "67 FD FF 8B 06 89 07 41 80 0F 0C";
        public string camCollisionAOB = "F3 0F 11 8F ?? ?? 00 00 0F";
        public string camCollisionAddrStr;
        public bool camCollisionEnabled = false;
        public bool infReachEnabled = false;
        public bool infReachPatched = false;
        public string infReachAddressStr;
        public UIntPtr infReachAddress;
        public byte[] infReachPatchedBytes = { 0x81, 0xBC, 0x24, 0x3C, 0x01, 0x00, 0x00, 0xF4, 0x7D, 0x00, 0x00, 0x0F, 0x8C, 0x16, 0x00, 0x00, 0x00, 0x81, 0xBC, 0x24, 0x3C, 0x01, 0x00, 0x00, 0x2C, 0x7E, 0x00, 0x00, 0x0F, 0x87, 0x05, 0x00, 0x00, 0x00, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x89, 0x07, 0x41, 0x80, 0x0F, 0x0C };
        public string wallhackAddress;
        public string wallhack2Address;
        public bool wallhackPatched;
        public bool wallhack2Patched;
        public bool glideEnabled = false;
        public VirtualKeyCode infJumpKey = VirtualKeyCode.SPACE;
        public VirtualKeyCode TPUpKey;
        public VirtualKeyCode TPDownKey;
        public VirtualKeyCode TPLeftKey;
        public VirtualKeyCode TPRightKey;
        public VirtualKeyCode TPForwardKey;
        public VirtualKeyCode TPBackwardKey;
        public VirtualKeyCode FreecamKey;
        public VirtualKeyCode TPToCamKey;
        public VirtualKeyCode NofallKey;
        public VirtualKeyCode GlideKey;
        public VirtualKeyCode SpeedKey;
        public VirtualKeyCode forwardsKey;
        public VirtualKeyCode backwardsKey;
        public VirtualKeyCode flyUpKey;
        public VirtualKeyCode flyDownKey;
        public VirtualKeyCode enableSpamKey;
        public string PlayerBaseAddress = "";
        public string CamBaseAddress = "";
        public UIntPtr playerBaseUInt;
        public UIntPtr camBaseUInt;
        public UIntPtr EspUint;
        public int baseAddr = 0;
        public UIntPtr noclipAddress;
        public string noclipAddressStr = "";
        public string glideAddrString;
        public string cameraAddress = "";
        public string cameraYAddress = "";
        public string cameraZAddress = "";
        public float xCoord = 0;
        public float yCoord = 0;
        public float zCoord = 0;
        public UIntPtr floorYAddr;
        public string floorYAddrStr;
        public float floorYValue;
        public float camSpeed = 0.04f;
        public float speedBoostMultiplier = 2f;
        public bool isSpeedBoostActive = false;
        public string xAddrString = "";
        public string yAddrString = "";
        public string zAddrString = "";
        public string devESPAddrString = "";
        public string velocityIndAddrStr = "";
        public string xCamAddrString = "";
        public string yCamAddrString = "";
        public string zCamAddrString = "";
        public string nofallAddrString = "";
        public string speedValueUIntString;
        public string speedHackAddrString;
        public string yawAddrString;
        public string pitchAddrString;
        public string wFloorAddrStr;
        public float walkableFloor;
        public float savedX = 0;
        public float savedY = 0;
        public float savedZ = 0;
        public UIntPtr xAddr;
        public UIntPtr yAddr;
        public UIntPtr zAddr;
        public UIntPtr xCamAddr;
        public UIntPtr yCamAddr;
        public UIntPtr zCamAddr;
        public UIntPtr nofallAddr;
        public UIntPtr pitchAddr;
        public UIntPtr yawAddr;
        public UIntPtr cameraYUInt;
        public UIntPtr cameraZUInt;
        public UIntPtr heightAddr;
        public UIntPtr movementModeAddr;
        public float playerXVelocity;
        public float playerYVelocity;
        public float playerZVelocity;
        public int throwbackValue;
        public float throwSavedX;
        public float throwSavedY;
        public float throwSavedZ;
        public bool throwBackSaved;
        public DateTime lastThrowbackTime = DateTime.Now;
        public string movementModeAddrStr;
        public string heightAddrString;
        public bool tpflag = false;
        public bool saveflag = false;
        public bool speedHackCave;
        public float playerHeight;
        public string userName = Environment.UserName;
        public UIntPtr speedValueUInt;
        public byte[] speedPatchedBytes;
        public bool speedPatched;
        public UIntPtr pbasecaveAddr;
        public bool isPVPEnabled = false;
        public string PbaseUintString;
        public string pvpAddrStr;
        public UIntPtr pvpAddr;
        public bool isSpeedhackEnabled = false;
        public byte[] noclipPatchedBytes = { };
        public byte[] cameraPatchedBytes = { };
        public byte[] cameraYPatchedBytes = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
        public byte[] cameraZPatchedBytes = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
        public byte[] noclipBytes = { 0x48, 0x8B, 0x01, 0x48, 0x8B, 0x40, 0x58 };
        public byte[] cameraBytes = { 0xF3, 0x0F, 0x10, 0x87, 0xD0, 0x03, 0x00, 0x00 };
        public byte[] speedBytes = { 0xF3, 0x0F, 0x10, 0xBE, 0xDC, 0x00, 0x00, 0x00, 0x0F, 0x28, 0xF7 };
        public byte[] cameraYBytes = { 0x89, 0x87, 0x10, 0x02, 0x00, 0x00, 0xF3 };
        public byte[] cameraZBytes = { 0xF2, 0x0F, 0x11, 0x87, 0x08, 0x02, 0x00, 0x00 };
        public byte[] patchedBytes = { 0xC7, 0x47, 0x10, 0x33, 0x33, 0x33, 0xBF, 0xF3, 0x44, 0x0F, 0x10, 0x4F, 0x10 };
        public byte[] originalBytes = { 0xF3, 0x44, 0x0F, 0x10, 0x4F, 0x10 };
        public byte[] gotoCaveBytes = { };
        public byte[] infReachAlreadyPatchedBytes = { };
        public byte[] infReachOriginalBytes = { 0x89, 0x07, 0x41, 0x80, 0x0F, 0x0C };
        public bool stuckPatched = false;
        public string stuckAddrStr;
        public UIntPtr stuckAddrUint;
        public bool isInfJumpToggled = false;
        public bool noKnockbackEnabled = false;
        public bool isInfJumpActivated = false;
    }
}