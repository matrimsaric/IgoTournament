using CommonModule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static StoneLedger.Views.JosekiStudy.JosekiStudyPage;

namespace StoneLedger.Resources.Dictionaries
{
    public static class StudyVisuals
    {
        public static readonly Dictionary<string, Color> BranchColours = new()
        {
            ["34"] = Color.FromArgb("#003366"),
            ["44"] = Color.FromArgb("#006633"),
            ["33"] = Color.FromArgb("#FFD700"),
            ["54"] = Color.FromArgb("#FF8C00"),

            ["nirensei"] = Color.FromArgb("#99CC66"),
            ["sanrensei"] = Color.FromArgb("#669933"),
            ["shusaku"] = Color.FromArgb("#FF66CC"),
            ["chinese"] = Color.FromArgb("#4B0082"),
            ["345"] = Color.FromArgb("#C299FF"),
        };

        public static readonly Dictionary<VariationType, string> ModeKanji = new()
        {
            [VariationType.None] = "丸",
            [VariationType.Joseki] = "定石",
            [VariationType.Fuseki] = "布石",
            [VariationType.Tesuji] = "手筋",
            [VariationType.Yose] = "後手",
        };
    }
}
