using System;


namespace NESv
{
    public class Cpu6502
    {
        public int a = 0x00;       // Accumulator Register
        public int x = 0x00;       // X Register
        public int y = 0x00;       // Y Register
        public int stkp = 0x00;    // Stack Point
        public int pc = 0x00;      // Program Counter
        public int status = 0X00;  // Status Register

        private int addr_abs = 0X0000;
        private int addr_rel = 0X00;
        private int opcode = 0X00;
        private int cycles = 0;/*  */

        private Bus bus;
        public Instraction[] lookup = new Instraction[]{
            new Instraction( "BRK", BRK, IMM, 7 ),new Instraction( "ORA",/*  */ ORA, IZX, 6 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 3 ),new Instraction( "ORA", ORA, ZP0, 3 ),new Instraction( "ASL", ASL, ZP0, 5 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "PHP", PHP, IMP, 3 ),new Instraction( "ORA", ORA, IMM, 2 ),new Instraction( "ASL", ASL, IMP, 2 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "ORA", ORA, ABS, 4 ),new Instraction( "ASL", ASL, ABS, 6 ),new Instraction( "???", XXX, IMP, 6 ),
            new Instraction( "BPL", BPL, REL, 2 ),new Instraction( "ORA", ORA, IZY, 5 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "ORA", ORA, ZPX, 4 ),new Instraction( "ASL", ASL, ZPX, 6 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "CLC", CLC, IMP, 2 ),new Instraction( "ORA", ORA, ABY, 4 ),new Instraction( "???", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 7 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "ORA", ORA, ABX, 4 ),new Instraction( "ASL", ASL, ABX, 7 ),new Instraction( "???", XXX, IMP, 7 ),
            new Instraction( "JSR", JSR, ABS, 6 ),new Instraction( "AND", AND, IZX, 6 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "BIT", BIT, ZP0, 3 ),new Instraction( "AND", AND, ZP0, 3 ),new Instraction( "ROL", ROL, ZP0, 5 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "PLP", PLP, IMP, 4 ),new Instraction( "AND", AND, IMM, 2 ),new Instraction( "ROL", ROL, IMP, 2 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "BIT", BIT, ABS, 4 ),new Instraction( "AND", AND, ABS, 4 ),new Instraction( "ROL", ROL, ABS, 6 ),new Instraction( "???", XXX, IMP, 6 ),
            new Instraction( "BMI", BMI, REL, 2 ),new Instraction( "AND", AND, IZY, 5 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "AND", AND, ZPX, 4 ),new Instraction( "ROL", ROL, ZPX, 6 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "SEC", SEC, IMP, 2 ),new Instraction( "AND", AND, ABY, 4 ),new Instraction( "???", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 7 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "AND", AND, ABX, 4 ),new Instraction( "ROL", ROL, ABX, 7 ),new Instraction( "???", XXX, IMP, 7 ),
            new Instraction( "RTI", RTI, IMP, 6 ),new Instraction( "EOR", EOR, IZX, 6 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 3 ),new Instraction( "EOR", EOR, ZP0, 3 ),new Instraction( "LSR", LSR, ZP0, 5 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "PHA", PHA, IMP, 3 ),new Instraction( "EOR", EOR, IMM, 2 ),new Instraction( "LSR", LSR, IMP, 2 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "JMP", JMP, ABS, 3 ),new Instraction( "EOR", EOR, ABS, 4 ),new Instraction( "LSR", LSR, ABS, 6 ),new Instraction( "???", XXX, IMP, 6 ),
            new Instraction( "BVC", BVC, REL, 2 ),new Instraction( "EOR", EOR, IZY, 5 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "EOR", EOR, ZPX, 4 ),new Instraction( "LSR", LSR, ZPX, 6 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "CLI", CLI, IMP, 2 ),new Instraction( "EOR", EOR, ABY, 4 ),new Instraction( "???", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 7 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "EOR", EOR, ABX, 4 ),new Instraction( "LSR", LSR, ABX, 7 ),new Instraction( "???", XXX, IMP, 7 ),
            new Instraction( "RTS", RTS, IMP, 6 ),new Instraction( "ADC", ADC, IZX, 6 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 3 ),new Instraction( "ADC", ADC, ZP0, 3 ),new Instraction( "ROR", ROR, ZP0, 5 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "PLA", PLA, IMP, 4 ),new Instraction( "ADC", ADC, IMM, 2 ),new Instraction( "ROR", ROR, IMP, 2 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "JMP", JMP, IND, 5 ),new Instraction( "ADC", ADC, ABS, 4 ),new Instraction( "ROR", ROR, ABS, 6 ),new Instraction( "???", XXX, IMP, 6 ),
            new Instraction( "BVS", BVS, REL, 2 ),new Instraction( "ADC", ADC, IZY, 5 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "ADC", ADC, ZPX, 4 ),new Instraction( "ROR", ROR, ZPX, 6 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "SEI", SEI, IMP, 2 ),new Instraction( "ADC", ADC, ABY, 4 ),new Instraction( "???", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 7 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "ADC", ADC, ABX, 4 ),new Instraction( "ROR", ROR, ABX, 7 ),new Instraction( "???", XXX, IMP, 7 ),
            new Instraction( "???", NOP, IMP, 2 ),new Instraction( "STA", STA, IZX, 6 ),new Instraction( "???", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "STY", STY, ZP0, 3 ),new Instraction( "STA", STA, ZP0, 3 ),new Instraction( "STX", STX, ZP0, 3 ),new Instraction( "???", XXX, IMP, 3 ),new Instraction( "DEY", DEY, IMP, 2 ),new Instraction( "???", NOP, IMP, 2 ),new Instraction( "TXA", TXA, IMP, 2 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "STY", STY, ABS, 4 ),new Instraction( "STA", STA, ABS, 4 ),new Instraction( "STX", STX, ABS, 4 ),new Instraction( "???", XXX, IMP, 4 ),
            new Instraction( "BCC", BCC, REL, 2 ),new Instraction( "STA", STA, IZY, 6 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "STY", STY, ZPX, 4 ),new Instraction( "STA", STA, ZPX, 4 ),new Instraction( "STX", STX, ZPY, 4 ),new Instraction( "???", XXX, IMP, 4 ),new Instraction( "TYA", TYA, IMP, 2 ),new Instraction( "STA", STA, ABY, 5 ),new Instraction( "TXS", TXS, IMP, 2 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "???", NOP, IMP, 5 ),new Instraction( "STA", STA, ABX, 5 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "???", XXX, IMP, 5 ),
            new Instraction( "LDY", LDY, IMM, 2 ),new Instraction( "LDA", LDA, IZX, 6 ),new Instraction( "LDX", LDX, IMM, 2 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "LDY", LDY, ZP0, 3 ),new Instraction( "LDA", LDA, ZP0, 3 ),new Instraction( "LDX", LDX, ZP0, 3 ),new Instraction( "???", XXX, IMP, 3 ),new Instraction( "TAY", TAY, IMP, 2 ),new Instraction( "LDA", LDA, IMM, 2 ),new Instraction( "TAX", TAX, IMP, 2 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "LDY", LDY, ABS, 4 ),new Instraction( "LDA", LDA, ABS, 4 ),new Instraction( "LDX", LDX, ABS, 4 ),new Instraction( "???", XXX, IMP, 4 ),
            new Instraction( "BCS", BCS, REL, 2 ),new Instraction( "LDA", LDA, IZY, 5 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "LDY", LDY, ZPX, 4 ),new Instraction( "LDA", LDA, ZPX, 4 ),new Instraction( "LDX", LDX, ZPY, 4 ),new Instraction( "???", XXX, IMP, 4 ),new Instraction( "CLV", CLV, IMP, 2 ),new Instraction( "LDA", LDA, ABY, 4 ),new Instraction( "TSX", TSX, IMP, 2 ),new Instraction( "???", XXX, IMP, 4 ),new Instraction( "LDY", LDY, ABX, 4 ),new Instraction( "LDA", LDA, ABX, 4 ),new Instraction( "LDX", LDX, ABY, 4 ),new Instraction( "???", XXX, IMP, 4 ),
            new Instraction( "CPY", CPY, IMM, 2 ),new Instraction( "CMP", CMP, IZX, 6 ),new Instraction( "???", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "CPY", CPY, ZP0, 3 ),new Instraction( "CMP", CMP, ZP0, 3 ),new Instraction( "DEC", DEC, ZP0, 5 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "INY", INY, IMP, 2 ),new Instraction( "CMP", CMP, IMM, 2 ),new Instraction( "DEX", DEX, IMP, 2 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "CPY", CPY, ABS, 4 ),new Instraction( "CMP", CMP, ABS, 4 ),new Instraction( "DEC", DEC, ABS, 6 ),new Instraction( "???", XXX, IMP, 6 ),
            new Instraction( "BNE", BNE, REL, 2 ),new Instraction( "CMP", CMP, IZY, 5 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "CMP", CMP, ZPX, 4 ),new Instraction( "DEC", DEC, ZPX, 6 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "CLD", CLD, IMP, 2 ),new Instraction( "CMP", CMP, ABY, 4 ),new Instraction( "NOP", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 7 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "CMP", CMP, ABX, 4 ),new Instraction( "DEC", DEC, ABX, 7 ),new Instraction( "???", XXX, IMP, 7 ),
            new Instraction( "CPX", CPX, IMM, 2 ),new Instraction( "SBC", SBC, IZX, 6 ),new Instraction( "???", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "CPX", CPX, ZP0, 3 ),new Instraction( "SBC", SBC, ZP0, 3 ),new Instraction( "INC", INC, ZP0, 5 ),new Instraction( "???", XXX, IMP, 5 ),new Instraction( "INX", INX, IMP, 2 ),new Instraction( "SBC", SBC, IMM, 2 ),new Instraction( "NOP", NOP, IMP, 2 ),new Instraction( "???", SBC, IMP, 2 ),new Instraction( "CPX", CPX, ABS, 4 ),new Instraction( "SBC", SBC, ABS, 4 ),new Instraction( "INC", INC, ABS, 6 ),new Instraction( "???", XXX, IMP, 6 ),
            new Instraction( "BEQ", BEQ, REL, 2 ),new Instraction( "SBC", SBC, IZY, 5 ),new Instraction( "???", XXX, IMP, 2 ),new Instraction( "???", XXX, IMP, 8 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "SBC", SBC, ZPX, 4 ),new Instraction( "INC", INC, ZPX, 6 ),new Instraction( "???", XXX, IMP, 6 ),new Instraction( "SED", SED, IMP, 2 ),new Instraction( "SBC", SBC, ABY, 4 ),new Instraction( "NOP", NOP, IMP, 2 ),new Instraction( "???", XXX, IMP, 7 ),new Instraction( "???", NOP, IMP, 4 ),new Instraction( "SBC", SBC, ABX, 4 ),new Instraction( "INC", INC, ABX, 7 ),new Instraction( "???", XXX, IMP, 7 ),
        };

        public Cpu6502()
        {

        }

        public void ConnectBus(Bus bus)
        {
            this.bus = bus;
        }

        public int Read(int addr)
        {
            return bus.Read(addr);
        }

        public void Write(int addr, int data)
        {
            bus.Write(addr, data);
        }

        public int GetFlag(FLAG f)
        {
            return (this.status & (int)f) > 0 ? 1 : 0;
        }

        public void SetFlag(FLAG f, bool v)
        {
            if (v)
                this.status |= (int)f;
            else
                this.status &= (int)~f;
        }


        public void clock()
        {
            if (cycles == 0)
            {
                //使用pc寄存器读取状态码
                opcode = Read(pc);
                pc++;
                //获取执行指令需要的时钟周期
                cycles = lookup[opcode].cycles;

                int additional_cycle1 = this.lookup[opcode].addrmode();
                int additional_cycle2 = this.lookup[opcode].operate();
                cycles += (additional_cycle1 & additional_cycle2);
            }
            cycles--;
        }

        public void reset() { }
        public void irq() { }
        public void nmi() { }

        public int fetch() { return 0; }

        public void Disassemble(int satrt, int stop)
        {
            int addr = satrt;

            while (addr <= stop)
            {
                int code = Read(addr++);
                Instraction instraction = lookup[code];
                System.Console.WriteLine(Convert.ToString(code, 16) + "\t" + instraction.name);
            }
        }

        // Addressing Modes
        private static int IMP() { return 0; }
        private static int IMM() { return 0; }
        private static int ZP0() { return 0; }
        private static int ZPX() { return 0; }
        private static int ZPY() { return 0; }
        private static int REL() { return 0; }
        private static int ABS() { return 0; }
        private static int ABX() { return 0; }
        private static int ABY() { return 0; }
        private static int IND() { return 0; }
        private static int IZX() { return 0; }
        private static int IZY() { return 0; }

        //
        private static int LDA() { return 0; }
        private static int LDX() { return 0; }
        private static int LDY() { return 0; }
        private static int STA() { return 0; }
        private static int STX() { return 0; }
        private static int STY() { return 0; }
        private static int STZ() { return 0; }
        private static int PHA() { return 0; }
        private static int PHX() { return 0; }
        private static int PHY() { return 0; }
        private static int PHP() { return 0; }
        private static int PLA() { return 0; }
        private static int PLX() { return 0; }
        private static int PLY() { return 0; }
        private static int PLP() { return 0; }
        private static int TSX() { return 0; }
        private static int TXS() { return 0; }
        private static int INA() { return 0; }
        private static int INX() { return 0; }
        private static int INY() { return 0; }
        private static int DEA() { return 0; }
        private static int DEX() { return 0; }
        private static int DEY() { return 0; }
        private static int INC() { return 0; }
        private static int DEC() { return 0; }
        private static int ASL() { return 0; }
        private static int LSR() { return 0; }
        private static int ROL() { return 0; }
        private static int ROR() { return 0; }
        private static int AND() { return 0; }
        private static int ORA() { return 0; }
        private static int EOR() { return 0; }
        private static int BIT() { return 0; }
        private static int CMP() { return 0; }
        private static int CPX() { return 0; }
        private static int CPY() { return 0; }
        private static int TRB() { return 0; }
        private static int TSB() { return 0; }
        private static int RMB() { return 0; }
        private static int SMB() { return 0; }
        private static int ADC() { return 0; }
        private static int SBC() { return 0; }
        private static int JMP() { return 0; }
        private static int JSR() { return 0; }
        private static int RTS() { return 0; }
        private static int RTI() { return 0; }
        private static int BRA() { return 0; }
        private static int BEQ() { return 0; }
        private static int BNE() { return 0; }
        private static int BCC() { return 0; }
        private static int BCS() { return 0; }
        private static int BVC() { return 0; }
        private static int BVS() { return 0; }
        private static int BMI() { return 0; }
        private static int BPL() { return 0; }
        private static int BBR() { return 0; }
        private static int BBS() { return 0; }
        private static int CLC() { return 0; }
        private static int CLD() { return 0; }
        private static int CLI() { return 0; }
        private static int CLV() { return 0; }
        private static int SEC() { return 0; }
        private static int SED() { return 0; }
        private static int SEI() { return 0; }
        private static int TAX() { return 0; }
        private static int TAY() { return 0; }
        private static int TXA() { return 0; }
        private static int TYA() { return 0; }
        private static int NOP() { return 0; }
        private static int BRK() { return 0; }
        private static int XXX() { return 0; }


        public enum FLAG
        {
            C = (1 << 0),   // Carry Bit
            Z = (1 << 1),   // Zero
            I = (1 << 2),   // Disable Inter
            D = (1 << 3),   // Decimal Mode
            B = (1 << 4),   // Break
            U = (1 << 5),   // Unused
            V = (1 << 6),   // Overflow
            N = (1 << 7),   // Negative
        }

        public readonly struct Instraction
        {
            public delegate int OperateFunction();
            public delegate int AddrmodeFunction();

            public readonly string name;
            public readonly OperateFunction operate;
            public readonly AddrmodeFunction addrmode;
            public readonly int cycles;

            public Instraction(string name, OperateFunction operate, AddrmodeFunction addrmode, int cycles)
            {
                this.name = name;
                this.operate = operate;
                this.addrmode = addrmode;
                this.cycles = cycles;
            }

            public override string ToString()
            {
                return $"name:{name} cycles:{cycles}";
            }
        }
    }

}