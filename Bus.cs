using System;

namespace NESv
{
    public class Bus
    {
        public Cpu6502 cpu;
        int[] ram = new int[64 * 1024];

        public Bus()
        {
            cpu = new Cpu6502();
            cpu.ConnectBus(this);
        }

        public void Write(int addr, int data)
        {
            //Check ram
            if (addr >= 0X0000 && addr <= 0XFFFF)
                //int type greater than 16bit only use 16bit
                ram[addr] = data & 0XFFFF;
        }

        public int Read(int addr, bool bReadOnly = false)
        {
            if (addr >= 0X0000 && addr <= 0XFFFF)
                return ram[addr];
            return 0X00;
        }
    }

}