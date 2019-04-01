using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace pa_04_Jaeguins {
    public enum SORT {
        TIME, IP, URL, STATUS
    }
    public class Program {
        public static SORT Flag = SORT.TIME;
        public static List<WebLog> logs = new List<WebLog>();
        static void Main(string[] args) {
            bool done;
            Console.WriteLine("Type 'help' to show instruction.");
            while (true) {
                Console.Write('$');
                string[] input = Console.ReadLine().Split(' ');
                done = false;
                switch (input[0]) {
                    case "read":
                        if (input.Length < 2) continue;
                        Read(input[1]);
                        done = true;
                        break;
                    case "sort":
                        if (input.Length < 2) continue;
                        switch (input[1]) {
                            case "-t":
                                Flag = SORT.TIME;
                                break;
                            case "-ip":
                                Flag = SORT.IP;
                                break;
                            case "-status":
                                Flag = SORT.STATUS;
                                break;
                            case "-url":
                                Flag = SORT.URL;
                                break;
                            default:
                                Console.WriteLine("Standard Not Found");
                                continue;
                        }
                        logs.Sort();
                        foreach (WebLog t in logs)
                            Console.WriteLine(t);
                        done = true;
                        break;
                    case "print":
                        foreach (WebLog t in logs)
                            Console.WriteLine(t);
                        done = true;
                        break;
                    case "help":
                        Console.WriteLine("\tWebLogger - help\n\n\n   exit - exit program.\n\n   help - show this message.\n\n   print - print all logs with current sort.\n\n   read [FilePath] - read file in path.\n\n   sort <-t|-ip|-status|-url> - sort logs with time/IP/status/URL.\n");
                        done = true;
                        break;
                    case "exit":
                        return;
                }
                if (!done) Console.WriteLine("Command Not Found.");
            }
        }
        static void Read(string path) {
            if (!File.Exists(path)) {
                Console.WriteLine("File Not Found");
                return;
            }
            string[] read = File.ReadAllLines(path);
            WebLog t;
            string line;
            for (int j = 1; j < read.Length; j++) {
                line = read[j];
                t = new WebLog();
                string[] words = line.Split(',');
                int i = 0;
                foreach (string IPdata in words[0].Split('.')) {
                    t.IP[i++] = byte.Parse(IPdata);
                }
                string[] Times = words[1].Substring(1).Split(':');
                for (i = 1; i < 4; i++)
                    t.Time[i + 2] = int.Parse(Times[i]);
                string[] Date = Times[0].Split('/');
                for (i = 0; i < 3; i++)
                    t.Time[i] = i == 1 ? MonthStringToInteger(Date[i]) : int.Parse(Date[i]);
                t.URL = words[2];
                t.Status = int.Parse(words[3]);
                logs.Add(t);
            }
            logs.Sort();
        }
        static int MonthStringToInteger(string month) {
            switch (month) {
                case "Jan":
                    return 1;
                case "Feb":
                    return 2;
                case "Mar":
                    return 3;
                case "Apr":
                    return 4;
                case "May":
                    return 5;
                case "Jun":
                    return 6;
                case "Jul":
                    return 7;
                case "Aug":
                    return 8;
                case "Sep":
                    return 9;
                case "Oct":
                    return 10;
                case "Nov":
                    return 11;
                case "Dec":
                    return 12;
                default:
                    return -1;
            }
        }
    }
    public class WebLog : IComparable<WebLog> {
        public static int[] timePriority = {
            2,1,0,3,4,5
        };
        public static string[] Months ={
            "None","Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"
        };
        public int[] Time = new int[6];
        public byte[] IP = new byte[4];
        public string URL;
        public int Status = 0;
        public int CompareTo(WebLog other) {
            switch (Program.Flag) {
                case SORT.IP:
                    for (int i = 0; i < 4; i++) {
                        if (IP[i].Equals(other.IP[i])) i++;
                        else return IP[i].CompareTo(other.IP[i]);
                    }
                    break;
                case SORT.URL:
                    return URL.CompareTo(other.URL);
                case SORT.STATUS:
                    return Status.CompareTo(other.Status);
                case SORT.TIME:
                    for (int i = 0; i < 6; i++) {
                        if (Time[timePriority[i]].Equals(other.Time[timePriority[i]])) i++;
                        else return Time[timePriority[i]].CompareTo(other.Time[timePriority[i]]);
                    }
                    break;
            }
            return 0;
        }
        static string TabLine = "\n\t";
        public override string ToString() {
            switch (Program.Flag) {
                case SORT.IP:
                    return GetIP() + TabLine + GetTime() + TabLine + GetURL() + TabLine + GetStatus();
                case SORT.TIME:
                    return GetTime() + TabLine + GetIP() + TabLine + GetURL() + TabLine + GetStatus();
                case SORT.URL:
                    return GetURL() + TabLine + GetIP() + TabLine + GetTime() + TabLine + GetStatus();
                case SORT.STATUS:
                    return GetStatus() + TabLine + GetIP() + TabLine + GetTime() + TabLine + GetURL();
                default:
                    throw new InvalidDataException();
            }
        }
        string GetIP() {
            string ret = Program.Flag == SORT.IP ? "" : "IP : ";
            for (int i = 0; i < 4; i++)
                ret += IP[i] + (i == 3 ? "" : ".");
            return ret;
        }
        string GetTime() {
            string ret = Program.Flag == SORT.TIME ? "" : "Time : ";
            for (int i = 0; i < 6; i++) {
                ret +=
                    (i == 1 ? Months[Time[i]] : Time[i] + "") +
                    (i < 3 ? "/" : i < 5 ? ":" : "");
            }
            return ret;
        }
        string GetURL() {
            return Program.Flag == SORT.URL ? "" : "URL : " + URL;
        }
        string GetStatus() {
            return Program.Flag == SORT.STATUS ? "" : "Status : " + Status;
        }
    }


}
