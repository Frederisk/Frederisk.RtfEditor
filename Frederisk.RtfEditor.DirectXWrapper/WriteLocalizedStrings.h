#pragma once

namespace Frederisk_RtfEditor_DirectXWrapper {
    public ref class WriteLocalizedStrings sealed {
    private:
        winrt::com_ptr<IDWriteLocalizedStrings> pLocalizedStrings;
    internal:
        WriteLocalizedStrings(winrt::com_ptr<IDWriteLocalizedStrings> pLocalizedStrings);
    public:
        int GetCount();
        Platform::String^ GetLocaleName(int index);
        Platform::String^ GetString(int index);
        bool FindLocaleName(Platform::String^ localeName, int* index);
    };
}