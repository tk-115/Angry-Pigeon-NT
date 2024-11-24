using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class LanguageControll {

    StringTable string_table;

    public void LoadLocaledTexts() {
        LocalizedStringTable m_StringTable = new LocalizedStringTable { TableReference = "StringTableLocalesCollection" };
        string_table = m_StringTable.GetTable();
    }

    public string GetLocaledTextByKey(string key) {
        StringTableEntry localized_character_name = string_table.GetEntry(key);
        return localized_character_name.GetLocalizedString();
    }
}

