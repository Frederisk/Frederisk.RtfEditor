#pragma once

#include "WriteLocalizedStrings.h"
#include "WriteFont.h"

namespace Frederisk_RtfEditor_DirectXWrapper {
    public ref class WriteFontFamily sealed {
    private:
        winrt::com_ptr<IDWriteFontFamily> pFontFamily;
    internal:
        WriteFontFamily(winrt::com_ptr<IDWriteFontFamily> pFontFamily);
    public:
        WriteLocalizedStrings^ GetFamilyNames();
        WriteFont^ GetFirstMatchingFont(Windows::UI::Text::FontWeight fontWeight, Windows::UI::Text::FontStretch fontStretch, Windows::UI::Text::FontStyle FontStyle);
    };
}