using System;
using System.Collections.Generic;

internal static class TMP_DropdownExistions
{
    public static void FillOptionsWithNamesEnum(this TMPro.TMP_Dropdown tMP_Dropdown, Type type)
    {
        List<string> options = new List<string>();

        for (int i = 0; i < Enum.GetNames(type).Length; i++)
            options.Add(Enum.GetNames(type)[i].ToString());

        tMP_Dropdown.AddOptions(options);
    }
}
