#pragma once

#include "WriteFontCollection.h"

namespace Frederisk_RtfEditor_DirectXWrapper {
    public ref class WriteFactory sealed {
    private:
        Microsoft::WRL::ComPtr<IDWriteFactory> pFactory;
    public:
        WriteFactory();
        WriteFontCollection^ GetSystemFontCollection();
        WriteFontCollection^ GetSystemFontCollection(bool checkForUpdates);
    };
}