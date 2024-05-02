#include "pch.h"
#include "framework.h"
#include "AtmApi.h"
#include <string>
#include <unordered_map>
#include <wchar.h>
#include <ctime>
#include <thread>

static const HANDLE hTempHandle { HANDLE(42) };
pAtmEventCallback pCallbackMethod {nullptr};

std::wstring userName {L"Valued Customer"};
std::wstring trxID {L"12345ABCDE"};
std::wstring languageID {L"en-US"};
std::wstring location {L"Hollywood, FL"};

ATMAPI HANDLE Connect(InstanceData *pInstanceData) {
    if (pInstanceData) {
        pInstanceData->apiAppData.appVersionData.major = 1;
        pInstanceData->apiAppData.appVersionData.minor = 0;
        pInstanceData->atmHandle = hTempHandle;
        wcsncpy_s(pInstanceData->location, LOCATION_SIZE, location.c_str(), LOCATION_SIZE - 1);

        /* This is only here to simulate an operation taking some time to complete*/
        std::this_thread::sleep_for(std::chrono::milliseconds(500));

        return pInstanceData->atmHandle;
    }

    return HANDLE(0);
}

ATMAPI HRESULT Disconnect([[maybe_unused]] HANDLE hInstance) {
    /* This is only here to simulate an operation taking some time to complete*/
    std::this_thread::sleep_for(std::chrono::milliseconds(500));

    return S_OK;
}


extern "C" ATMAPI HRESULT RegisterCallback([[maybe_unused]] HANDLE hInstance, pAtmEventCallback pCallback) {
    // Save the callback function pointer for future use
    // Invoke this callback to fire the event from the C++ DLL
    pCallbackMethod = pCallback;
    return S_OK;
}


extern "C" ATMAPI HRESULT StartTransaction([[maybe_unused]] HANDLE hInstance, TransactionData * pTrxData) {
    if (pTrxData) {
        wcsncpy_s(pTrxData->userName, USERNAME_SIZE, userName.c_str(), USERNAME_SIZE - 1);
        wcsncpy_s(pTrxData->trxID, TRXID_SIZE, trxID.c_str(), TRXID_SIZE - 1);
        wcsncpy_s(pTrxData->languageID, LANGUAGEID_SIZE, languageID.c_str(), LANGUAGEID_SIZE - 1);
        SYSTEMTIME systemTime;
        GetSystemTime(&systemTime);

        FILETIME fileTime;
        SystemTimeToFileTime(&systemTime, &fileTime);

        ULARGE_INTEGER largeInteger;
        largeInteger.LowPart = fileTime.dwLowDateTime;
        largeInteger.HighPart = fileTime.dwHighDateTime;

        pTrxData->transactionTime = largeInteger.QuadPart;

        /* This is only here to simulate an operation taking some time to complete*/
        std::this_thread::sleep_for(std::chrono::milliseconds(500));

        return S_OK;
    }

    return E_INVALIDARG;
}

extern "C" ATMAPI HRESULT EndTransaction([[maybe_unused]] HANDLE hInstance, TransactionData * pTrxData) {
    if (pTrxData) {
        memset(pTrxData, 0, sizeof(TransactionData));

        /* This is only here to simulate an operation taking some time to complete*/
        std::this_thread::sleep_for(std::chrono::milliseconds(500));

        return S_OK;
    }

    return E_INVALIDARG;
}

extern "C" ATMAPI HRESULT RegisterPhoneKYC([[maybe_unused]] HANDLE hInstance, RegistrationData * pRegistrationData) {
    if (pRegistrationData) {
        memset(pRegistrationData, 0, sizeof(RegistrationData));

        /* This is only here to simulate an operation taking some time to complete*/
        std::this_thread::sleep_for(std::chrono::milliseconds(500));

        return S_OK;
    }

    return E_INVALIDARG;
}