#pragma once

#include "WriteFontCollection.h"

namespace Frederisk_RtfEditor_DirectXWrapper {
    /// <summary>
    /// 實現IDWriteFactory的模型物件。
    /// </summary>
    public ref class WriteFactory sealed {
    private:
        /// <summary>
        /// IDWriteFactory物件。
        /// </summary>
        winrt::com_ptr<IDWriteFactory> pFactory;
    public:
        /// <summary>
        /// WriteFactory的無參構造器。
        /// </summary>
        WriteFactory();
        WriteFontCollection^ GetSystemFontCollection();
        WriteFontCollection^ GetSystemFontCollection(bool checkForUpdates);
    };
}
