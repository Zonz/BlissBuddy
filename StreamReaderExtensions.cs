using System;
using System.IO;

namespace BlissBuddy
{
    public static class StreamReaderExtensions
    {
        public static string[] ReadLines(this StreamReader reader, int count)
        {
            string[] strings = new string[count];
            for (int i = 0; i < count; i++)
                strings[i] = reader.ReadLine();
            return strings;
        }

        public static string ReadName(this StreamReader reader)
        {
            string line = reader.ReadLine();
            string[] split = line.Split(new string[] { "<strong>", "</strong>" }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 2)
                throw new Exception("Could not read the name.");
            return split[1];
        }

        public static (string Name, int Rank, int Experience, int Prestige) ReadSkill(this StreamReader reader)
        {
            // 6 lines of data, plus 3 empty lines, to move the pointer
            string[] data = reader.ReadLines(9);

            // Parsing skill name
            string name = data[0].Split(new string[] { "&skill=", "\"><img src=" }, StringSplitOptions.None)[1];

            // Parsing numbers - skipping unnecessary values.
            string[] numberSplit = new string[] { "<td>", "</td>" };
            string rank = data[1].Split(numberSplit, StringSplitOptions.None)[1].Replace(",", "");
            string experience = data[2].Split(numberSplit, StringSplitOptions.None)[1].Replace(",", "");
            string prestige = data[3].Split(numberSplit, StringSplitOptions.None)[1].Replace(",", "");
            // string toNextLevel = data[4].Split(numberSplit, StringSplitOptions.None)[1].Replace(",", "");
            // string level = data[5].Split(numberSplit, StringSplitOptions.None)[1].Replace(",", "");

            // Set numbers to -1 if no data is present
            rank = rank.Length > 0 ? rank : "-1";
            experience = experience.Length > 0 ? experience : "-1";
            prestige = prestige.Length > 0 ? prestige : "-1";
            // toNextLevel = toNextLevel.Length > 0 ? toNextLevel : "-1";
            // level = level.Length > 0 ? level : "-1";

            return (name, int.Parse(rank), int.Parse(experience), int.Parse(prestige));
        }
    }
}
