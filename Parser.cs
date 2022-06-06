using System.Diagnostics;

namespace Retro_Command_Interpretator;
public static class Parser
{
    public static void ParseScript(string input)
    {
        if (input!.StartsWith("sendOut"))
        {
            if (input.Length < 8)
                RCI_Core.WriteTip("Usage of \"sendOut\": sendOut <text>");
            else
            {
                if (input[7] == ' ')
                    Console.WriteLine(input[(input.IndexOf(' ')+1)..]);
                else
                    RCI_Core.WriteTip("Usage of \"sendOut\": sendOut <text>");
            }

        }
        else if (input!.StartsWith("cleanScreen"))
        {
            Console.Clear();
        }
        else if (input!.StartsWith("help"))
        {
            RCI_Core.SendHelp();
        }
        else if (input!.StartsWith("whatsHere"))
        {
            RCI_Core.PrintCurrentDirectory();
        }
        else if (input!.StartsWith("changelog"))
        {
            RCI_Core.Changelog();
        }
        else if (input!.StartsWith("goIn"))
        {
            if (input.Length < 6)
                RCI_Core.WriteTip("Usage of \"goIn\": goIn <directoryName>");
            else
            {
                if (input[4] != ' ')
                    RCI_Core.WriteTip("You need space before directory name");

                if (Directory.Exists(input[5..]))
                    Environment.CurrentDirectory = input[5..];
                else
                    RCI_Core.WriteError("Invalid directory");
            }
        }
        else if (input!.StartsWith("run"))
        {
            if (input.Length < 5)
                RCI_Core.WriteTip("Usage of \"run\": run <script>.rci");
            else
            {
                if (File.Exists(input[4..]) && Path.GetExtension(input[4..]) == ".rci")
                {
                    try
                    {
                        ParseScriptFile(input[4..]);
                    }
                    catch (Exception ex)
                    {
                        RCI_Core.WriteError($"{ex.Message}");
                    }
                }

                else
                    RCI_Core.WriteError("Specified script doesn\'t exists or it doesn\'t have \".rci\" extension");
            }
        }
        else if (input!.StartsWith("rm"))
        {
            if (input.Length < 4)
                RCI_Core.WriteTip("Usage of \"rm\": rm <file/dir>");
            else
            {
                Console.Write($"Are you sure to delete \"{input[3..]}\"? THIS CANNOT BE UNDONE!!! [Y, N] > ");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.Write("\n");

                if (key.Key == ConsoleKey.Y)
                {
                    bool success = false;
                    try
                    {
                        if (File.Exists(input[3..]))
                        {
                            File.Delete(input[3..]);
                            success = true;

                        }
                        else if (Directory.Exists(input[3..]))
                        {
                            Directory.Delete(input[3..]);
                            success = true;
                        }
                        else
                        {
                            throw new InterpreterException("File or directory with that name doesn't exists");
                        }

                        if (success)
                            RCI_Core.WriteSuccess($"Sucessfully removed \"{input[3..]}\"");
                    }
                    catch (Exception ex)
                    {
                        RCI_Core.WriteError(ex.Message);
                    }
                }
            }
        }
        else if (input!.StartsWith("md"))
        {
            if (input.Length < 4)
                RCI_Core.WriteTip("Usage of \"md\": md <dirname>");
            else
            {
                try
                {
                    Directory.CreateDirectory(input[3..]);
                    RCI_Core.WriteSuccess($"Successfully created directory with name \"{input[3..]}\"");
                }
                catch (Exception ex)
                {
                    RCI_Core.WriteError(ex.Message);
                }
            }
        }
        else if (input.Contains("::"))
        {
            RCI_Core.AccessFunction(input);
        }
        else if (string.IsNullOrWhiteSpace(input))
        {
            RCI_Core.WriteError("Command is empty");
        }
        else
        {
            if (!File.Exists(input))
                RCI_Core.WriteError("Invalid command or file name");
            else
            {
                try
                {
                    ProcessStartInfo info = new()
                    {
                        FileName = input,
                        UseShellExecute = true
                    };
                    Process.Start(info);
                }
                catch (Exception ex)
                {
                    RCI_Core.WriteError(ex.Message);
                }
            }
        }
    }
    public static void ParseScriptFile(string path)
    {
        foreach (string line in File.ReadAllLines(path))
        {
            ParseScript(line);
        }
    }
}