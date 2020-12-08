using System;
using System.Collections.Generic;
using System.IO;

namespace Day8
{
    public enum OpCode
    {
        Nop,
        Acc,
        Jmp,
    }

    public class Instruction
    {
        public OpCode OpCode { get; private set; }

        public int Value { get; private set; }

        public Instruction(OpCode code, int value)
        {
            OpCode = code;
            Value = value;
        }

        public void OverwriteOpCode(OpCode code)
        {
            OpCode = code;
        }
    }

    public class MachineState
    {
        public int ProgramCounter { get; set; } = 0;

        public int Accumulator { get; set; } = 0;

        public bool OpCodeOverwritten { get; set; } = false;
    }

    class Program
    {
        static void Main(string[] args)
        {
            // PartOne();
            PartTwo();
        }

        static void PartOne()
        {
            var visitedAddresses = new List<int>();
            var instructionSet = ParseBootCode();
            var state = new MachineState();

            while (state.ProgramCounter < instructionSet.Count)
            {
                if (visitedAddresses.Contains(state.ProgramCounter))
                {
                    Console.WriteLine($"Loop detected at instruction offset {state.ProgramCounter}, accumulator value {state.Accumulator}");
                    return;
                }

                var currentInstruction = instructionSet[state.ProgramCounter];
                visitedAddresses.Add(state.ProgramCounter);

                switch (currentInstruction.OpCode)
                {
                    case OpCode.Nop:
                        state.ProgramCounter++;
                        break;
                    case OpCode.Acc:
                        state.Accumulator += currentInstruction.Value;
                        state.ProgramCounter++;
                        break;
                    case OpCode.Jmp:
                        state.ProgramCounter += currentInstruction.Value;
                        break;
                }
            }
        }

        static void PartTwo()
        {
            var instructionSet = ParseBootCode();

            for (int i = 0; i < instructionSet.Count; i++)
            {
                var patchedInstructionSet = ParseBootCode();

                for (int j = i; j > 0; j--)
                {
                    if (patchedInstructionSet[j].OpCode == OpCode.Nop)
                    {
                        patchedInstructionSet[j].OverwriteOpCode(OpCode.Jmp);
                        break;
                    }
                    else if (patchedInstructionSet[j].OpCode == OpCode.Jmp)
                    {
                        patchedInstructionSet[j].OverwriteOpCode(OpCode.Nop);
                        break;
                    }
                }

                var runResult = RunBootCode(patchedInstructionSet);

                if (runResult != null)
                {
                    Console.WriteLine($"Program completed, accumulator value {runResult.Accumulator}");

                    return;
                }
            }
        }

        private static MachineState RunBootCode(List<Instruction> instructionSet)
        {
            var visitedAddresses = new List<int>();
            var state = new MachineState();

            while (state.ProgramCounter < instructionSet.Count)
            {
                if (visitedAddresses.Contains(state.ProgramCounter))
                {
                    return null;
                }

                var currentInstruction = instructionSet[state.ProgramCounter];
                visitedAddresses.Add(state.ProgramCounter);

                switch (currentInstruction.OpCode)
                {
                    case OpCode.Nop:
                        state.ProgramCounter++;
                        break;
                    case OpCode.Acc:
                        state.Accumulator += currentInstruction.Value;
                        state.ProgramCounter++;
                        break;
                    case OpCode.Jmp:
                        state.ProgramCounter += currentInstruction.Value;
                        break;
                }
            }

            return state;
        }

        static List<Instruction> ParseBootCode()
        {
            var instructions = new List<Instruction>();

            using (var inputFile = File.OpenRead("input.txt"))
            {
                using (var reader = new StreamReader(inputFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var currentLine = reader.ReadLine();
                        var instructionParts = currentLine.Split(' ');

                        var opCode = OpCode.Nop;
                        var opValue = int.Parse(instructionParts[1]);

                        switch (instructionParts[0])
                        {
                            case "nop":
                                opCode = OpCode.Nop;
                                break;
                            case "acc":
                                opCode = OpCode.Acc;
                                break;
                            case "jmp":
                                opCode = OpCode.Jmp;
                                break;
                        }

                        instructions.Add(new Instruction(opCode, opValue));
                    }
                }
            }

            return instructions;
        }
    }
}
