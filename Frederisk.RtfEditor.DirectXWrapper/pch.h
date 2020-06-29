#pragma once

// 標準先行檔頭，提供必要依賴的先決。

#include <collection.h>
#include <ppltasks.h>
#include <wrl.h>
#include <d2d1_1.h>
#include <d3d11_1.h>
#include <windows.ui.xaml.media.dxinterop.h>
#include <dwrite.h>


// ReSharper disable once CppInconsistentNaming
// ReSharper disable once CommentTypo
/**
 * \brief 對一個方法的結果進行測試，失敗則擲出例外。
 * \param action 一個會返回HRESULT型別的方法。
 */

#define Check(action) {\
    HRESULT __hr = (action);\
    if (!SUCCEEDED(__hr)) {\
        throw ref new COMException(__hr);\
    }\
}
