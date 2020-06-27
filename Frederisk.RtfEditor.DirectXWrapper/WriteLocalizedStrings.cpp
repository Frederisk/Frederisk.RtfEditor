#include "pch.h"
#include "WriteLocalizedStrings.h"

using namespace DirectXWrapper;
using namespace Platform;
using namespace Microsoft::WRL;

WriteLocalizedStrings::WriteLocalizedStrings(ComPtr<IDWriteLocalizedStrings> pLocalizedStrings) {
    this->pLocalizedStrings = pLocalizedStrings;
}

int WriteLocalizedStrings::GetCount() {
    this->pLocalizedStrings->GetCount();
}

String^ WriteLocalizedStrings::GetLocaleName(int index) {
    UINT32 length = 0;
    HRESULT hr = this->pLocalizedStrings->GetLocaleNameLength(index, &length);
    if (!SUCCEEDED(hr)) {
        throw ref new COMException(hr);
    }
    wchar_t* str = new (std::nothrow) wchar_t[length + 1];
    if (str == nullptr) {
        throw ref new COMException(E_OUTOFMEMORY);
    }
    hr = this->pLocalizedStrings->GetLocaleName(index, str, length + 1);
    if (!SUCCEEDED(hr)) {
        throw ref new COMException(hr);
    }
    String^ string = ref new String(str);
    delete[] str;
    return string;

}

String^ WriteLocalizedStrings::GetString(int index) {

}
