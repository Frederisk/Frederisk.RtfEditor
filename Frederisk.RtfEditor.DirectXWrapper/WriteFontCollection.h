#pragma once

#include "WriteFontFamily.h"

namespace Frederisk_RtfEditor_DirectXWrapper {
    public ref class WriteFontCollection sealed {
    private:
        winrt::com_ptr<IDWriteFontCollection> pFontCollection;
    internal:
        WriteFontCollection(winrt::com_ptr<IDWriteFontCollection> pFontCollection);
    public:
        bool FindFamilyName(Platform::String^ familyName, int* index);
        int GetFontFamilyCount();
        WriteFontFamily^ GetFontFamily(int index);
    };
}