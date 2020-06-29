#pragma once

#include "WriteFontMetrics.h"

namespace Frederisk_RtfEditor_DirectXWrapper {
    public ref class WriteFont sealed {
    private:
        winrt::com_ptr<IDWriteFont> pWriteFont;
    internal:
        WriteFont(winrt::com_ptr<IDWriteFont> pWriteFont);
    public:
        bool HasCharacter(UINT32 unicodeValue);
        bool IsSymbolFont();
        WriteFontMetrics GetMetrics();
    };
}