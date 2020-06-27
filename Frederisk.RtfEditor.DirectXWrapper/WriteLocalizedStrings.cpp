#include "pch.h"
#include "WriteLocalizedStrings.h"

using namespace Frederisk_RtfEditor_DirectXWrapper;
using namespace Platform;
using namespace Microsoft::WRL;

WriteLocalizedStrings::WriteLocalizedStrings(ComPtr<IDWriteLocalizedStrings> pLocalizedStrings) {
    this->pLocalizedStrings = pLocalizedStrings;
}

int WriteLocalizedStrings::GetCount() {
    return this->pLocalizedStrings->GetCount();
}

String^ WriteLocalizedStrings::GetLocaleName(int index) {
    UINT32 length = 0;
    Check(this->pLocalizedStrings->GetLocaleNameLength(index, &length));
    wchar_t* str = new (std::nothrow) wchar_t[length + 1];
    if (str == nullptr) throw ref new COMException(E_OUTOFMEMORY);
    Check(this->pLocalizedStrings->GetLocaleName(index, str, length + 1));
    String^ string = ref new String(str);
    delete[] str;
    return string;
}

String^ WriteLocalizedStrings::GetString(int index) {
    UINT32 length = 0;
    Check(this->pLocalizedStrings->GetStringLength(index, &length));
    wchar_t* str = new (std::nothrow) wchar_t[length + 1];
    if (str == nullptr) throw ref new COMException(E_OUTOFMEMORY);
    Check(this->pLocalizedStrings->GetString(index, str, length + 1));
    String^ string = ref new String(str);
    delete[] str;
    return string;
}

bool WriteLocalizedStrings::FindLocaleName(String^ localeName, int* index) {
    uint32 localeIndex = 0;
    BOOL existes = false;
    Check(this->pLocalizedStrings->FindLocaleName(localeName->Data(), &localeIndex, &existes));
    *index = localeIndex;
    return existes != 0;
}